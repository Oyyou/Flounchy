using Engine;
using Engine.Models;
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
  public abstract class Actor : Sprite
  {
    private float _distance = 0;
    private bool _goingUp;

    public ActionResult ActionResult;

    public Hand LeftHand;

    public Hand RightHand;

    public ActorModel ActorModel { get; set; }

    public Actor(ContentManager content, Vector2 position)
      : base(content)
    {
      _texture = content.Load<Texture2D>("Actor/Body");

      Position = position;

      LeftHand = new Hand(content.Load<Texture2D>("Actor/Hand"), -1)
      {
        Position = position + new Vector2(-40, 10),
      };

      RightHand = new Hand(content.Load<Texture2D>("Actor/Hand"), 1)
      {
        Position = position + new Vector2(40, 10),
      };
    }

    public override void Update(GameTime gameTime)
    {
      BobbingMovement();

      AttackMovement();
    }

    private void BobbingMovement()
    {
      int max = 3;
      var difference = max - _distance;

      var speed = 0.1f;// difference * 5;

      if (_goingUp)
      {
        Position.Y -= speed;
        _distance -= speed;
      }
      else
      {
        Position.Y += speed;
        _distance += speed;
      }

      if (_distance >= max ||
          _distance <= 0)
      {
        _goingUp = !_goingUp;
      }
    }

    private void AttackMovement()
    {
      LeftHand.AttackMovement();
      RightHand.AttackMovement();
    }

    public ActionResult GetAction()
    {
      ActionResult.Status = ActionStatuses.Running;

      return ActionResult;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);

      LeftHand.Colour = this.Colour;
      RightHand.Colour = this.Colour;

      LeftHand.Draw(gameTime, spriteBatch);

      RightHand.Draw(gameTime, spriteBatch);
    }
  }
}
