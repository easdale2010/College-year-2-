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

namespace Ball_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont mainfont;

        int boost = 3;
        Random randomiser = new Random();

        Vector2 position = new Vector2(100, 100); // position of the ball on the screen
        Texture2D myball; // Texture to hold football.png

        int displaywidth = 1024;
        int displayheight = 768;

        Vector2 velocity = new Vector2(5, -7); // velocity of the ball;

        Boolean visible = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

          // this.graphics.PreferredBackBufferWidth = displaywidth;
         //  this.graphics.PreferredBackBufferHeight = displayheight;

            this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            
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

            velocity = new Vector2(randomiser.Next(9) - 4, randomiser.Next(9) - 4);

           displaywidth = graphics.GraphicsDevice.Viewport.Width;
           displayheight = graphics.GraphicsDevice.Viewport.Height;

          // graphics.ToggleFullScreen();
           

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

            myball=Content.Load<Texture2D>("football"); // loads the football grahic into my ball
            mainfont = Content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                this.Exit();

            KeyboardState keys_p1 = Keyboard.GetState();
            GamePadState pad_p1= GamePad.GetState(PlayerIndex.One);
            MouseState mouse1 = Mouse.GetState();
           
           
                position.X = mouse1.X;
                position.Y = mouse1.Y;
                
           
            if (mouse1.LeftButton == ButtonState.Pressed)
                visible = false;
            if (mouse1.LeftButton == ButtonState.Released)
                visible = true;
            
            // TODO: Add your update logic here
           // position += velocity;

             

            if (position.X > displaywidth - myball.Width )
                position.X = displaywidth - myball.Width;
            if (position.X <= 0) position.X = 0;

            
            if (position.Y > displayheight -myball.Height )
                position.Y = displayheight-myball.Height;
            if (position.Y <= 0) position.Y = 0;
                
             

            float speed = 5 * (pad_p1.Triggers.Right+1); 

            if (keys_p1.IsKeyDown(Keys.Up)) position.Y -= speed;
            if (keys_p1.IsKeyDown(Keys.Down)) position.Y += speed ;
            if (keys_p1.IsKeyDown(Keys.Left)) position.X -= speed;
            if(keys_p1.IsKeyDown(Keys.Right)) position.X += speed;

            position.X += pad_p1.ThumbSticks.Left.X * speed;
            position.Y -= pad_p1.ThumbSticks.Left.Y * speed;

            



      

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tomato);

            spriteBatch.Begin();
            if (visible)
            spriteBatch.Draw(myball, position, Color.White);
            
            

                spriteBatch.DrawString(mainfont, "position X " + position.X.ToString(), new Vector2(100, 100), Color.Green);
                spriteBatch.DrawString(mainfont, "position Y " + position.Y.ToString(), new Vector2(100, 150), Color.Red);
                
            
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
