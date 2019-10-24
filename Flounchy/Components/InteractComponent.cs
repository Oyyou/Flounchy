using Flounchy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flounchy.Components
{
  public class InteractComponent : Component
  {
    /// <summary>
    /// The rectangle that needs to be touched to interact with
    /// </summary>
    public Rectangle Rectangle;

    /// <summary>
    /// What happens when the object is interacted with
    /// </summary>
    public Action OnInteract;

    public InteractComponent(Entity parent) 
      : base(parent)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      //throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
      //throw new NotImplementedException();
    }
  }
}
