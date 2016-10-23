using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using PrvaDZ;

namespace Pong
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }

        public Paddle PaddleBottom { get; private set; }
        public Paddle PaddleTop { get; private set; }
        public Ball ball { get; private set; }
        public Background background { get; private set; }
        public SoundEffect HitSound { get; private set; }
        public Song Music { get; private set; }
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();

        public interface IPhysicalObject2D
        {
            float X { get; set; }
            float Y { get; set; }
            int Width { get; set; }
            int Height { get; set; }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";

        }

       
        protected override void Initialize()
        {
            // Screen bounds details . Use this information to set up game objects positions.
            var screenBounds = GraphicsDevice.Viewport.Bounds;

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleBottom.X = screenBounds.Center.X - GameConstants.PaddleDefaultWidth / 2;
            PaddleBottom.Y = screenBounds.Bottom - GameConstants.PaddleDefaulHeight;

            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleTop.X = screenBounds.Center.X - GameConstants.PaddleDefaultWidth / 2;
            PaddleTop.Y = screenBounds.Top - GameConstants.PaddleDefaulHeight / 2;

            ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed, GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = 225,
                Y = 450
            };
            background = new Background(screenBounds.Width, screenBounds.Height);
            // Add our game objects to the sprites that should be drawn collection .. you ’ll see why in a second
            SpritesForDrawList.Add(background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(ball);

            Walls = new List<Wall>()
            {
                // try with 100 for default wall size !
                new Wall ( - GameConstants.WallDefaultSize ,0 , GameConstants.WallDefaultSize , screenBounds.Height ) ,
                new Wall ( screenBounds . Right ,0 , GameConstants.WallDefaultSize , screenBounds . Height ) ,
            };
            Goals = new List<Wall>()
            {
                new Wall (0 , screenBounds.Height , screenBounds.Width , GameConstants.WallDefaultSize ) ,
                new Wall (screenBounds.Top , - GameConstants.WallDefaultSize , screenBounds.Width , GameConstants.WallDefaultSize )
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            ball.Texture = Content.Load<Texture2D>("ball");
            background.Texture = Content.Load<Texture2D>("background");
            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>("hit");
            Music = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var touchState = Keyboard.GetState();
            var bounds = GraphicsDevice.Viewport.Bounds;
            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X = PaddleBottom.X - (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, bounds.Left, bounds.Right - PaddleBottom.Width);

            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X = PaddleBottom.X + (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, bounds.Left, bounds.Right - PaddleBottom.Width);

            }
            if (touchState.IsKeyDown(Keys.A))
            {
                PaddleTop.X = PaddleTop.X - (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleTop.X = MathHelper.Clamp(PaddleTop.X, bounds.Left, bounds.Right - PaddleTop.Width);

            }
            if (touchState.IsKeyDown(Keys.D))
            {
                PaddleTop.X = PaddleTop.X + (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                PaddleTop.X = MathHelper.Clamp(PaddleTop.X, bounds.Left, bounds.Right - PaddleTop.Width);

            }
            var ballPositionChange = ball.Direction * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * ball.Speed);
            ball.X += ballPositionChange.X;
            ball.Y += ballPositionChange.Y;

            // Ball - side walls
            if (Walls.Any(w => CollisionDetector.Overlaps(ball, w)))
            {
                ball.BounceX();
                ball.Acceleration();
            }
            // Ball - winning walls
            if (Goals.Any(w => CollisionDetector.Overlaps(ball, w)))
            {
                ball.X = 225;
                ball.Y = 460;
                ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }
            // Paddle - ball collision
            if (CollisionDetector.Overlaps(ball, PaddleTop) && ball.Direction.Y < 0
            || (CollisionDetector.Overlaps(ball, PaddleBottom) && ball.Direction.Y > 0))
            {
                ball.BounceY();
                ball.Acceleration();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ///GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Start drawing .
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }
            // End drawing .
            // Send all gathered details to the graphic card in one batch .

            spriteBatch.End();
            base.Draw(gameTime);
        }
        public abstract class Sprite : IPhysicalObject2D
        {
            public float X { get; set; }
            public float Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            /// <summary >
            /// Represents the texture of the Sprite on the screen .
            /// Texture2D is a type defined in Monogame framework .
            /// </ summary >
            public Texture2D Texture { get; set; }
            protected Sprite(int width, int height, float x = 0, float y = 0)
            {
                X = x;
                Y = y;
                Height = height;
                Width = width;
            }
            /// <summary >
            /// Base draw method
            /// </ summary >
            public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
                Width, Height), Color.White);
            }
        }
        /// <summary >
        /// Game background representation
        /// </ summary >
        public class Background : Sprite
        {
            public Background(int width, int height) : base(width, height)
            {
            }
        }
        /// <summary >
        /// Game ball object representation
        /// </ summary >
        public class Ball : Sprite
        {
            /// <summary >
            /// Defines current ball speed in time .
            /// </ summary >
            public float Speed { get; set; }
            public float BumpSpeedIncreaseFactor { get; set; }
            /// <summary >
            /// Defines ball direction .
            /// Valid values ( -1 , -1) , (1 ,1) , (1 , -1) , ( -1 ,1).
            /// Using Vector2 to simplify game calculation . Potentially
            /// dangerous because vector 2 can swallow other values as well .
            /// OPTIONAL TODO : create your own , more suitable type
            /// </ summary >
            public Vector2 Direction { get; set; }
            public Ball(int size, float speed, float
            defaultBallBumpSpeedIncreaseFactor) : base(size, size)
            {
                Speed = speed;
                BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
                // Initial direction
                Direction = new Vector2(1, 1);
            }
            public void BounceX()
            {
                float[] tmp = { -Direction.X, Direction.Y };
                Direction = new Vector2(tmp[0], tmp[1]);
            }
            public void BounceY()
            {
                float[] tmp = { Direction.X, -Direction.Y };
                Direction = new Vector2(tmp[0], tmp[1]);
            }
            public void Acceleration()
            {

                Speed = MathHelper.Clamp(Speed * GameConstants.DefaultBallBumpSpeedIncreaseFactor, GameConstants.DefaultInitialBallSpeed, GameConstants.DefaultMaxBallSpeed);
            }
        }
        /// <summary >
        /// Represents player paddle .
        /// </ summary >
        public class Paddle : Sprite
        {
            /// <summary >
            /// Current paddle speed in time
            /// </ summary >
            public float Speed { get; set; }
            public Paddle(int width, int height, float initialSpeed) : base(width,
            height)
            {
                Speed = initialSpeed;
            }
            /// <summary >
            /// Overriding draw method . Masking paddle texture with black color .
            /// </ summary >
            public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
                Width, Height), Color.GhostWhite);
            }
        }

        public class CollisionDetector :
        {
            public struct Point
            {
                public float X;
                public float Y;
                public Point(float x, float y)
                {
                    X = x;
                    Y = y;
                }

            }
            public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
            {
                Point upperLeftCorner = new Point(a.X, a.Y);
                Point upperRightCorner = new Point(a.X + a.Width, a.Y);
                Point lowerLeftCorner = new Point(a.X, a.Y + a.Height);
                Point lowerRightCorner = new Point(a.X + a.Width, a.Y + a.Height);

                Point xAxisPoints = new Point(b.X, b.X + b.Width);
                Point yAxisPoints = new Point(b.Y, b.Y + b.Height);

                if (Collision(upperLeftCorner, xAxisPoints, yAxisPoints)) return true;
                if (Collision(upperRightCorner, xAxisPoints, yAxisPoints)) return true;
                if (Collision(lowerLeftCorner, xAxisPoints, yAxisPoints)) return true;
                if (Collision(lowerRightCorner, xAxisPoints, yAxisPoints)) return true;
                return false;

            }

            public static bool Collision(Point a, Point xAxis, Point yAxis)
            {
                if ((xAxis.X <= a.X && a.X <= xAxis.Y) && (yAxis.X <= a.Y && a.Y <= yAxis.Y))
                {
                    return true;
                }
                return false;
            }
        }

        public class Wall : IPhysicalObject2D
        {
            public float X { get; set; }
            public float Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Wall(float x, float y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }


        public class GameConstants
        {
            public const float PaddleDefaulSpeed = 0.9f;
            public const int PaddleDefaultWidth = 200;
            public const int PaddleDefaulHeight = 20;
            public const float DefaultInitialBallSpeed = 0.4f;
            public const float DefaultMaxBallSpeed = 1.0f;
            public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f;
            public const int DefaultBallSize = 40;
            public const int WallDefaultSize = 100;
        }


    }
}