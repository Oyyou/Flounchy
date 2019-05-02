using Engine.Models;
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
  public enum BattleStates
  {
    AbilitySelection,
    EnemySelection,
    Attacking,
  }

  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private MouseState _currentMouse;
    private MouseState _previousMouse;

    private GameModel _gameModel;

    private int _currentActor;

    private List<Actor> _actors;

    private BattleGUI _battleGUI;

    public static Random Random;

    public List<Enemy> Enemies
    {
      get
      {
        return _actors.Where(c => c is Enemy).Cast<Enemy>().ToList();
      }
    }

    public List<Player> Players
    {
      get
      {
        return _actors.Where(c => c is Player).Cast<Player>().ToList();
      }
    }

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

      var abilityIcon = Content.Load<Texture2D>("Battle/AbilityIcon");

      _actors = new List<Actor>()
      {
        new Player(Content, new Vector2(200, 500), graphics.GraphicsDevice)
        {
          ActorModel = new ActorModel()
          {
            Name = "Jeoff",
            Attack = 3,
            Defence = 2,
            Health = 10,
            Speed = 3,
          },
          Colour = Color.Red,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          CurrentHealth = 6,
        },
        new Player(Content, new Vector2(400, 500), graphics.GraphicsDevice)
        {
          ActorModel = new ActorModel()
          {
            Name = "Spanders",
            Attack = 2,
            Defence = 2,
            Health = 10,
            Speed = 2,
          },
          Colour = Color.Purple,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Jab", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
        },
        new Player(Content, new Vector2(600, 500), graphics.GraphicsDevice)
        {
          ActorModel = new ActorModel()
          {
            Name = "Pleen",
            Attack = 5,
            Defence = 2,
            Health = 8,
            Speed = 2,
          },
          Colour = Color.Blue,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Poke", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          CurrentHealth = 5,
        },
        new Enemy(Content, new Vector2(200, 100), graphics.GraphicsDevice)
        {
          ActorModel = new ActorModel()
          {
            Name = "Klong",
            Attack = 3,
            Defence = 2,
            Health = 10,
            Speed = 3,
          },
          Colour = Color.White,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          CurrentHealth = 3,
        },
        new Enemy(Content, new Vector2(400, 100), graphics.GraphicsDevice)
        {
          ActorModel = new ActorModel()
          {
            Name = "Frank",
            Attack = 3,
            Defence = 2,
            Health = 10,
            Speed = 3,
          },
          Colour = Color.White,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          CurrentHealth = 7,
        },
      };

      _battleGUI = new BattleGUI(_gameModel);
      _battleGUI.SetAbilities(_actors.First().ActorModel, _actors.First().Abilities);
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
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      foreach (var actor in _actors)
      {
        actor.Update(gameTime);

        actor.ShowBorder = false;
        actor.ShowTurnBar = false;
      }

      ProcessTurns();

      _battleGUI.Update(gameTime);

      base.Update(gameTime);
    }

    private void ProcessTurns()
    {
      var buttons = _battleGUI.AbilityButtons
        .Where(c => c.Value.IsClicked)
        .ToDictionary(c => c.Key, v => v.Value);

      string ability = null;

      if (buttons.Count > 0)
      {
        var button = buttons.First();

        ability = button.Key;
      }

      var actor = _actors[_currentActor];
      actor.ShowTurnBar = true;

      var actionResult = actor.GetAction(ability);

      if (actionResult == null)
        return;

      if (actionResult.Status == Engine.ActionStatuses.WaitingForTarget)
      {
        var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

        var enemy = Enemies.Where(c => mouseRectangle.Intersects(c.Rectangle)).FirstOrDefault();

        if (enemy != null)
        {
          enemy.ShowBorder = true;

          if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
          {
            actionResult.Status = Engine.ActionStatuses.Running;
          }
        }
      }

      if (actionResult.Status == Engine.ActionStatuses.Running)
        actionResult.Action();

      if (actionResult.Status != Engine.ActionStatuses.Finished)
        return;

      _currentActor = (_currentActor + 1) % _actors.Count;
      _battleGUI.SetAbilities(_actors[_currentActor].ActorModel, _actors[_currentActor].Abilities);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();

      foreach (var actor in _actors)
        actor.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      _battleGUI.Draw(gameTime);

      base.Draw(gameTime);
    }
  }
}
