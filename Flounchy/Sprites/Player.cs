using Engine;
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
    private MouseState _currentMouse;
    private MouseState _previousMouse;

    private bool _attacked = false;

    private Sword _sword;

    public BattleStatsModel BattleStats { get; private set; }

    public Player(ContentManager content, Vector2 position, GraphicsDevice graphics)
      : base(content, position, graphics)
    {
      _texture = content.Load<Texture2D>("Actor/Body");
      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

      _sword = new Sword(content.Load<Texture2D>("Equipment/Sword"));

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
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      base.Update(gameTime);

      _sword.Position = LeftHand.Position;

      if (LeftHand.Attacking)
      {
        if (LeftHand.AttackingDown)
        {
          _sword.Rotation -= MathHelper.ToRadians(2);
        }
        else
        {
          _sword.Rotation += MathHelper.ToRadians(2);
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

      var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

      var enemy = enemies.Where(c => mouseRectangle.Intersects(c.Rectangle)).FirstOrDefault();

      if (enemy != null)
      {
        target = enemy;

        target.ShowBorder = true;

        if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
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

      _sword.Draw(gameTime, spriteBatch);

      base.Draw(gameTime, spriteBatch);
    }
  }
}
