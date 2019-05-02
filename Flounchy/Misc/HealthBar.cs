﻿using Flounchy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Misc
{
  public class HealthBar
  {
    private Actor _actor;

    private Sprite _background;
    private Sprite _foreground;
    private Sprite _border;

    public HealthBar(ContentManager content)
    {
      _background = new Sprite(content.Load<Texture2D>("Healthbar/HealthbarBackground"));
      _foreground = new Sprite(content.Load<Texture2D>("Healthbar/HealthbarForeground"));
      _border = new Sprite(content.Load<Texture2D>("Healthbar/HealthbarBorder"));
    }

    public void SetActor(Actor actor)
    {
      _actor = actor;

      var position = new Vector2(_actor.StartPosition.Value.X, ((_actor.StartPosition.Value.Y + _actor.Origin.Y) + _background.Rectangle.Height) + 15);

      _background.Position = position;
      _foreground.Position = position;
      _border.Position = position;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      _background.Draw(gameTime, spriteBatch);

      _foreground.SourceRectangle = new Rectangle(0, 0, (int)(_foreground.Rectangle.Width * ((double)_actor.CurrentHealth / _actor.MaxHealth)), _foreground.Rectangle.Height);
      _foreground.Draw(gameTime, spriteBatch);

      _border.Draw(gameTime, spriteBatch);
    }
  }
}
