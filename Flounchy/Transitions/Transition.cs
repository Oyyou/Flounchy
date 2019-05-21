using Engine.Models;
using Flounchy.GameStates;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Transitions
{
  public class Transition
  {
    public enum States
    {
      Waiting,
      FirstHalf,
      Middle,
      SecondHalf,
    }

    protected GameModel _gameModel;

    protected virtual List<Sprite> _sprites
    {
      get
      {
        return new List<Sprite>();
      }
    }

    public States State { get; set; }

    public Transition(GameModel gameModel)
    {
      _gameModel = gameModel;

      State = States.Waiting;
    }

    public virtual void Reset()
    {
      State = States.Waiting;
    }

    public void Start()
    {
      if (State != States.Waiting)
        return;

      State = States.FirstHalf;
    }

    public void Update(GameTime gameTime)
    {
      switch (State)
      {
        case States.Waiting:
          break;
        case States.FirstHalf:
          FirstHalf();
          break;
        case States.Middle:
          State = States.SecondHalf;
          break;
        case States.SecondHalf:
          SecondHalf();
          break;
        default:
          break;
      }
    }

    protected virtual void FirstHalf()
    {

    }

    protected virtual void SecondHalf()
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, spriteBatch);
    }
  }
}
