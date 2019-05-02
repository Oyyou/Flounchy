using Engine.Models;
using Flounchy.GUI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.BackEnd
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private GameModel _gameModel;

    private MainMenuGUI _mainMenu;

    private BattleGUI _battleGUI;

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
      Window.ClientSizeChanged += Window_ClientSizeChanged;

      IsMouseVisible = true;

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

      _mainMenu = new MainMenuGUI(_gameModel);

      _battleGUI = new BattleGUI(_gameModel);

      _battleGUI.SetAbilities(
        new ActorModel()
        {
          Name = "{Test}",
        },
        new AbilitiesModel()
        {
          Ability1 = new AbilityModel("Ability 1", Content.Load<Texture2D>("Battle/AbilityIcon")),
          Ability2 = new AbilityModel("Ability 2", Content.Load<Texture2D>("Battle/AbilityIcon")),
          Ability3 = new AbilityModel("Ability 3", Content.Load<Texture2D>("Battle/AbilityIcon")),
          Ability4 = new AbilityModel("Ability 4", Content.Load<Texture2D>("Battle/AbilityIcon")),
        });
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
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      _battleGUI.Update(gameTime);

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _battleGUI.Draw(gameTime);

      base.Draw(gameTime);
    }
  }
}
