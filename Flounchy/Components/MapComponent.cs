using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flounchy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flounchy.Misc;

namespace Flounchy.Components
{
  public class MapComponent : Component
  {
    private Func<Rectangle> _getRectangle;

    private Rectangle _previousRectangle;

    public readonly Map Map;

    public Rectangle MapRectangle
    {
      get
      {
        return _getRectangle();
      }
    }

    public MapComponent(Entity parent, Map map, Func<Rectangle> getRectangle)
      : base(parent)
    {
      Map = map;
      _getRectangle = getRectangle;
    }

    public override void Update(GameTime gameTime)
    {
      if (_previousRectangle != MapRectangle)
      {
        Map.RemoveItem(_previousRectangle);
        Map.AddItem(MapRectangle);
      }

      _previousRectangle = MapRectangle;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

    }
  }
}
