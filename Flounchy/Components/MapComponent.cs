using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flounchy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Components
{
  public class MapComponent : Component
  {
    public Rectangle MapRectangle;

    public MapComponent(Entity parent, Rectangle mapRectangle) 
      : base(parent)
    {
      MapRectangle = mapRectangle;
    }

    public override void Update(GameTime gameTime)
    {
      throw new NotImplementedException();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

    }
  }
}
