using Flounchy.Entities;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flounchy.Components
{
  public class MoveComponent : Component
  {

    /// <summary>
    /// How far the object has travelled
    /// </summary>
    private int _distanceTravelled;

    /// <summary>
    /// Where the object was when they started moving
    /// </summary>
    private Rectangle _startRectangle;

    /// <summary>
    /// Where the object will be after moving
    /// </summary>
    private Rectangle _endRectangle;

    /// <summary>
    /// What will happen if the object walks to the "EndRectangle"
    /// </summary>
    public Map.CollisionResults CollisionResult;

    /// <summary>
    /// The speed the object travels between start and end
    /// </summary>
    public int Speed { get; set; }

    /// <summary>
    /// The current velocity of the object
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// The map the object is traversing
    /// </summary>
    public readonly Map Map;

    /// <summary>
    /// Where the object is right now
    /// </summary>
    public Rectangle CurrentRectangle
    {
      get
      {
        return new Rectangle(
          (int)Parent.Position.X + CurrentRectangleOffset.X,
          (int)Parent.Position.Y + CurrentRectangleOffset.Y,
          40 + CurrentRectangleOffset.Width,
          40 + CurrentRectangleOffset.Height);
      }
    }

    public Rectangle CurrentRectangleOffset { get; set; }

    /// <summary>
    /// What happens if the object triggers a battle
    /// </summary>
    public Action OnBattle { get; set; }

    /// <summary>
    /// How the object determines their movement
    /// </summary>
    public Action<GameTime> SetMovement { get; set; }

    public MoveComponent(Entity parent, Map map, Action<GameTime> setMovement) : base(parent)
    {
      Map = map;

      SetMovement = setMovement;

      // Defaults
      Speed = 1;
    }

    public override void Update(GameTime gameTime)
    {
      Move(gameTime);

      Parent.Position += Velocity;
    }

    private void Move(GameTime gameTime)
    {
      if (Map.TileWidth % Speed != 0)
        throw new Exception($"Speed ({Speed}) doesn't fit into '{Map.TileWidth}'");

      var speed = Speed;

      CollisionResult = Map.CollisionResults.None;

      if (_distanceTravelled > 0)
      {
        _distanceTravelled += speed;

        if (_distanceTravelled > Map.TileWidth)
        {
          _distanceTravelled = 0;
          Velocity = new Vector2();
          Map.RemoveItem(_startRectangle);
          Map.Write();
        }
        else
        {
          return;
        }
      }

      SetMovement(gameTime);

      if (Velocity != Vector2.Zero)
      {
        var xSpeed = 0;
        var ySpeed = 0;

        if (Velocity.X < 0)
          xSpeed = -Map.TileWidth;
        else if (Velocity.X > 0)
          xSpeed = Map.TileWidth;

        if (Velocity.Y < 0)
          ySpeed = -Map.TileHeight;
        else if (Velocity.Y > 0)
          ySpeed = Map.TileHeight;

        _startRectangle = CurrentRectangle;
        _endRectangle = new Rectangle(
          CurrentRectangle.X + xSpeed,
          CurrentRectangle.Y + ySpeed,
          CurrentRectangle.Width,
          CurrentRectangle.Height);

        CollisionResult = Map.GetValue(_endRectangle);

        switch (CollisionResult)
        {
          case Map.CollisionResults.None:
            Map.AddItem(_endRectangle);
            Map.Write();
            break;
          case Map.CollisionResults.Colliding:
            Velocity = new Vector2();
            break;
          case Map.CollisionResults.Battle:
            OnBattle?.Invoke();
            Velocity = new Vector2();
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

      if (Velocity != Vector2.Zero)
      {
        _distanceTravelled += speed;
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

    }
  }
}
