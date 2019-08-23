using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites.Roaming
{
  public class Tree : Sprite
  {
    public override Rectangle CollisionRectangle => new Rectangle((int)Position.X, (int)Position.Y, 40, 40);

    public Tree(Texture2D texture) : base(texture)
    {
      Origin = new Vector2(0, 80);
    }
  }
}
