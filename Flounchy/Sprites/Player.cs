using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites
{
  public class Player : Actor
  {
    private MouseState _currentMouse;
    private MouseState _previousMouse;

    private bool _attacked = false;

    public Player(ContentManager content, Vector2 position)
      : base(content, position)
    {
      ActionResult = new Engine.ActionResult()
      {
        Action = Attack,
        Status = Engine.ActionStatuses.Waiting,
      };
    }

    public override void Update(GameTime gameTime)
    {
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      //if (_previousMouse.LeftButton == ButtonState.Pressed &&
      //    _currentMouse.LeftButton == ButtonState.Released)
      //{
      //  LeftHand.Attacking = true;
      //}

      //if (_previousMouse.RightButton == ButtonState.Pressed &&
      //    _currentMouse.RightButton == ButtonState.Released)
      //{
      //  RightHand.Attacking = true;
      //}

      base.Update(gameTime);
    }

    public void Attack()
    {
      if(_attacked && !LeftHand.Attacking)
      {
        _attacked = false;
        ActionResult.Status = Engine.ActionStatuses.Finished;

        return;
      }

      LeftHand.Attacking = true;

      _attacked = true;
    }
  }
}
