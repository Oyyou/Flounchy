using Flounchy.Misc;
using Flounchy.Sprites.Roaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Managers
{
  public class MapSpritesManager
  {
    private Texture2D _enemyTexture;

    private Texture2D _grassTexture;

    private Texture2D _treeTexture;

    public MapSpritesManager(ContentManager content)
    {
      _enemyTexture = content.Load<Texture2D>("Roaming/Enemy");
      _grassTexture = content.Load<Texture2D>("Roaming/Tiles/Grass");
      _treeTexture = content.Load<Texture2D>("Roaming/Tree");
    }

    public MapSprite GetEnemy(Vector2 position)
    {
      return new MapSprite(_enemyTexture, position, Color.DarkRed)
      {
        CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + Map.TileHeight, _enemyTexture.Width, _enemyTexture.Height - Map.TileHeight),
      };
    }

    public MapSprite GetGrass(Vector2 position)
    {
      return new MapSprite(_grassTexture, position, Color.LightGreen)
      {
        LayerOverride = 0.0f,
      };
    }

    public MapSprite GetTree(Vector2 position)
    {
      return new MapSprite(_treeTexture, position, Color.DarkGreen)
      {
        CollisionRectangle = new Rectangle((int)position.X, (int)position.Y + (Map.TileHeight * 2), _treeTexture.Width, _treeTexture.Height - (Map.TileHeight * 2)),
      };
    }

    public MapSprite GetFakeTree(Vector2 position)
    {
      var tree = GetTree(position);
      tree.MapColour.R += 100;
      tree.MapColour.G += 100;
      tree.MapColour.B += 100;
      tree.CollisionRectangle = null;

      return tree;
    }
  }
}
