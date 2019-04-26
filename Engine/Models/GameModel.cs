using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
  public class GameModel
  {
    public ContentManager ContentManger { get; set; }

    public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

    public SpriteBatch SpriteBatch { get; set; }

    public GameModel()
    {

    }
  }
}
