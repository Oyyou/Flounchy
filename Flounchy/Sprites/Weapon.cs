using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public abstract class Weapon : Sprite
  {
    public Weapon(Texture2D texture) : base(texture)
    {
    }

    /// <summary>
    /// The method called when the weapon is attacking
    /// </summary>
    public abstract void OnAttack(string ability, Hand leftHand, Hand rightHand, Weapon leftHandWeapon, Weapon rightHandWeapon);

    /// <summary>
    /// What happens when being attacked
    /// </summary>
    public abstract void OnDefend();
  }
}
