using Engine;
using Engine.Models;
using Flounchy.Managers;
using Flounchy.Misc;
using Flounchy.Sprites.Roaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flounchy.Areas
{
  public class Area2x1 : Area
  {
    public Area2x1(GameModel gameModel, int x, int y, Map map, Player player)
      : base(gameModel, x, y, map, player)
    {
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
      base.LoadContent(content, graphics);

      var bushTexture = content.Load<Texture2D>("Roaming/Bush");
      var building01Texture = content.Load<Texture2D>("Roaming/Buildings/Building_02");
      var treeTexture = content.Load<Texture2D>("Roaming/Tree");
      var grassTexture = content.Load<Texture2D>("Roaming/Tiles/Grass");

      var fogTexture = new Texture2D(graphics, Map.TileWidth, Map.TileHeight);
      Helpers.SetTexture(fogTexture, new Color(33, 33, 33));

      var buildingPosition = new Vector2(200, 160);

      MapSprites = new List<MapSprite>();

      for (int y = 0; y < _gameModel.ScreenHeight / Map.TileHeight; y++)
      {
        for (int x = 0; x < _gameModel.ScreenWidth / Map.TileWidth; x++)
        {
          MapSprites.Add(MapSpritesManager.GetGrass(new Vector2(x * Map.TileWidth, y * Map.TileHeight)));
        }
      }

      MapSprites.Add(new MapSprite(building01Texture, buildingPosition, Color.Blue)
      {
        CollisionRectangle = new Rectangle((int)buildingPosition.X, (int)buildingPosition.Y + (Map.TileHeight * 2), building01Texture.Width, building01Texture.Height - (Map.TileHeight * 2)),
      });
      MapSprites.Add(MapSpritesManager.GetTree(new Vector2(200, 360)));
      MapSprites.Add(MapSpritesManager.GetTree(new Vector2(360, 360)));

      var amount = _gameModel.ScreenWidth / treeTexture.Width;

      for (int y = -1; y < 3; y++)
      {
        for (int i = 0; i < amount; i++)
        {
          var position = new Vector2(i * treeTexture.Width, y * 40);

          MapSprites.Add(MapSpritesManager.GetTree(position));
        }
      }

      for (int y = 3; y < 10; y++)
      {
        var position = new Vector2(0, y * Map.TileHeight);

        MapSprites.Add(MapSpritesManager.GetTree(position));
      }

      var newX = _gameModel.ScreenWidth - treeTexture.Width;
      for (int y = 3; y < 16; y++)
      {
        var position = new Vector2(newX, y * Map.TileHeight);

        MapSprites.Add(MapSpritesManager.GetTree(position));
      }

      for (int x = 0; x < (_gameModel.ScreenWidth / treeTexture.Width); x++)
      {
        var position = new Vector2(x * treeTexture.Width, (_gameModel.ScreenHeight - treeTexture.Height));

        MapSprites.Add(MapSpritesManager.GetTree(position));
      }

      foreach (var sprite in MapSprites)
      {
        FogManager.AddItem(sprite);
      }

      NPCSprites.Add(new Animal(content.Load<Texture2D>("Roaming/Animals/Pig"), new Vector2(400, 400), 4, 4, 0.1f, _map));
      EnemySprites.Add(MapSpritesManager.GetEnemy(new Vector2(360, 440)));
    }
  }
}
