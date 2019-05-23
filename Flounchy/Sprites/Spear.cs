using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public class Spear : Weapon
  {
    public Spear(Texture2D texture)
      : base(texture)
    {
      Origin = new Vector2(texture.Width / 2, texture.Height - 30);
      //Rotation = MathHelper.ToRadians(45);
    }

    public override void OnAttack(string ability, Hand leftHand, Hand rightHand, Weapon leftHandWeapon, Weapon rightHandWeapon)
    {
      var lPoints = new List<Vector2>();
      var rPoints = new List<Vector2>();

      switch (ability)
      {
        case "Slash":
        default:

          SetSlashMovements(leftHand, rightHand, lPoints, rPoints);

          break;

        case "Stab":

          SetStabMovements(leftHand, rightHand, lPoints, rPoints);

          break;
      }

      leftHand.AttackMovement(lPoints);
      rightHand.AttackMovement(rPoints);

      leftHandWeapon.Position = leftHand.Position;
    }

    private static void SetStabMovements(Hand leftHand, Hand rightHand, List<Vector2> lPoints, List<Vector2> rPoints)
    {
      var lPosition = leftHand.Position;
      var rPosition = rightHand.Position;

      int amount = 20;
      for (int i = 0; i < amount; i++)
      {
        var difference = MathHelper.Distance(leftHand.Position.X, rightHand.Position.X);
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

    private static void SetSlashMovements(Hand leftHand, Hand rightHand, List<Vector2> lPoints, List<Vector2> rPoints)
    {
      for (int i = 0; i < 25; i++)
      {
        lPoints.Add(leftHand.Position + new Vector2(0, -(i * 1.0f)));
        rPoints.Add(rightHand.Position + new Vector2(-(i * 1.5f), -(i * 3.0f)));
      }
    }

    public override void OnDefend()
    {
      throw new NotImplementedException();
    }
  }
}
