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
  public class FourCornersTransition : Transition
  {
    private Sprite _transition1;
    private Sprite _transition2;
    private Sprite _transition3;
    private Sprite _transition4;

    protected override List<Sprite> _sprites
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

    public FourCornersTransition(GameModel gameModel)
      : base(gameModel)
    {
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

    public override void Reset()
    {
      base.Reset();

      _transition1.Position = new Vector2(-(_transition1.Rectangle.Width / 2), (_transition1.Rectangle.Height / 2));
      _transition2.Position = new Vector2(-(_transition1.Rectangle.Width / 2), _gameModel.ScreenHeight - (_transition1.Rectangle.Height / 2));
      _transition3.Position = new Vector2(_gameModel.ScreenWidth + (_transition1.Rectangle.Width / 2), (_transition1.Rectangle.Height / 2));
      _transition4.Position = new Vector2(_gameModel.ScreenWidth + (_transition1.Rectangle.Width / 2), _gameModel.ScreenHeight - (_transition1.Rectangle.Height / 2));
    }

    protected override void FirstHalf()
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

    protected override void SecondHalf()
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
  }
}
