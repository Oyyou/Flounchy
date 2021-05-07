using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Managers
{
  public class BorderManager
  {
    private GraphicsDevice _graphicsDevice;

    public BorderManager(GraphicsDevice graphicsDevice)
    {
      _graphicsDevice = graphicsDevice;
    }

    public Texture2D GenerateBoarder(int width, int height)
    {
      var texture = new Texture2D(_graphicsDevice, width, height);
      var colours = Helpers.GetBorder(texture);
      texture.SetData<Color>(colours);

      return texture;
    }
  }
}
