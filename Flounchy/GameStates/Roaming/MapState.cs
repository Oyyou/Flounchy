using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Engine.Models;
using Flounchy.Areas;
using Flounchy.Misc;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GameStates.Roaming
{
  public class MapState : BaseState
  {
    private List<Sprite> _sprites = new List<Sprite>();

    public MapState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {

    }

    public void SetContent(List<Area> areas)
    {
      var texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
      texture.SetData(new Color[] { Color.White });

      foreach (var area in areas)
      {
        var x = area.X * (_gameModel.ScreenWidth / Map.TileWidth);
        var y = area.Y * (_gameModel.ScreenHeight / Map.TileHeight);

        foreach (var sprite in area.Sprites)
        {
          var spriteX = sprite.Rectangle.X / Map.TileWidth;
          var spriteY = sprite.Rectangle.Y / Map.TileHeight;
          var spriteWidth = sprite.Rectangle.Width / Map.TileWidth;
          var spriteHeight = sprite.Rectangle.Height / Map.TileHeight;

          _sprites.Add(new Sprite(texture)
          {
            Scale = new Vector2(spriteWidth, spriteHeight),
            Position = new Vector2(x + spriteX, y + spriteY),
            Origin = new Vector2(0, 0),
          });
        }
      }

    }

    public override void Update(GameTime gameTime)
    {
      _previousScroll = _currentScroll;
      _currentScroll = GameMouse.ScrollWheelValue;

      var speed = 0.15f;

      if (_previousScroll < _currentScroll)
        _scale *= 1f + speed;
      else if (_previousScroll > _currentScroll)
        _scale *= 1f - speed;

      if (GameKeyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
      {
        _scale *= 1.05f;
      }
      else if (GameKeyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
      {
        _scale *= 0.95f;
      }

      _scale = MathHelper.Clamp(_scale, 1f, 5f);

      _transform = Matrix.CreateTranslation(0, 0, 0) * //new Vector3((_gameModel.ScreenWidth / 2), (_gameModel.ScreenHeight / 2), 0)) *
        Matrix.CreateScale(_scale);
    }

    private Matrix _transform;
    private float _scale = 1f;
    private float _currentScroll;
    private float _previousScroll;

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin(transformMatrix: _transform);

      foreach (var sprite in _sprites)
      {
        sprite.Draw(gameTime, _spriteBatch);
      }

      _spriteBatch.End();
    }
  }
}
