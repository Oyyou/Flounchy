using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Controls
{
  public abstract class Control
  {
    public abstract Rectangle Rectangle { get; }

    public abstract Vector2 Position { get; set; }

    public bool IsClicked
    {
      get
      {
        return IsHovering && GameMouse.IsLeftClicked;
      }
    }

    public bool IsHovering
    {
      get
      {
        return GameMouse.Intersects(Rectangle);
      }
    }

    public abstract void UnloadContent();

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
  }
}
