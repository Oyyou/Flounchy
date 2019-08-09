using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Controls;
using Engine.Models;
using Engine.Models.Skills;
using Flounchy.GUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Engine.Models.Skills.SkillModel;

namespace Flounchy.GameStates
{
  public class SkillTestState : BaseState
  {
    private SpriteFont _font;
    private Texture2D _abilityTexture;
    private Texture2D _statTexture;

    private List<Button> _sprites;

    public SkillTestState(GameModel gameModel, List<ActorModel> players)
      : base(gameModel, players)
    {
    }

    public override void LoadContent()
    {
      _sprites = new List<Button>();

      _font = _content.Load<SpriteFont>("Fonts/ButtonFont");
      _abilityTexture = _content.Load<Texture2D>("SkillTree/AbilityIcon");
      _statTexture = _content.Load<Texture2D>("SkillTree/StatIcon");


      var skill = _players[0].SkillsModel.Skills.FirstOrDefault();
      var sprite = new Button(_abilityTexture, _font)
      {
        Position = new Vector2(100, 360),
        Text = skill.Name,
        Origin = new Vector2(_abilityTexture.Width / 2, _abilityTexture.Height / 2),
      };
      _sprites.Add(sprite);

      SetSkill(sprite.Position, skill.ProceedingSkills);
    }

    public override void Update(GameTime gameTime)
    {

    }
    public override void Draw(GameTime gameTime)
    {
      _spriteBatch.Begin();

      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, _spriteBatch);

      _spriteBatch.End();
    }

    private void SetSkill(Vector2 position, List<SkillModel> skills)
    {
      for (int i = 0; i < skills.Count; i++)
      {
        var skill = skills[i];

        var flowDirection = skill.FlowDirection;

        var newPosition = position;

        switch (flowDirection)
        {
          case FlowDirections.Up:
            newPosition += new Vector2(120, -120);
            break;
          case FlowDirections.Down:
            newPosition += new Vector2(120, 120);
            break;
          case FlowDirections.None:
            newPosition += new Vector2(120, 0);
            break;
        }

        Texture2D texture = null;

        switch (skill.SkillType)
        {
          case SkillTypes.Stats:
            texture = _statTexture;
            break;
          case SkillTypes.Ability:
            texture = _abilityTexture;
            break;
          default:
            throw new Exception("Unknown SkillType: " + skill.SkillType.ToString());
        }


        var sprite = new Button(texture, _font)
        {
          Position = newPosition,
          Text = skill.Name,
          Origin = new Vector2(texture.Width / 2, texture.Height / 2),
        };

        _sprites.Add(sprite);

        SetSkill(newPosition, skill.ProceedingSkills);
      }
    }
  }
}
