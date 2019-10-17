using Flounchy.Misc;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Roaming
{
  public interface IMoveable
  {
    /// <summary>
    /// The speed the object travels between start and end
    /// </summary>
    int Speed { get; }

    /// <summary>
    /// The current velocity of the object
    /// </summary>
    Vector2 Velocity { get; set; }

    /// <summary>
    /// How far the object has travelled
    /// </summary>
    int DistanceTravelled { get; set; }

    /// <summary>
    /// The map the object is traversing
    /// </summary>
    Map Map { get; set; }

    /// <summary>
    /// Where the object was when they started moving
    /// </summary>
    Rectangle StartRectangle { get; set; }

    /// <summary>
    /// Where the object is right now
    /// </summary>
    Rectangle CurrentRectangle { get; }

    /// <summary>
    /// Where the object will be after moving
    /// </summary>
    Rectangle EndRectangle { get; set; }

    /// <summary>
    /// What will happen if the object walks to the "EndRectangle"
    /// </summary>
    Map.CollisionResults CollisionResult { get; set; }

    /// <summary>
    /// What happens if the object triggers a battle
    /// </summary>
    Action OnBattle { get; set; }

    /// <summary>
    /// How the object determines their movement
    /// </summary>
    Action<GameTime> SetMovement { get; set; }
  }
}
