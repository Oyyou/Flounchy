using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Controls
{
  public class WindowSection
  {
    private Rectangle _area;

    public Rectangle Area
    {
      get { return _area; }
      set
      {
        _area = value;
        Scrollbar.ScrollArea = _area;
      }
    }

    public Matrix Matrix
    {
      get
      {
        return Matrix.CreateTranslation(Camera.X, Camera.Y, 0);
      }
    }

    public Scrollbar Scrollbar;

    public Vector2 Camera
    {
      get
      {

        return new Vector2(0, MathHelper.Clamp(-Scrollbar._innerY, -1000, 2400));
      }
    }

    public IEnumerable<Control> Items;

    public void UnloadContent()
    {
      foreach (var item in Items)
        item.UnloadContent();
    }
  }

  public abstract class Window : ICloneable, IClickable
  {
    protected ContentManager _content;

    protected GameModel _gameModel;

    protected SpriteFont _font;

    protected bool _hasUpdated;

    public bool Close { get; set; }

    public string Name { get; protected set; }

    public Vector2 Position { get; set; }

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
      }
    }

    public abstract Rectangle WindowRectangle { get; }

    public Texture2D Texture { get; protected set; }

    public float Layer { get; set; }

    public Window(GameModel gameModel)
    {
      _gameModel = gameModel;

      Close = false;

      _content = gameModel.ContentManger;

      Name = "Window";

      _font = gameModel.ContentManger.Load<SpriteFont>("Fonts/Font");

      _hasUpdated = false;

      //Texture = gameModel.ContentManger.Load<Texture2D>("Controls/Windows/Window");

      Layer = 0.9f;
    }

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics);

    public void OnScreenResize()
    {
      SetPositions();
    }

    public abstract void SetPositions();

    public abstract void UnloadContent();

    public abstract void Update(GameTime gameTime);

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}