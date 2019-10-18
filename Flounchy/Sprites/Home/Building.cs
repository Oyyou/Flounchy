using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flounchy.Sprites.Home
{
  public class Building : Sprite
  {
    private Texture2D _border;

    private bool _showBorder;

    private Effect _effect;

    public Building(Texture2D texture, GraphicsDevice graphics, ContentManager content)
      : base(texture)
    {
      _border = new Texture2D(graphics, _texture.Width, _texture.Height);

      _border.SetData(Helpers.GetBorder(_border, 2));

      _effect = content.Load<Effect>("Shaders/OutlineDiffuse");

      //_effect.Parameters["_Outline"].SetValue(100f);
      //_effect.Parameters["_Color"].SetValue(new Vector4(1, 1, 1, 1));
      //_effect.Parameters["_MainTex_TexelSize"].SetValue(new Vector4(1, 1, 1, 1));
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      _showBorder = false;

      if (GameMouse.Intersects(this.Rectangle))
      {
        _showBorder = true;

        if (GameMouse.IsLeftClicked)
        {

        }
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      //_effect.CurrentTechnique.Passes[0].Apply();

      base.Draw(gameTime, spriteBatch);

      if (_showBorder)
        spriteBatch.Draw(_border, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
    }
  }
}
