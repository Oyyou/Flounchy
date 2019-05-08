using Engine.Models;
using Flounchy.GUI.States;
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

namespace Flounchy.GameStates
{
  public class BattleState : BaseState
  {
    public enum States
    {
      Attacking,
      Dying,
    }

    private int _currentActor;

    /// <summary>
    /// The actor being attacked
    /// </summary>
    private Actor _target;

    private List<Actor> _actors;

    private List<ActorModel> _players;

    private BattleGUI _battleGUI;

    public bool BattleFinished
    {
      get
      {
        return Enemies.Count == 0 ||
          Players.Count == 0;
      }
    }

    public List<Enemy> Enemies
    {
      get
      {
        return _actors
          .Where(c => c is Enemy)
          .Where(c => c.State == Actor.States.Alive)
          .Cast<Enemy>().ToList();
      }
    }

    public List<Player> Players
    {
      get
      {
        return _actors
          .Where(c => c is Player)
          .Where(c => c.State == Actor.States.Alive)
          .Cast<Player>().ToList();
      }
    }

    public BattleState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel)
    {
      _players = players;
    }

    public override void LoadContent()
    {
      var abilityIcon = _content.Load<Texture2D>("Battle/AbilityIcon");

      var position = new Vector2(200, 500);

      _actors = _players.Select(c =>
      {

        var player = new Player(_content, position, _graphics.GraphicsDevice)
        {
          ActorModel = c,
        };

        position += new Vector2(200, 0);

        return player;
      }).Cast<Actor>().ToList();

      _actors.Add(new Enemy(_content, new Vector2(200, 100), _graphics.GraphicsDevice)
      {
        ActorModel = new ActorModel()
        {
          Name = "Klong",
          Attack = 3,
          Defence = 2,
          Health = 10,
          Speed = 3,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          BattleStats = new BattleStatsModel(),
        },
        Colour = Color.White,
        CurrentHealth = 1,
      });
      _actors.Add(new Enemy(_content, new Vector2(400, 100), _graphics.GraphicsDevice)
      {
        ActorModel = new ActorModel()
        {
          Name = "Frank",
          Attack = 3,
          Defence = 2,
          Health = 10,
          Speed = 3,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Ability 2", abilityIcon),
            Ability3 = new AbilityModel("Ability 3", abilityIcon),
            Ability4 = new AbilityModel("Ability 4", abilityIcon),
          },
          BattleStats = new BattleStatsModel(),
        },
        Colour = Color.White,
        CurrentHealth = 2,
      });


      _battleGUI = new BattleGUI(_gameModel);
      _battleGUI.SetAbilities(_actors.First().ActorModel, _actors.First().ActorModel.Abilities);
    }

    public override void Update(GameTime gameTime)
    {
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      foreach (var actor in _actors)
      {
        actor.Update(gameTime);

        actor.ShowBorder = false;
        actor.ShowTurnBar = false;
      }

      ProcessTurns();

      for (int i = 0; i < _actors.Count; i++)
      {
        if (_actors[i].State == Actor.States.Dead)
        {
          _actors.RemoveAt(i);
          i--;
        }
      }

      _battleGUI.Update(gameTime);
    }

    private void ProcessTurns()
    {
      var buttons = _battleGUI.AbilityButtons
        .Where(c => c.Value.IsClicked)
        .ToDictionary(c => c.Key, v => v.Value);

      string ability = null;

      if (buttons.Count > 0)
      {
        var button = buttons.First();

        ability = button.Key;
      }

      var actor = _actors.Where(c => c.State == Actor.States.Alive).ToList()[_currentActor];
      actor.ShowTurnBar = true;

      var actionResult = actor.GetAction(ability);

      if (actionResult == null)
        return;

      if (actionResult.State == Engine.ActionStates.WaitingForTarget)
      {
        if (actor is Player)
        {
          _target = actor.GetTarget(Enemies.Cast<Actor>());
        }
        else if (actor is Enemy)
        {
          _target = actor.GetTarget(Players.Cast<Actor>());
        }
        else
        {
          throw new Exception("Unexpect type: " + actor.ToString());
        }
      }

      if (actionResult.State == Engine.ActionStates.Running)
        actionResult.Action();

      if (actionResult.State != Engine.ActionStates.Finished)
        return;

      // Since the attack is finished, we need to remove health from whoever has been hit     

      var damage = 1;
      _target.CurrentHealth -= damage;
      _target.ActorModel.BattleStats.DamageReceived += damage;
      actor.ActorModel.BattleStats.DamageDealt += damage;

      if (_target.CurrentHealth <= 0)
      {
        _target.State = Actor.States.Dying;

        actor.ActorModel.BattleStats.FinalBlows = 1;

        if (_actors.IndexOf(_target) < _currentActor)
          _currentActor--;
      }

      var validActors = _actors.Where(c => c.State == Actor.States.Alive).ToList();

      _currentActor = (_currentActor + 1) % validActors.Count;
      _battleGUI.SetAbilities(validActors[_currentActor].ActorModel, validActors[_currentActor].ActorModel.Abilities);

      // As the move is over, there is no longer a target
      _target = null;
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      foreach (var actor in _actors)
        actor.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();

      _battleGUI.Draw(gameTime);
    }
  }
}
