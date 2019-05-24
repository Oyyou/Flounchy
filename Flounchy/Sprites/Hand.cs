using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public class Hand : Sprite
  {
    public enum States
    {
      WaitingToAttack,
      Attacking,
      FinishedAttacking,
    }

    private int _attackPointIndex = 0;
    private List<Vector2> _points = new List<Vector2>();

    //public bool Attacking { get; set; } = false;

    public bool AttackingDown = false;

    public Vector2? StartPosition = null;

    public States State { get; set; }

    public Hand(Texture2D texture) : base(texture)
    {
      State = States.WaitingToAttack;
    }

    public void AttackMovement(List<Vector2> points = null, bool oneWay = false)
    {
      if (StartPosition == null)
        StartPosition = this.Position;

      SetAttackPoints(points);

      State = States.Attacking;

      this.Position = _points[_attackPointIndex];

      if (!AttackingDown)
        _attackPointIndex++;
      else
      {
        _attackPointIndex--;
      }

      if (_attackPointIndex >= _points.Count)
      {
        AttackingDown = true;
        _attackPointIndex--;

        // This will leave the hand at the final point
        if (oneWay)
        {
          _attackPointIndex = 0;
          if (_points.Last() != StartPosition)
            throw new Exception("Attack has been set to 'OneWay' but doesn't end at the start position");
        }
      }

      if (_attackPointIndex <= 0)
      {
        _attackPointIndex = 0;
        _points = new List<Vector2>();
        //Attacking = false;
        AttackingDown = false;
        this.Position = StartPosition.Value;
        State = States.FinishedAttacking;
      }
    }

    private void SetAttackPoints(List<Vector2> points = null)
    {
      if (_points.Count > 0)
        return;

      if (points != null)
        _points = points;
    }
  }
}
