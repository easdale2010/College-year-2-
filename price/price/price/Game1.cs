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

namespace price
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float price = 4.99f;  // creats a flost to hold the price
        int quantity = 1;     // creatats  interger to hold the quantity
        float total;    // creats a float to hold total

        SpriteFont font;  // craets a SPritefont

        Boolean iskeysreleased = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            font = Content.Load<SpriteFont>("arial"); // loads the font 

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (iskeysreleased)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    quantity++; // if the up arrow is pressed the quantity goes up
                    iskeysreleased = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    quantity--;  // if the down arrow is pressed the quantity goes down;
                    iskeysreleased = false;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
                iskeysreleased = true;



            total = quantity * price;  // this mulitiplies quantity by price and this the value to total;

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

            spriteBatch.DrawString(font, "price " + price.ToString(), new Vector2(50, 50), Color.White);
            spriteBatch.DrawString(font, "Quantity " + quantity.ToString(), new Vector2(50, 150), Color.White);



            if (total <= 0)
            {
                spriteBatch.DrawString(font, "Total " + total.ToString(), new Vector2(50, 250), Color.Red);
            }
            else
            {
                spriteBatch.DrawString(font, "Total " + total.ToString(), new Vector2(50, 250), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
