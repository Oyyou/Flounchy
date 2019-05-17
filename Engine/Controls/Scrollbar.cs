using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Controls
{
  public class Scrollbar
  {
    private int _currentScrollValue;

    private bool _isSelected = false;

    private Texture2D _inner;

    private Rectangle _innerRectangle
    {
      get
      {
        // TODO: Figure out how to set the height dynamically based off content height
        return new Rectangle((int)Position.X + _padding, _innerY, _inner.Width, 30);
      }
    }

    public int _innerY;

    private Texture2D _outer;

    private const int _padding = 3;

    private int _previousScrollValue;

    public int MinContentHeight { get; set; }

    public Rectangle ScrollArea { get; set; }

    public float Layer { get; set; }

    public int Width
    {
      get
      {
        return _outer.Width;
      }
    }

    /// <summary>
    /// The position of the scrollbar
    /// </summary>
    public Vector2 Position;

    public Scrollbar(Texture2D outer, Texture2D inner)
    {
      _outer = outer;

      _inner = inner;
    }

    public void Update(GameTime gameTime)
    {
      _previousScrollValue = _currentScrollValue;
      _currentScrollValue = GameMouse.ScrollWheelValue;

      if (GameMouse.Intersects(_innerRectangle) && GameMouse.IsLeftPressed)
      {
        _isSelected = true;
      }

      if (GameMouse.IsLeftReleased)
      {
        _isSelected = false;
      }

      if (_isSelected)
      {
        _innerY = GameMouse.Y - (_innerRectangle.Height / 2);
      }

      if (_previousScrollValue != _currentScrollValue && GameMouse.Intersects(ScrollArea))
      {
        _innerY += (_previousScrollValue - _currentScrollValue) / 10;
      }

      _innerY = MathHelper.Clamp(_innerY, (int)Position.Y + _padding, ((int)Position.Y + _outer.Height - _padding) - _innerRectangle.Height);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(texture: _outer, position: Position, color: Color.White, layerDepth: Layer);

      spriteBatch.Draw(texture: _inner, destinationRectangle: _innerRectangle, color: Color.White, layerDepth: Layer + 0.01f);
    }
  }
}
