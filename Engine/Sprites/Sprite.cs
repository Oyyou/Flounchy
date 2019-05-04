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
    protected float _opacity = 1f;

    protected Texture2D _texture;

    public Vector2 Position;

    public Color Colour = Color.White;

    public Rectangle? SourceRectangle = null;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), _texture.Width, _texture.Height);
      }
    }

    public Vector2 Origin { get; protected set; }

    public float Rotation { get; set; } = 0;

    public virtual float Opacity
    {
      get { return _opacity; }
      set
      {
        _opacity = value;
      }
    }

    public Sprite(Texture2D texture)
    {
      _texture = texture;

      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
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
        spriteBatch.Draw(_texture, Position, SourceRectangle, Colour * Opacity, Rotation, Origin, 1f, SpriteEffects.None, 0);
    }
  }
}
