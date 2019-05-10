using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Skills
{
  public class SkillModel
  {
    public enum SkillTypes
    {
      Stats,
      Ability,
    }

    public enum FlowDirections
    {
      None,
      Up,
      Down,
    }

    public bool IsUnlocked { get; set; }

    public readonly bool Condition;

    public readonly string Name;

    public List<SkillModel> ProceedingSkills = new List<SkillModel>();

    public FlowDirections FlowDirection { get; set; }

    public SkillTypes SkillType { get; set; }

    public SkillModel(string name, bool condition)
    {
      Name = name;
      Condition = condition;
    }
  }
}
