using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.Sprites.Roaming
{
  public class Player : AnimatedSprite, IMoveable
  {
    public enum Animations
    {
      WalkUp,
      WalkDown,
      WalkLeft,
      WalkRight,
    }

    public Sprite Lower;

    public bool EnterBattle { get; set; }

    #region IMoveable
    public int Speed
    {
      get
      {
        return 4;
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
        return new Rectangle(
          (int)Position.X,
          (int)Position.Y + 40,
          40,
          40);
      }
    }

    public Rectangle EndRectangle { get; set; }
    public Map.CollisionResults CollisionResult { get; set; }
    public Action OnBattle { get; set; }
    public Action<GameTime> SetMovement { get; set; }
    #endregion

    public Player(Texture2D texture, Vector2 position, int totalXFrames, int totalYFrames, float animationSpeed, Map map)
      : base(texture, position, totalXFrames, totalYFrames, animationSpeed)
    {
      Map = map;

      //Lower = new Sprite(content.Load<Texture2D>("Clothing/Lower/Clover"));

      OnBattle = () => EnterBattle = true;

      SetMovement = (gameTime) => SetMovementEvent(gameTime);
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyDown(Keys.D))
      {
        Velocity = new Vector2(Speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.A))
      {
        Velocity = new Vector2(-Speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.W))
      {
        Velocity = new Vector2(0, -Speed);
      }
      else if (GameKeyboard.IsKeyDown(Keys.S))
      {
        Velocity = new Vector2(0, Speed);
      }
    }

    public bool IsInRange(Vector2 position)
    {
      return Vector2.Distance(position, this.Position) <= 160;
    }

    public override void Update(GameTime gameTime)
    {
      CollisionResult = Map.CollisionResults.None;
      EnterBattle = false;

      InterfaceMethods.Move(gameTime, this);

      SetAnimation(gameTime);

      Position += Velocity;

      //Lower.Position = new Vector2(
      //  Position.X + (_texture.Width / 2),
      //  (Position.Y + (_texture.Height) - 5));
    }

    protected override float GetLayer()
    {
      return MathHelper.Clamp((CurrentRectangle.Y) / 1000f, 0, 1);
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

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);

      //Lower.Draw(gameTime, spriteBatch);
    }
  }
}
