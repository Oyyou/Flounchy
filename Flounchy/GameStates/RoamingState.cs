using Engine;
using Engine.Controls;
using Engine.Input;
using Engine.Models;
using Flounchy.Areas;
using Flounchy.Components;
using Flounchy.GameStates.Roaming;
using Flounchy.GUI.Controls;
using Flounchy.Misc;
using Flounchy.Sprites;
using Flounchy.Sprites.Roaming;
using Flounchy.Transitions;
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
      Battle,
      AfterBattle,
      Paused,
      Map,
    }

    private Map _map;

    private List<Sprite> _grids;

    private Entities.Roaming.Player _player;

    private bool _showGrid = false;

    private List<Area> _areas;

    private Area _currentArea;

    private States _currentState;

    private States _nextState;

    private Transition _transition;

    #region Sub-States
    private BattleState _battleState;

    private AfterBattleState _afterBattleState;

    private PauseState _pauseState;

    private MapState _mapState;
    #endregion

    public RoamingState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {

    }

    public override void LoadContent()
    {
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

      _map = new Map(tileXCount, tileYCount + 1);

      _player = new Entities.Roaming.Player(_content.Load<Texture2D>("Roaming/Player"), _map)
      {
        Position = new Vector2(240, 440),
      };

      _afterBattleState = new AfterBattleState(_gameModel, _players);
      _afterBattleState.LoadContent();

      _pauseState = new PauseState(_gameModel, _players);
      _pauseState.LoadContent();

      _mapState = new MapState(_gameModel, _players);
      _mapState.LoadContent();

      SetAreas();

      LoadArea(_areas.FirstOrDefault());

      _transition = new FourCornersTransition(_gameModel);
    }

    public override void UnloadContent()
    {
      _battleState?.UnloadContent();
      _afterBattleState?.UnloadContent();
      _pauseState?.UnloadContent();
      _mapState?.UnloadContent();
    }

    private void SetAreas()
    {
      var area1 = new Area1x1(_gameModel, 0, 0, _map, _player);
      var area2 = new Area2x1(_gameModel, 1, 0, _map, _player);

      Area.SetAreaSide(area1, area2, Area.Sides.Right);

      _areas = new List<Area>()
      {
        area2,
        area1,
      };
    }

    private void LoadArea(Area area)
    {
      if (area == null)
        return;

      // Updates the map when leaving an area
      if (_currentArea != null)
        _mapState.UpdateMap(_currentArea, null);

      _currentArea = area;

      if (!_currentArea.Loaded)
        _currentArea.LoadContent(_content, _graphics.GraphicsDevice);

      _map.Clear();

      foreach (var sprite in _currentArea.MapSprites)
      {
        if (sprite.CollisionRectangle != null)
          _map.AddItem(sprite.CollisionRectangle.Value, 1);
      }

      foreach (var sprite in _currentArea.Somethings)
      {
        var mapComponent = sprite.Components.GetComponent<MapComponent>();

        if (mapComponent == null)
          return;

        _map.AddItem(mapComponent.MapRectangle);
      }

      foreach (var sprite in _currentArea.EnemySprites)
      {
        if (sprite.CollisionRectangle != null)
          _map.AddItem(sprite.CollisionRectangle.Value, 2);
      }

      _map.Write();
    }

    public override void Update(GameTime gameTime)
    {
      _transition.Update(gameTime);

      if (_nextState != _currentState)
      {
        if (_transition.State == Transition.States.Waiting)
          _transition.Start();

        if (_transition.State == Transition.States.FirstHalf)
          return;

        // Change the state at the half-way point so it can't be seen
        if (_transition.State == Transition.States.Middle)
        {
          _currentState = _nextState;
        }
      }

      switch (_currentState)
      {
        case States.Playing:

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
          {
            _nextState = States.Paused;
            return;
          }

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
          {
            _nextState = States.Map;
            _mapState.UpdateMap(_currentArea, _player);
            return;
          }

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
          {
            foreach (var entity in _currentArea.Interactables)
            {
              if (_player.CanInteract(entity))
              {
                var interactedComponent = entity.Components.GetComponent<InteractComponent>();
                interactedComponent.OnInteract();
              }
            }
          }

          PlayerStateUpdate(gameTime);

          break;

        case States.Battle:

          _battleState.Update(gameTime);

          if (_battleState.BattleFinished)
            _nextState = States.AfterBattle;

          break;

        case States.AfterBattle:

          _afterBattleState.Update(gameTime);

          if (_afterBattleState.Continue)
            _nextState = States.Playing;

          break;

        case States.Paused:

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
          {
            _nextState = States.Playing;
            return;
          }

          _pauseState.Update(gameTime);

          break;

        case States.Map:

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape) ||
              GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
          {
            _nextState = States.Playing;
            return;
          }

          _mapState.Update(gameTime);

          break;
        default:
          throw new Exception("Unknown state: " + this._currentState.ToString());
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

      _player.Update(gameTime);

      _currentArea.Update(gameTime);

      switch (_player.CollisionResult)
      {
        case Map.CollisionResults.OffRight:
          LoadArea(_currentArea.RightArea);
          _player.X = -_player.CurrentRectangle.Width;
          break;
        case Map.CollisionResults.OffLeft:
          LoadArea(_currentArea.LeftArea);
          _player.X = _gameModel.ScreenWidth;
          break;
        case Map.CollisionResults.OffTop:
          LoadArea(_currentArea.TopArea);
          _player.Y = -_player.CurrentRectangle.Height;
          break;
        case Map.CollisionResults.OffBottom:
          LoadArea(_currentArea.BottomArea);
          _player.Y = _gameModel.ScreenHeight;
          break;
        case Map.CollisionResults.Battle:
          _nextState = States.Battle;
          _battleState = new BattleState(_gameModel, _players);
          _battleState.LoadContent();
          break;
        default:
          break;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      switch (_currentState)
      {
        case States.Playing:

          _spriteBatch.Begin(SpriteSortMode.FrontToBack);

          if (_showGrid)
          {
            foreach (var grid in _grids)
              grid.Draw(gameTime, _spriteBatch);
          }

          _currentArea.Draw(gameTime, _spriteBatch);

          _player.Draw(gameTime, _spriteBatch);

          _spriteBatch.End();

          break;

        case States.Battle:
          _battleState.Draw(gameTime);
          break;

        case States.AfterBattle:
          _afterBattleState.Draw(gameTime);
          break;

        case States.Paused:

          _pauseState.Draw(gameTime);

          break;

        case States.Map:

          _mapState.Draw(gameTime);

          break;
        default:
          break;
      }

      _spriteBatch.Begin();

      _transition.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
