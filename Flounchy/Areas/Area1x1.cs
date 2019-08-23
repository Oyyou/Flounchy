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
    public Area1x1(GameModel gameModel)
      : base(gameModel)
    {

    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
      base.LoadContent(content, graphics);

      var bushTexture = content.Load<Texture2D>("Roaming/Bush");
      var building01Texture = content.Load<Texture2D>("Roaming/Buildings/Building_02");

      Background = new Sprite(content.Load<Texture2D>("Battle/Grasses/Grass"))
      {
        Origin = new Vector2(0, 0),
      };

      var treeTexture = content.Load<Texture2D>("Roaming/Tree");
      var enemyTexture = content.Load<Texture2D>("Roaming/Enemy");
      var amount = _gameModel.ScreenWidth / treeTexture.Width;

      Sprites = new List<Sprite>()
      {
        new Tree(treeTexture)
        {
          Position = new Vector2(200, 200),
        },
        new Sprites.Roaming.Enemy(enemyTexture)
        {
          Position = new Vector2(240, 160),
        },
      };

      for (int y = 0; y < 3; y++)
      {
        for (int i = 0; i < amount; i++)
        {
          var position = new Vector2(i * treeTexture.Width, y * 40);

          Sprites.Add(new Tree(treeTexture)
          {
            Position = position,
          });
        }
      }

      var newX = _gameModel.ScreenWidth - treeTexture.Width;
      for (int y = 3; y < 10; y++)
      {
        var position = new Vector2(newX, y * 40);
        Sprites.Add(new Tree(treeTexture)
        {
          Position = position,
        });
      }

      for (int y = 3; y < 18; y++)
      {
        var position = new Vector2(0, y * 40);
        Sprites.Add(new Tree(treeTexture)
        {
          Position = position,
        });
      }

      for (int x = 0; x < (_gameModel.ScreenWidth / treeTexture.Width); x++)
      {
        var position = new Vector2(x * treeTexture.Width, _gameModel.ScreenHeight);

        Sprites.Add(new Tree(treeTexture)
        {
          Position = position,
        });
      }
    }
  }
}
