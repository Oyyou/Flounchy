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
    private bool _attackingDown = false;
    private int _attackPointIndex = 0;
    private List<Vector2> _points = new List<Vector2>();

    private float _xSpeed;

    public bool Attacking { get; set; } = false;

    public Vector2? StartPosition = null;

    public Hand(Texture2D texture, float xSpeed) : base(texture)
    {
      _xSpeed = xSpeed;
    }

    public void AttackMovement()
    {
      if (StartPosition == null)
        StartPosition = this.Position;

      if (!Attacking)
        return;

      SetAttackPoints();

      this.Position = _points[_attackPointIndex];

      if (!_attackingDown)
        _attackPointIndex++;
      else
      {
        _attackPointIndex--;
      }

      if (_attackPointIndex >= _points.Count)
      {
        _attackingDown = true;
        _attackPointIndex--;
      }

      if (_attackPointIndex <= 0)
      {
        _attackPointIndex = 0;
        _points = new List<Vector2>();
        Attacking = false;
        _attackingDown = false;
        this.Position = StartPosition.Value;
      }
    }

    private void SetAttackPoints()
    {
      if (_points.Count > 0)
        return;

      var start = this.Position;
      var end = this.Position + new Vector2(0, -90);

      var difference = Vector2.Distance(start, end);

      var x = start.X;

      for (int i = 0; i < difference; i += 3)
      {
        if (i < difference / 2)
        {
          x += _xSpeed;
        }
        else
        {
          x -= _xSpeed;
        }

        _points.Add(new Vector2(x, start.Y - i));
      }
    }
  }
}
