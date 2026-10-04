using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tourmi.EntityComponentSystem.Queries.Parameters;
using Tourmi.Monogame;
using Tourmi.Samples.TestScenes.Components;
using EcsEvents = Tourmi.EntityComponentSystem.Entities.Events;

namespace Tourmi.Samples.TestScenes;

/// <inheritdoc/>
internal sealed class TestScenesGame : Game
{
    private static readonly Point InternalResolution = new(320, 180);
    private static readonly Point TargetResolution = new(1920, 1080);

    private readonly IdentifierRegion _reservedRegion = new()
    {
        Name = "TestScenes Reserved",
        Amount = 0x0001_0000,
    };
    private readonly World _ecs;
    private readonly FrameTimeTracker _frameTimeTracker = new();
    private readonly Query _spritesQuery;
    private readonly NoiseGenerator _noiseGenerator = new();

    private uint _currentId;
    private int _debugMode = 0;

    private GraphicsDeviceManager? _graphicsDeviceManager;
    private Texture2D? _whitePixel;
    private Texture2D? _testTexture;
    private Texture2D? _fogTexture;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _whiteCircle;
    private SpriteFont? _font;
    private RenderTarget2D? _internalRenderTarget;

    private int _framerateLineOffset;
    private TimeSpan _deltaTime;

