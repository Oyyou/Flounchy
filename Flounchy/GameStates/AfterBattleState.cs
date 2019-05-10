using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.GUI.Components;
using Flounchy.Sprites;
using Flounchy.Sprites.AfterBattle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GameStates
{
  public class AfterBattleState : BaseState
  {
    private Button _button;

    private List<StatsPanel> _statsPanel;

    private Sprite _title;

    private float _timer;

    public bool Continue = false;

    public AfterBattleState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
      _players = players;

      var titleTexture = _content.Load<Texture2D>("Text/Victory");

      _title = new Sprite(titleTexture)
      {
        Position = new Vector2(_gameModel.ScreenWidth / 2, 100),
      };

      var panelWidth = 180;
      var positions = new List<Vector2>()
      {
        new Vector2(((_gameModel.ScreenWidth / 2) - ((panelWidth + 50) * 2)), 200),
        new Vector2(((_gameModel.ScreenWidth / 2) - (panelWidth + 50)), 200),
        new Vector2(((_gameModel.ScreenWidth / 2) + (50)), 200),
        new Vector2(((_gameModel.ScreenWidth / 2) + ((panelWidth + 100))), 200),
      };

      int i = 0;
      _statsPanel = _players.Select(c => new StatsPanel(gameModel.ContentManger, c, positions[i++])).ToList();

      var buttonTexture = _content.Load<Texture2D>("Buttons/Button");
      var buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      _button = new Button(buttonTexture, buttonFont)
      {
        Click = () => Continue = true,
        Position = new Vector2((_gameModel.ScreenWidth - buttonTexture.Width) - 20, (_gameModel.ScreenHeight - buttonTexture.Height) - 20),
        Text = "Continue",
        PenColour = Color.Black,
      };
    }

    public override void LoadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

      for (int i = 0; i < _statsPanel.Count; i++)
      {
        if (_timer > i * 250)
          _statsPanel[i].FadeIn = true;

        _statsPanel[i].Update(gameTime);
      }

      _button.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _title.Draw(gameTime, _spriteBatch);

      foreach (var sPanel in _statsPanel)
        sPanel.Draw(gameTime, _spriteBatch);

      _button.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
