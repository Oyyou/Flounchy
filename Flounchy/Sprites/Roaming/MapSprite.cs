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
    public enum Visibilities
    {
      See,
      Seen,
      Unseen,
    }

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

    #region Fog Members

    private Texture2D _fogTexture;

    private float _fogOpacity
    {
      get
      {
        switch (Visibility)
        {
          case Visibilities.See:
            return 0f;

          case Visibilities.Seen:
            return 0.6f;

          case Visibilities.Unseen:
            return 1.0f;

          default:
            throw new Exception("Unknown Visibility: " + Visibility.ToString());
        }
      }
    }

    private Vector2 _fogScale;

    #endregion

    public Visibilities Visibility;

    private MapSprite()
    {
      _texture = null;
      Position = Vector2.Zero;
      Colour = Color.White;

      CollisionRectangle = null;

      Visibility = Visibilities.Unseen;

      LayerOverride = null;
    }

    public MapSprite(Texture2D spriteTexture, Texture2D fogTexture, Vector2 position, Color mapColour)
      : this()
    {
      _texture = spriteTexture;
      _fogTexture = fogTexture;
      Position = position;
      MapColour = mapColour;

      _fogScale = new Vector2(spriteTexture.Width / fogTexture.Width, spriteTexture.Height / fogTexture.Height);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, null, Colour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer);
      spriteBatch.Draw(_fogTexture, Position, null, Colour * _fogOpacity, 0f, new Vector2(0, 0), _fogScale, SpriteEffects.None, Layer + 0.01f);
    }
  }
}
