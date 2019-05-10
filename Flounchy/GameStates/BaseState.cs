using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GameStates
{
  public abstract class BaseState
  {
    protected MouseState _currentMouse;
    protected MouseState _previousMouse;

    protected List<ActorModel> _players;

    #region Game Model
    protected readonly GameModel _gameModel;

    protected ContentManager _content
    {
      get
      {
        return _gameModel.ContentManger;
      }
    }

    protected GraphicsDeviceManager _graphics
    {
      get
      {
        return _gameModel.GraphicsDeviceManager;
      }
    }

    protected SpriteBatch _spriteBatch
    {
      get
      {
        return _gameModel.SpriteBatch;
      }
    }
    #endregion

    public BaseState(GameModel gameModel, List<ActorModel> players)
    {
      _gameModel = gameModel;

      _players = players;
    }

    public abstract void LoadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
  }
}
