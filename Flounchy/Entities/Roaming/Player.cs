using Engine.Input;
using Flounchy.Components;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flounchy.Misc.Map;

namespace Flounchy.Entities.Roaming
{
  public class Player : Entity
  {
    public enum Directions
    {
      Up,
      Down,
      Left,
      Right,
    }

    private readonly TextureAnimatedComponent _animationComponent;
    private readonly InteractComponent _interactComponent;
    private readonly MoveComponent _moveComponent;

    public Directions Direction;

    public CollisionResults CollisionResult
    {
      get
      {
        return _moveComponent.CollisionResult;
      }
    }

    public Rectangle CurrentRectangle
    {
      get
      {
        return _moveComponent.CurrentRectangle;
      }
    }

    public bool EnterBattle { get; set; }

    public Player(Texture2D texture, Map map)
      : base()
    {
      _animationComponent = new TextureAnimatedComponent(this, texture, 4, 4, 0.3f)
      {
        SetAnimation = (gameTime) => SetAnimationEvent(gameTime),
        GetLayer = () => MathHelper.Clamp((_moveComponent.CurrentRectangle.Y) / 1000f, 0, 1),
      };

      _interactComponent = new InteractComponent(this)
      {

      };

      _moveComponent = new MoveComponent(this, map, (gameTime) => SetMovementEvent(gameTime))
      {
        Speed = 2,
        CurrentRectangleOffset = new Rectangle(0, 40, 0, 0),
        OnBattle = () => EnterBattle = true,
      };

      Components.Add(_moveComponent);
      Components.Add(_interactComponent);
      Components.Add(_animationComponent);
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyPressed(Keys.D))
      {
        Direction = Directions.Right;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.A))
      {
        Direction = Directions.Left;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.W))
      {
        Direction = Directions.Up;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.S))
      {
        Direction = Directions.Down;
      }
      else if (GameKeyboard.IsKeyDown(Keys.D))
      {
        _moveComponent.Velocity = new Vector2(_moveComponent.Speed, 0);
        Direction = Directions.Right;
      }
      else if (GameKeyboard.IsKeyDown(Keys.A))
      {
        _moveComponent.Velocity = new Vector2(-_moveComponent.Speed, 0);
        Direction = Directions.Left;
      }
      else if (GameKeyboard.IsKeyDown(Keys.W))
      {
        _moveComponent.Velocity = new Vector2(0, -_moveComponent.Speed);
        Direction = Directions.Up;
      }
      else if (GameKeyboard.IsKeyDown(Keys.S))
      {
        _moveComponent.Velocity = new Vector2(0, _moveComponent.Speed);
        Direction = Directions.Down;
      }
    }

    public override void Update(GameTime gameTime)
    {
      EnterBattle = false;

      base.Update(gameTime);
    }

    private void SetAnimationEvent(GameTime gameTime)
    {
      _animationComponent.CurrentYFrame = (int)Direction;

      _animationComponent.Playing = _moveComponent.Velocity != Vector2.Zero;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      // stuff

      base.Draw(gameTime, spriteBatch);
    }
  }
}
