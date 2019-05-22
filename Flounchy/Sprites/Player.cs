using Engine;
using Engine.Input;
using Engine.Models;
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

namespace Flounchy.Sprites
{
  public class Player : Actor
  {
    private bool _attacked = false;

    public Weapon LeftHandWeapon;
    public Weapon RightHandWeapon;

    public BattleStatsModel BattleStats { get; private set; }

    public Clothing Lower = null;
    public Clothing Upper = null;

    public Player(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Body");
      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

      SetBorder(graphics);

      SetLeftHand(content.Load<Texture2D>("Actor/Hand"));

      SetRightHand(content.Load<Texture2D>("Actor/Hand"));

      ActionResult = new Engine.ActionResult()
      {
        Action = Attack,
        State = Engine.ActionStates.Waiting,
      };

      _turnBar = new TurnBar(content, new Vector2(Position.X, (Position.Y + Origin.Y) + 15));

      _setStance += SetStance;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      _setStance?.Invoke();

      switch (ActorModel.EquipmentModel.GetStanceType())
      {
        case EquipmentModel.StanceTypes.SingleHanded:
          AttackWithOneHand();
          break;
        case EquipmentModel.StanceTypes.BothHands:
          //AttackWithTwoHands();
          break;
        default:
          break;
      }
    }

    private void AttackWithOneHand()
    {
      if (LeftHandWeapon == null)
        return;

      if (LeftHand.Attacking)
      {
        if (LeftHand.AttackingDown)
        {
          LeftHandWeapon.Rotation -= MathHelper.ToRadians(2);
        }
        else
        {
          LeftHandWeapon.Rotation += MathHelper.ToRadians(2);
        }
      }

    }

    private void AttackWithTwoHands()
    {
      if (LeftHandWeapon == null)
        return;

      if (LeftHand.Attacking)
      {
        if (LeftHand.AttackingDown)
        {
          LeftHandWeapon.Rotation -= MathHelper.ToRadians(2);
        }
        else
        {
          LeftHandWeapon.Rotation += MathHelper.ToRadians(2);
        }
      }
    }

    protected override void AttackMovement()
    {
      // If we're attacked with our fists, then do the default "AttackMovement"
      if (LeftHand.Attacking && LeftHandWeapon == null ||
          RightHand.Attacking && RightHandWeapon == null)
      {
        base.AttackMovement();
        return;
      }

      if (!LeftHand.Attacking && !RightHand.Attacking)
        return;

      var lPoints = new List<Vector2>();
      var rPoints = new List<Vector2>();

      for (int i = 0; i < 50; i++)
      {
        lPoints.Add(LeftHand.Position + new Vector2(0, -(i * 0.5f)));
        rPoints.Add(RightHand.Position + new Vector2(-(i * 0.75f), -(i * 1.5f)));
      }

      LeftHand.AttackMovement(lPoints);
      RightHand.AttackMovement(rPoints);

      LeftHandWeapon.Position = LeftHand.Position;

      if (RightHand.AttackingDown)
      {
        LeftHandWeapon.Rotation += MathHelper.ToRadians(0.80f);
      }
      else
      {
        LeftHandWeapon.Rotation -= MathHelper.ToRadians(0.80f);
      }
    }

    private Action _setStance;

    private void SetStance()
    {
      _setStance -= SetStance;

      var stanceType = ActorModel.EquipmentModel.GetStanceType();

      switch (stanceType)
      {
        case EquipmentModel.StanceTypes.SingleHanded:
          break;
        case EquipmentModel.StanceTypes.BothHands:
          RightHand.Position = this.Position + new Vector2(40, -10);
          LeftHand.Position = this.Position + new Vector2(-40, 30);
          LeftHandWeapon.Position = LeftHand.Position;
          LeftHandWeapon.Rotation = MathHelper.ToRadians(63);
          break;
        default:
          throw new Exception("Unknown StanceType: " + stanceType);
      }
    }

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.State != ActionStates.Waiting && ActionResult.State != ActionStates.Finished)
        return ActionResult;

      ActionResult.State = ActionStates.Waiting;

      var abilityList = ActorModel.Abilities.Get();
      foreach (var abil in abilityList)
      {
        if (abil.Text == ability)
        {
          ActionResult.State = ActionStates.WaitingForTarget;
        }
      }

      return ActionResult;
    }

    public override Actor GetTarget(IEnumerable<Actor> enemies)
    {
      Actor target = null;

      var enemy = enemies.Where(c => GameMouse.Intersects(c.Rectangle)).FirstOrDefault();

      if (enemy != null)
      {
        target = enemy;

        target.ShowBorder = true;

        if (GameMouse.IsLeftClicked)
        {
          ActionResult.State = Engine.ActionStates.Running;
        }
      }

      return target;
    }

    public void Attack()
    {
      switch (ActorModel.EquipmentModel.GetStanceType())
      {
        case EquipmentModel.StanceTypes.SingleHanded:

          if (_attacked && !LeftHand.Attacking)
          {
            _attacked = false;
            ActionResult.State = Engine.ActionStates.Finished;

            return;
          }

          LeftHand.Attacking = true;

          break;
        case EquipmentModel.StanceTypes.BothHands:

          if (_attacked && !RightHand.Attacking)
          {
            _attacked = false;
            ActionResult.State = Engine.ActionStates.Finished;

            return;
          }

          RightHand.Attacking = true;
          LeftHand.Attacking = true;

          break;
        default:
          break;
      }

      _attacked = true;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (ActorModel.EquipmentModel.GetStanceType() == EquipmentModel.StanceTypes.BothHands)
      {
        LeftHandWeapon?.Draw(gameTime, spriteBatch);
      }
      else
      {
        LeftHandWeapon?.Draw(gameTime, spriteBatch);
        RightHandWeapon?.Draw(gameTime, spriteBatch);
      }

      base.Draw(gameTime, spriteBatch);

      if (Upper != null)
      {
        Upper.ShowBack();
        Upper.Position = new Vector2(Position.X, Position.Y);
        Upper.Draw(gameTime, spriteBatch);
      }

      if (Lower != null)
      {
        Lower.ShowBack();
        Lower.Position = new Vector2(Position.X, (Position.Y + Origin.Y) - (Lower.Origin.Y));
        Lower.Draw(gameTime, spriteBatch);
      }
    }
  }
}
