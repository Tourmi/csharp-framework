using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tourmi.Samples.Particles.Components;
using Color = Tourmi.Samples.Particles.Components.Color;
using EcsEvents = Tourmi.EntityComponentSystem.Entities.Events;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Tourmi.Samples.Particles;

/// <inheritdoc/>
internal sealed class ParticlesGame : Game
{
    private const int ParticleCount = 100000;
    private const float MaximumSpeed = 100;
    private const float FrictionCoefficient = 0.9f;

    private static readonly Rectangle Bounds = new(0, 0, 1920, 1080);

    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly World _ecs;
    private readonly Query _drawablesQuery;
    private readonly FrameTimeTracker _frameTimeTracker = new();

    private SpriteBatch? _spriteBatch;
    private Texture2D? _whitePixel;
    private Texture2D? _whiteCircle;
    private SpriteFont? _font;

    private int _framerateLineOffset;
    private MouseState _mouseState;
    private TimeSpan _deltaTime;

    private TimeSpan _refreshDisplayTime;

    public ParticlesGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this)
        {
            IsFullScreen = false,
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
            SynchronizeWithVerticalRetrace = false,
        };
        IsFixedTimeStep = false;
        IsMouseVisible = true;

        _ecs = World.Create();
        _drawablesQuery = Query.FromQueryParam<ParamGroup<Position, Color, Size, OpacityMultiplier, With<DrawableParticle>>>(_ecs);

        Services.AddService(_graphicsDeviceManager);

        Content.RootDirectory = "Content";
    }

    public Texture2D WhitePixel => _whitePixel.ThrowIfNull();
    public Texture2D WhiteCircle => _whiteCircle.ThrowIfNull();

    public SpriteFont Font => _font.ThrowIfNull();

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
            var maxOpacity = MathF.Min(1f, 4f / MathF.Sqrt(ParticleCount));
            entity.Set<Color>(new(new(0.1f + random.NextSingle() * 0.8f, 0.1f + random.NextSingle() * 0.8f, 0.1f + random.NextSingle() * 0.8f, maxOpacity + maxOpacity * i / ParticleCount)));
            entity.Set<Position>(new(new(random.NextSingle() * Bounds.Width, random.NextSingle() * Bounds.Height)));
            entity.Set<Speed>(new(new((random.NextSingle() * 2 - 1) * MaximumSpeed, (random.NextSingle() * 2 - 1) * MaximumSpeed)));
            var size = 5 + (1 - ((float)i / ParticleCount)) * (1 - MathF.Sqrt(random.NextSingle())) * 250;
            entity.Set<Size>(new(size, size));
            entity.Set<OpacityMultiplier>(new(1));
        }

        _ = _ecs.CreateEventSystem<EcsEvents.PreTick>(() =>
        {
            _mouseState = Mouse.GetState();
            _frameTimeTracker.AddFrameTime(_deltaTime);

            _refreshDisplayTime -= _deltaTime;
            if (_refreshDisplayTime <= TimeSpan.Zero)
            {
                _refreshDisplayTime = TimeSpan.FromSeconds(0.5);
                _frameTimeTracker.Refresh();
            }
        });

        _ = _ecs.CreateTickSystem(() =>
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        });

        _ = _ecs.AddSystem((Ref<Speed> speedRef, With<BouncingParticle> _) =>
        {
            ref var speed = ref speedRef.Reference;
            var sp = speed.Value;

            sp *= MathF.Pow(FrictionCoefficient, (float)_deltaTime.TotalSeconds);

            speed = new(sp);
        });

        _ = _ecs.AddSystem((Position position, Ref<Speed> speedRef, Size size, With<BouncingParticle> _) =>
        {
            var direction = 0;
            if (_mouseState.LeftButton is ButtonState.Pressed)
            {
                direction += 1;
            }
            if (_mouseState.RightButton is ButtonState.Pressed)
            {
                direction += -1;
            }

            if (direction is 0)
            {
                return;
            }

            var sp = speedRef.Reference.Value;

            var dirToMouse = _mouseState.Position.ToVector2() - position.Value;
            var distanceSquaredToMouse = dirToMouse.LengthSquared();

            if (distanceSquaredToMouse is 0)
            {
                return;
            }

            dirToMouse.Normalize();
            dirToMouse *= direction;

            var force = MathF.Min(MathF.Sqrt(size.Width * size.Height), 5000000f / distanceSquaredToMouse);

            sp += force * dirToMouse * (float)_deltaTime.TotalSeconds;

            speedRef.Reference = new(sp);
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

        _ = _ecs.CreateEventSystem<Events.PreDraw>(() => GraphicsDevice.Clear(XnaColor.Black));
        _ = _ecs.CreateEventSystem<Events.Draw>(() =>
        {
            SpriteBatch.Begin(blendState: BlendState.Additive);
            _drawablesQuery.ForEach((Position position, Size size, Color color, OpacityMultiplier mult)
                => SpriteBatch.Draw(WhiteCircle, new Rectangle(position.Value.ToPoint() - (size / 2).ToPoint(), size.ToPoint()), null, new(color.XnaColor, mult.Value * (color.XnaColor.A / 255f))));
            SpriteBatch.End();
        });
        _ = _ecs.CreateEventSystem<Events.Draw>(() =>
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(WhitePixel, new Rectangle(new(5), new Point(200, _framerateLineOffset * 3 - 5)), null, new XnaColor(XnaColor.Black, 1f));
            SpriteBatch.End();
        });
        _ = _ecs.CreateEventSystem<Events.Draw>(() =>
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Font, $"Frame Time {_frameTimeTracker.AverageFrameTimeMilliseconds:00.00} ms", 5 * Vector2.One, XnaColor.Green);
            SpriteBatch.DrawString(Font, $"Worst Time {_frameTimeTracker.WorstFrameTime:00.00} ms", 5 * Vector2.One + new Vector2(0, _framerateLineOffset), XnaColor.Green);
            SpriteBatch.DrawString(Font, $"{_frameTimeTracker.AverageFrameRateSeconds:0000.00} fps", 5 * Vector2.One + new Vector2(0, 2 * _framerateLineOffset), XnaColor.Green);
            SpriteBatch.End();
        });
    }

    /// <inheritdoc/>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whitePixel = Content.Load<Texture2D>("white-pixel");
        _whiteCircle = Content.Load<Texture2D>("white-circle");
        _font = Content.Load<SpriteFont>("Fonts/Hud");

        _framerateLineOffset = (_font.MeasureString("0 ms") + new Vector2(0, 5)).ToPoint().Y;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _whitePixel?.Dispose();
        _whiteCircle?.Dispose();
        _drawablesQuery?.Dispose();
        _spriteBatch?.Dispose();
        _graphicsDeviceManager.Dispose();

        base.Dispose(disposing);
    }

    /// <inheritdoc/>
    protected override void Update(GameTime gameTime)
    {
        _deltaTime = gameTime.ElapsedGameTime;
        _ecs.Tick();
    }

    /// <inheritdoc/>
    protected override void Draw(GameTime gameTime)
    {
        _ecs.Raise<Events.PreDraw>();
        _ecs.Raise<Events.Draw>();
        _ecs.Raise<Events.PostDraw>();
    }
}
