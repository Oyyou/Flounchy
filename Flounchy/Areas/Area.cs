using Flounchy.Sprites;
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
  public class Area
  {
    public enum Sides
    {
      Left,
      Right,
      Top,
      Bottom,
    }

    #region Touching areas
    public Area LeftArea { get; set; }

    public Area RightArea { get; set; }

    public Area TopArea { get; set; }

    public Area BottomArea { get; set; }
    #endregion

    public Sprite Background { get; protected set; }

    public List<Sprite> Sprites { get; protected set; }

    public Area()
    {

    }

    public void UnloadContent()
    {
      Sprites.Clear();
    }

    public void LoadContent(ContentManager content)
    {
      var bushTexture = content.Load<Texture2D>("Roaming/Bush");
      var building01Texture = content.Load<Texture2D>("Roaming/Buildings/Building_02");

      Background = new Sprite(content.Load<Texture2D>("Battle/Grasses/Grass"))
      {
        Origin = new Vector2(0, 0),
      };

      Sprites = new List<Sprite>()
      {
        new Sprite(bushTexture)
        {
          Position = new Vector2(60, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(100, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(140, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 100),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 140),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(140, 140),
        },
        new Sprite(building01Texture)
        {
          Position = new Vector2(200 + (building01Texture.Width/2), 120 + (building01Texture.Height / 2)),
        },
      };
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
