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
using System.IO;
using gamelib2d;
namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Boolean gameover = false;

        int displaywidth = 800;
        int displayheight = 480;
        Random randomiser = new Random();
        int ystop = 0;
        animation explosion;
        animation death;
        float invunrelabletime ;

        int lives = 5;
        int score = 0;
        int highscore = 0;

        struct graphics2d
        {
            public Texture2D image;
            public Vector3 position;
            public Vector3 oldposition;
            public Rectangle rect;
            public Vector2 origin;
            public float size;
            public Vector3 velocity;
            public BoundingBox bbox;
            public BoundingSphere bsphere;
            public float power;
            public Boolean visible;
            public int sheildlives;
        }

        graphics2d background;
        const int columns = 10;
        const int rows = 4;

        graphics2d[,] invader = new graphics2d[columns, rows];
        graphics2d gameoverimage;
        graphics2d[] sheild = new graphics2d[4];

        int invadercounter;
        graphics2d laserbase;
        graphics2d goodlaser;
        graphics2d[] badlaser = new graphics2d[3];
        float lasercounter = 0;
        SoundEffect goodlasersound, badlasersound, goodhit,badhit,gameoversound;
        SpriteFont mainfont;

        float gameruntime;
  
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = displaywidth;
            this.graphics.PreferredBackBufferHeight = displayheight;
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
            if (File.Exists(@"highscore.txt"))
            {
                String line;
                StreamReader sr = new StreamReader(@"highscore.txt");
                line = sr.ReadLine();
                sr.Close();
                line = line.Trim();
                highscore = Convert.ToInt32(line);
            }

            mainfont = Content.Load<SpriteFont>("font");

            explosion = new animation(Content, "fire_001", 0, 0, .5f, Color.White, false, 24, 4, 5, false, false,false);
            death = new animation(Content, "fire_002", 0, 0, 1, Color.White, false, 24, 5, 5, false, false, false);
            background.image = Content.Load<Texture2D>("universe");
            background.rect.X = 0;
            background.rect.Y = 0;
            background.rect.Width = displaywidth;
            background.rect.Height = displayheight;

            gameoverimage.image = Content.Load<Texture2D>("gameover");
            gameoverimage.rect.Y = 0;
            gameoverimage.rect.X = 0;
            gameoverimage.rect.Width = displaywidth;
            gameoverimage.rect.Height = displayheight;

            gameoversound = Content.Load<SoundEffect>("explosion");

            laserbase.image = Content.Load<Texture2D>("goodship");
            laserbase.size = 0.3f;
            laserbase.origin.X = laserbase.image.Width / 2;
            laserbase.origin.Y = laserbase.image.Height / 2;
            laserbase.rect.Width = (int)(laserbase.image.Width * laserbase.size);
            laserbase.rect.Height = (int)(laserbase.image.Height * laserbase.size);
            laserbase.power = 1f;

            for (int i = 0; i < badlaser.Count(); i++)
            {
                badlaser[i].image = Content.Load<Texture2D>("invader_bullet");
                badlaser[i].size = 0.3f;
                badlaser[i].origin.X = badlaser[i].image.Width/2;
                badlaser[i].origin.Y = badlaser[i].image.Height / 2;
                badlaser[i].rect.Width = (int)(badlaser[i].image.Width * badlaser[i].size);
                badlaser[i].rect.Height = (int)(badlaser[i].image.Height * badlaser[i].size);
                badlaser[i].velocity = new Vector3(0, 10, 0);

            }
            badlasersound = Content.Load<SoundEffect>("laser");
            badhit = Content.Load<SoundEffect>("ballhit");


            goodlaser.image = Content.Load<Texture2D>("tank_bullet_1");
            goodlaser.size = 0.3f;
            goodlaser.origin.X = goodlaser.image.Width / 2;
            goodlaser.origin.Y = goodlaser.image.Height / 2;
            goodlaser.rect.Height = (int)(goodlaser.image.Height * goodlaser.size);
            goodlaser.rect.Width = (int)(goodlaser.image.Width * goodlaser.size);
            goodlaser.velocity = new Vector3(0, -5, 0);

            goodlasersound = Content.Load<SoundEffect>("laser");
            goodhit = Content.Load<SoundEffect>("crash");

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    invader[x, y].image = Content.Load<Texture2D>("invader");
                    invader[x, y].size = 0.12f;
                    invader[x, y].origin.X = invader[x, y].image.Width / 2;
                    invader[x, y].origin.Y = invader[x, y].image.Height / 2;
                    invader[x, y].rect.Width = (int)(invader[x, y].image.Width * 0.14);
                    invader[x, y].rect.Height = (int)(invader[x, y].image.Height * invader[x, y].size);
                    invader[x, y].power = 2;
                }

            sheild[0].image = Content.Load < Texture2D>("sheild1");
            sheild[1].image = Content.Load<Texture2D>("sheild2");
            sheild[2].image = Content.Load<Texture2D>("sheild3");
            sheild[3].image = Content.Load<Texture2D>("sheild4");

            for (int s = 0; s < sheild.Count(); s++)
            {
                sheild[s].origin.X = sheild[s].image.Width / 2;
                sheild[s].origin.Y = sheild[s].image.Height / 2;
                sheild[s].size = 0.4f;
                sheild[s].rect.Width = (int)(sheild[s].image.Width * sheild[s].size);
                sheild[s].rect.Height = (int)(sheild[s].image.Height * sheild[s].size);
            }


            sheild[0].rect.X = 100;
            sheild[0].rect.Y = displayheight - 155;
            sheild[1].rect.X = 300;
            sheild[1].rect.Y = displayheight - 155;
            sheild[2].rect.X = 500;
            sheild[2].rect.Y = displayheight - 155;
            sheild[3].rect.X = 700;
            sheild[3].rect.Y = displayheight - 155;
 
            for (int s = 0; s < sheild.Count(); s++)
            {
                sheild[s].bsphere = new BoundingSphere(new Vector3((float)sheild[s].rect.X, (float)sheild[s].rect.Y, 0), sheild[s].rect.Width / 2);
            }
            resetgame();

        }

        void resetship()
        {
            if (lives > 0)
            {
                invunrelabletime = 2000;
                laserbase.visible = true;
                                
            }
        }
        void resetgame()
        {
            gameover = false;
            lives = 5;

            for (int s = 0; s < sheild.Count(); s++)
            {
                sheild[s].sheildlives =8;
            }
            
            laserbase.position = new Vector3(displaywidth / 2, displayheight - 30, 0);
            score = 0;
          

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    invader[x, y].velocity.X = invader[x, y].power;
                    invader[x, y].velocity.Y = 0;
                }
            resetlevel();
        }

        void resetlevel()
        {
            invadercounter = rows * columns;
            for (int s = 0; s < sheild.Count(); s++)
            {
                sheild[s].visible = true;
                sheild[s].sheildlives += 2;
            }

            int firstx = 40;
            int firsty = 20;

            int xspacing = 10;
            int yspacing = 10;

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    invader[x, y].position.X = firstx + (x * invader[x, y].rect.Width *1.5f + xspacing);
                    invader[x, y].position.Y = firsty + (y * invader[x, y].rect.Height *1.2f + yspacing);
                    invader[x, y].visible = true;
                }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            StreamWriter sw = new StreamWriter(@"highscore.txt");
            sw.WriteLine(highscore.ToString());
            sw.Close();
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
            GamePadState[] pad = new GamePadState[1];
            pad[0] = GamePad.GetState(PlayerIndex.One);
            const float friction = 0.9f;
            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float starttime = (float)gameTime.TotalGameTime.TotalSeconds;
            float timegone = ((float)gameTime.TotalGameTime.TotalSeconds - starttime);

            if (!gameover)
            {
                gameruntime += timebetweenupdates;
                invunrelabletime-=timebetweenupdates;
                if (pad[0].Buttons.Back == ButtonState.Pressed)
                    gameover = true;
                
                laserbase.velocity.X += pad[0].ThumbSticks.Left.X * laserbase.power;
                laserbase.position += laserbase.velocity;
                laserbase.velocity *= friction;
                if (laserbase.position.X > displaywidth - laserbase.rect.Width / 2)
                {
                    laserbase.position.X = displaywidth - laserbase.rect.Width / 2;
                    laserbase.velocity.X = 0;
                }
                if (laserbase.position.X < laserbase.rect.Width / 2)
                {
                    laserbase.position.X = laserbase.rect.Width / 2;
                    laserbase.velocity.X = 0;
                }
                laserbase.rect.X = (int)laserbase.position.X;
                laserbase.rect.Y = (int)laserbase.position.Y;

                laserbase.bbox = new BoundingBox(new Vector3(laserbase.position.X - laserbase.rect.Width / 2, laserbase.position.Y - laserbase.rect.Height / 2, 0),
                    new Vector3(laserbase.position.X + laserbase.rect.Width / 2, laserbase.position.Y + laserbase.rect.Height / 2, 0));
                if (pad[0].Buttons.A == ButtonState.Pressed && !goodlaser.visible)
                {
                    goodlaser.visible = true;
                    goodlaser.position = laserbase.position;
                    goodlasersound.Play();
                }
                if (goodlaser.visible)
                {
                    goodlaser.position += goodlaser.velocity;
                    goodlaser.rect.Y = (int)goodlaser.position.Y;
                    goodlaser.rect.X = (int)goodlaser.position.X;

                    goodlaser.bbox = new BoundingBox(new Vector3(goodlaser.position.X - goodlaser.rect.Width / 2, goodlaser.position.Y - goodlaser.rect.Height / 2, 0),
                        new Vector3(goodlaser.position.X + goodlaser.rect.Width, goodlaser.position.Y + goodlaser.rect.Height / 2, 0));

                    if (goodlaser.position.Y + goodlaser.rect.Height / 2 < 0)
                        goodlaser.visible = false;
                }

                lasercounter -= timebetweenupdates;
                Boolean hitside = false;

                for (int x = 0; x < columns; x++)
                    for (int y = 0; y < rows; y++)
                    {
                        invader[x, y].velocity *= 1.0001f;

                        if (invader[x, y].visible)
                        {
                            invader[x, y].position += invader[x, y].velocity;

                            if (invader[x, y].position.X - invader[x, y].rect.Width / 2 <= 0 ||
                                invader[x, y].position.X + invader[x, y].rect.Width / 2 >= displaywidth)
                                hitside = true;

                            if (lasercounter < 0 && (invader[x, y].position.X - invader[x, y].rect.Width / 2 < laserbase.position.X) &&
                                (invader[x, y].position.X + invader[x, y].rect.Width / 2 > laserbase.position.X))
                            {
                                for(int i=0; i < badlaser.Count();i ++)
                                    if (!badlaser[i].visible)
                                    {
                                        badlasersound.Play();
                                        badlaser[i].visible = true;
                                        badlaser[i].position = invader[x, y].position;
                                        lasercounter = 600;
                                        break;
                                    }
                            }

                            
                        }
                    }

                
                for (int i = 0; i < badlaser.Count(); i++)
                    if (badlaser[i].visible)
                    {
                        badlaser[i].position += badlaser[i].velocity;
                        badlaser[i].rect.X = (int)badlaser[i].position.X;
                        badlaser[i].rect.Y = (int)badlaser[i].position.Y;
                        badlaser[i].bbox = new BoundingBox(new Vector3(badlaser[i].position.X - badlaser[i].rect.Width / 2, badlaser[i].position.Y - badlaser[i].rect.Height / 2, 0),
                            new Vector3(badlaser[i].position.X + badlaser[i].rect.Width / 2, badlaser[i].position.Y + badlaser[i].rect.Height / 2, 0));

                        if (badlaser[i].bbox.Intersects(laserbase.bbox)&& invunrelabletime<=0)
                        {
                            badlaser[i].visible = false;
                            lives--;
                            laserbase.visible = false;
                            
                            resetship();
                            goodhit.Play();
                            death.start(laserbase.position);
                            if (lives <= 0)
                            {
                                gameover = true;
                                gameoversound.Play();

                                if (score > highscore)
                                    highscore = score;

                            }
                          
                        }
                        death.update(timebetweenupdates);
                        if (badlaser[i].position.Y - badlaser[i].rect.Height / 2 > displayheight)
                            badlaser[i].visible = false;
                        
                    }

                for (int i = 0; i < badlaser.Count(); i++)
                    for (int s = 0; s < sheild.Count(); s++)
                    {
                        if (badlaser[i].visible && badlaser[i].bbox.Intersects(sheild[s].bsphere) && sheild[s].visible)
                        {
                            sheild[s].sheildlives--;
                            badlaser[i].visible = false;
                            
                        }
                        if (sheild[s].sheildlives <= 0)
                            sheild[s].visible = false;
                    }

                


                for (int y = rows - 1; y >= 0; y--)
                    for (int x = 0; x < columns; x++)
                    {
                        if (hitside)
                        {


                            invader[x, y].position.Y += invader[x, y].power * 3;
                            invader[x, y].velocity.X = -invader[x, y].velocity.X;


                        }
                        invader[x, y].rect.X = (int)invader[x, y].position.X;
                        invader[x, y].rect.Y = (int)invader[x, y].position.Y;

                        invader[x, y].bbox = new BoundingBox(new Vector3(invader[x, y].position.X - invader[x, y].rect.Width / 2, invader[x, y].position.Y - invader[x, y].rect.Height / 2, 0),
                    new Vector3(invader[x, y].position.X + invader[x, y].rect.Width / 2, invader[x, y].position.Y + invader[x, y].rect.Height / 2, 0));
                        if(goodlaser.visible && invader[x,y].visible && goodlaser.bbox.Intersects(invader[x,y].bbox))
                        {
                            goodlaser.visible=false;
                            invader[x,y].visible=false;
                            score +=10;
                            invadercounter--;
                            explosion.start(goodlaser.position);
                        }
                    }
                if (invadercounter <= 0)
                {
                    score += 50;
                    resetlevel();
                }
                explosion.update(timebetweenupdates);


            }

            else
            {
                // Game is Over
                if (pad[0].Buttons.Start == ButtonState.Pressed)
                    resetgame();
                if (pad[0].Buttons.Back == ButtonState.Pressed)
                    this.Exit();
            }
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
            spriteBatch.Draw(background.image, background.rect, Color.White);

            spriteBatch.DrawString(mainfont, "Score " + score.ToString() + " High Score " + highscore.ToString(), new Vector2(50, 10), Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);

            spriteBatch.DrawString(mainfont, " Lives " + lives.ToString(), new Vector2(displaywidth - 250, 10),
                Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);

            for (int i = 0; i < badlaser.Count(); i++)
            {
                if (badlaser[i].visible)
                    spriteBatch.Draw(badlaser[i].image, badlaser[i].rect, null, Color.White, 0, badlaser[i].origin, SpriteEffects.None, 0);
            }

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    if (invader[x, y].visible)
                        spriteBatch.Draw(invader[x, y].image, invader[x, y].rect, null, Color.White, 0, invader[x, y].origin, SpriteEffects.None, 0);

                }
            if (goodlaser.visible)
                spriteBatch.Draw(goodlaser.image, goodlaser.rect, null, Color.White, 0, goodlaser.origin, SpriteEffects.None, 0);

            if (invunrelabletime % 200 < 100)
                spriteBatch.Draw(laserbase.image, laserbase.rect, null, Color.White, 0, laserbase.origin, SpriteEffects.None, 0);
            explosion.drawme(ref spriteBatch);
            death.drawme(ref spriteBatch);
            

            for (int s = 0; s < sheild.Count(); s++)
            {
                if(sheild[s].visible)
                spriteBatch.Draw(sheild[s].image, sheild[s].rect,null, Color.White,0,sheild[s].origin,SpriteEffects.None,0);
            }
            if (gameover)
            {
                spriteBatch.Draw(gameoverimage.image, gameoverimage.rect, Color.White);
                spriteBatch.DrawString(mainfont, " Sorry Player 1 Your Game is Over".ToString(), new Vector2(100, 100), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
