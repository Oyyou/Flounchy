using Engine.Models.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
  public class ActorModel
  {
    private static int _ids;

    public readonly int Id;

    public string Name { get; set; }

    public int Health { get; set; }

    public int Speed { get; set; }

    public int Defence { get; set; }

    public int Attack { get; set; }

    public int Level { get; set; }

    public AbilitiesModel Abilities { get; set; }

    public BattleStatsModel BattleStats { get; set; }

    public SkillsModel SkillsModel { get; set; }

    public EquipmentModel EquipmentModel { get; set; }

    #region Clothing

    public string Lower { get; set; }

    public string Upper { get; set; }

    #endregion

    public ActorModel()
    {
      Id = _ids++;
    }
  }
}
