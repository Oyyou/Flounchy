using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
  public static class Helpers
  {
    public static Color[] GetBorder(Texture2D texture, int thickness = 1, Color? colour = null)
    {
      return GetBorder(texture.Width, texture.Height, thickness, colour);
    }
    public static Color[] GetBorder(int width, int height, int thickness = 1, Color? colour = null)
    {
      thickness = Math.Max(1, thickness);

      var colours = new Color[width * height];

      var index = 0;
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          var newColour = new Color(0, 0, 0, 0);

          if (x < thickness || x > (width - 1) - thickness ||
              y < thickness || y > (height - 1) - thickness)
          {
            newColour = colour != null ? colour.Value : new Color(255, 255, 0, 10);
          }

          colours[index] = newColour;

          index++;
        }
      }

      return colours;
    }

    public static void SetTexture(Texture2D texture, Color backgroundColour, Color? borderColour = null, int borderWidth = 2)
    {
      var colours = new Color[texture.Width * texture.Height];

      int index = 0;

      for (int y = 0; y < texture.Height; y++)
      {
        for (int x = 0; x < texture.Width; x++)
        {
          var colour = backgroundColour;

          if (borderColour != null)
          {
            if (x < borderWidth || x > (texture.Width - 1) - borderWidth ||
               y < borderWidth || y > (texture.Height - 1) - borderWidth)
              colour = borderColour.Value;
          }

          colours[index++] = colour;
        }
      }

      texture.SetData(colours);
    }

    public static int GetWidth(this int[,] value)
    {
      return value.GetLength(1);
    }

    public static int GetHeight(this int[,] value)
    {
      return value.GetLength(0);
    }
  }
}
