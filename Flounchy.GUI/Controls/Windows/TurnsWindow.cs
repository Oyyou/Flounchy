using Engine;
using Engine.Controls;
using Engine.Input;
using Engine.Models;
using Flounchy.GUI.Controls.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.GUI.Controls.Windows
{
  public class TurnsWindow : Window
  {
    private WindowSection _section;

    public override Rectangle WindowRectangle => Rectangle;

    public int SelectedActorId { get; set; }

    public TurnsWindow(GameModel gameModel)
      : base(gameModel)
    {
      Name = "Turns";

      var width = 180;
      var height = gameModel.ScreenHeight - 80;

      Texture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, width, height);

      var outerTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, 20, height - 35 - 10);
      var innerTexture = new Texture2D(_gameModel.GraphicsDeviceManager.GraphicsDevice, 14, 1);

      Helpers.SetTexture(Texture, new Color(43, 43, 43, 200), new Color(0, 0, 0, 200));
      Helpers.SetTexture(outerTexture, new Color(43, 43, 43), new Color(0, 0, 0));
      Helpers.SetTexture(innerTexture, new Color(69, 69, 69), new Color(0, 0, 0), 0);

      _section = new WindowSection()
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

      Position = new Vector2((screenWidth - 10) - WindowRectangle.Width, 10);

      var y = (int)Position.Y + 20;

      var height = Texture.Height - 35 - 10;

      _section.Area = new Rectangle((int)Position.X, y, this.WindowRectangle.Width, height);
      _section.Scrollbar.Position = new Vector2(_section.Area.Right - _section.Scrollbar.Width - 10, y);
    }

    public void SetItems(List<ActorModel> actors)
    {
      var buttonTexture = _content.Load<Texture2D>("Buttons/Button");
      var buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");

      _section.Items = actors.Select(c => new TurnButton(buttonTexture, buttonFont, c.Id) { Text = c.Name, PenColour = Color.Black, }).ToList();

      if (_section.Items.Count() > 0)
      {
        // Wanted to change the highlight of the first button
      }

      SetSectionPositions(_section);
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

      _section.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      _hasUpdated = true;

      SelectedActorId = -1;

      UpdateButtons();
    }

    private void UpdateButtons()
    {
      var translation = _section.Matrix.Translation;

      var mouseRectangleWithCamera_Jobs = new Rectangle(
        (int)((GameMouse.X - Position.X) - translation.X),
        (int)((GameMouse.Y - (Position.Y + 20)) - translation.Y),
        1,
        1
      );

      var windowRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

      foreach (var button in (_section.Items as List<TurnButton>))
      {
        var isHovering = mouseRectangleWithCamera_Jobs.Intersects(button.Rectangle) && GameMouse.Rectangle.Intersects(windowRectangle);

        if(isHovering)
        {
          SelectedActorId = button.ActorId;
        }
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
      var original = graphics.GraphicsDevice.Viewport;

      DrawWindow(gameTime, spriteBatch);

      graphics.GraphicsDevice.Viewport = new Viewport(_section.Area);

      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, transformMatrix: _section.Matrix);

      foreach (var button in _section.Items)
        button.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      graphics.GraphicsDevice.Viewport = original;
    }

    protected void DrawWindow(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.05f);

      //_section.Scrollbar.Draw(gameTime, spriteBatch);

      spriteBatch.DrawString(_font, Name, new Vector2(Position.X + 10, Position.Y + 10), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.06f);

      spriteBatch.End();
    }
  }
}
