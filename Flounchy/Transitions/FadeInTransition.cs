using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Transitions
{
  public class FadeInTransition : Transition
  {
    private Sprite _transition;

    protected override List<Sprite> _sprites
    {
      get
      {
        return new List<Sprite>()
        {
          _transition,
        };
      }
    }

    public FadeInTransition(GameModel gameModel)
      : base(gameModel)
    {
      var transitionTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, _gameModel.ScreenWidth, _gameModel.ScreenHeight);

      var colours = new Color[transitionTexture.Width * transitionTexture.Height];

      var index = 0;
      for (int y = 0; y < transitionTexture.Height; y++)
      {
        for (int x = 0; x < transitionTexture.Width; x++)
        {
          var colour = Color.White;

          colours[index] = colour;

          index++;
        }
      }

      transitionTexture.SetData(colours);

      _transition = new Sprite(transitionTexture)
      {
        Position = new Vector2(transitionTexture.Width / 2, transitionTexture.Height / 2),
      };

      Reset();
    }

    public override void Reset()
    {
      base.Reset();

      _transition.Opacity = 0f;
    }

    protected override void FirstHalf()
    {
      _transition.Opacity = 1f;
      State = States.Middle;
    }

    protected override void SecondHalf()
    {
      _transition.Opacity -= 0.005f;

      if (_transition.Opacity <= 0)
      {
        State = States.Waiting;
        Reset();
      }
    }
  }
}
