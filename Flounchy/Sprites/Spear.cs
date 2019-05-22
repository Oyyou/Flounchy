using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public class Spear : Weapon
  {
    public Spear(Texture2D texture)
      : base(texture)
    {
      Origin = new Vector2(texture.Width / 2, texture.Height - 30);
      Rotation = MathHelper.ToRadians(45);
    }

    public override void OnAttack()
    {
      throw new NotImplementedException();
    }

    public override void OnDefend()
    {
      throw new NotImplementedException();
    }
  }
}
