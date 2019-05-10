using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Skills
{
  public class SwordSkillsModel : SkillsModel
  {
    public SwordSkillsModel(ActorModel actor) 
      : base(actor)
    {
    }

    protected override List<SkillModel> GetSkills()
    {
      return new List<SkillModel>()
      {
        new SkillModel("Slash", _actor.Level >= 1)
        {
          SkillType = SkillModel.SkillTypes.Ability,
          ProceedingSkills = new List<SkillModel>()
          {
            // Quick
            new SkillModel("Cross Slash", _actor.Level >= 2)
            {
              SkillType = SkillModel.SkillTypes.Ability,
              FlowDirection = SkillModel.FlowDirections.Up,
              ProceedingSkills = new List<SkillModel>()
              {
                new SkillModel("Stat 1", _actor.Level >= 3)
                {
                  SkillType = SkillModel.SkillTypes.Stats,
                  FlowDirection = SkillModel.FlowDirections.None,
                  ProceedingSkills = new List<SkillModel>()
                  {
                    new SkillModel("Stat 2", _actor.Level >= 4)
                    {
                      SkillType = SkillModel.SkillTypes.Stats,
                      FlowDirection = SkillModel.FlowDirections.None,
                      ProceedingSkills = new List<SkillModel>()
                      {
                        new SkillModel("Slash X 2", _actor.Level >= 5)
                        {
                          SkillType = SkillModel.SkillTypes.Ability,
                          FlowDirection = SkillModel.FlowDirections.Up,
                          ProceedingSkills = new List<SkillModel>()
                          {
                            new SkillModel("Stat 3", _actor.Level >= 6)
                            {
                              SkillType = SkillModel.SkillTypes.Stats,
                              FlowDirection = SkillModel.FlowDirections.None,
                              ProceedingSkills = new List<SkillModel>()
                              {
                                new SkillModel("Stat 4", _actor.Level >= 7)
                                {
                                  SkillType = SkillModel.SkillTypes.Stats,
                                  FlowDirection = SkillModel.FlowDirections.None,
                                  ProceedingSkills = new List<SkillModel>()
                                  {
                                    new SkillModel("Final", _actor.Level >= 8)
                                    {
                                      SkillType = SkillModel.SkillTypes.Ability,
                                      FlowDirection = SkillModel.FlowDirections.None,
                                    },
                                  }
                                }
                              }
                            },
                          }
                        },
                        new SkillModel("Slash X 3", _actor.Level >= 5)
                        {
                          SkillType = SkillModel.SkillTypes.Ability,
                          FlowDirection = SkillModel.FlowDirections.None,
                          ProceedingSkills = new List<SkillModel>()
                          {
                            new SkillModel("Stat 3", _actor.Level >= 6)
                            {
                              SkillType = SkillModel.SkillTypes.Stats,
                              FlowDirection = SkillModel.FlowDirections.None,
                              ProceedingSkills = new List<SkillModel>()
                              {
                                new SkillModel("Stat 4", _actor.Level >= 7)
                                {
                                  SkillType = SkillModel.SkillTypes.Stats,
                                  FlowDirection = SkillModel.FlowDirections.None,
                                  ProceedingSkills = new List<SkillModel>()
                                  {
                                    new SkillModel("Death Stand", _actor.Level >= 8)
                                    {
                                      SkillType = SkillModel.SkillTypes.Ability,
                                      FlowDirection = SkillModel.FlowDirections.None,
                                    },
                                  }
                                }
                              }
                            },
                          }
                        },
                      }
                    }
                  }
                },
              }
            },
            // Strong
            new SkillModel("Charged Slash", _actor.Level >= 2)
            {
              SkillType = SkillModel.SkillTypes.Ability,
              FlowDirection = SkillModel.FlowDirections.Down,
              ProceedingSkills = new List<SkillModel>()
              {
                new SkillModel("Stat 1", _actor.Level >= 3)
                {
                  SkillType = SkillModel.SkillTypes.Stats,
                  FlowDirection = SkillModel.FlowDirections.None,
                  ProceedingSkills = new List<SkillModel>()
                  {
                    new SkillModel("Stat 2", _actor.Level >= 4)
                    {
                      SkillType = SkillModel.SkillTypes.Stats,
                      FlowDirection = SkillModel.FlowDirections.None,
                      ProceedingSkills = new List<SkillModel>()
                      {
                        new SkillModel("Charge X 2", _actor.Level >= 5)
                        {
                          SkillType = SkillModel.SkillTypes.Ability,
                          FlowDirection = SkillModel.FlowDirections.Down,
                        },
                        new SkillModel("Charge X 3", _actor.Level >= 5)
                        {
                          SkillType = SkillModel.SkillTypes.Ability,
                          FlowDirection = SkillModel.FlowDirections.None,
                        },
                      }
                    }
                  }
                },
              }
            }

          }
        }
      };
    }
  }
}
