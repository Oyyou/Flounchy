using Engine.Input;
using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Misc
{
  public class ChatBox
  {
    private GameModel _gameModel;

    private Texture2D _texture;

    private SpriteFont _font;

    private Vector2 _position;

    private string _currentText;

    private string _fullText;

    private float _timer;

    public bool IsFinished { get; set; }

    public ChatBox(GameModel gameModel, SpriteFont font)
    {
      _gameModel = gameModel;

      _texture = new Texture2D(gameModel.GraphicsDeviceManager.GraphicsDevice, gameModel.ScreenWidth, (gameModel.ScreenHeight / 5));

      _font = font;

      var colours = new Color[_texture.Width * _texture.Height];

      int index = 0;
      for (int y = 0; y < _texture.Height; y++)
      {
        for (int x = 0; x < _texture.Width; x++)
        {
          colours[index] = new Color(55, 55, 55);

          index++;
        }
      }

      _texture.SetData(colours);

      _position = new Vector2(0, gameModel.ScreenHeight - _texture.Height);

      _currentText = "";
      _fullText = "";
    }

    public void Write(string text)
    {
      IsFinished = false;
      _currentText = "";
      _fullText = text;
    }

    public void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_currentText == _fullText)
      {
        if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        {
          IsFinished = true;
        }
      }
      else
      {
        if (_timer >= 0.05f)
        {
          _timer = 0;

          var character = _fullText[_currentText.Length];

          do
          {
            _currentText += character;

            if (_currentText.Length < _fullText.Length)
              character = _fullText[_currentText.Length];

          } while (character == ' ');
        }

        if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        {
          _currentText = _fullText;
        }
      }
    }

    public void Draw(GameTime gameTime)
    {
      _gameModel.SpriteBatch.Draw(_texture, _position, Color.White);

      _gameModel.SpriteBatch.DrawString(_font, _currentText, _position + new Vector2(10, 10), Color.White);
    }
  }
}
