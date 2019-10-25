using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.Sprites;
using Flounchy.Sprites.Home;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GameStates
{
  public class HomeState : BaseState
  {
    public List<Building> _buildings;

    public HomeState(GameModel gameModel, List<ActorModel> players) 
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      _buildings = new List<Building>()
      {
        new Building(_content.Load<Texture2D>("Home/Buildings/Blacksmith"), _graphics.GraphicsDevice, _content)
        {
          Position = new Vector2(330, 440),
        },
        new Building(_content.Load<Texture2D>("Home/Buildings/Tavern"), _graphics.GraphicsDevice, _content)
        {
          Position = new Vector2(1000, 540),
        },
      };
    }

    public override void UnloadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {
      foreach (var sprite in _buildings)
        sprite.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

      foreach (var sprite in _buildings)
        sprite.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
