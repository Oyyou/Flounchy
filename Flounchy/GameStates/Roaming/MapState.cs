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
using Flounchy.Sprites.Roaming;
using Flounchy.Managers;

namespace Flounchy.GameStates.Roaming
{
  public class MapState : BaseState
  {
    private List<Sprite> _sprites = new List<Sprite>();

    private Texture2D _mapTexture;

    public MapState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      _mapTexture = new Texture2D(_graphics.GraphicsDevice, _gameModel.ScreenWidth, _gameModel.ScreenHeight);

      var colours = new Color[_mapTexture.Width * _mapTexture.Height];

      _mapTexture.SetData(colours);
    }

    public void SetContent(List<Area> areas, Area currentArea, Sprites.Roaming.Player player)
    {
      var texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
      texture.SetData(new Color[] { Color.White });

      foreach (var area in areas)
      {
        var xCount = (_gameModel.ScreenWidth / Map.TileWidth);
        var yCount = (_gameModel.ScreenHeight / Map.TileHeight);
        var x = area.X * xCount;
        var y = area.Y * yCount;
        //var total = xCount * yCount;


        //var colours = new Color[total];
        //_mapTexture.GetData(0, new Rectangle(x, y, xCount, yCount), colours, 0, total);

        //for (int i = 0; i < colours.Count(); i++)
        //  colours[i] = new Color(255, 255, 255, 255);

        if (area.FogManager != null)
        {
          foreach (var sprite in area.FogManager.FogItems)
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
              Colour = GetSpriteColor(sprite),
            });
          }
        }
      }

      var currentAreaX = currentArea.X * (_gameModel.ScreenWidth / Map.TileWidth);
      var currentAreaY = currentArea.Y * (_gameModel.ScreenHeight / Map.TileHeight);
      var playerX = player.Rectangle.X / Map.TileWidth;
      var playerY = player.Rectangle.Y / Map.TileHeight;
      var playerWidth = player.Rectangle.Width / Map.TileWidth;
      var playerHeight = player.Rectangle.Height / Map.TileHeight;

      _sprites.Add(new Sprite(texture)
      {
        Scale = new Vector2(playerWidth, playerHeight),
        Position = new Vector2(currentAreaX + playerX, currentAreaY + playerY),
        Origin = new Vector2(0, 0),
        Colour = Color.Red
      });

    }

    private Color GetSpriteColor(FogItem sprite)
    {
      switch (sprite.Visibility)
      {
        case FogItem.Visibilities.See:
        case FogItem.Visibilities.Seen:
          return sprite.Colour;

        default:
          return Color.Black;
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

      _scale = MathHelper.Clamp(_scale, 1f, 10f);

      _transform = Matrix.CreateTranslation(0, 0, 0) * //new Vector3((_gameModel.ScreenWidth / 2), (_gameModel.ScreenHeight / 2), 0)) *
        Matrix.CreateScale(_scale);
    }

    private Matrix _transform;
    private float _scale = 1f;
    private float _currentScroll;
    private float _previousScroll;

    public override void Draw(GameTime gameTime)
    {
      _graphics.GraphicsDevice.Clear(new Color(255, 218, 28));

      _spriteBatch.Begin(transformMatrix: _transform);

      //_spriteBatch.Draw(_mapTexture, new Vector2(0, 0), Color.White);

      foreach (var sprite in _sprites)
      {
        sprite.Draw(gameTime, _spriteBatch);
      }

      _spriteBatch.End();
    }
  }
}
