using Flounchy.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
  public class AbilityModel
  {
    public enum TargetTypes
    {
      Single,
      All,
    }

    public readonly string IconName;

    public readonly string Text;

    public readonly TargetTypes TargetType;

    public AbilityModel(string text, string iconName, TargetTypes targetType)
    {
      Text = text;

      IconName = iconName;

      TargetType = targetType;
    }
  }
}
