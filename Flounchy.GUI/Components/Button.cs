using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.Components
{
  public class Button
  {
    protected MouseState _currentMouse;

    protected MouseState _previousMouse;

    private Texture2D _texture;

    private SpriteFont _font;

    private Color _colour = Color.White;

    public Action Click;

    public bool IsClicked { get; set; }

    public bool IsHovering { get; set; }

    public bool IsSelected { get; set; }

    public Vector2 Position;

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
      }
    }

    public Vector2 Origin { get; set; }

    public string Text { get; set; }

    public Color PenColour { get; set; } = Color.White;

    public Button(Texture2D texture)
    {
      _texture = texture;

      Origin = new Vector2(0, 0);
    }

    public Button(Texture2D texture, SpriteFont font)
      : this(texture)
    {
      _font = font;
    }

    public void Update(GameTime gameTime)
    {
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

      IsHovering = false;
      IsClicked = false;
      _colour = Color.White;

      if (mouseRectangle.Intersects(Rectangle))
      {
        IsHovering = true;
        _colour = Color.Yellow;

        if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
        {
          IsClicked = true;
          OnClick();
        }
      }

      //if (IsHovering)
      //  OnHover();
      //else OffHover();
    }

    public virtual void OnClick()
    {
      Click?.Invoke();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_texture, Position, null, _colour, 0f, Origin, 1f, SpriteEffects.None, 0);

      DrawText(spriteBatch);
    }

    protected void DrawText(SpriteBatch spriteBatch)
    {
      if (string.IsNullOrEmpty(Text) || _font == null)
        return;

      float x = ((Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2)) - Origin.X;
      float y = ((Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2)) - Origin.Y;

      spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
