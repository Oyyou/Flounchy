using Engine.Models;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
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

    protected Sprite _leftHandWeapon;

    protected Sprite _rightHandWeapon;

    #region constructors

    /// <summary>
    /// Fists
    /// </summary>
    /// <param name="leftHand"></param>
    /// <param name="rightHand"></param>
    public Equipment(Hand leftHand, Hand rightHand)
      : this(leftHand, rightHand, null)
    {

    }

    /// <summary>
    /// Either a weapon that requires both hands, or a single-handed weapon
    /// </summary>
    /// <param name="leftHand"></param>
    /// <param name="rightHand"></param>
    /// <param name="weapon"></param>
    public Equipment(Hand leftHand, Hand rightHand, Sprite weapon)
      : this(leftHand, rightHand, weapon, null)
    {

    }

    /// <summary>
    /// Two weapons (different or the same)
    /// </summary>
    /// <param name="leftHand"></param>
    /// <param name="rightHand"></param>
    /// <param name="leftHandWeapon"></param>
    /// <param name="rightHandWeapon"></param>
    public Equipment(Hand leftHand, Hand rightHand, Sprite leftHandWeapon, Sprite rightHandWeapon)
    {
      _leftHand = leftHand;
      _rightHand = rightHand;
      _leftHandWeapon = leftHandWeapon;
      _rightHandWeapon = rightHandWeapon;
    }

    #endregion

    public abstract void SetStance(Vector2 position);

    public abstract void SetEquipmentRotation();

    public abstract void OnAttack(AbilityModel ability);
  }
}
