using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public class Sword : Weapon
  {
    public Sword(Texture2D texture)
      : base(texture)
    {
      Origin = new Vector2(texture.Width / 2, texture.Height - 5);
      Rotation = MathHelper.ToRadians(-30);
    }

    public override void OnAttack(string ability, Hand leftHand, Hand rightHand, Weapon leftHandWeapon, Weapon rightHandWeapon)
    {
      throw new NotImplementedException();
    }

    public override void OnDefend()
    {
      throw new NotImplementedException();
    }
  }
}
