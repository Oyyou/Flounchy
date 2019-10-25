using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Input;
using Engine.Models;
using Flounchy.Areas;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flounchy.Managers;
using Flounchy.Entities;
using Flounchy.Components;

namespace Flounchy.GameStates.Roaming
{
  public class MapState : BaseState
  {
    private Texture2D _mapTexture;
    private Vector2 _mapPosition;

    #region Camera stuff
    private Matrix _transform;
    private float _scale = 1f;
    private float _currentScroll;
    private float _previousScroll;
    #endregion

    public MapState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      _mapTexture = new Texture2D(_graphics.GraphicsDevice, _gameModel.ScreenWidth, _gameModel.ScreenHeight);
      _mapPosition = new Vector2(0, 0);

      var colours = new Color[_mapTexture.Width * _mapTexture.Height];

      _scale = 10f;

      _mapTexture.SetData(colours);
    }

    public override void UnloadContent()
    {

    }

    public void UpdateMap(Area area, Entity player)
    {
      var width = (_gameModel.ScreenWidth / Map.TileWidth);
      var height = (_gameModel.ScreenHeight / Map.TileHeight);

      var startX = area.X * width;
      var startY = area.Y * height;

      var colours = new Color[_gameModel.ScreenWidth * _gameModel.ScreenHeight];
      _mapTexture.GetData(colours);

      var fogItems = area.FogManager.FogItems.ToList();

      if (player != null)
      {
        var playerMovementComp = player.Components.GetComponent<MoveComponent>();

        if (playerMovementComp != null)
        {
          fogItems.Add(new FogItem(null)
          {
            Layer = 1,
            Rectangle = playerMovementComp.CurrentRectangle,
            Visibility = FogItem.Visibilities.Seen,
            Colour = Color.Red,
          });
        }
      }

      for (int y = startY; y < (startY + height); y++)
      {
        for (int x = startX; x < (startX + width); x++)
        {
          var index = x + (_gameModel.ScreenWidth * y);

          for (int i = 0; i < fogItems.Count; i++)
          {
            var sprite = fogItems[i];
            var colour = GetSpriteColor(sprite);

            var spriteX = (sprite.Rectangle.X / Map.TileWidth) + startX;
            var spriteY = (sprite.Rectangle.Y / Map.TileHeight) + startY;

            if (x == spriteX && y == spriteY)
            {
              colours[index] = colour;
              fogItems.RemoveAt(i);
              i--;
            }
          }
        }
      }

      //var currentAreaX = currentArea.X * (_gameModel.ScreenWidth / Map.TileWidth);
      //var currentAreaY = currentArea.Y * (_gameModel.ScreenHeight / Map.TileHeight);
      //var playerX = player.CurrentRectangle.X / Map.TileWidth;
      //var playerY = player.CurrentRectangle.Y / Map.TileHeight;
      //var playerWidth = player.CurrentRectangle.Width / Map.TileWidth;
      //var playerHeight = player.CurrentRectangle.Height / Map.TileHeight;

      //_sprites.Add(new Sprite(texture)
      //{
      //  Scale = new Vector2(playerWidth, playerHeight),
      //  Position = new Vector2(currentAreaX + playerX, currentAreaY + playerY),
      //  Origin = new Vector2(0, 0),
      //  Colour = Color.Red
      //});

      _mapTexture.SetData(colours);
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

    public override void Draw(GameTime gameTime)
    {
      _graphics.GraphicsDevice.Clear(new Color(255, 218, 28));

      _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _transform);

      _spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);

      _spriteBatch.End();
    }
  }
}
