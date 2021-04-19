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
  public class TextureAnimatedComponent : TextureComponent
  {
    private float _timer;

    public int TotalXFrames;

    public int TotalYFrames;

    public float AnimationSpeed;

    public int CurrentXFrame;

    public int CurrentYFrame;

    public bool Playing;

    public Action<GameTime> SetAnimation { get; set; }

    public TextureAnimatedComponent(Entity parent, Texture2D texture, int totalXFrames, int totalYFrames, float animationSpeed) : base(parent, texture)
    {
      TotalXFrames = totalXFrames;
      TotalYFrames = totalYFrames;
      AnimationSpeed = animationSpeed;

      GetLayer = null;
      Playing = true;
    }

    public override int Width
    {
      get
      {
        return _texture.Width / TotalXFrames;
      }
    }

    public override int Height
    {
      get
      {
        return _texture.Height / TotalYFrames;
      }
    }

    public override Rectangle SourceRectangle
    {
      get
      {
        return new Rectangle(
          CurrentXFrame * Width,
          CurrentYFrame * Height,
          Width,
          Height);
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      SetAnimation(gameTime);

      if (!Playing)
      {
        CurrentXFrame = 0;
      }
      else
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

      base.Draw(gameTime, spriteBatch);
    }
  }
}
