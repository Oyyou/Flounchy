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

      _interactComponent = new InteractComponent(this, () => _moveComponent.CurrentRectangle)
      {

      };

      _moveComponent = new MoveComponent(this, map, (gameTime) => SetMovementEvent(gameTime))
      {
        Speed = 2,
        CurrentRectangleOffset = new Rectangle(0, 40, 0, 0),
        OnBattle = () => EnterBattle = true,
      };

      _mapComponent = new MapComponent(this, map, GetMapRectangle);

      Components.Add(_moveComponent);
      Components.Add(_interactComponent);
      Components.Add(_animationComponent);
      //Components.Add(_mapComponent);
    }

    private Rectangle GetMapRectangle()
    {
      return new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        _animationComponent.Width,
        _animationComponent.Height
      );
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

    public bool CanInteract(Entity entity)
    {
      var interactedComponent = entity.Components.GetComponent<InteractComponent>();

      if (interactedComponent == null)
        return false;

      var playerRectangle = _interactComponent.GetRectangle();
      var entityRectangle = interactedComponent.GetRectangle();

      switch (Direction)
      {
        case Directions.Up:
          var rectangleUp = new Rectangle(playerRectangle.X, playerRectangle.Y - Map.TileHeight, playerRectangle.Width, playerRectangle.Height);

          if (entityRectangle == rectangleUp)
            return true;

          break;
        case Directions.Down:
          var rectangleDown = new Rectangle(playerRectangle.X, playerRectangle.Y + Map.TileHeight, playerRectangle.Width, playerRectangle.Height);

          if (entityRectangle == rectangleDown)
            return true;

          break;
        case Directions.Left:
          var rectangleLeft = new Rectangle(playerRectangle.X - Map.TileWidth, playerRectangle.Y, playerRectangle.Width, playerRectangle.Height);

          if (entityRectangle == rectangleLeft)
            return true;

          break;
        case Directions.Right:
          var rectangleRight = new Rectangle(playerRectangle.X + Map.TileWidth, playerRectangle.Y, playerRectangle.Width, playerRectangle.Height);

          if (entityRectangle == rectangleRight)
            return true;

          break;
      }

      return false;
    }
  }
}
