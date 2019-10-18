using Engine;
using Engine.Models;
using Flounchy.Equipments;
using Flounchy.Misc;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Battle
{
  public abstract class Actor : Sprite
  {
    public enum States
    {
      Alive,
      Dying,
      Dead,
    }

    protected float _distance = 0;
    protected bool _goingUp;

    protected Texture2D _border = null;

    protected AbilityModel _ability;

    protected Equipment _equipment;

    protected HealthBar _healthBar;

    protected TurnBar _turnBar;

    private Action _onFirstUpdate;

    private SpriteFont _font;

    public ActionResult ActionResult;

    public Hand LeftHand;

    public Hand RightHand;

    public Sprite LeftHandWeapon;

    public Sprite RightHandWeapon;

    public ActorModel ActorModel { get; set; }

    public override float Opacity
    {
      get { return _opacity; }
      set
      {
        _opacity = value;

        RightHand.Opacity = _opacity;
        LeftHand.Opacity = _opacity;
      }
    }

    public bool ShowTurnBar = false;

    public int MaxHealth { get; set; } = 8;

    public int CurrentHealth { get; set; } = 8;

    public Vector2? StartPosition { get; private set; } = null;

    public bool ShowBorder = false;

    public States State { get; set; }

    public Actor(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content)
    {
      Position = position;

      _font = content.Load<SpriteFont>("Fonts/ButtonFont");

      _healthBar = new HealthBar(content);

      _onFirstUpdate += OnFirstUpdate;
    }

    protected void SetBorder(GraphicsDevice graphics)
    {
      _border = new Texture2D(graphics, _texture.Width, _texture.Height);

      _border.SetData(Helpers.GetBorder(_border, 2));
    }

    protected void SetLeftHand(Texture2D texture)
    {
      LeftHand = new Hand(texture)
      {
        Position = this.Position + new Vector2(-40, 10),
      };
    }

    protected void SetRightHand(Texture2D texture)
    {
      RightHand = new Hand(texture)
      {
        Position = this.Position + new Vector2(40, 10),
      };
    }

    public override void Update(GameTime gameTime)
    {
      _onFirstUpdate?.Invoke();

      if (StartPosition == null)
        StartPosition = Position;

      switch (State)
      {
        case States.Alive:

          IdleMovement();

          AttackMovement();

          if (ShowTurnBar)
            _turnBar?.Update(gameTime);

          break;
        case States.Dying:

          Opacity -= 0.05f;

          if (Opacity <= 0)
            State = States.Dead;

          break;
        case States.Dead:
          break;
        default:
          break;
      }
    }

    private void OnFirstUpdate()
    {
      var stance = this.ActorModel.EquipmentModel.GetStanceType();

      var equipmentType = this.ActorModel.EquipmentModel.EquipmentType.ToString();

      var possibleWeaponType = equipmentType.Split('_');

      object[] parameters = null;

      switch (stance)
      {
        case EquipmentModel.StanceTypes.Fists:
          parameters = new object[]
          {
            LeftHand,
            RightHand,
          };
          break;
        case EquipmentModel.StanceTypes.SingleHanded:
          parameters = new object[]
          {
            LeftHand,
            RightHand,
            null,
            RightHandWeapon,
          };
          break;
        case EquipmentModel.StanceTypes.DualHanded:
          parameters = new object[]
          {
            LeftHand,
            RightHand,
            LeftHandWeapon,
            RightHandWeapon,
          };
          break;
        case EquipmentModel.StanceTypes.BothHands:
          parameters = new object[]
          {
            LeftHand,
            RightHand,
            LeftHandWeapon,
          };
          break;
        default:
          break;
      }

      Type t = null;

      foreach (var value in possibleWeaponType)
      {
        t = Type.GetType("Flounchy.Equipments." + value);

        if (t != null)
          continue;
      }

      if (t == null)
        throw new Exception("Setup isn't complete for: " + equipmentType);

      this._equipment = (Equipments.Equipment)Activator.CreateInstance(t, parameters);

      this._equipment.SetStance(this.Position);
      this._equipment.SetEquipmentRotation();

      _onFirstUpdate = null;
    }

    protected virtual void IdleMovement()
    {
      int max = 3;
      var difference = max - _distance;

      var speed = 0.1f;

      if (_goingUp)
      {
        Position.Y -= speed;
        _distance -= speed;
      }
      else
      {
        Position.Y += speed;
        _distance += speed;
      }

      if (_distance >= max ||
          _distance <= 0)
      {
        _goingUp = !_goingUp;
      }
    }

    protected bool _attacked = false;

    protected void AttackMovement()
    {
      if (!_attacked)
        return;

      this._equipment.OnAttack(_ability);
      this._equipment.SetEquipmentRotation();
    }

    protected virtual void Attack()
    {
      bool haveHandsFinished = false;

      if (LeftHand.State == Hand.States.FinishedAttacking)
      {
        haveHandsFinished = true;

        if (RightHand.State == Hand.States.Attacking)
          haveHandsFinished = false;
      }

      if (RightHand.State == Hand.States.FinishedAttacking)
      {
        haveHandsFinished = true;

        if (LeftHand.State == Hand.States.Attacking)
          haveHandsFinished = false;
      }

      if (_attacked && haveHandsFinished)
      {
        _attacked = false;
        ActionResult.State = Engine.ActionStates.Finished;

        return;
      }

      _attacked = true;
    }

    public abstract ActionResult GetAction(string ability);

    public abstract List<Actor> GetTargets(IEnumerable<Actor> actors);

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      LeftHand.Colour = this.Colour;
      RightHand.Colour = this.Colour;

      LeftHand.Draw(gameTime, spriteBatch);

      RightHand.Draw(gameTime, spriteBatch);

      base.Draw(gameTime, spriteBatch);

      if (ShowBorder && _border != null)
        spriteBatch.Draw(_border, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);

      _healthBar.SetActor(this);
      _healthBar.Draw(gameTime, spriteBatch);

      var nameWidth = _font.MeasureString(ActorModel.Name).X;
      var nameHeight = _font.MeasureString(ActorModel.Name).Y;

      spriteBatch.DrawString(_font, ActorModel.Name, _healthBar.Position + new Vector2(-(nameWidth / 2), (nameHeight / 2)), Color.Black);

      if (ShowTurnBar)
        _turnBar?.Draw(gameTime, spriteBatch);
    }
  }
}
