using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.GameStates
{
  public class OpeningState : BaseState
  {
    public enum States
    {
      Text,
      Flash,
      Fade,
    }

    private class Background
    {
      public float Timer;

      public Color Colour;

      public Background(float timer, Color colour)
      {
        Timer = timer;
        Colour = colour;
      }
    }

    private SpriteFont _font;

    private string _currentText = "";
    private List<string> _texts;
    private int _textIndex;
    private float _textOpacity = 1f;

    private float _timer;

    private List<Background> _backgrounds;
    private Color _finalBackground;

    public States State;

    public OpeningState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      _font = _content.Load<SpriteFont>("Fonts/Font");

      _texts = new List<string>()
      {
        "Fool!",
        "Wake up",
        "You're in danger!",
        "... and need pants",
      };

      var darkerWhite = new Color(150, 150, 150);
      _finalBackground = darkerWhite;

      _backgrounds = new List<Background>()
      {
        new Background(0.05f, darkerWhite),
        new Background(0.50f, Color.Black),
        new Background(0.05f, darkerWhite),
        new Background(0.50f, Color.Black),
        new Background(0.05f, darkerWhite),
        new Background(0.50f, Color.Black),

        new Background(0.025f, darkerWhite),
        new Background(0.20f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.20f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.20f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.20f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.20f, Color.Black),

        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
        new Background(0.025f, Color.Black),
        new Background(0.025f, darkerWhite),
      };

      State = States.Text;
    }

    public override void UnloadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {
      switch (State)
      {
        case States.Text:
          UpdateWakeUpText(gameTime);
          break;
        case States.Flash:

          _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

          if (_backgrounds.Count > 1)
          {
            if (_timer >= _backgrounds.FirstOrDefault().Timer)
            {
              _backgrounds.RemoveAt(0);
              _timer = 0;
            }
          }
          else
          {
            if (_finalBackground.R < 255)
            {
              byte incrementSpeed = 1;

              _finalBackground.R += incrementSpeed;
              _finalBackground.G += incrementSpeed;
              _finalBackground.B += incrementSpeed;
            }
            else
            {
              State = States.Fade;
            }
          }

          break;
        case States.Fade:
          break;
        default:
          break;
      }
    }

    private void UpdateWakeUpText(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_currentText == _texts.First())
      {


        if (GameKeyboard.IsKeyPressed(Keys.Space))
        {
          _textOpacity = 0;
        }

        if (_timer > 1f)
        {
          _textOpacity -= 0.01f;

          if (_textOpacity <= 0)
          {
            if (_texts.Count > 1)
            {
              _textOpacity = 1f;
              _texts.RemoveAt(0);
              _textIndex = 0;
              _currentText = "";
            }
          }
        }

        if (_textOpacity <= -1)
        {
          _timer = 0;
          State = States.Flash;
        }

        return;
      }

      if (GameKeyboard.IsKeyPressed(Keys.Space))
      {
        var text = _texts.First();

        _currentText = text;
        _textIndex = text.Length;
      }

      if (_timer >= 0.3)
      {
        _timer = 0.0f;

        var text = _texts.First();

        if (_textIndex < text.Length)
        {
          var value = text[_textIndex];

          _currentText += value;

          while (value == ' ')
          {
            _textIndex++;

            value = text[_textIndex];

            _currentText += value;
          }
        }

        _textIndex++;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      switch (State)
      {
        case States.Text:

          _graphics.GraphicsDevice.Clear(Color.Black);

          _spriteBatch.Begin();

          _spriteBatch.DrawString(_font, _currentText, new Vector2((_gameModel.ScreenWidth / 2) - (_font.MeasureString(_texts.First()).X / 2), 250), Color.White * _textOpacity);

          _spriteBatch.End();

          break;
        case States.Flash:


          if (_backgrounds.Count > 1)
          {
            _graphics.GraphicsDevice.Clear(_backgrounds.FirstOrDefault().Colour);
          }
          else
          {
            _graphics.GraphicsDevice.Clear(_finalBackground);
          }

          break;
        case States.Fade:

          _graphics.GraphicsDevice.Clear(_finalBackground);

          break;
        default:
          break;
      }
    }
  }
}
