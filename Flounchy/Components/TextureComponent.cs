using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Flounchy.Entities;

namespace Flounchy.Components
{
  public class TextureComponent : Component
  {
    protected Texture2D _texture;

    public Func<float> GetLayer;

    public virtual int Width
    {
      get
      {
        return _texture.Width;
      }
    }

    public virtual int Height
    {
      get
      {
        return _texture.Height;
      }
    }

    public virtual Rectangle SourceRectangle
    {
      get
      {
        return new Rectangle(
          0,
          0,
          Width,
          Height);
      }
    }

    public TextureComponent(Entity parent, Texture2D texture) : base(parent)
    {
      _texture = texture;
    }

    public float Layer
    {
      get
      {
        return GetLayer != null ? GetLayer() : 0;
      }
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Parent.Position, SourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
    }
  }
}
