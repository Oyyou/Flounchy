using Engine.Models;
using Flounchy.Misc;
using Flounchy.Sprites.Roaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Managers
{
  public class FogItem
  {
    public enum Visibilities
    {
      Unseen,
      Seen,
      See,
    }

    public Rectangle Rectangle;

    public float Layer;

    public Color Colour;

    public Texture2D Texture;

    public Visibilities Visibility;

    public float Opacity
    {
      get
      {
        switch (Visibility)
        {
          case Visibilities.See:
            return 0f;

          case Visibilities.Seen:
            return 0.6f;

          case Visibilities.Unseen:
            return 1.0f;

          default:
            throw new Exception("Unknown Visibility: " + Visibility.ToString());
        }
      }
    }

    public FogItem(Texture2D texture)
    {
      Texture = texture;
    }
  }

  public class FogManager
  {
    private Texture2D _fogTexture;

    private GameModel _gameModel;

    public List<FogItem> FogItems { get; private set; }

    public FogManager(Texture2D fogTexture, GameModel gameModel)
    {
      _fogTexture = fogTexture;

      _gameModel = gameModel;

      FogItems = new List<FogItem>();
    }

    public void AddItem(MapSprite mapSprite)
    {
      var items = GetSplitItem(mapSprite);

      foreach (var item in items)
      {
        if (item.Rectangle.X < 0 || item.Rectangle.X >= _gameModel.ScreenWidth ||
            item.Rectangle.Y < 0 || item.Rectangle.Y >= _gameModel.ScreenHeight)
          continue;

        bool canAdd = true;
        for (int i = 0; i < FogItems.Count; i++)
        {

          var fItem = FogItems[i];

          if (item.Rectangle.X == fItem.Rectangle.X &&
              item.Rectangle.Y == fItem.Rectangle.Y)
          {
            if (item.Layer > fItem.Layer)
            {
              FogItems.RemoveAt(i);
              i--;
            }
            else
            {
              canAdd = false;
            }
          }
        }

        if (canAdd)
        {
          FogItems.Add(item);
        }
      }
    }

    private IEnumerable<FogItem> GetSplitItem(MapSprite mapSprite)
    {
      var yCount = mapSprite.Rectangle.Height / Map.TileHeight;
      var xCount = mapSprite.Rectangle.Width / Map.TileWidth;

      for (int y = 0; y < yCount; y++)
      {
        for (int x = 0; x < xCount; x++)
        {
          var xPos = mapSprite.Rectangle.X + (x * Map.TileWidth);
          var yPos = mapSprite.Rectangle.Y + (y * Map.TileHeight);

          yield return new FogItem(_fogTexture)
          {
            Rectangle = new Rectangle(xPos, yPos, Map.TileWidth, Map.TileHeight),
            Colour = mapSprite.MapColour,
            Layer = mapSprite.Layer,
          };
        }
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach(var item in FogItems)
      {
        spriteBatch.Draw(item.Texture, item.Rectangle, null, Color.White * item.Opacity, 0f, new Vector2(0, 0), SpriteEffects.None, 0.9f);
      }
    }
  }
}
