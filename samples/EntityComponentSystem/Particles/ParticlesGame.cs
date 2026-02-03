using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tourmi.Samples.Particles.Components;

using Color = Tourmi.Samples.Particles.Components.Color;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Tourmi.Samples.Particles;

/// <inheritdoc/>
internal sealed class ParticlesGame : Game
{
    private const int ParticleCount = 10000;
    private const float MaximumSpeed = 100;

    private static readonly Rectangle Bounds = new(0, 0, 1920, 1080);

    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly World _ecs;
    private readonly Query _drawablesQuery;

    private SpriteBatch? _spriteBatch;
    private Texture2D? _whitePixel;

    private MouseState _mouseState;
    private TimeSpan _deltaTime;

    public ParticlesGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this)
        {
            IsFullScreen = false,
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
        };
        IsFixedTimeStep = false;
        IsMouseVisible = true;

        _ecs = World.Create();
        _drawablesQuery = Query.FromQueryParam<ParamGroup<Position, Color, With<DrawableParticle>>>(_ecs);

        Services.AddService(_graphicsDeviceManager);

        Content.RootDirectory = "Content";
    }

    public Texture2D WhitePixel => _whitePixel.ThrowIfNull();

    public SpriteBatch SpriteBatch => _spriteBatch.ThrowIfNull();

    /// <inheritdoc/>
    protected override void Initialize()
    {
        base.Initialize();

        var random = new Random();

        for (var i = 0; i < ParticleCount; i++)
        {
            var entity = _ecs.CreateEntity();
            entity.Add<BouncingParticle>();
            entity.Add<DrawableParticle>();
            entity.Set<Color>(new(new(random.NextSingle(), random.NextSingle(), random.NextSingle())));
            entity.Set<Position>(new(new(random.NextSingle() * Bounds.Width, random.NextSingle() * Bounds.Height)));
            entity.Set<Speed>(new(new((random.NextSingle() * 2 - 1) * MaximumSpeed, (random.NextSingle() * 2 - 1) * MaximumSpeed)));
        }

        _ = _ecs.CreateTickSystem(() =>
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        });

        _ = _ecs.AddSystem((Position position, Ref<Speed> speedRef, With<BouncingParticle> _) =>
        {
            var direction = 0;
            if (_mouseState.LeftButton is ButtonState.Pressed)
            {
                direction = 1;
            }
            if (_mouseState.RightButton is ButtonState.Pressed)
            {
                direction = -1;
            }

            if (direction is 0)
            {
                return;
            }

            ref var speed = ref speedRef.Reference;

            var sp = speed.Value;

            var dirToMouse = _mouseState.Position.ToVector2() - position.Value;
            var distanceToMouse = dirToMouse.LengthSquared();

            if (distanceToMouse is > 40000 or < 1)
            {
                return;
            }

            dirToMouse.Normalize();
            dirToMouse *= direction;

            sp += 0.03f * dirToMouse * (40000 - distanceToMouse) * (float)_deltaTime.TotalSeconds;

            speed = new(sp);
        });

        _ = _ecs.AddSystem((Ref<Speed> speedRef, With<BouncingParticle> _) =>
        {
            ref var speed = ref speedRef.Reference;
            var sp = speed.Value;

            sp *= MathF.Pow(0.9f, (float)_deltaTime.TotalSeconds);

            speed = new(sp);
        });

        _ = _ecs.AddSystem((Ref<Position> positionRef, Ref<Speed> speedRef, With<BouncingParticle> _) =>
        {
            ref var position = ref positionRef.Reference;
            ref var speed = ref speedRef.Reference;

            var pos = position.Value;
            var sp = speed.Value;
            pos += ((float)_deltaTime.TotalSeconds) * sp;
            if (pos.Y > Bounds.Bottom)
            {
                pos.Y -= 2 * (pos.Y - Bounds.Bottom);
                sp.Y = -MathF.Abs(sp.Y);
            }
            else if (pos.Y < Bounds.Top)
            {
                pos.Y -= 2 * (pos.Y - Bounds.Top);
                sp.Y = MathF.Abs(sp.Y);
            }

            if (pos.X > Bounds.Right)
            {
                pos.X -= 2 * (pos.X - Bounds.Right);
                sp.X = -MathF.Abs(sp.X);
            }
            else if (pos.X < Bounds.Left)
            {
                pos.X -= 2 * (pos.X - Bounds.Left);
                sp.X = MathF.Abs(sp.X);
            }

            position = new(pos);
            speed = new(sp);
        });
    }

    /// <inheritdoc/>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whitePixel = Content.Load<Texture2D>("white-pixel");
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _whitePixel?.Dispose();
        _drawablesQuery?.Dispose();
        _spriteBatch?.Dispose();
        _graphicsDeviceManager.Dispose();

        base.Dispose(disposing);
    }

    /// <inheritdoc/>
    protected override void Update(GameTime gameTime)
    {
        _deltaTime = gameTime.ElapsedGameTime;
        _mouseState = Mouse.GetState();

        _ecs.Tick();

        base.Update(gameTime);
    }

    /// <inheritdoc/>
    protected override void Draw(GameTime gameTime)
    {
        // Clears the screen with the MonoGame orange color before drawing.
        GraphicsDevice.Clear(XnaColor.Black);

        SpriteBatch.Begin();

        _drawablesQuery.ForEach((Position position, Color color) => SpriteBatch.Draw(WhitePixel, new Rectangle(position.Value.ToPoint() - new Point(1), new Point(2)), null, color.XnaColor));

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
