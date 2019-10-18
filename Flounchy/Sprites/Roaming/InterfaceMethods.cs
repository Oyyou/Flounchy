using Flounchy.Misc;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Roaming
{
  /// <summary>
  /// Move all these methods to the interfaces once we have C# 8
  /// </summary>
  public static class InterfaceMethods
  {
    public static void Move(GameTime gameTime, IMoveable moveableObj)
    {
      var speed = moveableObj.Speed;

      if (moveableObj.DistanceTravelled > 0)
      {
        moveableObj.DistanceTravelled += speed;

        if (moveableObj.DistanceTravelled > Map.TileWidth)
        {
          moveableObj.DistanceTravelled = 0;
          moveableObj.Velocity = new Vector2();
          moveableObj.Map.RemoveItem(moveableObj.StartRectangle);
          moveableObj.Map.Write();
        }
        else
        {
          return;
        }
      }

      moveableObj.SetMovement(gameTime);

      if (moveableObj.Velocity != Vector2.Zero)
      {
        var xSpeed = 0;
        var ySpeed = 0;

        if (moveableObj.Velocity.X < 0)
          xSpeed = -Map.TileWidth;
        else if (moveableObj.Velocity.X > 0)
          xSpeed = Map.TileWidth;

        if (moveableObj.Velocity.Y < 0)
          ySpeed = -Map.TileHeight;
        else if (moveableObj.Velocity.Y > 0)
          ySpeed = Map.TileHeight;

        moveableObj.StartRectangle = moveableObj.CurrentRectangle;
        moveableObj.EndRectangle = new Rectangle(
          moveableObj.CurrentRectangle.X + xSpeed,
          moveableObj.CurrentRectangle.Y + ySpeed,
          moveableObj.CurrentRectangle.Width,
          moveableObj.CurrentRectangle.Height);

        moveableObj.CollisionResult = moveableObj.Map.GetValue(moveableObj.EndRectangle);

        switch (moveableObj.CollisionResult)
        {
          case Map.CollisionResults.None:
            moveableObj.Map.AddItem(moveableObj.EndRectangle);
            moveableObj.Map.Write();
            break;
          case Map.CollisionResults.Colliding:
            moveableObj.Velocity = new Vector2();
            break;
          case Map.CollisionResults.Battle:
            moveableObj.OnBattle();
            moveableObj.Velocity = new Vector2();
            break;
          case Map.CollisionResults.OffRight:
          case Map.CollisionResults.OffLeft:
          case Map.CollisionResults.OffTop:
          case Map.CollisionResults.OffBottom:
            speed = 0;
            break;
          default:
            break;
        }
      }

      if (moveableObj.Velocity != Vector2.Zero)
        moveableObj.DistanceTravelled += speed;
    }
  }
}
