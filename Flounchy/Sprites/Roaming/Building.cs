using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites.Roaming
{
  public class Building : Sprite
  {
    public override Rectangle CollisionRectangle => new Rectangle((int)Rectangle.X, (int)Rectangle.Y + 80, _texture.Width, _texture.Height - 80);

    public Building(Texture2D texture) : base(texture)
    {
    }
  }
}
