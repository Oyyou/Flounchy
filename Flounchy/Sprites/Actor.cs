using Engine;
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
  public abstract class Actor : Sprite
  {
    protected float _distance = 0;
    protected bool _goingUp;

    protected Texture2D _border = null;

    public ActionResult ActionResult;

    public Hand LeftHand;

    public Hand RightHand;

    public ActorModel ActorModel { get; set; }

    public AbilitiesModel Abilities { get; set; }

    protected HealthBar _healthBar;

    protected TurnBar _turnBar;

    public bool ShowTurnBar = false;

    public int MaxHealth { get; set; } = 8;

    public int CurrentHealth { get; set; } = 8;

    public Vector2? StartPosition { get; private set; } = null;

    public bool ShowBorder = false;

    public Actor(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content)
    {
      Position = position;

      _healthBar = new HealthBar(content);
    }

    protected void SetBorder(GraphicsDevice graphics)
    {
      _border = new Texture2D(graphics, _texture.Width, _texture.Height);

      var colours = new Color[_border.Width * _border.Height];

      int thickness = 2;

      var index = 0;
      for (int y = 0; y < _texture.Height; y++)
      {
        for (int x = 0; x < _texture.Width; x++)
        {
          var colour = new Color(0, 0, 0, 0);

          if (x < thickness || x > (_texture.Width - 1) - thickness ||
              y < thickness || y > (_texture.Height - 1) - thickness)
          {
            colour = new Color(255, 255, 0, 10);
          }

          colours[index] = colour;

          index++;
        }
      }

      _border.SetData(colours);
    }

    protected void SetLeftHand(Texture2D texture)
    {
      LeftHand = new Hand(texture, -1)
      {
        Position = this.Position + new Vector2(-40, 10),
      };
    }

    protected void SetRightHand(Texture2D texture)
    {
      RightHand = new Hand(texture, 1)
      {
        Position = this.Position + new Vector2(40, 10),
      };
    }

    public override void Update(GameTime gameTime)
    {
      if (StartPosition == null)
        StartPosition = Position;

      IdleMovement();

      AttackMovement();

      if (ShowTurnBar)
        _turnBar?.Update(gameTime);
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

    protected virtual void AttackMovement()
    {
      LeftHand.AttackMovement();
      RightHand.AttackMovement();
    }

    public abstract ActionResult GetAction(string ability);

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);

      if (ShowBorder && _border != null)
        spriteBatch.Draw(_border, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);

      LeftHand.Colour = this.Colour;
      RightHand.Colour = this.Colour;

      LeftHand.Draw(gameTime, spriteBatch);

      RightHand.Draw(gameTime, spriteBatch);

      _healthBar.SetActor(this);
      _healthBar.Draw(gameTime, spriteBatch);

      if (ShowTurnBar)
        _turnBar?.Draw(gameTime, spriteBatch);
    }
  }
}
