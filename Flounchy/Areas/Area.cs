using Engine;
using Engine.Models;
using Flounchy.Misc;
using Flounchy.Sprites;
using Flounchy.Sprites.Roaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Areas
{
  public abstract class Area
  {
    public enum Sides
    {
      Left,
      Right,
      Top,
      Bottom,
    }

    protected GameModel _gameModel;

    #region Touching areas
    public Area LeftArea { get; private set; }

    public Area RightArea { get; private set; }

    public Area TopArea { get; private set; }

    public Area BottomArea { get; private set; }
    #endregion

    public Sprite Background { get; protected set; }

    public List<Sprite> Sprites { get; protected set; } = new List<Sprite>();

    public List<Fog> Fog { get; protected set; }

    public bool Loaded { get; private set; } = false;

    public readonly int X;

    public readonly int Y;

    public Area(GameModel gameModel, int x, int y)
    {
      _gameModel = gameModel;

      X = x;

      Y = y;
    }

    public virtual void UnloadContent()
    {
      Sprites.Clear();
    }

    public virtual void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
      Loaded = true;

      var width = _gameModel.ScreenWidth / Map.TileWidth;
      var height = _gameModel.ScreenHeight / Map.TileHeight;

      var fogTexture = new Texture2D(graphics, Map.TileWidth, Map.TileHeight);
      Helpers.SetTexture(fogTexture, new Color(33, 33, 33));

      Fog = new List<Fog>();

      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          Fog.Add(new Fog(fogTexture)
          {
            Position = new Vector2(x * fogTexture.Width, y * fogTexture.Height),
            Origin = new Vector2(0, 0),
            Seen = false,
          });
        }
      }
    }

    private Vector2 _lastPlayerPosition;

    public void Update(Sprites.Roaming.Player player)
    {
      if (_lastPlayerPosition == player.Position)
        return;

      _lastPlayerPosition = player.Position;

      foreach (var fogTile in Fog)
      {
        if (Vector2.Distance(fogTile.Position, _lastPlayerPosition) <= 160)
        {
          fogTile.Opacity = 0;
          fogTile.Seen = true;
        }
        else
        {
          if (fogTile.Seen)
            fogTile.Opacity = 0.6f;
          else
            fogTile.Opacity = 1;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="area1"></param>
    /// <param name="area2"></param>
    /// <param name="side">Which side 'area2' is on of 'area1'</param>
    public static void SetAreaSide(Area area1, Area area2, Sides side)
    {
      switch (side)
      {
        case Sides.Left:

          area1.LeftArea = area2;
          area2.RightArea = area1;

          break;
        case Sides.Right:

          area1.RightArea = area2;
          area2.LeftArea = area1;

          break;
        case Sides.Top:

          area1.TopArea = area2;
          area2.BottomArea = area1;

          break;
        case Sides.Bottom:

          area1.BottomArea = area2;
          area2.TopArea = area1;

          break;
        default:
          break;
      }
    }
  }
}
