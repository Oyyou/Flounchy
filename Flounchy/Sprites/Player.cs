using Engine;
using Flounchy.Misc;
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
    private bool _attacked = false;

    public Player(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Body");

      SetBorder(graphics);

      SetLeftHand(content.Load<Texture2D>("Actor/Hand"));

      SetRightHand(content.Load<Texture2D>("Actor/Hand"));

      ActionResult = new Engine.ActionResult()
      {
        Action = Attack,
        Status = Engine.ActionStatuses.Waiting,
      };

      _turnBar = new TurnBar(content, new Vector2(Position.X, (Position.Y + Origin.Y) + 15));
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.Status != ActionStatuses.Waiting && ActionResult.Status != ActionStatuses.Finished)
        return ActionResult;

      ActionResult.Status = ActionStatuses.Waiting;

      var abilityList = Abilities.Get();
      foreach (var abil in abilityList)
      {
        if (abil.Text == ability)
        {
          ActionResult.Status = ActionStatuses.WaitingForTarget;
        }
      }

      return ActionResult;
    }

    public void Attack()
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
  }
}
