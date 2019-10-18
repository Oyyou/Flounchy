using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.Sprites.Roaming
{
  public class Animal : AnimatedSprite, IMoveable
  {
    public enum Animations
    {
      WalkUp,
      WalkDown,
      WalkLeft,
      WalkRight,
    }

    #region IMoveable
    public int Speed
    {
      get
      {
        return 1;
      }
    }
    public Vector2 Velocity { get; set; }
    public int DistanceTravelled { get; set; }
    public Map Map { get; set; }
    public Rectangle StartRectangle { get; set; }
    public Rectangle CurrentRectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, FrameWidth, FrameHeight);
      }
    }
    public Rectangle EndRectangle { get; set; }
    public Map.CollisionResults CollisionResult { get; set; }
    public Action OnBattle { get; set; }
    public Action<GameTime> SetMovement { get; set; }
    #endregion

    public Animal(Texture2D texture, Vector2 position, int totalXFrames, int totalYFrames, float animationSpeed, Map map)
      : base(texture, position, totalXFrames, totalYFrames, animationSpeed)
    {
      Map = map;

      OnBattle = () => { };

      SetMovement = (gameTime) => SetMovementEvent(gameTime);
    }

    private float _movementTimer = 0f;
    private float _nextMovementTimer = 1f;
    private int _xDistance;
    private int _yDistance;
    private const int _maxDistance = 2;

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
          Velocity = new Vector2(Speed, 0);
          _yDistance++;
          break;
        case 1 when _yDistance > -_maxDistance:
          Velocity = new Vector2(-Speed, 0);
          _yDistance--;
          break;
        case 2 when _xDistance > -_maxDistance:
          Velocity = new Vector2(0, -Speed);
          _xDistance--;
          break;
        case 3 when _xDistance < _maxDistance:
          Velocity = new Vector2(0, Speed);
          _xDistance++;
          break;
      }
    }

    public override void Update(GameTime gameTime)
    {
      CollisionResult = Map.CollisionResults.None;

      InterfaceMethods.Move(gameTime, this);

      SetAnimation(gameTime);

      Position += Velocity;
    }

    private void SetAnimation(GameTime gameTime)
    {
      bool isIdle = false;

      if (Velocity.Y < 0)
        CurrentYFrame = (int)Animations.WalkUp;
      else if (Velocity.Y > 0)
        CurrentYFrame = (int)Animations.WalkDown;
      else if (Velocity.X < 0)
        CurrentYFrame = (int)Animations.WalkLeft;
      else if (Velocity.X > 0)
        CurrentYFrame = (int)Animations.WalkRight;
      else
        isIdle = true;

      if (!isIdle)
        Play(gameTime);
      else
        CurrentXFrame = 0;
    }
  }
}
