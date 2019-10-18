using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Roaming
{
  public interface IInteractable
  {
    bool Interacted { get; set; }
    Action OnInteract { get; set; }
  }
}
