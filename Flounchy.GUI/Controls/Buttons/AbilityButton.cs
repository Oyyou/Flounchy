using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GUI.Controls.Buttons
{
  public class AbilityButton : Button
  {

    public AbilityButton(Texture2D texture, SpriteFont font) 
      : base(texture, font)
    {

    }

    public override void OnClick()
    {
      base.OnClick();
    }
  }
}
