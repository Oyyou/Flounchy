using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
  public static class Helpers
  {
    public static Color[] GetBorder(Texture2D texture, int thickness = 1, Color? colour = null)
    {
      thickness = Math.Max(1, thickness);

      var colours = new Color[texture.Width * texture.Height];

      var index = 0;
      for (int y = 0; y < texture.Height; y++)
      {
        for (int x = 0; x < texture.Width; x++)
        {
          var newColour = new Color(0, 0, 0, 0);

          if (x < thickness || x > (texture.Width - 1) - thickness ||
              y < thickness || y > (texture.Height - 1) - thickness)
          {
            newColour = colour != null ? colour.Value : new Color(255, 255, 0, 10);
          }

          colours[index] = newColour;

          index++;
        }
      }

      return colours;
    }

    public static void SetTexture(Texture2D texture, Color backgroundColour, Color borderColour, int borderWidth = 2)
    {
      var colours = new Color[texture.Width * texture.Height];

      int index = 0;

      for (int y = 0; y < texture.Height; y++)
      {
        for (int x = 0; x < texture.Width; x++)
        {
          var colour = backgroundColour;

          if (x < borderWidth || x > (texture.Width - 1) - borderWidth ||
             y < borderWidth || y > (texture.Height - 1) - borderWidth)
            colour = borderColour;

          colours[index++] = colour;
        }
      }

      texture.SetData(colours);
    }
  }
}