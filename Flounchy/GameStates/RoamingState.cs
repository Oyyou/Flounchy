using Engine.Models;
using Flounchy.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GameStates
{
  public class RoamingState : BaseState
  {
    private Button _enterBattleButton;

    public bool EnterBattle = false;

    public RoamingState(GameModel gameModel)
      : base(gameModel)
    {
    }

    public override void LoadContent()
    {
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
