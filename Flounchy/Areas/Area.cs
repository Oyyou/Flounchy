using Engine;
using Engine.Models;
using Flounchy.Components;
using Flounchy.Entities;
using Flounchy.Managers;
using Flounchy.Misc;
using Flounchy.Sprites.Roaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

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

    protected readonly Map _map;

    protected Entity _player;

    #region Touching areas
    public Area LeftArea { get; private set; }

    public Area RightArea { get; private set; }

    public Area TopArea { get; private set; }

    public Area BottomArea { get; private set; }
    #endregion

    public List<MapSprite> MapSprites { get; protected set; } = new List<MapSprite>();

    public List<Entity> NPCSprites { get; protected set; } = new List<Entity>();

    public List<Entity> Somethings { get; protected set; } = new List<Entity>();

    public List<MapSprite> EnemySprites { get; protected set; } = new List<MapSprite>();

    public bool Loaded { get; private set; } = false;

    public readonly int X;

    public readonly int Y;

    public FogManager FogManager { get; protected set; }

    public MapSpritesManager MapSpritesManager { get; protected set; }

    public IEnumerable<Entity> Interactables
    {
      get
      {
        return NPCSprites.Where(c => c.Components.GetComponent<InteractComponent>() != null);
      }
    }

    public Area(GameModel gameModel, int x, int y, Map map, Entity player)
    {
      _gameModel = gameModel;
      X = x;
      Y = y;
      _map = map;
      _player = player;
    }

    public virtual void UnloadContent()
    {
      MapSprites.Clear();
    }

    public virtual void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
      Loaded = true;

      var fogTexture = new Texture2D(graphics, Map.TileWidth, Map.TileHeight);
      Helpers.SetTexture(fogTexture, new Color(33, 33, 33));

      FogManager = new FogManager(fogTexture, _gameModel);
      MapSpritesManager = new MapSpritesManager(_gameModel.ContentManger);
    }

    private Vector2 _lastPlayerPosition;

    public void Update(GameTime gameTime)
    {
      var expected = _player.Position;// + new Vector2(20, 20);

      foreach (var sprite in Somethings)
        sprite.Update(gameTime);

      foreach (var sprite in NPCSprites)
        sprite.Update(gameTime);

      if (_lastPlayerPosition == expected)
        return;

      _lastPlayerPosition = expected;

      foreach (var sprite in FogManager.FogItems)
      {
        var fogCentre = new Vector2((sprite.Rectangle.X + (sprite.Rectangle.Width / 2)),
                                    (sprite.Rectangle.Y + (sprite.Rectangle.Height / 2)));

        if (IsInRange(_player.Position, fogCentre))
        {
          sprite.Visibility = FogItem.Visibilities.See;
        }
        else
        {
          if (sprite.Visibility != FogItem.Visibilities.Unseen)
            sprite.Visibility = FogItem.Visibilities.Seen;
        }
      }
    }

    private bool IsInRange(Vector2 pos1, Vector2 pos2)
    {
      return Vector2.Distance(pos1, pos2) <= 160;
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

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach (var sprite in MapSprites)
        sprite.Draw(gameTime, spriteBatch);

      foreach (var sprite in EnemySprites)
        sprite.Draw(gameTime, spriteBatch);

      FogManager.Draw(gameTime, spriteBatch);

      foreach (var sprite in Somethings)
      {
        sprite.Draw(gameTime, spriteBatch);
      }

      foreach (var sprite in NPCSprites)
      {
        if (IsInRange(_player.Position, sprite.Position + new Vector2(20, 20)))
          sprite.Draw(gameTime, spriteBatch);
      }
    }
  }
}
