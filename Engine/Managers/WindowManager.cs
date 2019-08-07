using Engine.Controls;
using Engine.Input;
using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Managers
{
  public class WindowManager
  {
    private GameModel _gameModel;

    private List<Window> _windows;

    public Window Window { get; protected set; }

    public bool IsWindowOpen
    {
      get
      {
        return Window != null;
      }
    }

    public Rectangle WindowRectangle
    {
      get
      {
        return Window != null ? Window.WindowRectangle : new Rectangle(0, 0, 0, 0);
      }
    }

    public WindowManager(GameModel gameModel, List<Window> windows)
    {
      _gameModel = gameModel;

      _windows = windows;
    }

    public void CloseWindow()
    {
      // Can't close the window if it's not open
      if (Window == null)
        return;


      if (_windows.Any(c => c.GetType() == Window.GetType()))
      {
        for (int i = 0; i < _windows.Count; i++)
        {
          if (_windows[i].GetType() == Window.GetType())
          {
            _windows[i] = Window;
            break;
          }
        }
      }
      else
      {
        _windows.Add(Window.Clone() as Window);
      }

      Console.WriteLine(string.Join("\n", GameMouse.ClickableObjects.Select(c => c.ToString()).ToArray()));

      GameMouse.ClickableObjects.Remove(Window);
      Window = null;
    }

    public void OpenWindow(string name)
    {
      var window = _windows.Where(c => c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

      Window = window ?? throw new Exception($"Window '{name}' doesn't exist");

      Window.SetPositions();
    }

    public void Update(GameTime gameTime)
    {
      if (Window == null)
        return;

      Window.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
      if (Window == null)
        return;

      Window.Draw(gameTime, _gameModel.SpriteBatch, _gameModel.GraphicsDeviceManager);
    }
  }
}
