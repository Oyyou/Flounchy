using Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Sprites.AfterBattle
{
  public class StatsPanel
  {
    private SpriteFont _font;

    private ActorModel _player;

    private Vector2 _position;

    public StatsPanel(ContentManager content, ActorModel player, Vector2 position)
    {
      _font = content.Load<SpriteFont>("Fonts/ButtonFont");

      _player = player;

      _position = position;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      var newPosition = _position;

      var content = new Dictionary<string, string>()
      {
        { _player.Name, "" },
        { "Damage dealt:", _player.BattleStats.DamageDealt.ToString() },
        { "Damage received:", _player.BattleStats.DamageReceived.ToString() },
        { "Final blows:", _player.BattleStats.FinalBlows.ToString() },
        { "Experience earned:", _player.BattleStats.ExpEarned.ToString() },
      };

      foreach (var value in content)
      {
        spriteBatch.DrawString(_font, value.Key, newPosition, Color.Black);
        spriteBatch.DrawString(_font, value.Value, newPosition + new Vector2(150, 0), Color.Black);

        newPosition += new Vector2(0, 30);
      }
    }
  }
}
