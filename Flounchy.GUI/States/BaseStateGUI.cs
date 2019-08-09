using Engine.Controls;
using Engine.Models;
using Flounchy.GUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

    protected ContentManager _content
    {
      get
      {
        return _gameModel.ContentManger;
      }
    }

    protected SpriteBatch _spriteBatch
    {
      get
      {
        return _gameModel.SpriteBatch;
      }
    }

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
