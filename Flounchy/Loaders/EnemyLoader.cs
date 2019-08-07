using Engine.Models;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Loaders
{
  public class EnemyLoader
  {
    private string _abilityIcon;

    public enum Areas
    {
      CalmLands
    }

    public EnemyLoader(ContentManager content)
    {
      _abilityIcon = "Battle/AbilityIcon";
    }

    public List<ActorModel> Load(Areas area)
    {
      switch (area)
      {
        case Areas.CalmLands:

          return LoadCalmLands();

        default:
          break;
      }

      return new List<ActorModel>();
    }

    private List<ActorModel> LoadCalmLands()
    {
      // Group 1 - 50
      // Group 2 - 40
      // Group 3 - 10
      // 145 (0, 144)

      var groups = new List<Tuple<int, List<ActorModel>>>()
      {
        new Tuple<int, List<ActorModel>>( // Group 1
          50,
          new List<ActorModel>()
          {
            new ActorModel()
            {
              Name = "Klong",
              Attack = 2,
              Defence = 2,
              Health = 10,
              Speed = 3,
              Abilities = new AbilitiesModel()
              {
                Ability1 = new AbilityModel("Slash", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability2 = new AbilityModel("Ability 2", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability3 = new AbilityModel("Ability 3", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability4 = new AbilityModel("Ability 4", _abilityIcon, AbilityModel.TargetTypes.All),
              },
              BattleStats = new BattleStatsModel(),
              EquipmentModel = new EquipmentModel()
              {
                EquipmentType = EquipmentModel.EquipmentTypes.Fists,
                LeftHandEquipmentPath = null,
                RightHandEquipmentPath = null,
              },
            },
            new ActorModel()
            {
              Name = "Frank",
              Attack = 3,
              Defence = 1,
              Health = 10,
              Speed = 3,
              Abilities = new AbilitiesModel()
              {
                Ability1 = new AbilityModel("Slash", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability2 = new AbilityModel("Stab", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability3 = new AbilityModel("Shank", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability4 = new AbilityModel("Shiv", _abilityIcon, AbilityModel.TargetTypes.All),
              },
              BattleStats = new BattleStatsModel(),
              EquipmentModel = new EquipmentModel()
              {
                EquipmentType = EquipmentModel.EquipmentTypes.Both_Spear,
                LeftHandEquipmentPath = "Equipment/Spear",
                RightHandEquipmentPath = null,
              },
            }
          }
        ),
        new Tuple<int, List<ActorModel>>( // Group 2
          1,
          new List<ActorModel>()
          {
            new ActorModel()
            {
              Name = "Frank",
              Attack = 3,
              Defence = 2,
              Health = 10,
              Speed = 3,
              Abilities = new AbilitiesModel()
              {
                Ability1 = new AbilityModel("Slash", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability2 = new AbilityModel("Stab", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability3 = new AbilityModel("Shank", _abilityIcon, AbilityModel.TargetTypes.Single),
                Ability4 = new AbilityModel("Shiv", _abilityIcon, AbilityModel.TargetTypes.All),
              },
              BattleStats = new BattleStatsModel(),
              EquipmentModel = new EquipmentModel()
              {
                EquipmentType = EquipmentModel.EquipmentTypes.Both_Spear,
                LeftHandEquipmentPath = "Equipment/Spear",
                RightHandEquipmentPath = null,
              },
            }
          }
        ),
      };

      return GetGroup(groups);
    }

    private static List<ActorModel> GetGroup(List<Tuple<int, List<ActorModel>>> groups)
    {
      var randMax = groups.Sum(c => c.Item1);
      int randValue = Game1.Random.Next(0, randMax);

      int index = 0;

      for (int i = groups[index].Item1; i <= randMax; i += groups[index].Item1)
      {
        if (randValue < i)
          return groups[index].Item2;

        index++;
      }

      throw new Exception("Random index is out: " + randMax);
    }
  }
}
