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

    private readonly TextureStaticComponent _textureComponent;
    private readonly InteractComponent _interactComponent;

    public Building(Texture2D texture)
    {
      _textureComponent = new TextureStaticComponent(this, texture)
      {
        GetLayer = () => MathHelper.Clamp((Y + 40) / 1000f, 0, 1),
      };

      _interactComponent = new InteractComponent(this, GetInteractRectangle)
      {
        OnInteract = () => Console.WriteLine("Enter me"),
      };

      Components.Add(_interactComponent);
      Components.Add(_textureComponent);
    }

    private Rectangle GetInteractRectangle()
    {
      return new Rectangle(
        (int)Position.X + (Map.TileWidth * 2),
        ((int)Position.Y + _textureComponent.Height) - Map.TileHeight,
        Map.TileWidth,
        Map.TileHeight);
    }
  }
}
