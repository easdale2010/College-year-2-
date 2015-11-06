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

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Variable To Store Whether The Game Is Over Or Not
        Boolean gameover = false;


        //Screen Size
        int displaywidth = 800;
        int displayheight = 540;
        int brickcount;


        //Variable To generate Random Numbers
        Random randomiser = new Random();

        
        //Structure For Moving 2D Graphics
        struct graphics2d
        {

            public Texture2D image;         //Holds 2d Graphic
            public Vector3 position;        //On Screen Position
            public Vector3 oldposition;     //Position Of Graphic Before Collision
            public Rectangle rect;          //Holds Position And Dimensiong Of Graphic
            public Vector2 origin;          //Point On The Image Where The Positing Points To
            public float size;              //Size Ratio, For Scaling Up Or Down The Image Size When Drawn
            public float rotation;          //Amount Or Rotation To Apply
            public Vector3 velocity;        //Direction And Speed Of Object
            public BoundingSphere bsphere;  //Bounding Sphere For Object
            public BoundingBox bbox;        //Bounding Box For Object
            public float power;             //Power Of An Object With Regards To Acceleration & Speed
            public float rotationspeed;     //Speed At Which The Object Can Rotate, Turn Or Spin
            public int score;               //Score
            public float vibration ;
            public Boolean visible;

        }

        graphics2d ball;                            //Ball Graphic

        graphics2d bat;       //Array Of Bats

        graphics2d background;                      //Background Graphic

        SpriteFont mainfont;                        //Font For Drawing Text On The Screen

        const int maxscore = 200;                        // Sets A Max Score

        SoundEffect lightsabre, yoda, darth,winner;           //In-Game Sounds

        SoundEffect soundtrack;
        SoundEffectInstance music;

        const int columns = 10;
        const int rows = 4;
        graphics2d[,] brick = new graphics2d[columns, rows];
        int lives =5;
        int highscore = 0;

        int batmove = 0;



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
        /// 

        void resetgame()
        {
            gameover = false;
            lives = 3;
                        //Set Game Over False For The Start Of A New Game

            //Set Initial Positions Of The 2 Bats
            bat.position = new Vector3(200, displayheight -50, 0);
            

            //Resets Scores
            
            bat.score = 0;

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                    brick[x, y].visible = true;

            //Resets The Ball
            resetlevel();
        }

        void resetlevel()
        {
            brickcount = 40;

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                    brick[x,y].visible = true;
            resetball();
            ball.oldposition = ball.position;
            ball.bsphere = new BoundingSphere(ball.position, ball.rect.Width / 2);
        }


        //Set Initial Position Of Ball To The Middle Of The Screen
        void resetball()
        {
            ball.position = new Vector3(displaywidth / 2, displayheight / 2, 0);


            //Generate Random Velocities For The Ball
            do
            {
                ball.velocity.X = (randomiser.Next(3) - 1) * (5 + randomiser.Next(3));
                ball.velocity.Y = randomiser.Next(5);
            } while (ball.velocity.X == 0 || ball.velocity.Y == 0);

        }


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

            // TODO: use this.Content to load your game content here
            background.image = Content.Load<Texture2D>("brickbreak");                                        //Load Bullet Image
            background.rect.X = 0;
            background.rect.Y = 0;
            background.rect.Width = displaywidth;
            background.rect.Height = displayheight;

            ball.image = Content.Load<Texture2D>("starwarsremoteball");                                 //Load Ball Image
            ball.size = 0.15f;                                                                          //Set Size Ratio
            ball.origin.X = ball.image.Width / 2;                                                       //Set Origin To Centre Of Image
            ball.origin.Y = ball.image.Height/ 2;                                                       //Same As Above
            ball.rect.Width = (int)(ball.image.Width * ball.size);                                      //Set Size Of Rectangle Based On Size Ratio
            ball.rect.Height = (int)(ball.image.Height*ball.size);                                      //Same As Above
            ball.rotationspeed = 0.1f;                                                                  //How Fast The Ball Can Spin

            
            mainfont = Content.Load<SpriteFont>("quartz4");                                             //Load The quartz Font
    
            resetball();

            resetgame();


            lightsabre = Content.Load<SoundEffect>("cameras");
            yoda = Content.Load<SoundEffect>("strongam");
            darth = Content.Load<SoundEffect>("darkside");
            winner = Content.Load<SoundEffect>("explosion");
            soundtrack = Content.Load<SoundEffect>("MainMenu");
            music = soundtrack.CreateInstance();
            music.IsLooped = true;

         //   if (music.State == SoundState.Stopped) music.Play();
          //  if (music.State == SoundState.Playing) music.Stop();
          //  if (music.State == SoundState.Playing) music.Pause();
         //   if (music.State == SoundState.Paused) music.Resume();
  

            bat.image = Content.Load<Texture2D>("camera");                                          //Load Bat Graphic For Player 1
                                                                 //Load Bat Graphic For Player 2
            bat.size = .5f;                                                                 //Set The Size Ratio
                bat.origin.X = bat.image.Width / 2;                                       //Set The Origin To The Centre
                bat.origin.Y = bat.image.Height / 2;                                      //As Above
                bat.rect.Width = (int)(bat.image.Width / 2* bat.size);                //Set Size Of Rectangle Based On Size Ratio         
                bat.rect.Height = (int)(bat.image.Height /2  * bat.size);                //As Above
                bat.rotation = 0;                                                                //Set Initial Rotation
                bat.power = 3f;                                                                //Set Power Of Bat With Regards To Speed
            

           int firstx = 80;
           int firsty = 60;

           int xspacing = 5;
           int yspacing = 10;

            for( int y =0; y < rows;y++)
                for (int x = 0; x < columns; x++)
                {
                    brick[x, 0].image = Content.Load<Texture2D>("BRICK_green");
                    brick[x, 1].image = Content.Load<Texture2D>("BRICK_blue");
                    brick[x, 2].image = Content.Load<Texture2D>("BRICK_red");
                    brick[x, 3].image = Content.Load<Texture2D>("BRICK_purple");
                    brick[x, y].size = 0.5f;
                    brick[x, y].origin.X = brick[x, y].image.Width / 2;
                    brick[x, y].origin.Y = brick[x, y].image.Height / 2;
                    brick[x, y].rect.Width = (int)(brick[x, y].image.Width * brick[x, y].size);
                    brick[x, y].rect.Height = (int)(brick[x, y].image.Height * brick[x, y].size);
                    brick[x, y].rect.X = firstx + (x * (brick[x, y].rect.Width + xspacing));
                    brick[x, y].rect.Y = firsty + (y * (brick[x, y].rect.Height + yspacing));

                }
            if (File.Exists(@"highscore.txt"))
            {
                String line;
                StreamReader sr = new StreamReader(@"highscore.txt");
                line = sr.ReadLine();
                sr.Close();
                line = line.Trim();
                highscore = Convert.ToInt32(line);
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
            // TODO: Add your update logic here
            //Reads The Joypads
            GamePadState pad_p1 = GamePad.GetState(PlayerIndex.One);
                                    //Reads GamePad 1
           // pad[1] = GamePad.GetState(PlayerIndex.Two);                         //Reads GamePad 2
           

            const float friction = .8f;

            if(gameover)
                music.Play();

            if (!gameover)
            {


                if (pad_p1.Buttons.Back == ButtonState.Pressed)
                    gameover = true;

                //Move Two Bats


                if (!pad_p1.IsConnected)
                    if (bat.position.X > ball.position.X)
                        bat.velocity.X -= bat.power;
                    else
                        bat.velocity.X += bat.power;

                //Move The Bats Based On Joypad Input
                //  bat[i].position.Y -= pad[i].ThumbSticks.Left.Y * bat[i].power;
                bat.velocity.X += pad_p1.ThumbSticks.Left.X * bat.power;
                bat.position += bat.velocity;

                bat.velocity *= friction;

                //Alter Bat Direction When Bat Hits Bounderies
                //Bat Hits The Bottom Of The Screen
                if (bat.position.Y > displayheight - bat.rect.Height / 2)
                {
                    bat.position.Y = displayheight - bat.rect.Height / 2;
                    bat.velocity.Y = 0;
                }
                //Bat Hits The Top Of The Screen
                if (bat.position.Y < bat.rect.Height / 2)
                {
                    bat.position.Y = bat.rect.Height / 2;
                    bat.velocity.Y = 0;
                }

                if (bat.position.X > displaywidth - bat.rect.Width / 2)
                {
                    bat.position.X = displaywidth - bat.rect.Width / 2;
                    bat.velocity.X = 0;
                }
                if (bat.position.X < bat.rect.Width / 2)
                {
                    bat.position.X = bat.rect.Width / 2;
                    bat.velocity.X = 0;
                }

                //Set The Bat Rectangle To The Current Position
                bat.rect.X = (int)bat.position.X;
                bat.rect.Y = (int)bat.position.Y;

                //Create Bounding Box
                bat.bbox = new BoundingBox(new Vector3(bat.position.X - bat.rect.Width / 2, bat.position.Y - bat.rect.Height / 2, 0),
                              new Vector3(bat.position.X + bat.rect.Width / 2, bat.position.Y + bat.rect.Height / 2, 0));



                ball.position += ball.velocity;                 //Move The Ball
                ball.rotation += ball.rotationspeed;            //Spins Ball
                ball.velocity *= 1.0005f;                       //Speeds Ball Up Over Time


                //If The Ball Hits The Top Or The Bottom Of The Screen Reverse Its Direction On The Y Axis
                if (ball.position.Y < ball.rect.Height / 2)
                {
                    ball.velocity.Y = -ball.velocity.Y;
                    ball.position.Y = ball.rect.Height / 2;
                }
                if (ball.position.Y > displayheight + ball.rect.Height / 2)
                {
                    lives--;
                    resetball();
                    if (lives <= 0)
                    {
                        gameover = true;
                        if (bat.score > highscore)
                            highscore = bat.score;
                    }
                }

                //Check If The Ball Goes Out Either Side
                if (ball.position.X > displaywidth - ball.rect.Width / 2)
                {
                    //  bat[0].score += 5;
                    //  resetball();
                    //  yoda.Play();
                    ball.position.X = displaywidth - ball.rect.Height / 2;
                    ball.velocity.X = -ball.velocity.X;

                }


                if (ball.position.X < ball.rect.Width / 2)
                {
                    // bat[1].score += 5;
                    //  resetball();
                    // darth.Play();
                    ball.position.X = ball.rect.Width / 2;
                    ball.velocity.X = -ball.velocity.X;


                }



                float ballspeed = ball.velocity.Length();
                ball.velocity.Normalize();

                Boolean hitball = false;

                //Create BoundingSphere Around The Ball
                ball.bsphere = new BoundingSphere(ball.position, ball.rect.Width / 2);




                // Bounce Ball Off Bat 1
                if (bat.bbox.Intersects(ball.bsphere) && bat.position.Y >= ball.position.Y)
                {
                    hitball = true;
                    ball.velocity.Y = -Math.Abs(ball.velocity.Y);
                    ball.velocity.X = (ball.position.X - bat.position.X) / 40f;//Force Ball To Go Left
                    // ball.velocity.Y += (ball.position.Y - bat[1].position.Y) / 40f;
                    lightsabre.Play();
                    bat.vibration = 1f;
                }

                for (int y = 0; y < rows; y++)
                    for (int x = 0; x < columns; x++)
                    {
                        brick[x, y].bbox = new BoundingBox(new Vector3(brick[x, y].rect.X - brick[x, y].rect.Width / 2, brick[x, y].rect.Y - brick[x, y].rect.Height / 2, 0),
                            new Vector3(brick[x, y].rect.X + brick[x, y].rect.Width / 2, brick[x, y].rect.Y + brick[x, y].rect.Height / 2, 0));

                        if (ball.bsphere.Intersects(brick[x, y].bbox) && brick[x, y].visible)
                        {
                            hitball = true;

                            if (ball.oldposition.Y - ball.rect.Height / 2 > brick[x, y].rect.Y + brick[x, y].rect.Height / 2)
                                ball.velocity.Y = Math.Abs(ball.velocity.Y);
                            if (ball.oldposition.Y + ball.rect.Height / 2 < brick[x, y].rect.Y - brick[x, y].rect.Height / 2)
                                ball.velocity.Y = -Math.Abs(ball.velocity.Y);
                            if (ball.oldposition.X + ball.rect.Width / 2 < brick[x, y].rect.X - brick[x, y].rect.Width / 2)
                                ball.velocity.X = -Math.Abs(ball.velocity.X);
                            if (ball.oldposition.X - ball.rect.Width / 2 > brick[x, y].rect.X + brick[x, y].rect.Width / 2)
                                ball.velocity.X = Math.Abs(ball.velocity.X);

                            brick[x, y].visible = false;

                            bat.score += 5;
                            brickcount--;
                                
                            if (brickcount <= 0)
                                resetlevel();
                        }

                        brick[x, y].rect.X += batmove;

                        if (brickcount == 5 && batmove == 0)
                        {
                            batmove = 5;
                        }

                        if (brick[x, y].rect.X > displaywidth - brick[x, y].rect.Width / 2)
                            batmove = -Math.Abs(batmove);
                        else if (brick[x, y].rect.X < brick[x, y].rect.Width / 2)
                            batmove = Math.Abs(batmove); 

                    }






                GamePad.SetVibration(PlayerIndex.One, bat.vibration, bat.vibration);

                if (bat.vibration > 0)
                {
                    bat.vibration -= 0.1f;
                }


                ball.velocity.Normalize();
                ball.velocity *= ballspeed;

                if (!hitball)
                    ball.oldposition = ball.position;
                else
                    ball.position = ball.oldposition;

                //Set Position Of Ball Rectangle To Match Its Position For Drawing Purposes
                ball.rect.X = (int)ball.position.X;
                ball.rect.Y = (int)ball.position.Y;

                //If Either Player Reaches Maxscore, End Game
                if (lives == 0)
                {
                    winner.Play();
                    gameover = true;

                    //winner.Play();
                }
            }
            else
            {
                //Game Is Over
                //Start Game Again When User Presses Start

                if (pad_p1.Buttons.Start == ButtonState.Pressed)
                    resetgame();
                if (pad_p1.Buttons.Back == ButtonState.Pressed)
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

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background.image, background.rect, Color.White);


            for(int y=0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    if (brick[x, y].visible)
                        spriteBatch.Draw(brick[x, y].image, brick[x, y].rect, null, Color.White, brick[x, y].rotation, brick[x, y].origin, SpriteEffects.None, 0);
                }

            spriteBatch.Draw(ball.image, ball.rect, null, Color.White, ball.rotation, ball.origin, SpriteEffects.None, 0);
            spriteBatch.Draw(bat.image, bat.rect, null, Color.White, bat.rotation, bat.origin, SpriteEffects.None, 0);
            
           

           
           // spriteBatch.DrawString(mainfont, "P1 Score " + bat[0].score.ToString(), new Vector2(10, 10), Color.Yellow);
           // spriteBatch.DrawString(mainfont, " P2 Score " + bat[1].score.ToString(), new Vector2(displaywidth - 220, 10), Color.Yellow);
            spriteBatch.DrawString(mainfont, "Lives " + lives.ToString(), new Vector2(50, 50), Color.Yellow);
            spriteBatch.DrawString(mainfont, "Score " + bat.score.ToString(), new Vector2(550, 50), Color.White);

            spriteBatch.DrawString(mainfont, "Highscore " + highscore.ToString(), new Vector2(200, 50), Color.Turquoise);
            

      
          //  spriteBatch.DrawString(mainfont, "LIVES" + lives.ToString(), new Vector2(50,50), Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
