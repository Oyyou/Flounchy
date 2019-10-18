using Engine;
using Engine.Input;
using Engine.Models;
using Flounchy.Equipments;
using Flounchy.Misc;
using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.Battle
{
  public class Player : Actor
  {
    public BattleStatsModel BattleStats { get; private set; }

    public Clothing Lower = null;
    public Clothing Upper = null;

    public Player(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Body");
      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

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

    public override ActionResult GetAction(string ability)
    {
      if (ActionResult.State != ActionStates.Waiting && ActionResult.State != ActionStates.Finished)
        return ActionResult;

      ActionResult.State = ActionStates.Waiting;

      if (ability == null)
        return ActionResult;

      _ability = null;

      var abilityList = ActorModel.Abilities.Get();
      foreach (var abil in abilityList)
      {
        if (abil.Text == ability)
        {
          ActionResult.State = ActionStates.WaitingForTarget;
          _ability = abil;
        }
      }

      return ActionResult;
    }

    public override List<Actor> GetTargets(IEnumerable<Actor> enemies)
    {
      var targets = new List<Actor>();

      var enemy = enemies.Where(c => GameMouse.Intersects(c.Rectangle)).FirstOrDefault();

      if (enemy != null)
      {
        switch (_ability.TargetType)
        {
          case AbilityModel.TargetTypes.Single:

            enemy.ShowBorder = true;

            targets.Add(enemy);

            break;
          case AbilityModel.TargetTypes.All:

            foreach(var target in enemies)
            {
              target.ShowBorder = true;
              targets.Add(target);
            }

            break;
          default:
            break;
        }

        if (GameMouse.IsLeftClicked)
        {
          ActionResult.State = Engine.ActionStates.Running;
        }
      }

      return targets;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      LeftHandWeapon?.Draw(gameTime, spriteBatch);
      RightHandWeapon?.Draw(gameTime, spriteBatch);

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
