using Engine.Controls;
using Engine.Models;
using Flounchy.GUI.Controls;
using Flounchy.GUI.Controls.Buttons;
using Flounchy.GUI.Controls.Windows;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.States
{
  public class BattleStateGUI : BaseStateGUI
  {
    private ActorModel _actorModel;

    private AbilitiesModel _abilities;

    private SpriteFont _buttonFont;

    private Sprite _heroIcon;

    private Sprite _panel;

    private TurnsWindow _turnsWindow;

    private ButtonGroup _abilityButtonGroup;

    public Dictionary<string, Button> AbilityButtons { get; private set; }

    public int SelectedActorId { get; set; }

    public BattleStateGUI(GameModel gameModel)
      : base(gameModel)
    {
      _buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      var panelTexture = _content.Load<Texture2D>("Battle/Panel");
      var heroIconTexture = _content.Load<Texture2D>("Battle/HeroIcon");

      _panel = new Sprite(panelTexture)
      {
        Position = new Vector2((panelTexture.Width / 2) + 10,
                               _gameModel.ScreenHeight - (panelTexture.Height / 2) - 10),
      };

      _heroIcon = new Sprite(heroIconTexture)
      {
        Position = new Vector2((heroIconTexture.Width / 2) + 14,
                               _gameModel.ScreenHeight - (heroIconTexture.Height / 2) - 14),
      };

      _turnsWindow = new TurnsWindow(_gameModel);
      _turnsWindow.SetPositions();
    }

    public void SetAbilities(ActorModel actor)
    {
      _actorModel = actor;

      _abilities = actor.Abilities;

      var abilityList = _abilities.Get();

      var iconTexture = _content.Load<Texture2D>(abilityList.FirstOrDefault().IconName);

      var y = _gameModel.ScreenHeight - iconTexture.Height - 13;
      var x = 87;

      AbilityButtons = new Dictionary<string, Button>();

      for (int i = 0; i < abilityList.Count; i++)
      {
        var ability = abilityList[i];

        var key = ability.Text;

        if (!AbilityButtons.ContainsKey(key))
          AbilityButtons.Add(key, null);

        AbilityButtons[key] = new AbilityButton(iconTexture, _buttonFont)
        {
          Position = new Vector2(x, y),
          Text = key,
        };

        x += AbilityButtons[key].Rectangle.Width + 2;
      }

      _abilityButtonGroup = new ButtonGroup(AbilityButtons.Select(c => c.Value).ToList());
    }

    public void SetTurns(List<ActorModel> actors)
    {
      _turnsWindow.SetItems(actors);
    }

    public override void Update(GameTime gameTime)
    {
      _abilityButtonGroup.Update(gameTime);

      _turnsWindow.Update(gameTime);
      SelectedActorId = _turnsWindow.SelectedActorId;
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _panel.Draw(gameTime, _spriteBatch);

      _spriteBatch.DrawString(_buttonFont, _actorModel.Name, new Vector2(16, (_panel.Position.Y - _panel.Origin.Y) + 2), Color.White);

      _heroIcon.Draw(gameTime, _spriteBatch);

      _abilityButtonGroup.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();

      _turnsWindow.Draw(gameTime, _spriteBatch, _gameModel.GraphicsDeviceManager);
    }
  }
}
