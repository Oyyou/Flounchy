using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Roaming
{
  public class MapSprite
  {
    private Texture2D _texture;

    public Vector2 Position;

    public Color Colour;

    public float Layer
    {
      get
      {
        if (LayerOverride != null)
          return LayerOverride.Value;

        var yPosition = Position.Y;

        if (CollisionRectangle != null)
          yPosition = CollisionRectangle.Value.Y;

        return MathHelper.Clamp(yPosition / 1000f, 0, 1);
      }
    }

    public Rectangle? CollisionRectangle;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
      }
    }

    public Color MapColour;

    public float? LayerOverride;

    private MapSprite()
    {
      _texture = null;
      Position = Vector2.Zero;
      Colour = Color.White;

      CollisionRectangle = null;

      LayerOverride = null;
    }

    public MapSprite(Texture2D spriteTexture, Vector2 position, Color mapColour)
      : this()
    {
      _texture = spriteTexture;
      Position = position;
      MapColour = mapColour;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, null, Colour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
    }
  }
}
