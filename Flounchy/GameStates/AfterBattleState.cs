using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.Sprites;
using Flounchy.Sprites.AfterBattle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GameStates
{
  public class AfterBattleState : BaseState
  {
    private List<ActorModel> _players;

    private List<StatsPanel> _statsPanel;

    private Sprite _title;

    public AfterBattleState(GameModel gameModel, List<ActorModel> players) 
      : base(gameModel)
    {
      _players = players;

      var titleTexture = _content.Load<Texture2D>("Text/Victory");

      _title = new Sprite(titleTexture)
      {
        Position = new Vector2(_gameModel.ScreenWidth / 2, 100),
      };

      var position = new Vector2(100, 200);
      _statsPanel = _players.Select(c =>
      {
        var value = new StatsPanel(gameModel.ContentManger, c, position);

        position += new Vector2(300, 0);

        return value;

      }).ToList();
    }

    public override void LoadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _title.Draw(gameTime, _spriteBatch);

      foreach (var sPanel in _statsPanel)
        sPanel.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
