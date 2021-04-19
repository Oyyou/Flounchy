using Flounchy.Components;
using Flounchy.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flounchy.Entities.Roaming
{
  public class Animal : Entity
  {
    public enum Directions
    {
      Up,
      Down,
      Left,
      Right,
    }

    private SpriteFont _font;
    private string _text;
    private float _textTimer;
    private int _count;

    #region Movement
    private float _movementTimer = 0f;
    private float _nextMovementTimer = 1f;
    private int _xDistance;
    private int _yDistance;
    private const int _maxDistance = 2;
    #endregion

    public Directions Direction;

    public Animal(Texture2D texture, SpriteFont font, Map map)
      : base()
    {
      _animationComponent = new TextureAnimatedComponent(this, texture, 4, 4, 0.3f)
      {
        SetAnimation = (gameTime) => SetAnimationEvent(gameTime),
        GetLayer = () => MathHelper.Clamp((_moveComponent.CurrentRectangle.Y) / 1000f, 0, 1),
      };

      _interactComponent = new InteractComponent(this, () => _moveComponent.CurrentRectangle)
      {
        OnInteract = () => OnInteractEvent(),
      };

      _moveComponent = new MoveComponent(this, map, (gameTime) => SetMovementEvent(gameTime))
      {
        Speed = 1,
      };

      _mapComponent = new MapComponent(this, map, GetMapRectangle);

      Components.Add(_moveComponent);
      Components.Add(_interactComponent);
      Components.Add(_animationComponent);
      Components.Add(_mapComponent);

      _font = font;
    }

    private void OnInteractEvent()
    {
      if (_textTimer > 0f)
        _count++;
      else _count = 0;

      _text = "Oink";
      _textTimer = 0f;

      if (_count >= 5)
        _text = "Can you not, mate!?";

      if (_count >= 10)
        _text = "I'm not porkin' around mate, you're about to feel the heat of my meat";
    }

    private void SetMovementEvent(GameTime gameTime)
    {
      _movementTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_movementTimer < _nextMovementTimer)
        return;

      _movementTimer = 0;
      _nextMovementTimer = Game1.Random.Next(2, 5);

      switch (Game1.Random.Next(0, 4))
      {
        case 0 when _yDistance < _maxDistance:
          _moveComponent.Velocity = new Vector2(_moveComponent.Speed, 0);
          _yDistance++;
          Direction = Directions.Right;
          break;
        case 1 when _yDistance > -_maxDistance:
          _moveComponent.Velocity = new Vector2(-_moveComponent.Speed, 0);
          _yDistance--;
          Direction = Directions.Left;
          break;
        case 2 when _xDistance > -_maxDistance:
          _moveComponent.Velocity = new Vector2(0, -_moveComponent.Speed);
          _xDistance--;
          Direction = Directions.Up;
          break;
        case 3 when _xDistance < _maxDistance:
          _moveComponent.Velocity = new Vector2(0, _moveComponent.Speed);
          _xDistance++;
          Direction = Directions.Down;
          break;
      }
    }

    private void SetAnimationEvent(GameTime gameTime)
    {
      _animationComponent.CurrentYFrame = (int)Direction;

      _animationComponent.Playing = _moveComponent.Velocity != Vector2.Zero;
    }

    public override void Update(GameTime gameTime)
    {
      if (!string.IsNullOrEmpty(_text))
      {
        _textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_textTimer > 3f)
        {
          _text = "";
          _count = 0;
        }
      }

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (!string.IsNullOrEmpty(_text))
      {
        spriteBatch.DrawString(_font, _text, GetTextPosition(), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, _animationComponent.Layer + 0.1f);
      }

      base.Draw(gameTime, spriteBatch);
    }

    private Vector2 GetTextPosition()
    {
      return new Vector2((_moveComponent.CurrentRectangle.X + (_moveComponent.CurrentRectangle.Width / 2)) - (_font.MeasureString(_text).X / 2), _moveComponent.CurrentRectangle.Y - 20);
    }

    private Rectangle GetMapRectangle()
    {
      return new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        _animationComponent.Width,
        _animationComponent.Height
      );
    }
  }
}
