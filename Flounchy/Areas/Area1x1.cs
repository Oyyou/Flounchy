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
  public class Area1x1 : Area
  {
    public Area1x1(GameModel gameModel, int x, int y) 
      : base(gameModel, x, y)
    {
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
      base.LoadContent(content, graphics);

      var bushTexture = content.Load<Texture2D>("Roaming/Bush");
      var building01Texture = content.Load<Texture2D>("Roaming/Buildings/Building_02");
      var grassTexture = content.Load<Texture2D>("Roaming/Tiles/Grass");

      var fogTexture = new Texture2D(graphics, Map.TileWidth, Map.TileHeight);
      Helpers.SetTexture(fogTexture, new Color(33, 33, 33));

      var treeTexture = content.Load<Texture2D>("Roaming/Tree");
      var enemyTexture = content.Load<Texture2D>("Roaming/Enemy");
      var amount = _gameModel.ScreenWidth / treeTexture.Width;

      MapSprites = new List<MapSprite>();

      for (int y = 0; y < _gameModel.ScreenHeight / Map.TileHeight; y++)
      {
        for (int x = 0; x < _gameModel.ScreenWidth / Map.TileWidth; x++)
        {
          MapSprites.Add(new MapSprite(grassTexture, fogTexture, new Vector2(x * Map.TileWidth, y * Map.TileHeight), Color.LightGreen)
          {
            LayerOverride = 0.0f,
          });
        }
      }

      for (int y = 0; y < 3; y++)
      {
        for (int i = 0; i < amount; i++)
        {
          var position = new Vector2(i * treeTexture.Width, y * 40);

          MapSprites.Add(new MapSprite(treeTexture, fogTexture, position, Color.DarkGreen)
          {
            CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + (Map.TileHeight * 2), treeTexture.Width, treeTexture.Height - (Map.TileHeight * 2)),
          });
        }
      }

      var newX = _gameModel.ScreenWidth - treeTexture.Width;
      for (int y = 3; y < 10; y++)
      {
        var position = new Vector2(newX, y * 40);

        MapSprites.Add(new MapSprite(treeTexture, fogTexture, position, Color.DarkGreen)
        {
          CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + (Map.TileHeight * 2), treeTexture.Width, treeTexture.Height - (Map.TileHeight * 2)),
        });
      }

      for (int y = 3; y < 18; y++)
      {
        var position = new Vector2(0, y * 40);

        MapSprites.Add(new MapSprite(treeTexture, fogTexture, position, Color.DarkGreen)
        {
          CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + (Map.TileHeight * 2), treeTexture.Width, treeTexture.Height - (Map.TileHeight * 2)),
        });
      }

      for (int x = 0; x < (_gameModel.ScreenWidth / treeTexture.Width); x++)
      {
        var position = new Vector2(x * treeTexture.Width, (_gameModel.ScreenHeight - treeTexture.Height));

        MapSprites.Add(new MapSprite(treeTexture, fogTexture, position, Color.DarkGreen)
        {
          CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + (Map.TileHeight * 2), treeTexture.Width, treeTexture.Height - (Map.TileHeight * 2)),
        });
      }
    }
  }
}
