using Engine;
using Engine.Controls;
using Engine.Input;
using Engine.Models;
using Flounchy.Areas;
using Flounchy.GUI.Controls;
using Flounchy.Misc;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GameStates
{
  public class RoamingState : BaseState
  {
    public enum States
    {
      Playing,
      TransitioningLeave,
      TransitioningEnter,
    }

    private Button _enterBattleButton;

    private Map _map;

    private List<Sprite> _grids;

    private List<Sprite> _sprites;

    private Sprites.Roaming.Player _player;

    public bool EnterBattle = false;

    private bool _showGrid = false;

    private List<Area> _areas;

    private Area _currentArea;

    private Area _nextArea;

    private States State;

    public RoamingState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {

    }

    public override void LoadContent()
    {
      var buttonTexture = _content.Load<Texture2D>("Buttons/Button");
      var buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      _enterBattleButton = new Button(buttonTexture, buttonFont)
      {
        Click = () => EnterBattle = true,
        Position = new Vector2((_gameModel.ScreenWidth / 2) - (buttonTexture.Width / 2), 300),
        Text = "Enter Battle",
        PenColour = Color.Black,
      };

      int width = Map.TileWidth;
      int height = Map.TileHeight;
      var gridTexture = new Texture2D(_graphics.GraphicsDevice, width, height);
      gridTexture.SetData(Helpers.GetBorder(width, height, 1, Color.Black));

      _grids = new List<Sprite>();

      int tileXCount = _gameModel.ScreenWidth / width;
      int tileYCount = _gameModel.ScreenHeight / height;

      for (int y = 0; y < tileYCount; y++)
      {
        for (int x = 0; x < tileXCount; x++)
        {
          var position = new Vector2((x * width) - (width / 2), y * height - (height / 2));

          _grids.Add(new Sprite(gridTexture) { Position = position });
        }
      }

      _map = new Map(tileXCount, tileYCount);

      _player = new Sprites.Roaming.Player(_content, _content.Load<Texture2D>("Actor/Roaming_Body_Front"), _map)
      {
        Position = new Vector2(200, 400),
      };

      SetAreas();

      LoadArea(_areas.FirstOrDefault());

      _transitioningSprites = new List<Sprite>();
    }

    private void SetAreas()
    {
      var area1 = new Area();
      var area2 = new Area();

      Area.SetAreaSide(area1, area2, Area.Sides.Right);
      Area.SetAreaSide(area1, area2, Area.Sides.Left);

      _areas = new List<Area>()
      {
        area1,
        area2,
      };
    }

    private void LoadArea(Area area)
    {
      if (area == null)
        return;

      _currentArea = area;

      _currentArea.LoadContent(_content);

      _sprites = _currentArea.Sprites;

      _map.Clear();

      foreach (var sprite in _sprites)
        _map.AddItem(sprite.Rectangle);

      _map.Write();
    }

    private List<Sprite> _transitioningSprites;
    private float _transitioningTimer = 0;
    int column = 0;

    public override void Update(GameTime gameTime)
    {
      switch (State)
      {
        case States.Playing:

          PlayerStateUpdate(gameTime);

          break;
        case States.TransitioningLeave:

          TransitioningLeaveStateUpdate(gameTime);

          break;
        case States.TransitioningEnter:

          TransitioningEnterStateUpdate(gameTime);

          break;
        default:
          break;
      }
    }

    /// <summary>
    /// The transition that plays once the player enters the next area
    /// </summary>
    /// <param name="gameTime"></param>
    private void TransitioningEnterStateUpdate(GameTime gameTime)
    {
      var height = _gameModel.ScreenHeight / Map.TileHeight;

      for (int i = 0; i < Math.Min(height, _transitioningSprites.Count); i++)
      {
        _transitioningSprites.RemoveAt(i);

        if (_transitioningSprites.Count == 0)
        {
          State = States.Playing;
          break;
        }
      }
    }

    /// <summary>
    /// The transition that plays when the player leaves the area
    /// </summary>
    /// <param name="gameTime"></param>
    private void TransitioningLeaveStateUpdate(GameTime gameTime)
    {
      if (column > _gameModel.ScreenWidth / Map.TileWidth || column < -(_gameModel.ScreenWidth / Map.TileWidth))
      {
        State = States.TransitioningEnter;
        _transitioningTimer = 0;
        column = 0;
      }
      else
      {
        _transitioningTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_transitioningTimer >= 0.0200)
        {
          _transitioningTimer = 0;

          int max = 0;
          int start = 0;
          int columnSpeed = 1;

          switch (_player.CollisionResult)
          {
            case Map.CollisionResults.OffRight:
              max = _gameModel.ScreenHeight / Map.TileHeight;
              start = 0;
              columnSpeed = 1;
              break;

            case Map.CollisionResults.OffLeft:
              max = _gameModel.ScreenHeight / Map.TileHeight;
              start = _gameModel.ScreenWidth / Map.TileWidth;
              columnSpeed = -1;
              break;

            case Map.CollisionResults.OffTop:
              max = _gameModel.ScreenWidth / Map.TileWidth;
              start = max;
              columnSpeed = -1;
              break;

            case Map.CollisionResults.OffBottom:
              max = _gameModel.ScreenWidth / Map.TileWidth;
              start = 0;
              columnSpeed = 1;
              break;

            default:
              break;
          }


          var texture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, Map.TileWidth, Map.TileHeight);

          var colours = new Color[texture.Width * texture.Height];

          for (int y = 0; y < colours.Length; y++)
          {
            colours[y] = new Color(30, 30, 30);
          }

          texture.SetData(colours);

          switch (_player.CollisionResult)
          {
            case Map.CollisionResults.OffRight:


              for (int i = 0; i < max; i++)
              {
                _transitioningSprites.Add(new Sprite(texture)
                {
                  Origin = new Vector2(0, 0),
                  Position = new Vector2(column * Map.TileWidth, i * Map.TileHeight),
                  Opacity = Game1.Random.NextDouble() > 0.6 ? 1 : 0,
                });
              }

              break;

            case Map.CollisionResults.OffLeft:


              for (int i = 0; i < max; i++)
              {
                _transitioningSprites.Add(new Sprite(texture)
                {
                  Origin = new Vector2(0, 0),
                  Position = new Vector2((column + (_gameModel.ScreenWidth / Map.TileWidth)) * Map.TileWidth, i * Map.TileHeight),
                  Opacity = Game1.Random.NextDouble() > 0.2 ? 1 : 0,
                });
              }

              break;

            case Map.CollisionResults.OffTop:

              break;

            case Map.CollisionResults.OffBottom:

              break;

            default:
              break;
          }

          if (column != 0)
          {
            foreach (var sprite in _transitioningSprites)
            {
              if (sprite.Position.X / Map.TileWidth == ((start + column) - (columnSpeed * 2)))
              {
                sprite.Opacity = 1;
              }
            }
          }

          column += columnSpeed;
        }
      }
    }

    /// <summary>
    /// What happens when wandering around the area
    /// </summary>
    /// <param name="gameTime"></param>
    private void PlayerStateUpdate(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
        _showGrid = !_showGrid;

      _enterBattleButton.Update(gameTime);
      _player.Update(gameTime);

      switch (_player.CollisionResult)
      {
        case Map.CollisionResults.OffRight:
          LoadArea(_currentArea.RightArea);
          _player.Position.X = -_player.Rectangle.Width;
          State = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffLeft:
          LoadArea(_currentArea.LeftArea);
          _player.Position.X = _gameModel.ScreenWidth;
          State = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffTop:
          LoadArea(_currentArea.TopArea);
          _player.Position.Y = -_player.Rectangle.Height;
          State = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffBottom:
          LoadArea(_currentArea.BottomArea);
          _player.Position.Y = _gameModel.ScreenHeight;
          State = States.TransitioningLeave;
          break;
        default:
          break;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      if (_showGrid)
      {
        foreach (var grid in _grids)
          grid.Draw(gameTime, _spriteBatch);
      }

      _currentArea.Background.Draw(gameTime, _spriteBatch);

      foreach (var bush in _sprites)
        bush.Draw(gameTime, _spriteBatch);

      _player.Draw(gameTime, _spriteBatch);

      _enterBattleButton.Draw(gameTime, _spriteBatch);

      foreach (var sprite in _transitioningSprites)
        sprite.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
