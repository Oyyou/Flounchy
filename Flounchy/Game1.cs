using Engine.Models;
using Flounchy.GameStates;
using Flounchy.GUI.States;
using Flounchy.Misc;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flounchy
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private KeyboardState _currentKey;
    private KeyboardState _previousKey;

    public static Random Random;

    private GameModel _gameModel;

    private BaseState _currentState;

    private Sprite _transition1;
    private Sprite _transition2;
    private Sprite _transition3;
    private Sprite _transition4;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;
      graphics.ApplyChanges();

      Window.ClientSizeChanged += Window_ClientSizeChanged;

      IsMouseVisible = true;

      Random = new Random();

      base.Initialize();
    }

    private void UpdateWindowValues()
    {
      _gameModel.ScreenWidth = graphics.PreferredBackBufferWidth;
      _gameModel.ScreenHeight = graphics.PreferredBackBufferHeight;
    }

    private void Window_ClientSizeChanged(object sender, System.EventArgs e)
    {
      UpdateWindowValues();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      _gameModel = new GameModel()
      {
        ContentManger = Content,
        GraphicsDeviceManager = graphics,
        SpriteBatch = spriteBatch,
      };

      UpdateWindowValues();

      _currentState = new RoamingState(_gameModel);
      _currentState.LoadContent();

      var transitionTexture = new Texture2D(graphics.GraphicsDevice, _gameModel.ScreenWidth / 2,
        _gameModel.ScreenHeight / 2);

      var colours = new Color[transitionTexture.Width * transitionTexture.Height];

      var index = 0;
      for (int y = 0; y < transitionTexture.Height; y++)
      {
        for (int x = 0; x < transitionTexture.Width; x++)
        {
          var colour = Color.Gray;

          colours[index] = colour;

          index++;
        }
      }

      transitionTexture.SetData(colours);

      _transition1 = new Sprite(transitionTexture)
      {
        Position = new Vector2(-(transitionTexture.Width / 2), (transitionTexture.Height / 2)),
      };

      _transition2 = new Sprite(transitionTexture)
      {
        Position = new Vector2(-(transitionTexture.Width / 2), _gameModel.ScreenHeight - (transitionTexture.Height / 2)),
      };

      _transition3 = new Sprite(transitionTexture)
      {
        Position = new Vector2(_gameModel.ScreenWidth + (transitionTexture.Width / 2), (transitionTexture.Height / 2)),
      };

      _transition4 = new Sprite(transitionTexture)
      {
        Position = new Vector2(_gameModel.ScreenWidth + (transitionTexture.Width / 2), _gameModel.ScreenHeight - (transitionTexture.Height / 2)),
      };
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    private bool _goingIn = false;
    private bool _goingOut = false;

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      _previousKey = _currentKey;
      _currentKey = Keyboard.GetState();

      switch (_currentState)
      {
        case RoamingState roamingState:

          if (roamingState.EnterBattle)
          {
            roamingState.EnterBattle = false;
            _goingIn = true;
          }

          break;

        case BattleState battleState:

          break;

        default:
          throw new Exception("Unexpected state: " + _currentState.ToString());
      }

      float speed = 10f;

      if (_goingIn)
      {
        _transition1.Position.X += speed;
        _transition2.Position.X += speed;
        _transition3.Position.X -= speed;
        _transition4.Position.X -= speed;

        if (_transition1.Position.X >= ((_gameModel.ScreenWidth / 2) - _transition1.Origin.X))
        {
          _goingIn = false;
          _goingOut = true;

          _currentState = new BattleState(_gameModel);
          _currentState.LoadContent();
        }
      }
      else if (_goingOut)
      {
        _transition1.Position.Y -= speed;
        _transition2.Position.Y += speed;
        _transition3.Position.Y -= speed;
        _transition4.Position.Y += speed;
      }

      _currentState.Update(gameTime);

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _currentState.Draw(gameTime);

      spriteBatch.Begin();

      _transition1.Draw(gameTime, spriteBatch);
      _transition2.Draw(gameTime, spriteBatch);
      _transition3.Draw(gameTime, spriteBatch);
      _transition4.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
