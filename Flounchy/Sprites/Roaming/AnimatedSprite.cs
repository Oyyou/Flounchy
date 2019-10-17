using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Roaming
{
  public abstract class AnimatedSprite
  {
    private float _timer;

    private Texture2D _texture;

    public Vector2 Position;

    public int TotalXFrames;

    public int TotalYFrames;

    public float AnimationSpeed;

    public int CurrentXFrame;

    public int CurrentYFrame;

    public float Layer
    {
      get
      {
        return MathHelper.Clamp(Position.Y / 1000f, 0, 1);
      }
    }

    protected AnimatedSprite(Texture2D texture, Vector2 position, int totalXFrames, int totalYFrames, float animationSpeed)
    {
      _texture = texture;
      Position = position;
      TotalXFrames = totalXFrames;
      TotalYFrames = totalYFrames;
      AnimationSpeed = animationSpeed;
    }

    public int FrameWidth
    {
      get
      {
        return _texture.Width / TotalXFrames;
      }
    }

    public int FrameHeight
    {
      get
      {
        return _texture.Height / TotalYFrames;
      }
    }

    public Rectangle SourceRectangle
    {
      get
      {
        return new Rectangle(
          CurrentXFrame * FrameWidth,
          CurrentYFrame * FrameHeight,
          FrameWidth,
          FrameHeight);
      }
    }

    public virtual void Update(GameTime gameTime)
    {

    }

    protected void Play(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= AnimationSpeed)
      {
        _timer = 0;
        CurrentXFrame++;

        if (CurrentXFrame >= TotalXFrames)
          CurrentXFrame = 0;
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, SourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
    }
  }
}
