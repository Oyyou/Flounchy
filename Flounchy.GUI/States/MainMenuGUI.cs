using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.States
{
  public class MainMenuGUI : BaseStateGUI
  {
    public MainMenuGUI(GameModel gameModel) 
	  : base(gameModel)
    {
      Buttons.Add("Test", new Components.Button(_gameModel.ContentManger.Load<Texture2D>("Buttons/Button"))
      {
		Position = new Vector2(100, 100),
      });
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var button in Buttons)
        button.Value.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _gameModel.SpriteBatch.Begin();

      foreach (var button in Buttons)
        button.Value.Draw(gameTime, _gameModel.SpriteBatch);

      _gameModel.SpriteBatch.End();
    }
  }
}
