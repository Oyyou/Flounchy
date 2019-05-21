using Engine;
using Engine.Input;
using Engine.Models;
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
    private Button _enterBattleButton;

    private Map _map;

    private List<Sprite> _grids;

    private List<Sprite> _sprites;

    private Sprites.Roaming.Player _player;

    public bool EnterBattle = false;

    private bool _showGrid = false;

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

      for (int y = 0; y < 100; y++)
      {
        for (int x = 0; x < 100; x++)
        {
          var position = new Vector2((x * width) - (width / 2), y * height - (height / 2));

          _grids.Add(new Sprite(gridTexture) { Position = position });
        }
      }

      var bushTexture = _content.Load<Texture2D>("Roaming/Bush");
      var building01Texture = _content.Load<Texture2D>("Roaming/Buildings/Building_01");
      _sprites = new List<Sprite>()
      {
        new Sprite(bushTexture)
        {
          Position = new Vector2(60, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(100, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(140, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 60),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 100),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(180, 140),
        },
        new Sprite(bushTexture)
        {
          Position = new Vector2(140, 140),
        },
        new Sprite(building01Texture)
        {
          Position = new Vector2(200 + (building01Texture.Width/2), 120 + (building01Texture.Height / 2)),
        },
      };

      _map = new Map();

      foreach (var sprite in _sprites)
        _map.AddItem(sprite.Rectangle);

      _map.Write();

      _player = new Sprites.Roaming.Player(_content, _map)
      {
        Position = new Vector2(200, 400),
      };
    }

    public override void Update(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
        _showGrid = !_showGrid;

      _enterBattleButton.Update(gameTime);
      _player.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      if (_showGrid)
      {
        foreach (var grid in _grids)
          grid.Draw(gameTime, _spriteBatch);
      }

      foreach (var bush in _sprites)
        bush.Draw(gameTime, _spriteBatch);

      _player.Draw(gameTime, _spriteBatch);

      _enterBattleButton.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }
  }
}
