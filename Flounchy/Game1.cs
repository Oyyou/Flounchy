using Engine.Models;
using Flounchy.GameStates;
using Flounchy.GUI.States;
using Flounchy.Misc;
using Flounchy.Sprites;
using Flounchy.Transitions;
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

    private Transition _transition;

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
        new ActorModel()
        {
          Name = "Kandra",
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

      _currentState = new AfterBattleState(_gameModel, _players);
      _currentState.LoadContent();

      _transition = new Transition(_gameModel);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

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
            _transition.Start();
          }

          if (_transition.State == Transition.States.Middle)
          {
            _currentState = new BattleState(_gameModel, _players);
            _currentState.LoadContent();
          }

          break;

        case BattleState battleState:

          if (battleState.BattleFinished)
          {
            _transition.Start();
          }

          if (_transition.State == Transition.States.Middle)
          {
            _currentState = new AfterBattleState(_gameModel, _players);
            _currentState.LoadContent();
          }

          break;

        case AfterBattleState afterBattleState:

          if (afterBattleState.Continue)
          {
            _transition.Start();
          }

          if (_transition.State == Transition.States.Middle)
          {
            _currentState = new RoamingState(_gameModel);
            _currentState.LoadContent();
          }

          break;

        default:
          throw new Exception("Unexpected state: " + _currentState.ToString());
      }

      _transition.Update(gameTime);
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

      _transition.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
