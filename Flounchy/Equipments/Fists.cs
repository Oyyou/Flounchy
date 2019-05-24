using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;

namespace Flounchy.Equipments
{
  public class Fists : Equipment
  {
    public Fists(Hand leftHand, Hand rightHand)
      : base(leftHand, rightHand)
    {
    }

    public override void SetStance(Vector2 position)
    {

    }

    public override void SetEquipmentRotation()
    {

    }

    public override void OnAttack(string ability)
    {
      switch (ability)
      {

        case "Slap":
        default:
          Slap();
          break;

        case "xyz":
          xyz();
          break;
      }
    }

    private void Slap()
    {
      var points = new List<Vector2>();

      var start = _leftHand.Position;
      var end = start + new Vector2(0, -90);

      var difference = Vector2.Distance(start, end);

      var x = start.X;

      var speed = -1;

      for (int i = 0; i < difference; i += 3)
      {
        if (i < difference / 2)
        {
          x += speed;
        }
        else
        {
          x -= speed;
        }

        points.Add(new Vector2(x, start.Y - i));
      }

      _leftHand.AttackMovement(points);
    }

    private void xyz()
    {
      var lPoints = new List<Vector2>();
      var rPoints = new List<Vector2>();

      var lPosition = _leftHand.Position;
      var rPosition = _rightHand.Position;

      for (int i = 0; i < 50; i++)
      {
        lPosition += new Vector2(-1, 0);
        rPosition += new Vector2(1, 0);

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }

      for (int i = 0; i < 50; i++)
      {
        lPosition += new Vector2(1.5f, -1.5f);
        rPosition += new Vector2(-1.5f, -1.5f);

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }

      for (int i = 0; i < 5; i++)
      {
        lPosition += new Vector2(0, -10f);
        rPosition += new Vector2(0, -10f);

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }

      var lDifference = _leftHand.Position - lPosition;
      var rDifference = _rightHand.Position - rPosition;

      for (int i = 0; i < 50; i++)
      {
        lPosition += lDifference / 50;
        rPosition += rDifference / 50;

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }

      _leftHand.AttackMovement(lPoints, true);
      _rightHand.AttackMovement(rPoints, true);
    }
  }
}
