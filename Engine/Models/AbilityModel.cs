using Flounchy.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
  public class AbilityModel
  {
    public readonly Texture2D Icon;

    public readonly string Text;

    public AbilityModel(string text, Texture2D icon)
    {
      Text = text;

      Icon = icon;
    }
  }
}
