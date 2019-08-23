using Engine;
using Engine.Input;
using Engine.Models;
using Engine.Models.Skills;
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

      var abilityIcon ="Battle/AbilityIcon";

      _players = new List<ActorModel>()
      {
        new ActorModel()
        {
          Name = "Nude man",
          Attack = 1,
          Defence = 1,
          Health = 10,
          Speed = 5,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slap", abilityIcon, AbilityModel.TargetTypes.Single),
            Ability2 = new AbilityModel("Punch", abilityIcon, AbilityModel.TargetTypes.Single),
            Ability3 = new AbilityModel("A", abilityIcon, AbilityModel.TargetTypes.All),
            Ability4 = new AbilityModel("xyz", abilityIcon, AbilityModel.TargetTypes.Single),
          },
          BattleStats = new BattleStatsModel(),
          EquipmentModel = new EquipmentModel()
          {
            EquipmentType = EquipmentModel.EquipmentTypes.Fists,
            LeftHandEquipmentPath = null,
            RightHandEquipmentPath = null,
          },
          Lower = "Clothing/Lower/Clover",
        },
        new ActorModel()
        {
          Name = "Glenda",
          Attack = 300,
          Defence = 2,
          Health = 10,
          Speed = 2,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon, AbilityModel.TargetTypes.Single),
            Ability2 = new AbilityModel("Stab", abilityIcon, AbilityModel.TargetTypes.Single),
            Ability3 = new AbilityModel("Ability 3", abilityIcon, AbilityModel.TargetTypes.Single),
            Ability4 = new AbilityModel("Ability 4", abilityIcon, AbilityModel.TargetTypes.All),
          },
          BattleStats = new BattleStatsModel(),
          EquipmentModel = new EquipmentModel()
          {
            EquipmentType = EquipmentModel.EquipmentTypes.Both_Spear,
            LeftHandEquipmentPath = "Equipment/Spear",
            RightHandEquipmentPath = null,
          },
          Lower = "Clothing/Lower/RangerPants",
          Upper = "Clothing/Upper/RangerTop",
        },
      };

      // This will be assigned in-game rather than in code like this
      _players[0].SkillsModel = new SwordSkillsModel(_players[0]);

      //_currentState = new OpeningState(_gameModel, _players);
      //_currentState = new BattleState(_gameModel, _players,
      //  //null,
      //  new List<string>()
      //  {
      //    "Glenda: Any reason why you're completely nude, and surrounded by vampire snakes?",
      //    "Nude man: A really fun night I guess..?",
      //    "Glenda: Uugh. Just kill them!",
      //  }
      //);
      _currentState = new RoamingState(_gameModel, _players);
      _currentState.LoadContent();

      _transition = new FourCornersTransition(_gameModel);
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
      GameMouse.Update(gameTime);
      GameKeyboard.Update(gameTime);

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
            if (!(_transition is FourCornersTransition))
              _transition = new FourCornersTransition(_gameModel);
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
            _currentState = new RoamingState(_gameModel, _players);
            _currentState.LoadContent();
          }

          break;

        case OpeningState openingState:

          if (openingState.State == OpeningState.States.Fade)
          {
            _transition.Start();
          }

          if (_transition.State == Transition.States.Middle)
          {
            _currentState = new BattleState(_gameModel, _players, new List<string>()
            {
              "Glenda: Any reason why you're completely nude, and surrounded by vampire snakes?",
              "Nude man: A really fun night I guess..?",
              "Glenda: Uugh. Just kill them!",
            });
            _currentState.LoadContent();
          }

          break;

        default:
          //throw new Exception("Unexpected state: " + _currentState.ToString());
          break;
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
