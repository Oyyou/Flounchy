using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites.Roaming
{
  public class Fog : Sprite
  {
    public bool Seen { get; set; }

    public Fog(Texture2D texture) : base(texture)
    {
    }
  }
}
