using Flounchy.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Equipments
{
  public abstract class Equipment
  {
    protected Hand _leftHand;

    protected Hand _rightHand;

    protected Weapon _leftHandWeapon;

    protected Weapon _rightHandWeapon;

    public Equipment(Hand leftHand, Hand rightHand, Weapon leftHandWeapon, Weapon rightHandWeapon)
    {
      _leftHand = leftHand;
      _rightHand = rightHand;
      _leftHandWeapon = leftHandWeapon;
      _rightHandWeapon = rightHandWeapon;
    }

    public Equipment(Hand leftHand, Hand rightHand, Weapon weapon)
    {
      _leftHand = leftHand;
      _rightHand = rightHand;
      _leftHandWeapon = weapon;
    }

    public abstract void SetEquipmentRotation();

    public abstract void OnAttack(string ability);
  }
}
