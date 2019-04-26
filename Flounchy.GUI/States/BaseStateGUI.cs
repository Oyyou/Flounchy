using Engine.Models;
using Flounchy.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.States
{
  public abstract class BaseStateGUI
  {
    protected readonly GameModel _gameModel;

    public Dictionary<string, Button> Buttons { get; protected set; }

    public BaseStateGUI(GameModel gameModel)
    {
      _gameModel = gameModel;

      Buttons = new Dictionary<string, Button>();
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
  }
}
