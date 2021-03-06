﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
  public class AbilitiesModel
  {
    public AbilityModel Ability1 { get; set; }

    public AbilityModel Ability2 { get; set; }

    public AbilityModel Ability3 { get; set; }

    public AbilityModel Ability4 { get; set; }

    /// <summary>
    /// Get the abilities for this actor
    /// </summary>
    /// <returns>A list of all the abilities</returns>
    public List<AbilityModel> Get()
    {
      return new List<AbilityModel>()
      {
        Ability1,
        Ability2,
        Ability3,
        Ability4,
      };
    }
  }
}
