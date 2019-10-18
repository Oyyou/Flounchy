using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites.Battle
{
  public class Clothing : Sprite
  {
    public Clothing(Texture2D texture) : base(texture)
    {
    }

    public void ShowBack()
    {
      var width = Rectangle.Width / 2;
      var height = Rectangle.Height;

      SourceRectangle = new Rectangle(width, 0, width, height);
      Origin = new Vector2(width / 2, height / 2);
    }

    public void ShowFront()
    {
      var width = Rectangle.Width / 2;
      var height = Rectangle.Height;

      SourceRectangle = new Rectangle(0, 0, width, height);
      Origin = new Vector2(width / 2, height / 2);
    }
  }
}
