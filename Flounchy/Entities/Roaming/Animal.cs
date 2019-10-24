using Flounchy.Components;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Entities.Roaming
{
  public class Animal : Entity
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

    private float _movementTimer = 0f;
    private float _nextMovementTimer = 1f;
    private int _xDistance;
    private int _yDistance;
    private const int _maxDistance = 2;

    public Directions Direction;

    public Animal(Texture2D texture, Map map)
      : base()
    {
      _animationComponent = new TextureAnimatedComponent(this, texture, 4, 4, 0.3f)
      {
        SetAnimation = (gameTime) => SetAnimationEvent(gameTime),
        GetLayer = () => MathHelper.Clamp((_moveComponent.CurrentRectangle.Y) / 1000f, 0, 1),
      };

      _interactComponent = new InteractComponent(this)
      {
        OnInteract = () => OnInteractEvent(),
      };

      _moveComponent = new MoveComponent(this, map, (gameTime) => SetMovementEvent(gameTime))
      {
        Speed = 1,
      };

      Components.Add(_moveComponent);
      Components.Add(_interactComponent);
      Components.Add(_animationComponent);
    }

    private void OnInteractEvent()
    {
      throw new NotImplementedException();
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      _movementTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_movementTimer < _nextMovementTimer)
        return;

      _movementTimer = 0;
      _nextMovementTimer = Game1.Random.Next(2, 5);

      switch (Game1.Random.Next(0, 4))
      {
        case 0 when _yDistance < _maxDistance:
          _moveComponent.Velocity = new Vector2(_moveComponent.Speed, 0);
          _yDistance++;
          Direction = Directions.Right;
          break;
        case 1 when _yDistance > -_maxDistance:
          _moveComponent.Velocity = new Vector2(-_moveComponent.Speed, 0);
          _yDistance--;
          Direction = Directions.Left;
          break;
        case 2 when _xDistance > -_maxDistance:
          _moveComponent.Velocity = new Vector2(0, -_moveComponent.Speed);
          _xDistance--;
          Direction = Directions.Up;
          break;
        case 3 when _xDistance < _maxDistance:
          _moveComponent.Velocity = new Vector2(0, _moveComponent.Speed);
          _xDistance++;
          Direction = Directions.Down;
          break;
      }
    }

    private void SetAnimationEvent(GameTime gameTime)
    {
      _animationComponent.CurrentYFrame = (int)Direction;

      _animationComponent.Playing = _moveComponent.Velocity != Vector2.Zero;
    }
  }
}
