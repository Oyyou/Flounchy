﻿using Engine;
using Engine.Input;
using Engine.Models;
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

    public Sword LeftHandWeapon;

    public BattleStatsModel BattleStats { get; private set; }

    public Clothing Lower = null;
    public Clothing Upper = null;

    public Player(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Body");
      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

      LeftHandWeapon = new Sword(content.Load<Texture2D>("Equipment/Sword"));

      SetBorder(graphics);

      SetLeftHand(content.Load<Texture2D>("Actor/Hand"));

      SetRightHand(content.Load<Texture2D>("Actor/Hand"));

      ActionResult = new Engine.ActionResult()
      {
        Action = Attack,
        State = Engine.ActionStates.Waiting,
      };

      _turnBar = new TurnBar(content, new Vector2(Position.X, (Position.Y + Origin.Y) + 15));
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      LeftHandWeapon.Position = LeftHand.Position;

      if (LeftHand.Attacking)
      {
        if (LeftHand.AttackingDown)
        {
          LeftHandWeapon.Rotation -= MathHelper.ToRadians(2);
        }
        else
        {
          LeftHandWeapon.Rotation += MathHelper.ToRadians(2);
        }
      }
    }

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.State != ActionStates.Waiting && ActionResult.State != ActionStates.Finished)
        return ActionResult;

      ActionResult.State = ActionStates.Waiting;

      var abilityList = ActorModel.Abilities.Get();
      foreach (var abil in abilityList)
      {
        if (abil.Text == ability)
        {
          ActionResult.State = ActionStates.WaitingForTarget;
        }
      }

      return ActionResult;
    }

    public override Actor GetTarget(IEnumerable<Actor> enemies)
    {
      Actor target = null;

      var enemy = enemies.Where(c => GameMouse.Intersects(c.Rectangle)).FirstOrDefault();

      if (enemy != null)
      {
        target = enemy;

        target.ShowBorder = true;

        if (GameMouse.IsLeftClicked)
        {
          ActionResult.State = Engine.ActionStates.Running;
        }
      }

      return target;
    }

    public void Attack()
    {
      if (_attacked && !LeftHand.Attacking)
      {
        _attacked = false;
        ActionResult.State = Engine.ActionStates.Finished;

        return;
      }

      LeftHand.Attacking = true;

      _attacked = true;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

      LeftHandWeapon?.Draw(gameTime, spriteBatch);

      base.Draw(gameTime, spriteBatch);

      if (Upper != null)
      {
        Upper.ShowBack();
        Upper.Position = new Vector2(Position.X, Position.Y);
        Upper.Draw(gameTime, spriteBatch);
      }

      if (Lower != null)
      {
        Lower.ShowBack();
        Lower.Position = new Vector2(Position.X, (Position.Y + Origin.Y) - (Lower.Origin.Y));
        Lower.Draw(gameTime, spriteBatch);
      }
    }
  }
}
