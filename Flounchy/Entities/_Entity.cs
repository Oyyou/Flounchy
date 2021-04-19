using Flounchy.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Entities
{
  public class Entity
  {
    protected TextureComponent _textureComponent;
    protected TextureAnimatedComponent _animationComponent;
    protected InteractComponent _interactComponent;
    protected MoveComponent _moveComponent;
    protected MapComponent _mapComponent;

    public readonly List<Component> Components;

    public Vector2 Position { get; set; }

    public float X
    {
      get { return Position.X; }
      set
      {
        Position = new Vector2(value, Position.Y);
      }
    }

    public float Y
    {
      get { return Position.Y; }
      set
      {
        Position = new Vector2(Position.X, value);
      }
    }

    public Entity()
    {
      Components = new List<Component>();
    }

    public void Add(Component component)
    {
      Components.Add(component);
    }

    public virtual void Update(GameTime gameTime)
    {
      foreach (var component in Components.OrderBy(c => c.UpdateOrder))
        component?.Update(gameTime);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach (var component in Components.OrderBy(c => c.DrawOrder))
        component?.Draw(gameTime, spriteBatch);
    }
  }
}
