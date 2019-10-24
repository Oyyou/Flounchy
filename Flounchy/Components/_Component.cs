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
  public abstract class Component
  {
    public int UpdateOrder;

    public int DrawOrder;

    public readonly Entity Parent;

    public Component(Entity parent)
    {
      Parent = parent;
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
  }
}
