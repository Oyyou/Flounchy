using Engine;
using Engine.Controls;
using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.Controls.Windows
{
  public class TavernWindow : Window
  {
    private List<ActorModel> _actors;

    private List<WindowSection> _sections
    {
      get
      {
        return new List<WindowSection>()
        {
          _leftSection,
          _rightSection,
        };
      }
    }

    private WindowSection _leftSection;

    private WindowSection _rightSection;

    public override Rectangle WindowRectangle => Rectangle;

    public TavernWindow(GameModel gameModel, List<ActorModel> actors)
      : base(gameModel)
    {
      _actors = actors;

      Name = "Tavern";

      var width = gameModel.ScreenWidth - 20;
      var height = gameModel.ScreenHeight - 20 - 100;

      Texture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, width, height);

      var outerTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, 20, height - 35 - 10); // 35 is space at top, 10 is space at bottom
      var innerTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, 14, 1);

      Helpers.SetTexture(Texture, new Color(43, 43, 43, 200), new Color(0, 0, 0, 200));
      Helpers.SetTexture(outerTexture, new Color(43, 43, 43), new Color(0, 0, 0));
      Helpers.SetTexture(innerTexture, new Color(69, 69, 69), new Color(0, 0, 0), 0);

      _leftSection = new WindowSection()
      {
        Scrollbar = new Scrollbar(outerTexture, innerTexture)
        {
          Layer = this.Layer + 0.01f,
        },
        Items = new List<Control>(),
      };

      _rightSection = new WindowSection()
      {
        Scrollbar = new Scrollbar(outerTexture, innerTexture)
        {
          Layer = this.Layer + 0.01f,
        },
        Items = new List<Control>(),
      };
    }

    public override void SetPositions()
    {
      var screenWidth = _gameModel.ScreenWidth;
      var screenHeight = _gameModel.ScreenHeight;

      Position = new Vector2((screenWidth / 2) - (WindowRectangle.Width / 2), 10);

      var y = (int)Position.Y + 35;

      var height = Texture.Height - 35 - 10;

      var buttonTexture = _content.Load<Texture2D>("Buttons/Button");
      var buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      _leftSection.Area = new Rectangle((int)Position.X, y, buttonTexture.Width + 20 + _leftSection.Scrollbar.Width, height);
      _leftSection.Scrollbar.Position = new Vector2(_leftSection.Area.X + _leftSection.Area.Width - 20, y);

      _leftSection.Items = _actors.Select(c => new Button(buttonTexture, buttonFont) { Text = c.Name, PenColour = Color.Black, }).ToList();

      _rightSection.Area = new Rectangle((int)_leftSection.Area.Right, y, (Texture.Width - 10) - _leftSection.Area.Width, height);
      _rightSection.Scrollbar.Position = new Vector2(_rightSection.Area.X + _rightSection.Area.Width - 20, y);

      //_rightSection.Items = _actors.Select(c => new Button(buttonTexture, buttonFont) { Text = c.Name, PenColour = Color.Black, }).ToList();

      SetSectionPositions(_leftSection);
      SetSectionPositions(_rightSection);
    }

    private void SetSectionPositions(WindowSection section)
    {
      if (section.Items == null)
        return;

      if (section.Items.Count() == 0)
        return;

      var buttonHeight = section.Items.FirstOrDefault().Rectangle.Height;
      var buttonWidth = section.Items.FirstOrDefault().Rectangle.Width;

      var x = 10;
      var y = (section.Area.Y) + 3;

      foreach (var button in section.Items)
      {
        button.Position = new Vector2(x, y);
        x += button.Rectangle.Width + 10;

        if ((x + button.Rectangle.Width) > (section.Area.Width) - 30)
        {
          x = 10;
          y += buttonHeight + 10;
        }
      }
    }

    public override void UnloadContent()
    {
      Texture.Dispose();

      foreach (var section in _sections)
        section.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      _hasUpdated = true;

      foreach (var section in _sections)
        section.Scrollbar.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
      if (!_hasUpdated)
        return;

      var original = graphics.GraphicsDevice.Viewport;

      DrawWindow(gameTime, spriteBatch);

      foreach (var section in _sections)
      {
        graphics.GraphicsDevice.Viewport = new Viewport(section.Area);

        spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, transformMatrix: section.Matrix);

        foreach (var button in section.Items)
          button.Draw(gameTime, spriteBatch);

        spriteBatch.End();
      }

      graphics.GraphicsDevice.Viewport = original;
    }

    protected void DrawWindow(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.05f);

      foreach (var section in _sections)
        section.Scrollbar.Draw(gameTime, spriteBatch);

      spriteBatch.DrawString(_font, Name, new Vector2(Position.X + 10, Position.Y + 10), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.06f);

      spriteBatch.End();
    }
  }
}
