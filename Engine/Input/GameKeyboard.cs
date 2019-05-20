using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Input
{
  public static class GameKeyboard
  {
    private static KeyboardState _currentKeyboard;
    private static KeyboardState _previouseKeyboard;

    public static void Update(GameTime gameTime)
    {
      _previouseKeyboard = _currentKeyboard;
      _currentKeyboard = Keyboard.GetState();
    }

    public static bool IsKeyDown(Keys key)
    {
      return _currentKeyboard.IsKeyDown(key);
    }

    public static bool IsKeyPressed(Keys key)
    {
      return _previouseKeyboard.IsKeyDown(key) &&
        _currentKeyboard.IsKeyUp(key);
    }
  }
}
