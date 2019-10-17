﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.Sprites.Roaming
{
  public class Player : Sprite, IMoveable
  {
    public Sprite Lower;

    public override Rectangle CollisionRectangle => new Rectangle((int)Position.X, (int)Position.Y + 40, 40, 40);

    public bool EnterBattle { get; set; }

    #region IMoveable
    public int Speed
    {
      get
      {
        return 4;
      }
    }
    public Vector2 Velocity { get; set; }
    public int DistanceTravelled { get; set; }
    public Map Map { get; set; }
    public Rectangle StartRectangle { get; set; }
    public Rectangle CurrentRectangle
    {
      get
      {
        return new Rectangle(
          (int)Position.X,
          (int)Position.Y + 40,
          40,
          40);
      }
    }

    public Rectangle EndRectangle { get; set; }
    public Map.CollisionResults CollisionResult { get; set; }
    public Action OnBattle { get; set; }
    public Action<GameTime> SetMovement { get; set; }
    #endregion

    public Player(ContentManager content, Texture2D texture, Map map)
      : base(content)
    {
      Map = map;

      _texture = texture;

      Lower = new Sprite(content.Load<Texture2D>("Clothing/Lower/Clover"));

      OnBattle = () => EnterBattle = true;

      SetMovement = (gameTime) => SetMovementEvent(gameTime);
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      if (GameKeyboard.IsKeyDown(Keys.D))
      {
        Velocity = new Vector2(Speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.A))
      {
        Velocity = new Vector2(-Speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.W))
      {
        Velocity = new Vector2(0, -Speed);
      }
      else if (GameKeyboard.IsKeyDown(Keys.S))
      {
        Velocity = new Vector2(0, Speed);
      }
    }

    public bool IsInRange(Vector2 position)
    {
      return Vector2.Distance(position, this.Position) <= 160;
    }

    public override void Update(GameTime gameTime)
    {
      CollisionResult = Map.CollisionResults.None;
      EnterBattle = false;

      Move(gameTime, this);

      Position += Velocity;

      Lower.Position = new Vector2(
        Position.X + (_texture.Width / 2),
        (Position.Y + (_texture.Height) - 5));
    }

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

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);

      //Lower.Draw(gameTime, spriteBatch);
    }
  }
}
