using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace Flounchy.Misc
{
  public class Map
  {
    public int[,] _map = new int[0, 0];

    public static int TileWidth = 40;

    public static int TileHeight = 40;

    public void AddItem(Rectangle rectangle)
    {
      ValidXY(rectangle, out int x, out int y, out int width, out int height);

      for(int newY = y; newY < (y + height); newY++)
      {
        for (int newX = x; newX < (x + width); newX++)
        {
          _map[newY, newX] = 1;
        }
      }
    }

    private int[,] GetNewMap(int width, int height)
    {
      var newWidth = Math.Max(width, _map.GetWidth());
      var newHeight = Math.Max(height, _map.GetHeight());

      var newMap = new int[newWidth + 1, newHeight + 1];

      for (int y = 0; y < _map.GetHeight(); y++)
      {
        for (int x = 0; x < _map.GetWidth(); x++)
        {
          newMap[y, x] = _map[y, x];
        }
      }

      return newMap;
    }

    public int GetValue(Rectangle rectangle)
    {
      ValidXY(rectangle, out int x, out int y, out int width, out int height);

      if (x < 0 || y < 0)
        return 1;

      return _map[y, x];
    }

    private void ValidXY(Rectangle rectangle, out int spriteX, out int spriteY, out int spriteWidth, out int spriteHeight)
    {
      spriteX = rectangle.X / TileWidth;
      spriteY = rectangle.Y / TileHeight;

      spriteWidth = rectangle.Width / TileWidth;
      spriteHeight = rectangle.Height / TileHeight;

      var width = _map.GetWidth();
      var height = _map.GetHeight();

      for (int y = spriteY; y < (spriteY + spriteHeight); y++)
      {
        for (int x = spriteX; x < (spriteX + spriteWidth); x++)
        {
          _map = GetNewMap(x, y);
        }
      }
    }

    public void Write()
    {
      Console.Clear();

      for (int y = 0; y < _map.GetHeight(); y++)
      {
        for (int x = 0; x < _map.GetWidth(); x++)
        {
          Console.Write(_map[y, x]);
        }

        Console.WriteLine();
      }

      Console.WriteLine();
    }
  }
}

