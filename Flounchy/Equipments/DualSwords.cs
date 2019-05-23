using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flounchy.Sprites;

namespace Flounchy.Equipments
{
  public class DualSwords : Equipment
  {
    public DualSwords(Hand leftHand, Hand rightHand, Weapon leftHandWeapon, Weapon rightHandWeapon) 
      : base(leftHand, rightHand, leftHandWeapon, rightHandWeapon)
    {
    }

    public override void SetEquipmentRotation()
    {
      throw new NotImplementedException();
    }

    public override void OnAttack(string ability)
    {
      throw new NotImplementedException();
    }
  }
}
