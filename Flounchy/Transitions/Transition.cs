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

    private GameModel _gameModel;

    private Sprite _transition1;
    private Sprite _transition2;
    private Sprite _transition3;
    private Sprite _transition4;

    private List<Sprite> _sprites
    {
      get
      {
        return new List<Sprite>()
        {
          _transition1,
          _transition2,
          _transition3,
          _transition4,
        };
      }
    }

    public States State { get; set; }

    public Transition(GameModel gameModel)
    {
      _gameModel = gameModel;

      var transitionTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, _gameModel.ScreenWidth / 2,
        _gameModel.ScreenHeight / 2);

      var colours = new Color[transitionTexture.Width * transitionTexture.Height];

      var index = 0;
      for (int y = 0; y < transitionTexture.Height; y++)
      {
        for (int x = 0; x < transitionTexture.Width; x++)
        {
          var colour = Color.Gray;

          colours[index] = colour;

          index++;
        }
      }

      transitionTexture.SetData(colours);

      _transition1 = new Sprite(transitionTexture);
      _transition2 = new Sprite(transitionTexture);
      _transition3 = new Sprite(transitionTexture);
      _transition4 = new Sprite(transitionTexture);

      Reset();
    }

    public void Reset()
    {
      _transition1.Position = new Vector2(-(_transition1.Rectangle.Width / 2), (_transition1.Rectangle.Height / 2));
      _transition2.Position = new Vector2(-(_transition1.Rectangle.Width / 2), _gameModel.ScreenHeight - (_transition1.Rectangle.Height / 2));
      _transition3.Position = new Vector2(_gameModel.ScreenWidth + (_transition1.Rectangle.Width / 2), (_transition1.Rectangle.Height / 2));
      _transition4.Position = new Vector2(_gameModel.ScreenWidth + (_transition1.Rectangle.Width / 2), _gameModel.ScreenHeight - (_transition1.Rectangle.Height / 2));

      State = States.Waiting;
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
      float speed = 10f;

      _transition1.Position.X += speed;
      _transition2.Position.X += speed;
      _transition3.Position.X -= speed;
      _transition4.Position.X -= speed;

      if (_transition1.Position.X >= ((_gameModel.ScreenWidth / 2) - _transition1.Origin.X))
      {
        State = States.Middle;
      }
    }

    protected virtual void SecondHalf()
    {
      float speed = 10f;

      _transition1.Position.Y -= speed;
      _transition2.Position.Y += speed;
      _transition3.Position.Y -= speed;
      _transition4.Position.Y += speed;

      if (_transition1.Position.Y <= 0 - _transition1.Origin.Y)
      {
        State = States.Waiting;
        Reset();
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, spriteBatch);
    }

    public void Start()
    {
      if (State != States.Waiting)
        return;

      State = States.FirstHalf;
    }
  }
}
