using Flounchy.Components;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Entities.Roaming
{
  public class Building : Entity
  {
    protected TextureComponent _textureComponent;
    protected InteractComponent _interactComponent;
    protected MapComponent _mapComponent;

    public Building(Texture2D texture, Map map)
    {
      _textureComponent = new TextureComponent(this, texture)
      {
        GetLayer = () => MathHelper.Clamp((Y + 40) / 1000f, 0, 1),
      };

      _interactComponent = new InteractComponent(this, GetInteractRectangle)
      {
        OnInteract = () => Console.WriteLine("Enter me"),
      };

      _mapComponent = new MapComponent(this, map, GetMapRectangle);

      Components.Add(_interactComponent);
      Components.Add(_textureComponent);
      Components.Add(_mapComponent);
    }

    private Rectangle GetInteractRectangle()
    {
      return new Rectangle(
        (int)Position.X + (Map.TileWidth * 2),
        ((int)Position.Y + _textureComponent.Height) - Map.TileHeight,
        Map.TileWidth,
        Map.TileHeight);
    }

    private Rectangle GetMapRectangle()
    {
      return new Rectangle(
        (int)Position.X,
        (int)Position.Y + (Map.TileHeight * 2),
        _textureComponent.Width,
        _textureComponent.Height - (Map.TileHeight * 2)
      );
    }
  }
}
