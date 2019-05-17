using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controls;
using Engine.Managers;
using Engine.Models;
using Flounchy.GUI.Controls.Windows;
using Microsoft.Xna.Framework;

namespace Flounchy.GUI.States
{
  public class HomeStateGUI : BaseStateGUI
  {
    private List<ActorModel> _actors;

    private WindowManager _windowManager;

    public HomeStateGUI(GameModel gameModel, List<ActorModel> actors) 
      : base(gameModel)
    {
      _actors = actors;

      _windowManager = new WindowManager(gameModel, 
        new List<Window>()
        {
          new TavernWindow(gameModel, _actors),
        }
      );

      _windowManager.OpenWindow("Tavern");
    }

    public override void Update(GameTime gameTime)
    {
      _windowManager.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _windowManager.Draw(gameTime);
    }
  }
}
