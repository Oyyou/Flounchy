using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.AfterBattle
{
  public class StatsPanel
  {
    private readonly SpriteFont _font;

    private readonly ActorModel _player;

    public readonly Vector2 _position;

    private readonly Sprite _displayPicture;

    private float _opacity = 0f;

    public bool FadeIn = false;

    public StatsPanel(ContentManager content, ActorModel player, Vector2 position)
    {
      _font = content.Load<SpriteFont>("Fonts/ButtonFont");

      _player = player;

      _position = position;

      var displayPictureTexture = content.Load<Texture2D>("AfterBattle/DisplayPicture");

      _displayPicture = new Sprite(displayPictureTexture)
      {
        Position = position + new Vector2((displayPictureTexture.Width / 2) - 10, (displayPictureTexture.Height / 2) + 30),
        Opacity = 0f,
      };
    }

    public void Update(GameTime gameTime)
    {
      if (!FadeIn)
        return;

      float fadeSpeed = 0.025f;

      if (_displayPicture.Opacity <= 1)
        _displayPicture.Opacity += fadeSpeed;
      else _displayPicture.Opacity = 1f;

      if (_displayPicture.Opacity >= 0.5f)
      {
        _opacity += fadeSpeed;
      }

      if(_opacity >= 1f)
      {
        FadeIn = false;
        _opacity = 1f;
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      var newPosition = _position;

      spriteBatch.DrawString(_font, _player.Name, _position, Color.Black * _opacity);
      _displayPicture.Draw(gameTime, spriteBatch);
      newPosition += new Vector2(0, _displayPicture.Rectangle.Height + 50);

      var content = new Dictionary<string, string>()
      {
        { "Damage dealt:", _player.BattleStats.DamageDealt.ToString() },
        { "Damage received:", _player.BattleStats.DamageReceived.ToString() },
        { "Final blows:", _player.BattleStats.FinalBlows.ToString() },
        { "Experience earned:", _player.BattleStats.ExpEarned.ToString() },
      };

      foreach (var value in content)
      {
        spriteBatch.DrawString(_font, value.Key, newPosition, Color.Black * _opacity);
        spriteBatch.DrawString(_font, value.Value, newPosition + new Vector2(150, 0), Color.Black * _opacity);

        newPosition += new Vector2(0, 30);
      }
    }
  }
}