    public TestScenesGame()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this)
        {
            IsFullScreen = false,
            PreferredBackBufferWidth = TargetResolution.X,
            PreferredBackBufferHeight = TargetResolution.Y,
            SynchronizeWithVerticalRetrace = false,
        };
        IsFixedTimeStep = false;
        IsMouseVisible = true;

        _ecs = World.Create(c => c.ReservedIdentifierRegions = [_reservedRegion]);
        _currentId = _reservedRegion.Offset;

        _spritesQuery = _ecs.GetCachedQueryFor<ParamGroup<Sprite, Position>>();

        Content.RootDirectory = "Content";
    }

    public Texture2D WhitePixel => _whitePixel.ThrowIfNull();

    public Texture2D WhiteCircle => _whiteCircle.ThrowIfNull();

    public SpriteFont Font => _font.ThrowIfNull();

    public SpriteBatch SpriteBatch => _spriteBatch.ThrowIfNull();

    /// <inheritdoc/>
    protected override void LoadContent()
    {
        _internalRenderTarget = new RenderTarget2D(GraphicsDevice, InternalResolution.X, InternalResolution.Y);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData([Color.White]);
        _whiteCircle = Content.Load<Texture2D>("white-circle");
        _font = Content.Load<SpriteFont>("Fonts/Hud");

        _testTexture = new Texture2D(GraphicsDevice, InternalResolution.X, InternalResolution.Y);
        _fogTexture = new Texture2D(GraphicsDevice, 256, 256);

        _framerateLineOffset = (_font.MeasureString("0 ms") + new Vector2(0, 5)).ToPoint().Y;
    }

    /// <inheritdoc/>
    protected override void Initialize()
    {
        base.Initialize();

        _ = _ecs.CreateEventSystem<EcsEvents.PreTick>(() =>
        {
            _frameTimeTracker.AddFrameTime(_deltaTime);
        });

        _ = CreateRect(_testTexture.ThrowIfNull(), new(default, InternalResolution), Color.White);

        var fogTransparency = 0.5f;
        var fog1 = CreateRect(_fogTexture.ThrowIfNull(), new(default, InternalResolution), new Color(Color.White, fogTransparency));
        fog1.Set<Fog>(new() { MinWrapAround = new(-256), Wind = new Vector2(-11f, 0f) });
        var fog2 = CreateRect(_fogTexture.ThrowIfNull(), new(default, InternalResolution), new Color(Color.White, fogTransparency));
        fog2.Set<Fog>(new() { MinWrapAround = new(-256), Position = new Vector2(64, 64), Wind = new Vector2(7f, 0) });
        var fog3 = CreateRect(_fogTexture.ThrowIfNull(), new(default, InternalResolution), new Color(Color.White, fogTransparency));
        fog3.Set<Fog>(new() { MinWrapAround = new(-256), Position = new Vector2(128, 128), Wind = new Vector2(13f, 0) });
        var fog4 = CreateRect(_fogTexture.ThrowIfNull(), new(default, InternalResolution), new Color(Color.White, fogTransparency));
        fog4.Set<Fog>(new() { MinWrapAround = new(-256), Position = new Vector2(192, 192), Wind = new Vector2(-17f, 0) });

        var every30TickSchedule = _ecs.CreateEntity("Every30TickSchedule");
        every30TickSchedule.Set<Schedule>(new(TimeSpan.Zero, 30));
        every30TickSchedule.SubscribedTo<EcsEvents.Tick>();

        _ = _ecs.CreateEventSystem(every30TickSchedule, _frameTimeTracker.Refresh);

        _ = _ecs.CreateTickSystem(() =>
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                _debugMode = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F10))
            {
                _debugMode = 0;
            }
        });
        _ = _ecs.CreateTickSystem(() =>
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                _testTexture.SetData(_noiseGenerator.SimpleGray(InternalResolution.X, InternalResolution.Y));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                _testTexture.SetData(_noiseGenerator.PerlinGray(InternalResolution.X, InternalResolution.Y));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                _testTexture.SetData(_noiseGenerator.FractalPerlinAlpha(InternalResolution.X, InternalResolution.Y, 4));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                _testTexture.SetData(_noiseGenerator.FractalPerlinThresholds(InternalResolution.X, InternalResolution.Y, 1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                _testTexture.SetData(_noiseGenerator.FractalPerlinThresholds(InternalResolution.X, InternalResolution.Y, 8));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                _testTexture.SetData(_noiseGenerator.FractalPerlinHSVColors(InternalResolution.X, InternalResolution.Y, 5));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                _testTexture.SetData(_noiseGenerator.FractalPerlinRGBColors(InternalResolution.X, InternalResolution.Y, 5));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D9))
            {
                _fogTexture.SetData(new Color[256 * 256]);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _fogTexture.SetData(_noiseGenerator.FractalPerlinAlpha(256, 256, 3));
                _testTexture.SetData(new Color[InternalResolution.X * InternalResolution.Y]);
            }
        });

        _ = _ecs.AddSystem((ParamGroup<Ref<Fog>, Sprite> param) =>
        {
            var (fogRef, sprite) = param;
            ref var fog = ref fogRef.Reference;

            var newPosition = fog.Position + (float)_deltaTime.TotalSeconds * fog.Wind;
            if (newPosition.X < fog.MinWrapAround.X)
            {
                newPosition.X -= fog.MinWrapAround.X;
            }

            if (newPosition.X > -fog.MinWrapAround.X)
            {
                newPosition.X += fog.MinWrapAround.X;
            }

            if (newPosition.Y < fog.MinWrapAround.Y)
            {
                newPosition.Y -= fog.MinWrapAround.Y;
            }

            if (newPosition.Y > -fog.MinWrapAround.Y)
            {
                newPosition.Y += fog.MinWrapAround.Y;
            }

            fog = fog with { Position = newPosition };

            var rect = sprite.SourceRectangle ?? new Rectangle(default, InternalResolution);
            sprite.SourceRectangle = new Rectangle(newPosition.Floored().ToPoint(), rect.Size);
        });

        _ = _ecs.CreateEventSystem<Events.PreDraw>(() =>
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(_internalRenderTarget);
            SpriteBatch.Begin();
            SpriteBatch.Draw(_whitePixel, new Rectangle(default, TargetResolution), Color.Black);
            SpriteBatch.End();
        });
        _ = _ecs.CreateEventSystem<Events.Draw>(() =>
        {
            SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointWrap);
            _spritesQuery.ForEach((Sprite sprite, Position pos) =>
            {
                if (sprite.Texture is null)
                {
                    return;
                }

                SpriteBatch.Draw(
                    sprite.Texture,
                    sprite.DestinationRectangle.Translate(pos.Value.Floored().ToPoint()),
                    sprite.SourceRectangle,
                    sprite.Color,
                    sprite.Rotation,
                    sprite.Origin,
                    sprite.SpriteEffects,
                    0f);
            });
            SpriteBatch.End();
        });

        _ = _ecs.CreateEventSystem<Events.PostDraw>(() =>
        {
            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.Draw(_whitePixel, new Rectangle(default, TargetResolution), Color.Black);
            SpriteBatch.Draw(_internalRenderTarget, new Rectangle(default, TargetResolution), Color.White);
            SpriteBatch.End();
        });
        _ = _ecs.CreateEventSystem<Events.PreDrawUI>(() =>
        {
            GraphicsDevice.SetRenderTarget(null);
        });
        _ = _ecs.CreateEventSystem<Events.DrawUI>(() =>
        {
            if (_debugMode != 1)
            {
                return;
            }

            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred);
            SpriteBatch.Draw(WhitePixel, new Rectangle(new(5), new Point(200, _framerateLineOffset * 3 - 5)), null, new Color(Color.Black, 0.5f));
            SpriteBatch.DrawString(Font, $"Frame Time {_frameTimeTracker.AverageFrameTimeMilliseconds:00.00} ms", 5 * Vector2.One, Color.Green);
            SpriteBatch.DrawString(Font, $"Worst Time {_frameTimeTracker.WorstFrameTime:00.00} ms", 5 * Vector2.One + new Vector2(0, _framerateLineOffset), Color.Green);
            SpriteBatch.DrawString(Font, $"{_frameTimeTracker.AverageFrameRateSeconds:0000.00} fps", 5 * Vector2.One + new Vector2(0, 2 * _framerateLineOffset), Color.Green);
            SpriteBatch.End();
        });
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _whitePixel?.Dispose();
        _whiteCircle?.Dispose();
        _spriteBatch?.Dispose();
        _internalRenderTarget?.Dispose();
        _internalRenderTarget = null;
        _graphicsDeviceManager?.Dispose();
        _graphicsDeviceManager = null;
        _testTexture?.Dispose();
        _fogTexture?.Dispose();
        _spritesQuery.Dispose();

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
        _deltaTime = gameTime.ElapsedGameTime;

        _ecs.Raise<Events.PreDraw>();
        _ecs.Raise<Events.Draw>();
        _ecs.Raise<Events.PostDraw>();
        _ecs.Raise<Events.PreDrawUI>();
        _ecs.Raise<Events.DrawUI>();
        _ecs.Raise<Events.PostDrawUI>();
    }

    private void CreateCross(int lineWidth)
    {
        _ = CreateRect(WhitePixel, new((InternalResolution.X - lineWidth) / 2, 0, lineWidth, InternalResolution.Y), Color.White);
        _ = CreateRect(WhitePixel, new(0, (InternalResolution.Y - lineWidth) / 2, InternalResolution.X, lineWidth), Color.White);
    }

    private Entity CreateRect(Texture2D texture, Rectangle rect, Color color)
    {
        var entity = _ecs.CreateEntity();
        entity.Set<Position>(new(0, 0));
        entity.Set<Sprite>(new()
        {
            Texture = texture,
            DestinationRectangle = rect,
            Color = color,
        });

        return entity;
    }
}
