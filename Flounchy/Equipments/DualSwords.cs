using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.Sprites;
using Flounchy.Sprites.Battle;
using Microsoft.Xna.Framework;

namespace Flounchy.Equipments
{
  public class DualSwords : Equipment
  {
    public DualSwords(Hand leftHand, Hand rightHand, Sprite leftHandWeapon, Sprite rightHandWeapon) 
      : base(leftHand, rightHand, leftHandWeapon, rightHandWeapon)
    {
    }

    public override void SetStance(Vector2 position)
    {
      throw new NotImplementedException();
    }

    public override void SetEquipmentRotation()
    {
      throw new NotImplementedException();
    }

    public override void OnAttack(AbilityModel ability)
    {
      throw new NotImplementedException();
    }
  }
}
