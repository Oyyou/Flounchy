using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites
{
  public class Sprite
  {
    protected Texture2D _texture;

    public Vector2 Position;

    public Color Colour = Color.White;

    public virtual Vector2 Origin
    {
      get
      {
        return new Vector2(_texture.Width / 2, _texture.Height / 2);
      }
    }

    public Sprite(Texture2D texture)
    {
      _texture = texture;
    }

    public Sprite(ContentManager content)
    {
      _texture = null;
    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (_texture != null)
        spriteBatch.Draw(_texture, Position, null, Colour, 0f, Origin, 1f, SpriteEffects.None, 0);
    }
  }
}
