using Engine.Models;
using Flounchy.GUI.States;
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

    private Sprite _background;

    private List<Actor> _actors;

    private BattleStateGUI _battleGUI;

    private List<string> _conversation;

    private ChatBox _chatBox;

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

    public BattleState(GameModel gameModel, List<ActorModel> players, List<string> conversation = null)
      : base(gameModel, players)
    {
      _conversation = conversation ?? new List<string>();
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
          LeftHandWeapon = GetWeapon(c.EquipmentModel.EquipmentType, c.EquipmentModel.LeftHandEquipmentPath),
          Lower = !string.IsNullOrEmpty(c.Lower) ? new Clothing(_content.Load<Texture2D>(c.Lower)) : null,
          Upper = !string.IsNullOrEmpty(c.Upper) ? new Clothing(_content.Load<Texture2D>(c.Upper)) : null,
        };

        position += new Vector2(200, 0);

        return player;
      }).Cast<Actor>().ToList();

      var grassTexture = _content.Load<Texture2D>("Battle/Grasses/Grass");
      _background = new Sprite(grassTexture)
      {
        Position = new Vector2(grassTexture.Width / 2, grassTexture.Height / 2),
      };

      var enemies = new List<ActorModel>()
      {
        new ActorModel()
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
          EquipmentModel = new EquipmentModel()
          {
            EquipmentType = EquipmentModel.EquipmentTypes.Fists,
            LeftHandEquipmentPath = null,
            RightHandEquipmentPath = null,
          },
        },
        new ActorModel()
        {
          Name = "Frank",
          Attack = 3,
          Defence = 2,
          Health = 10,
          Speed = 3,
          Abilities = new AbilitiesModel()
          {
            Ability1 = new AbilityModel("Slash", abilityIcon),
            Ability2 = new AbilityModel("Stab", abilityIcon),
            Ability3 = new AbilityModel("Slash", abilityIcon),
            Ability4 = new AbilityModel("Stab", abilityIcon),
          },
          BattleStats = new BattleStatsModel(),
          EquipmentModel = new EquipmentModel()
          {
            EquipmentType = EquipmentModel.EquipmentTypes.Both_Spear,
            LeftHandEquipmentPath = "Equipment/Spear",
            RightHandEquipmentPath = null,
          },
        }
      };

      var enemyPosition = new Vector2(200, 100);

      _actors.AddRange(enemies.Select(c =>
       {
         var enemy = new Enemy(_content, enemyPosition, _graphics.GraphicsDevice)
         {
           ActorModel = c,
           LeftHandWeapon = GetWeapon(c.EquipmentModel.EquipmentType, c.EquipmentModel.LeftHandEquipmentPath),
         };

         enemyPosition += new Vector2(200, 0);

         return enemy;
       }).Cast<Actor>().ToList()
     );


      _battleGUI = new BattleStateGUI(_gameModel);
      _battleGUI.SetAbilities(_actors.First().ActorModel);

      _chatBox = new ChatBox(_gameModel, _content.Load<SpriteFont>("Fonts/Font"));

      if (_conversation != null && _conversation.Count > 0)
        _chatBox.Write(_conversation.First());
    }

    private Sprite GetWeapon(EquipmentModel.EquipmentTypes equipment, string path)
    {
      if (equipment == EquipmentModel.EquipmentTypes.Fists)
        return null;

      if (string.IsNullOrEmpty(path))
        return null;

      var texture = _content.Load<Texture2D>(path);

      switch (equipment)
      {
        case EquipmentModel.EquipmentTypes.Fists:
          break;
        case EquipmentModel.EquipmentTypes.Single_Sword:
          return new Sprite(texture)
          {
            Origin = new Vector2(texture.Width / 2, texture.Height - 5)
          };

        case EquipmentModel.EquipmentTypes.Single_Shield:
          break;

        case EquipmentModel.EquipmentTypes.Both_Spear:
          return new Sprite(texture)
          {
            Origin = new Vector2(texture.Width / 2, texture.Height - 30),
          };

        case EquipmentModel.EquipmentTypes.Single_Axe:
          break;
        case EquipmentModel.EquipmentTypes.Single_Hammer:
          break;
        case EquipmentModel.EquipmentTypes.Both_Staff:
          break;
        case EquipmentModel.EquipmentTypes.Single_Wand:
          break;
        default:
          break;
      }

      return null;
    }

    public override void Update(GameTime gameTime)
    {
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

      _chatBox.Update(gameTime);

      if (_chatBox.IsFinished && _conversation.Count > 0)
      {
        _conversation.RemoveAt(0);
        if (_conversation.Count > 0)
          _chatBox.Write(_conversation.First());
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
          throw new Exception("Unexpected type: " + actor.ToString());
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

        actor.ActorModel.BattleStats.FinalBlows += 1;

        if (_actors.IndexOf(_target) < _currentActor)
          _currentActor--;
      }

      var validActors = _actors.Where(c => c.State == Actor.States.Alive).ToList();

      _currentActor = (_currentActor + 1) % validActors.Count;
      _battleGUI.SetAbilities(validActors[_currentActor].ActorModel);

      // As the move is over, there is no longer a target
      _target = null;
    }

    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      _background.Draw(gameTime, _spriteBatch);

      foreach (var actor in _actors)
        actor.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();

      if (_conversation.Count > 0)
      {
        _spriteBatch.Begin();

        _chatBox.Draw(gameTime);

        _spriteBatch.End();
      }
      else
      {
        _battleGUI.Draw(gameTime);
      }
    }
  }
}
