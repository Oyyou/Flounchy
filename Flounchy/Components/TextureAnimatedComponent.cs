using Flounchy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Components
{
  public class TextureAnimatedComponent : Component
  {
    private float _timer;

    protected Texture2D _texture;

    public int TotalXFrames;

    public int TotalYFrames;

    public float AnimationSpeed;

    public int CurrentXFrame;

    public int CurrentYFrame;

    public Func<float> GetLayer;

    public bool Playing;

    public Action<GameTime> SetAnimation { get; set; }

    public TextureAnimatedComponent(Entity parent, Texture2D texture, int totalXFrames, int totalYFrames, float animationSpeed) : base(parent)
    {
      _texture = texture;
      TotalXFrames = totalXFrames;
      TotalYFrames = totalYFrames;
      AnimationSpeed = animationSpeed;

      GetLayer = null;
      Playing = true;
    }

    public float Layer
    {
      get
      {
        return GetLayer != null ? GetLayer() : 0;
      }
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

    public override void Update(GameTime gameTime)
    {
      SetAnimation(gameTime);

      if(!Playing)
      {
        CurrentXFrame = 0;
        return;
      }

      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= AnimationSpeed)
      {
        _timer = 0;
        CurrentXFrame++;

        if (CurrentXFrame >= TotalXFrames)
          CurrentXFrame = 0;
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Parent.Position, SourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
    }
  }
}
