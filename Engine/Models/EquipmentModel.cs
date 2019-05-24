using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models
{
  public class EquipmentModel
  {
    public enum StanceTypes
    {
      Fists,
      SingleHanded,
      DualHanded,
      BothHands,
    }

    public enum EquipmentTypes
    {
      Fists,
      Single_Sword,
      Single_Shield,
      Single_Axe,
      Single_Hammer,
      Single_Wand,
      Dual_Sword,
      Dual_Shield,
      Dual_Axe,
      Dual_Hammer,
      Dual_Wand,
      Sword_AndShield,
      Axe_AndShield,
      Hammer_AndShield,
      Wand_AndShield,
      Both_Spear,
      Both_Staff,
    }

    public EquipmentTypes EquipmentType { get; set; }

    public string LeftHandEquipmentPath { get; set; }

    public string RightHandEquipmentPath { get; set; }

    public StanceTypes GetStanceType()
    {
      if (EquipmentType.ToString().Contains("Both"))
        return StanceTypes.BothHands;      

      if (EquipmentType.ToString().Contains("Single"))
        return StanceTypes.SingleHanded;

      if (EquipmentType.ToString().Contains("Dual"))
        return StanceTypes.DualHanded;

      if (EquipmentType.ToString().Contains("AndShield"))
        return StanceTypes.DualHanded;

      if (EquipmentType == EquipmentTypes.Fists)
        return StanceTypes.Fists;

      throw new Exception("'EquipmentType' must contain one of these to be valid: 'Single', Dual', 'AndShield', or 'Both'. Incorrect value is: " + EquipmentType.ToString());
    }
  }
}
