using Engine.Models;
using Flounchy.GUI.Components;
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
  public class BattleGUI : BaseStateGUI
  {
    private ActorModel _actorModel;

    private AbilitiesModel _abilities;

    private SpriteFont _buttonFont;

    private Sprite _heroIcon;

    private Sprite _panel;

    public Dictionary<string, Button> AbilityButtons;

    public BattleGUI(GameModel gameModel)
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
    }

    public void SetAbilities(ActorModel actor, AbilitiesModel abilities)
    {
      _actorModel = actor;

      _abilities = abilities;

      var abilityList = abilities.Get();

      var y = _gameModel.ScreenHeight - abilityList.FirstOrDefault().Icon.Height - 13;
      var x = 87;

      AbilityButtons = new Dictionary<string, Button>();

      for (int i = 0; i < abilityList.Count; i++)
      {
        var ability = abilityList[i];

        var key = ability.Text;

        if (!AbilityButtons.ContainsKey(key))
          AbilityButtons.Add(key, null);

        AbilityButtons[key] = new Components.Button(ability.Icon, _buttonFont)
        {
          Position = new Vector2(x, y),
          Text = key,
        };

        x += AbilityButtons[key].Rectangle.Width + 2;
      }
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var button in AbilityButtons)
        button.Value.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _panel.Draw(gameTime, _spriteBatch);

      _spriteBatch.DrawString(_buttonFont, _actorModel.Name, new Vector2(16, (_panel.Position.Y - _panel.Origin.Y) + 2), Color.White);

      _heroIcon.Draw(gameTime, _spriteBatch);

      foreach (var button in AbilityButtons)
      {
        button.Value.Draw(gameTime, _spriteBatch);
      }

      _spriteBatch.End();
    }
  }
}
