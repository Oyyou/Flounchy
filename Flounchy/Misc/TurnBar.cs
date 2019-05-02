using Flounchy.Sprites;
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
  public class TurnBar
  {
    private Sprite _mainBar;

    private Sprite _miniBar1;
    private Sprite _miniBar2;

    public TurnBar(ContentManager content, Vector2 position)
    {
      _mainBar = new Sprite(content.Load<Texture2D>("TurnBar/TurnBar1"))
      {
        Position = position,
      };
      _miniBar1 = new Sprite(content.Load<Texture2D>("TurnBar/TurnBar2"))
      {
        Position = position,
      };
      _miniBar2 = new Sprite(content.Load<Texture2D>("TurnBar/TurnBar2"))
      {
        Position = position + new Vector2(1, 0),
      };
    }

    private bool _left = true;

    public void Update(GameTime gameTime)
    {
      if (_left)
      {
        _miniBar1.Position.X -= 1;
        _miniBar2.Position.X += 1;
      }
      else
      {
        _miniBar1.Position.X += 1;
        _miniBar2.Position.X -= 1;
      }

      if (_miniBar1.Position.X <= (_mainBar.Position.X - _mainBar.Origin.X) ||
          _miniBar1.Position.X > (_mainBar.Position.X))
      {
        _left = !_left;
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      _mainBar.Draw(gameTime, spriteBatch);
      _miniBar1.Draw(gameTime, spriteBatch);
      _miniBar2.Draw(gameTime, spriteBatch);
    }
  }
}
