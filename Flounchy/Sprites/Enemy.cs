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
    private Sprite _tail;

    public override float Opacity
    {
      get { return _opacity; }
      set
      {
        _opacity = value;

        RightHand.Opacity = _opacity;
        LeftHand.Opacity = _opacity;
        _tail.Opacity = _opacity;
      }
    }

    public Enemy(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Enemy1/EnemyBody");
      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

      SetBorder(graphics);

      var tailTexture = content.Load<Texture2D>("Actor/Enemy1/EnemyTail");

      _tail = new Sprite(tailTexture)
      {
        Position = new Vector2(Position.X + (_texture.Width / 2),
                               Position.Y + ((_texture.Height / 2) - (tailTexture.Height / 2)) + 10),
      };

      SetLeftHand(content.Load<Texture2D>("Actor/Enemy1/EnemyRHand"));

      SetRightHand(content.Load<Texture2D>("Actor/Enemy1/EnemyLHand"));

      ActionResult = new Engine.ActionResult()
      {
        State = Engine.ActionStates.Waiting,
      };

      _turnBar = new TurnBar(content, new Vector2(Position.X, (Position.Y + Origin.Y) + 15));

      this._equipment = new Equipments.Fists(LeftHand, RightHand);
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

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.State == ActionStates.Running)
        return ActionResult;

      var abilities = ActorModel.Abilities.Get();

      _ability = abilities[Game1.Random.Next(0, abilities.Count)];

      ActionResult.Action = Attack;

      ActionResult.State = ActionStates.WaitingForTarget;

      return ActionResult;
    }

    public override List<Actor> GetTargets(IEnumerable<Actor> players)
    {
      var targets = new List<Actor>();

      var values = players.Select(c =>
      {
        var endValue = 0;

        var _a = Math.Max(1, (c.CurrentHealth + (double)c.MaxHealth));
        var _b = Math.Max(1, (c.MaxHealth - (double)c.CurrentHealth));
        var _c = _a * _b;

        // 1st calculation is who is closest to dying
        endValue += (int)Math.Max(1, Math.Ceiling(_c));

        // 2nd is the damage dealer
        endValue += c.ActorModel.Attack;

        return endValue;

      }).ToList();

      switch (_ability.TargetType)
      {
        case Engine.Models.AbilityModel.TargetTypes.Single:

          var randValue = Game1.Random.Next(0, values.Sum());

          int total = 0;
          int i = 0;

          for (i = 0; i < values.Count; i++)
          {
            total += values[i];

            if (randValue < total)
            {
              break;
            }
          }

          // Add logic in here to determine which player is being attacked
          targets.Add(players.ToList()[i]);

          break;
        case Engine.Models.AbilityModel.TargetTypes.All:

          targets.AddRange(players);

          break;
        default:
          break;
      }

      ActionResult.State = Engine.ActionStates.Running;

      return targets;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      _tail.Draw(gameTime, spriteBatch);

      base.Draw(gameTime, spriteBatch);

      LeftHandWeapon?.Draw(gameTime, spriteBatch);
      RightHandWeapon?.Draw(gameTime, spriteBatch);
    }
  }
}
