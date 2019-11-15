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
  public class TextureStaticComponent : Component
  {
    protected Texture2D _texture;

    public Func<float> GetLayer;

    public float Layer
    {
      get
      {
        return GetLayer != null ? GetLayer() : 0;
      }
    }

    public int Height
    {
      get
      {
        return _texture.Height;
      }
    }

    public int Width
    {
      get
      {
        return _texture.Width;
      }
    }

    public TextureStaticComponent(Entity parent, Texture2D texture) 
      : base(parent)
    {
      _texture = texture;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Parent.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
    }
  }
}
