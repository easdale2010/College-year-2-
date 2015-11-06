using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BallGame_2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int displaywidth;
        int displayheight;

        Texture2D myball;
        Vector3 position = new Vector3(100, 100, 0);
        Rectangle ballrect;
        Rectangle ballrect2;
        Vector3 position2 = new Vector3(200, 150, 0);
        Vector2 origin;

        Vector3 oldposition;
        Vector3 oldposition2;
        float ballsize = 2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode. Height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            displaywidth = graphics.GraphicsDevice.Viewport.Width;
            displayheight = graphics.GraphicsDevice.Viewport.Height;

            //graphics.ToggleFullScreen();

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

            myball=Content.Load<Texture2D>("ball");
           
            origin.X = myball.Width / 2;
            origin.Y = myball.Height / 2;
            ballrect.Width = (int)(myball.Width * ballsize);
            ballrect.Height =(int)(myball.Height * ballsize);
            ballrect2.Height = (int)(myball.Height * ballsize);
            ballrect2.Width = (int)(myball.Width * ballsize);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed||
                Keyboard.GetState().IsKeyDown(Keys.Escape))  this.Exit();

            float speed = 10;
            GamePadState pad_p1 = GamePad.GetState(PlayerIndex.One);

            KeyboardState keys = Keyboard.GetState();

            if(keys. IsKeyDown(Keys.Up))position2.Y-=speed;
            if (keys.IsKeyDown(Keys.Down)) position2.Y += speed;
            if(keys.IsKeyDown(Keys.Right))position2.X+= speed;
            if (keys.IsKeyDown(Keys.Left)) position2.X -= speed;


            position.X += pad_p1.ThumbSticks.Left.X * speed;
            position.Y -= pad_p1.ThumbSticks.Left.Y * speed;

            position2.X += pad_p1.ThumbSticks.Right.X * speed;
            position2.Y -= pad_p1.ThumbSticks.Right.Y * speed;


            if (position.X <= ballrect.Width / 2) position.X = ballrect.Width / 2;
            if (position.X > displaywidth - ballrect.Width / 2) position.X = displaywidth - ballrect.Width/2;

            if (position.Y <= ballrect.Height/2) position.Y = ballrect.Height/2;
            if (position.Y > displayheight - ballrect.Height/2) position.Y = displayheight - ballrect.Width/2;

            if (position2.X-ballrect.Width / 2 <= 0) position2.X = ballrect.Width /2 ;
            if (position2.X + ballrect.Width / 2 > displaywidth) position2.X = displaywidth - ballrect.Width / 2;
            if (position2.Y - ballrect.Width / 2<= 0) position2.Y = ballrect.Width / 2;
            if (position2.Y + ballrect.Height /2  > displayheight) position2.Y = displayheight - ballrect.Width / 2;

           

            BoundingSphere ballsphere = new BoundingSphere(position, ballrect.Width / 2);
            BoundingSphere ballsphere2 = new BoundingSphere(position2, ballrect2.Width / 2);

            if (ballsphere.Intersects(ballsphere2))
            {
                position = oldposition;
                position2 = oldposition2;
            }
            else
            {
                oldposition = position;
                oldposition2 = position2;
            }

            ballrect.Y = (int)position.Y;
            ballrect.X = (int)position.X;

            ballrect2.Y = (int)position2.Y;
            ballrect2.X = (int)position2.X;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(myball, ballrect2, null, Color.Beige, 0, origin, SpriteEffects.None, 0);
            spriteBatch.Draw(myball, ballrect, null, Color.White, 0, origin, SpriteEffects.None, 0);
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
