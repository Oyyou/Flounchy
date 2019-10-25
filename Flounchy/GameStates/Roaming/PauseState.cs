using System;
using System.Collections.Generic;
using Engine.Controls;
using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GameStates.Roaming
{
  public class PauseState : BaseState
  {
    private Button _enterBattleButton;

    public bool EnterBattle;

    public PauseState(GameModel gameModel, List<ActorModel> players) 
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      EnterBattle = false;

      var buttonTexture = _content.Load<Texture2D>("Buttons/Button");
      var buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      _enterBattleButton = new Button(buttonTexture, buttonFont)
      {
        Click = () => EnterBattle = true,
        Position = new Vector2((_gameModel.ScreenWidth / 2) - (buttonTexture.Width / 2), 300),
        Text = "Enter Battle",
        PenColour = Color.Black,
      };
    }

    public override void UnloadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {
      _enterBattleButton.Update(gameTime);
    }
    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _enterBattleButton.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
