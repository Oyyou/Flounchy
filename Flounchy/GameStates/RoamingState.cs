﻿using Engine;
using Engine.Controls;
using Engine.Input;
using Engine.Models;
using Flounchy.Areas;
using Flounchy.GameStates.Roaming;
using Flounchy.GUI.Controls;
using Flounchy.Misc;
using Flounchy.Sprites;
using Flounchy.Sprites.Roaming;
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
      Paused,
      Map,
    }

    private Map _map;

    private List<Sprite> _grids;

    private Sprites.Roaming.Player _player;

    public bool EnterBattle = false;

    private bool _showGrid = false;

    private bool _actionKeyPressed;

    private List<Area> _areas;

    private Area _currentArea;

    private States _currentState;

    private States _nextState;

    #region Sub-States
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

      _player = new Sprites.Roaming.Player(_content.Load<Texture2D>("Roaming/Player"), new Vector2(240, 440), 4, 4, 0.1f, _map);

      _pauseState = new PauseState(_gameModel, _players);
      _pauseState.LoadContent();

      _mapState = new MapState(_gameModel, _players);
      _mapState.LoadContent();

      SetAreas();

      LoadArea(_areas.FirstOrDefault());

      _transitioningSprites = new List<Sprite>();
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

      foreach (var sprite in _currentArea.EnemySprites)
      {
        if (sprite.CollisionRectangle != null)
          _map.AddItem(sprite.CollisionRectangle.Value, 2);
      }

      _map.Write();
    }

    private List<Sprite> _transitioningSprites;
    private float _transitioningTimer = 0;
    //int column = 0;

    public override void Update(GameTime gameTime)
    {
      if (_nextState != _currentState)
        _currentState = _nextState;

      _actionKeyPressed = false;

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
            //_mapState.SetContent(_areas, _currentArea, _player);
            return;
          }

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
            _actionKeyPressed = true;

          PlayerStateUpdate(gameTime);

          break;
        case States.TransitioningLeave:
          _nextState = States.Playing;

          //TransitioningLeaveStateUpdate(gameTime);

          break;
        case States.TransitioningEnter:

          TransitioningEnterStateUpdate(gameTime);

          break;

        case States.Paused:

          if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
          {
            _nextState = States.Playing;
            return;
          }

          _pauseState.Update(gameTime);

          EnterBattle = _pauseState.EnterBattle;
          if (EnterBattle)
            _nextState = States.Playing;

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
          _nextState = States.Playing;
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
      // The max we can have in the direction we're moving
      var maxAcross = 0;

      // How many are added per increment
      var addCount = 0;

      switch (_player.CollisionResult)
      {
        case Map.CollisionResults.OffRight:
          maxAcross = _gameModel.ScreenWidth / Map.TileWidth;
          addCount = _gameModel.ScreenHeight / Map.TileHeight;

          break;

        case Map.CollisionResults.OffLeft:
          maxAcross = _gameModel.ScreenWidth / Map.TileWidth;
          addCount = _gameModel.ScreenHeight / Map.TileHeight;

          break;

        case Map.CollisionResults.OffTop:
          maxAcross = _gameModel.ScreenHeight / Map.TileHeight;
          addCount = _gameModel.ScreenWidth / Map.TileWidth;

          break;

        case Map.CollisionResults.OffBottom:
          maxAcross = _gameModel.ScreenHeight / Map.TileHeight;
          addCount = _gameModel.ScreenWidth / Map.TileWidth;

          break;

        default:
          break;
      }

      // How far we've made it across
      var currentAcross = _transitioningSprites.Count / addCount;

      if (currentAcross > maxAcross)
      {
        _nextState = States.TransitioningEnter;
        _transitioningTimer = 0;
        //column = 0;

        return;
      }

      _transitioningTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_transitioningTimer >= 0.0200)
      {
        _transitioningTimer = 0;

        int max = 0;
        int start = 0;

        switch (_player.CollisionResult)
        {
          case Map.CollisionResults.OffRight:
            max = _gameModel.ScreenHeight / Map.TileHeight;
            start = 0;
            break;

          case Map.CollisionResults.OffLeft:
            max = _gameModel.ScreenHeight / Map.TileHeight;
            start = _gameModel.ScreenWidth / Map.TileWidth;
            break;

          case Map.CollisionResults.OffTop:
            max = _gameModel.ScreenWidth / Map.TileWidth;
            start = max;
            break;

          case Map.CollisionResults.OffBottom:
            max = _gameModel.ScreenWidth / Map.TileWidth;
            start = 0;
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


            for (int i = 0; i < addCount; i++)
            {
              _transitioningSprites.Add(new Sprite(texture)
              {
                Origin = new Vector2(0, 0),
                Position = new Vector2(currentAcross * Map.TileWidth, i * Map.TileHeight),
                Opacity = Game1.Random.NextDouble() > 0.6 ? 1 : 0,
              });
            }

            break;

          case Map.CollisionResults.OffLeft:


            for (int i = 0; i < addCount; i++)
            {
              _transitioningSprites.Add(new Sprite(texture)
              {
                Origin = new Vector2(0, 0),
                Position = new Vector2((-(currentAcross) + (_gameModel.ScreenWidth / Map.TileWidth)) * Map.TileWidth, i * Map.TileHeight),
                Opacity = Game1.Random.NextDouble() > 0.6 ? 1 : 0,
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

        if (currentAcross < 2)
        {
          // The first set
          for (int i = 0; i < addCount; i++)
          {
            if (_transitioningSprites[i].Opacity < 1)
            {
              _transitioningSprites[i].Opacity = Game1.Random.NextDouble() > 0.7 ? 1 : 0;
            }
          }
        }
        else if (currentAcross < 3)
        {
          for (int i = 0; i < addCount * 2; i++)
          {
            double chance = 0;

            if (i < addCount)
              chance = 0.6;
            else chance = 0.7;

            if (_transitioningSprites[i].Opacity < 1)
            {
              _transitioningSprites[i].Opacity = Game1.Random.NextDouble() > chance ? 1 : 0;
            }
          }
        }
        else if (currentAcross < 4)
        {
          for (int i = 0; i < addCount * 3; i++)
          {
            double chance = 0;

            if (i < addCount)
              chance = 0.5;
            else if (i < addCount * 2)
              chance = 0.6;
            else chance = 0.7;

            if (_transitioningSprites[i].Opacity < 1)
            {
              _transitioningSprites[i].Opacity = Game1.Random.NextDouble() > chance ? 1 : 0;
            }
          }
        }
        else
        {
          for (int i = 0; i < addCount * currentAcross; i++)
          {
            double chance = 0;

            if (i < addCount)
              chance = 0;
            else if (i < addCount * 2)
              chance = 0.5;
            else if (i < addCount * 3)
              chance = 0.6;
            else chance = 0.7;

            if (_transitioningSprites[i].Opacity < 1)
            {
              _transitioningSprites[i].Opacity = Game1.Random.NextDouble() > chance ? 1 : 0;
            }
          }
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

      _player.Update(gameTime);

      EnterBattle = _player.EnterBattle;
      _currentArea.Update(gameTime);

      switch (_player.CollisionResult)
      {
        case Map.CollisionResults.OffRight:
          LoadArea(_currentArea.RightArea);
          _player.Position.X = -_player.CurrentRectangle.Width;
          _nextState = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffLeft:
          LoadArea(_currentArea.LeftArea);
          _player.Position.X = _gameModel.ScreenWidth;
          _nextState = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffTop:
          LoadArea(_currentArea.TopArea);
          _player.Position.Y = -_player.CurrentRectangle.Height;
          _nextState = States.TransitioningLeave;
          break;
        case Map.CollisionResults.OffBottom:
          LoadArea(_currentArea.BottomArea);
          _player.Position.Y = _gameModel.ScreenHeight;
          _nextState = States.TransitioningLeave;
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
        case States.TransitioningLeave:
        case States.TransitioningEnter:

          _spriteBatch.Begin(SpriteSortMode.FrontToBack);

          if (_showGrid)
          {
            foreach (var grid in _grids)
              grid.Draw(gameTime, _spriteBatch);
          }

          _currentArea.Draw(gameTime, _spriteBatch);

          _player.Draw(gameTime, _spriteBatch);

          //var animalCentre = _animalTest.Position + new Vector2(20, 20);
          //if (_player.IsInRange(animalCentre))
          //  _animalTest.Draw(gameTime, _spriteBatch);

          foreach (var sprite in _transitioningSprites)
            sprite.Draw(gameTime, _spriteBatch);

          _spriteBatch.End();

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
    }
  }
}
