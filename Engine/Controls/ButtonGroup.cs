using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controls
{
  public class ButtonGroup
  {
    public List<Button> Buttons { get; private set; }

    public ButtonGroup(List<Button> buttons)
    {
      Buttons = buttons;
    }

    public void Update(GameTime gameTime)
    {
      foreach (var button in Buttons)
      {
        button.Update(gameTime);
      }

      if (Buttons.Where(c => c.IsSelected).Count() > 1)
      {
        foreach (var button in Buttons.Where(c => c.IsSelected && !c.IsClicked))
        {
          button.IsSelected = false;
        }
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

      foreach (var button in Buttons)
      {
        button.Draw(gameTime, spriteBatch);
      }
    }
  }
}
