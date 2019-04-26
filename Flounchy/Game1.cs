using Engine.Models;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Flounchy
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private int _currentActor;

    private List<Player> _players;

    private Queue<int> _turns;

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
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      _players = new List<Player>()
      {
        new Player(Content, new Vector2(200, 400))
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
        },
        new Player(Content, new Vector2(400, 400))
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
        },
        new Player(Content, new Vector2(600, 400))
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
        }
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

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      foreach (var player in _players)
        player.Update(gameTime);

      ProcessTurns();

      base.Update(gameTime);
    }

    private void ProcessTurns()
    {
      var actionResult = _players[_currentActor].GetAction();

      if (actionResult == null)
        return;

      actionResult.Action();

      if (actionResult.Status == Engine.ActionStatuses.Running)
        return;

      _currentActor = (_currentActor + 1) % _players.Count;
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();

      foreach (var player in _players)
        player.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
