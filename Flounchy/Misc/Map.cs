﻿using Microsoft.Xna.Framework;
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
    public enum CollisionResults
    {
      None,
      Colliding,
      Battle,
      OffRight,
      OffLeft,
      OffTop,
      OffBottom,
    }

    private int _width;

    private int _height;

    public int[,] _map = new int[0, 0];

    public static int TileWidth = 40;

    public static int TileHeight = 40;

    public Map(int width, int height)
    {
      _width = width;
      _height = height;

      Clear();
    }

    public void AddItem(Rectangle rectangle, int value = 1)
    {
      ValidXY(rectangle, out int x, out int y, out int width, out int height);

      for (int newY = y; newY < (y + height); newY++)
      {
        for (int newX = x; newX < (x + width); newX++)
        {
          if (newX < 0 || newY < 0)
            continue;

          if (newX >= _map.GetLength(1) ||
              newY >= _map.GetLength(0))
            continue;

          _map[newY, newX] = value;
        }
      }
    }

    public void RemoveItem(Rectangle rectangle)
    {
      ValidXY(rectangle, out int x, out int y, out int width, out int height);

      for (int newY = y; newY < (y + height); newY++)
      {
        for (int newX = x; newX < (x + width); newX++)
        {
          if (newX < 0 || newY < 0)
            continue;

          if (newX >= _map.GetLength(1) ||
              newY >= _map.GetLength(0))
            continue;

          _map[newY, newX] = 0;
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

    public CollisionResults GetValue(Rectangle rectangle)
    {
      ValidXY(rectangle, out int x, out int y, out int width, out int height);

      if (x < 0)
        return CollisionResults.OffLeft;

      if (y < 0)
        return CollisionResults.OffTop;

      if (x > (_width - 1))
        return CollisionResults.OffRight;

      if (y > (_height - 1))
        return CollisionResults.OffBottom;

      if (_map[y, x] == 1)
        return CollisionResults.Colliding;

      if (_map[y, x] == 2)
        return CollisionResults.Battle;

      return CollisionResults.None;
    }

    public void Clear()
    {
      _map = new int[_height, _width];
    }

    private void ValidXY(Rectangle rectangle, out int spriteX, out int spriteY, out int spriteWidth, out int spriteHeight)
    {
      spriteX = rectangle.X / TileWidth;
      spriteY = rectangle.Y / TileHeight;

      spriteWidth = rectangle.Width / TileWidth;
      spriteHeight = rectangle.Height / TileHeight;
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

