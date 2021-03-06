﻿using Flounchy.Entities;
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
    /// What happens when the object is interacted with
    /// </summary>
    public Action OnInteract;

    /// <summary>
    /// The rectangle that needs to be touched to interact with
    /// </summary>
    public Func<Rectangle> GetRectangle;

    public InteractComponent(Entity parent, Func<Rectangle> getRectangle) 
      : base(parent)
    {
      GetRectangle = getRectangle;
    }

    public override void Update(GameTime gameTime)
    {
      //throw new NotImplementedException();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      //throw new NotImplementedException();
    }
  }
}
