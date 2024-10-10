using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame_Topic_3___Loops__Lists__and_Input
{
    //Christian Moyes
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;

        Texture2D grassTexture;
        Texture2D mowerTexture;

        Rectangle window;
        Rectangle mowerRect;

        SoundEffect mowerSound;
        SoundEffectInstance mowerSoundInstance;

        Vector2 mowerSpeed;

        List<Rectangle> grassTiles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 600, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            mowerRect = new Rectangle(100, 100, 30, 30);

            grassTiles = new List<Rectangle>();

            for (int x = 0; x < window.Width; x += 5)
            {
                for (int y = 0; y < window.Height; y += 5)
                {
                    grassTiles.Add(new Rectangle(x, y, 5, 5));
                }
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            mowerTexture = Content.Load<Texture2D>("Images/mower");
            grassTexture = Content.Load<Texture2D>("Images/long_grass");

            mowerSound = Content.Load<SoundEffect>("Sounds/mower_sound");
            mowerSoundInstance = mowerSound.CreateInstance();
            mowerSoundInstance.IsLooped = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            mowerSpeed = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                mowerSpeed.Y -= 1;

                if (mowerRect.Y == 0)
                {
                    mowerSpeed.Y = 0;
                }
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                mowerSpeed.Y += 1;

                if (mowerRect.Bottom == window.Bottom)
                {
                    mowerSpeed.Y = 0;
                }
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                mowerSpeed.X -= 1;

                if (mowerRect.X == 0)
                {
                    mowerSpeed.X = 0;
                }
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                mowerSpeed.X += 1;

                if (mowerRect.Right == window.Right)
                {
                    mowerSpeed.X = 0;
                }
            }

            for (int i = 0; i < grassTiles.Count; i++)
            {
                if (mowerRect.Contains(grassTiles[i]))
                {
                    grassTiles.Remove(grassTiles[i]);
                    i--;
                }
            }





            if (mowerSpeed == Vector2.Zero)
            {
                mowerSoundInstance.Stop();
            }
            else
            {
                mowerSoundInstance.Play();
            }


            mowerRect.Offset(mowerSpeed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            _spriteBatch.Begin();

            foreach (Rectangle grass in grassTiles)
            {
                _spriteBatch.Draw(grassTexture, grass, Color.White);
            }

            _spriteBatch.Draw(mowerTexture, mowerRect, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
