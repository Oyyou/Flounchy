using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites
{
  public class Enemy : Actor
  {
    private bool _attacked = false;

    private Sprite _tail;

    public Enemy(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Enemy1/EnemyBody");

      SetBorder(graphics);

      var tailTexture = content.Load<Texture2D>("Actor/Enemy1/EnemyTail");

      _tail = new Sprite(tailTexture)
      {
        Position = new Vector2(Position.X + (_texture.Width / 2),
                               Position.Y + ((_texture.Height / 2) - (tailTexture.Height / 2)) + 10),
      };

      SetLeftHand(content.Load<Texture2D>("Actor/Enemy1/EnemyHand"));

      SetRightHand(content.Load<Texture2D>("Actor/Enemy1/EnemyHand"));

      ActionResult = new Engine.ActionResult()
      {
        Status = Engine.ActionStatuses.Waiting,
      };

      _turnBar = new TurnBar(content, new Vector2(Position.X, (Position.Y + Origin.Y) + 15));
    }

    protected override void IdleMovement()
    {
      int max = 3;
      var difference = max - _distance;

      var speed = 0.05f;

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

    public void AttackLeftHand()
    {
      if (_attacked && !LeftHand.Attacking)
      {
        _attacked = false;
        ActionResult.Status = Engine.ActionStatuses.Finished;

        return;
      }

      LeftHand.Attacking = true;

      _attacked = true;
    }

    public void AttackRightHand()
    {
      if (_attacked && !RightHand.Attacking)
      {
        _attacked = false;
        ActionResult.Status = Engine.ActionStatuses.Finished;

        return;
      }

      RightHand.Attacking = true;

      _attacked = true;
    }

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.Status == ActionStatuses.Running)
        return ActionResult;

      var value = Game1.Random.Next(0, 2);
      switch (value)    
      {
        case 0:
          ActionResult.Action = AttackLeftHand;

          break;

        case 1:
          ActionResult.Action = AttackRightHand;

          break;

        default:
          throw new Exception("Unexpected value: " + value);
      }

      ActionResult.Status = ActionStatuses.Running;

      return ActionResult;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      _tail.Draw(gameTime, spriteBatch);

      base.Draw(gameTime, spriteBatch);
    }
  }
}
