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

namespace Flounchy.Entities.Roaming
{
  public class Enemy : Entity
  {
    protected TextureComponent _bodyComponent;
    protected TextureComponent _tailComponent;
    protected InteractComponent _interactComponent;
    protected MoveComponent _moveComponent;
    protected MapComponent _mapComponent;

    #region Movement
    private float _movementTimer = 0f;
    private float _nextMovementTimer = 1f;
    private int _xDistance;
    private int _yDistance;
    private const int _maxDistance = 2;
    #endregion

    public Directions Direction;

    public Enemy(Texture2D body, Texture2D tail, Map map)
      : base()
    {
      _bodyComponent = new TextureComponent(this, body)
      {
        GetLayer = () => GetBodyLayer(),
        PositionOffset = new Vector2(-((body.Width % Map.TileWidth) / 2), Map.TileHeight - (body.Height % Map.TileHeight))
      };

      _tailComponent = new TextureComponent(this, tail)
      {
        GetLayer = () => Direction == Directions.Up ? _bodyComponent.Layer + 0.001f : _bodyComponent.Layer - 0.001f,
        PositionOffset = new Vector2(0, body.Height - tail.Height),
      };

      _moveComponent = new MoveComponent(this, map, (gameTime) => SetMovementEvent(gameTime))
      {
        CurrentRectangleOffset = new Rectangle(0, 40, 0, 40),
        Speed = 1,
      };

      _mapComponent = new MapComponent(this, map, GetMapRectangle);

      _interactComponent = new InteractComponent(this, () => _mapComponent.MapRectangle)
      {
        OnInteract = () => OnInteractEvent(),
      };

      Components.Add(_moveComponent);
      Components.Add(_bodyComponent);
      Components.Add(_tailComponent);
      Components.Add(_mapComponent);
      Components.Add(_interactComponent);
    }

    private float GetBodyLayer()
    {
      return MathHelper.Clamp((_moveComponent.CurrentRectangle.Y + _bodyComponent.PositionOffset.Y) / 1000f, 0, 1);
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyPressed(Keys.Right))
      {
        Direction = Directions.Right;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.Left))
      {
        Direction = Directions.Left;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.Up))
      {
        Direction = Directions.Up;
      }
      else if (GameKeyboard.IsKeyPressed(Keys.Down))
      {
        Direction = Directions.Down;
      }
      else if (GameKeyboard.IsKeyDown(Keys.Right))
      {
        _moveComponent.Velocity = new Vector2(_moveComponent.Speed, 0);
        Direction = Directions.Right;
      }
      else if (GameKeyboard.IsKeyDown(Keys.Left))
      {
        _moveComponent.Velocity = new Vector2(-_moveComponent.Speed, 0);
        Direction = Directions.Left;
      }
      else if (GameKeyboard.IsKeyDown(Keys.Up))
      {
        _moveComponent.Velocity = new Vector2(0, -_moveComponent.Speed);
        Direction = Directions.Up;
      }
      else if (GameKeyboard.IsKeyDown(Keys.Down))
      {
        _moveComponent.Velocity = new Vector2(0, _moveComponent.Speed);
        Direction = Directions.Down;
      }
    }

    //private void SetMovementEvent(GameTime gameTime)
    //{
    //  _movementTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

    //  if (_movementTimer < _nextMovementTimer)
    //    return;

    //  _movementTimer = 0;
    //  _nextMovementTimer = Game1.Random.Next(2, 5);

    //  switch (Game1.Random.Next(0, 4))
    //  {
    //    case 0 when _yDistance < _maxDistance:
    //      _moveComponent.Velocity = new Vector2(_moveComponent.Speed, 0);
    //      _yDistance++;
    //      Direction = Directions.Right;
    //      break;
    //    case 1 when _yDistance > -_maxDistance:
    //      _moveComponent.Velocity = new Vector2(-_moveComponent.Speed, 0);
    //      _yDistance--;
    //      Direction = Directions.Left;
    //      break;
    //    case 2 when _xDistance > -_maxDistance:
    //      _moveComponent.Velocity = new Vector2(0, -_moveComponent.Speed);
    //      _xDistance--;
    //      Direction = Directions.Up;
    //      break;
    //    case 3 when _xDistance < _maxDistance:
    //      _moveComponent.Velocity = new Vector2(0, _moveComponent.Speed);
    //      _xDistance++;
    //      Direction = Directions.Down;
    //      break;
    //  }
    //}

    private void OnInteractEvent()
    {
      Console.WriteLine("Hey");
    }

    private Rectangle GetMapRectangle()
    {
      var offX = (_bodyComponent.Width % Map.TileWidth);
      var offY = _bodyComponent.Height % Map.TileHeight;

      return new Rectangle(
        (int)Position.X,
        (int)Position.Y + Map.TileHeight,
        _bodyComponent.Width - offX,
        _bodyComponent.Height - offY
      );
    }
  }
}
