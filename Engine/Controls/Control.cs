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

    public abstract void UnloadContent();

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
  }
}
