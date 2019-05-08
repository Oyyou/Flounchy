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
    public static List<Point> Resolutions = new List<Point>()
    {
      new Point(1024, 576),
      new Point(1280, 720),
    };

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private KeyboardState _currentKey;
    private KeyboardState _previousKey;

    public static Random Random;

    private GameModel _gameModel;

    private List<ActorModel> _players;

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
      foreach (var resolution in Resolutions.OrderByDescending(c => c.X))
      {
        if (resolution.Y < graphics.GraphicsDevice.DisplayMode.Height)
        {
          graphics.PreferredBackBufferWidth = resolution.X;
          graphics.PreferredBackBufferHeight = resolution.Y;
          graphics.ApplyChanges();

          break;
        }
      }

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

      var abilityIcon = Content.Load<Texture2D>("Battle/AbilityIcon");

      _players = new List<ActorModel>()
      {
        new ActorModel()
        {
          Name = "Jeoff",
          Attack = 3,
          Defence = 2,
          Health = 10,
          Speed = 3,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          BattleStats = new BattleStatsModel()
          {

          },
        },
        new ActorModel()
        {
          Name = "Spanders",
          Attack = 2,
          Defence = 2,
          Health = 10,
          Speed = 2,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Jab", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          BattleStats = new BattleStatsModel()
          {

          },
        },
        new ActorModel()
        {
          Name = "Pleen",
          Attack = 5,
          Defence = 2,
          Health = 8,
          Speed = 2,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Poke", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          BattleStats = new BattleStatsModel()
          {

          },
        },
      };

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

      float speed = 10f;

      switch (_currentState)
      {
        case RoamingState roamingState:

          if (roamingState.EnterBattle)
          {
            roamingState.EnterBattle = false;
            _goingIn = true;
          }

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

              _currentState = new BattleState(_gameModel, _players);
              _currentState.LoadContent();
            }
          }

          break;

        case BattleState battleState:

          if (_goingOut)
          {
            _transition1.Position.Y -= speed;
            _transition2.Position.Y += speed;
            _transition3.Position.Y -= speed;
            _transition4.Position.Y += speed;

            if (_transition1.Position.Y <= 0 - _transition1.Origin.Y)
            {
              _goingOut = false;
            }
          }

          if (battleState.BattleFinished)
          {
            _goingIn = true;
          }

          if (_goingIn)
          {
            _transition1.Position.Y += speed;
            _transition2.Position.Y -= speed;
            _transition3.Position.Y += speed;
            _transition4.Position.Y -= speed;

            if (_transition1.Position.Y >= ((_gameModel.ScreenHeight / 2) - _transition1.Origin.Y))
            {
              _goingIn = false;
              _goingOut = true;

              _currentState = new AfterBattleState(_gameModel, _players);
              _currentState.LoadContent();
            }
          }

          break;

        case AfterBattleState afterBattleState:

          if (_goingOut)
          {
            _transition1.Position.X -= speed;
            _transition2.Position.X -= speed;
            _transition3.Position.X += speed;
            _transition4.Position.X += speed;

            if (_transition1.Position.X <= 0 - _transition1.Origin.X)
            {
              _goingOut = false;
            }
          }

          break;

        default:
          throw new Exception("Unexpected state: " + _currentState.ToString());
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
