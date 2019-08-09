using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.GUI.Controls.Buttons
{
  public class TurnButton : Button
  {
    public readonly int ActorId;

    public TurnButton(Texture2D texture, SpriteFont font, int actorId) : base(texture, font)
    {
      ActorId = actorId;
    }
  }
}
