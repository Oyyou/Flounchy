﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Input;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flounchy.Sprites.Roaming
{
  public class Player : Sprite
  {
    private Vector2 _velocity;

    private int _distanceTravelled = 0;

    private Map _map;

    public Player(Texture2D texture, Map map)
      : base(texture)
    {
      _map = map;
    }

    public override void Update(GameTime gameTime)
    {
      Move();

      Position += _velocity;
    }

    private void Move()
    {
      int speed = 4;

      if (_distanceTravelled > 0)
      {
        _distanceTravelled += speed;

        if (_distanceTravelled > Map.TileWidth)
        {
          _distanceTravelled = 0;
          _velocity = new Vector2();
        }
        else
        {
          return;
        }
      }

      if (GameKeyboard.IsKeyDown(Keys.D))
      {
        _velocity = new Vector2(speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.A))
      {
        _velocity = new Vector2(-speed, 0);
      }
      else if (GameKeyboard.IsKeyDown(Keys.W))
      {
        _velocity = new Vector2(0, -speed);
      }
      else if (GameKeyboard.IsKeyDown(Keys.S))
      {
        _velocity = new Vector2(0, speed);
      }

      var newRectangle = new Rectangle(Rectangle.X + ((int)_velocity.X * 10), Rectangle.Y + ((int)_velocity.Y * 10), Rectangle.Width, Rectangle.Height);

      if (_map.GetValue(newRectangle) == 1)
      {
        _velocity = new Vector2();
        //_map.Write();
      }

      if (_velocity != Vector2.Zero)
        _distanceTravelled += speed;
    }
  }
}
