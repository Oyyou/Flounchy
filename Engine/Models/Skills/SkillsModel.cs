using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Skills
{
  public abstract class SkillsModel
  {
    protected readonly ActorModel _actor;

    public List<SkillModel> Skills
    {
      get
      {
        return GetSkills();
      }
    }

    public SkillsModel(ActorModel actor)
    {
      _actor = actor;
    }

    protected abstract List<SkillModel> GetSkills();
  }
}
