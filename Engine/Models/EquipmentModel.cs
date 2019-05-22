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
      SingleHanded,
      BothHands,
    }

    public enum EquipmentTypes
    {
      Fist,
      Sword,
      Shield,
      Axe,
      Hammer,
      Spear,
      Staff,
      Wand,
    }

    public EquipmentTypes LeftHandEquipment { get; set; }

    public EquipmentTypes RightHandEquipment { get; set; }

    public string LeftHandEquipmentPath { get; set; }

    public string RightHandEquipmentPath { get; set; }

    public StanceTypes GetStanceType()
    {
      var equipments = new List<EquipmentTypes>()
      {
        LeftHandEquipment,
        RightHandEquipment,
      };

      var twoHandedWeapons = new List<EquipmentTypes>()
      {
        EquipmentTypes.Spear,
        EquipmentTypes.Staff,
      };

      if (equipments.Any(c => twoHandedWeapons.Contains(c)))
      {
        return StanceTypes.BothHands;
      }

      return StanceTypes.SingleHanded;
    }
  }
}
