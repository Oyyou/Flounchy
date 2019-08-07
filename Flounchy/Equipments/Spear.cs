using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;

namespace Flounchy.Equipments
{
  public class Spear : Equipment
  {
    public Spear(Hand leftHand, Hand rightHand, Sprite weapon) 
      : base(leftHand, rightHand, weapon)
    {
    }

    public override void SetStance(Vector2 position)
    {
      _rightHand.Position = position + new Vector2(40, -10);
      _leftHand.Position = position + new Vector2(-40, 30);
      _leftHandWeapon.Position = _leftHand.Position;
    }

    public override void SetEquipmentRotation()
    {
      var distance = _leftHand.Position - _rightHand.Position;

      var roation = (float)Math.Atan2(distance.Y, distance.X);

      _leftHandWeapon.Rotation = roation - MathHelper.ToRadians(90);
    }

    public override void OnAttack(AbilityModel ability)
    {
      var lPoints = new List<Vector2>();
      var rPoints = new List<Vector2>();

      switch (ability.Text)
      {
        case "Slash":
        default:

          SetSlashMovements(lPoints, rPoints);

          break;

        case "Stab":

          SetStabMovements(lPoints, rPoints);

          break;
      }

      _leftHand.AttackMovement(lPoints);
      _rightHand.AttackMovement(rPoints);

      _leftHandWeapon.Position = _leftHand.Position;
    }

    private void SetSlashMovements(List<Vector2> lPoints, List<Vector2> rPoints)
    {
      for (int i = 0; i < 25; i++)
      {
        lPoints.Add(_leftHand.Position + new Vector2(0, -(i * 1.0f)));
        rPoints.Add(_rightHand.Position + new Vector2(-(i * 1.5f), -(i * 3.0f)));
      }
    }

    private void SetStabMovements(List<Vector2> lPoints, List<Vector2> rPoints)
    {
      var lPosition = _leftHand.Position;
      var rPosition = _rightHand.Position;

      int amount = 20;
      for (int i = 0; i < amount; i++)
      {
        var difference = MathHelper.Distance(_leftHand.Position.X, _rightHand.Position.X);
        var speed = difference / amount;

        lPosition += new Vector2(0, (1));
        rPosition += new Vector2(-(speed), -((2)));

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }

      for (int i = 0; i < 10; i++)
      {
        lPosition += new Vector2(0, -3);
        rPosition += new Vector2(0, -3);

        lPoints.Add(lPosition);
        rPoints.Add(rPosition);
      }
    }
  }
}
