using LineRunnerShooter.Weapons.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace LineRunnerShooter //TODO: REFRACTOR REQUIRED !! 
{
    /// <summary>
    /// This is the main type for your game.
    /// Explenation required
    /// </summary>
    /// 
    //http://rbwhitaker.wikidot.com/monogame-drawing-text-with-spritefonts Text in the game
    public class Game1 : Game
    {
        //TODO: change explanation jump from A to Z
        //TODO: Remove collision box enemy
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        BigBoy boss;
        List<Texture2D> _afbeeldingBlokken;
        List<Texture2D> _afbeeldingEnemys;
        List<Texture2D> _levelMaps;
        List<Song> music; //http://www.gamefromscratch.com/post/2015/07/25/MonoGame-Tutorial-Audio.aspx
        int currentLevel;
        int isNextLevel;
        LevelControl level;
        Hiro2 held;
        //Orih orih;
        List<Enemy> orihList;
 
        Lift startLift;
        Lift eindLift;
        List<LiftSide> liftSides;
        Vector2 mouse;

        UI ui;
        bool isDebug;

        int points;

        public static SpriteFont font;

        bool enableUpdate;
        bool showVictory;

        int intro = 0;
        double lastEnter =0;

        public Game1() //https://stackoverflow.com/questions/720429/how-do-i-set-the-window-screen-size-in-xna Set Window Size
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
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
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
            currentLevel = 0;
            enableUpdate = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            General.font = Content.Load<SpriteFont>("textFont");
            General.fontBig = Content.Load<SpriteFont>("textFontBigger");

            _afbeeldingBlokken = new List<Texture2D>() {
             Content.Load<Texture2D>("platformSpritSheet2"), //0
             Content.Load<Texture2D>("RotPlat"),
             Content.Load<Texture2D>("lava2"),
             Content.Load<Texture2D>("DreadBlock"),
             Content.Load<Texture2D>("liftSideIntro"),
             Content.Load<Texture2D>("CrossHair"), //5
             Content.Load<Texture2D>("Lift"),
             Content.Load<Texture2D>("RatchetBackgrond"),
             Content.Load<Texture2D>("introTitle"),
             Content.Load<Texture2D>("introExplain"),
             Content.Load<Texture2D>("UI2"), //10
             Content.Load<Texture2D>("Complete"),
             Content.Load<Texture2D>("target"),
             Content.Load<Texture2D>("ChallangeFailed"),
             Content.Load<Texture2D>("RESULT"),
             Content.Load<Texture2D>("Complete2") //15
             };


            _afbeeldingEnemys = new List<Texture2D>() { 
            Content.Load<Texture2D>("SmallGuyL"), //0
            Content.Load<Texture2D>("SmallGuyR"),
            Content.Load<Texture2D>("ARM"),
            Content.Load<Texture2D>("energyBall"),
            Content.Load<Texture2D>("BigGuy"),
            Content.Load<Texture2D>("BigGuyR"), //5
            Content.Load<Texture2D>("TestGuy"),
            Content.Load<Texture2D>("TestGuyHDR"),
            Content.Load<Texture2D>("SpShSmallGuy"),
            Content.Load<Texture2D>("Fireball"),
            Content.Load<Texture2D>("FireballUp"), //10
            Content.Load<Texture2D>("Sheep")
            };


            _levelMaps = new List<Texture2D>() { 
            Content.Load<Texture2D>("Map"),
            Content.Load<Texture2D>("Map2"),
            Content.Load<Texture2D>("Map3"),
            Content.Load<Texture2D>("Map4")
            };

            music = new List<Song>() { 
            Content.Load<Song>("introS"),
            Content.Load<Song>("level"),
            Content.Load<Song>("boss"),
            Content.Load<Song>("CompleteS"),
            Content.Load<Song>("failed")
            };


            General._afbeeldingBlokken = _afbeeldingBlokken;
            General._afbeeldingEnemys = _afbeeldingEnemys;
            General._levelMaps = _levelMaps;
            General.random = new Random();
            //held = new Hiro2(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 250, 1750);
            ui = new UI();
            

            held = new Hiro2(8, new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], new Vector2(100, 800));
            LoadLevel0();
            isDebug = false;
            showVictory = false;
            // TODO: slaag afbeeldingen op in variabelen

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
        protected override void Update(GameTime gameTime)  //http://xnaresources.com/default.asp?page=Tutorial:StarDefense:9 gebruik levels en intro scherm
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            KeyboardState stateKey = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            mouse = mouseState.Position.ToVector2() / zoom + camera.Position - new Vector2(65, 65);

             /* Debug mode:
              *     enable with home
              *     level 1 with F1
              *     level 2 with F2
              *     boss fight with F3
              *     F5 and F6 control the camera rotation
              *     F7 and F8 control the camera zoom
              *     F9 show the result screen
              *     !! All weapons are available with debug mode 
              *     
              */ 
            if(isDebug)
            {
                if (stateKey.IsKeyDown(Keys.F1))
                {
                    LoadLevel1(gameTime);
                }
                if (stateKey.IsKeyDown(Keys.F2))
                {
                    LoadLevel2(gameTime);
                }
                if (stateKey.IsKeyDown(Keys.F3))
                {
                    LoadLevel3(gameTime);
                }
                
                if (stateKey.IsKeyDown(Keys.F5))
                {
                    rotation += .1f;
                }
                if (stateKey.IsKeyDown(Keys.F6))
                {
                    rotation -= .1f;
                }
                if (stateKey.IsKeyDown(Keys.F7))
                {
                    zoom += .1f;
                }
                if (stateKey.IsKeyDown(Keys.F8))
                {
                    zoom -= .1f;
                }
                if (stateKey.IsKeyDown(Keys.F9))
                {
                    isNextLevel = 4;
                    currentLevel = -1;
                }
                if (stateKey.IsKeyDown(Keys.PageDown))
                {
                    camPos.X -= 1;
                }
                if (stateKey.IsKeyDown(Keys.PageUp))
                {
                    camPos.X += 1;
                }
                held.MaxArms = 5;
            }

            if(stateKey.IsKeyDown(Keys.Home))
            {
                isDebug = true;
            }
            

            base.Update(gameTime);

            List<Rectangle> rectList = new List<Rectangle>();
            if (enableUpdate)
            {
                points += level.GetPoints();
                switch (currentLevel)
                {
                    case -1: //Loading screen, 
                        {
                            orihList = new List<Enemy>();
                            camPos = new Vector2(0, 0);
                            currentLevel = isNextLevel;
                            isNextLevel = 0;
                            switch (currentLevel)
                            {
                                case 0:
                                    {
                                        LoadLevel0();
                                        break;
                                    }
                                case 1:
                                    {
                                        LoadLevel1(gameTime);
                                        break;
                                    }
                                case 2:
                                    {
                                        LoadLevel2(gameTime);
                                        break;
                                    }
                                case 3:
                                    {
                                        LoadLevel3(gameTime);
                                        break;
                                    }
                                case 4:
                                    {
                                        ui.StopTimer(gameTime);
                                        LoadResult();
                                        break;
                                    }
                            }

                            break;
                        }
                    case 0:

                        {
                            if (stateKey.IsKeyDown(Keys.Enter) && !startLift.isActive && lastEnter < 0)
                            {
                                intro++;
                                lastEnter = 100;
                            }
                            if (stateKey.IsKeyUp(Keys.Enter))
                            {
                                lastEnter = -1;
                            }
                            if (stateKey.IsKeyDown(Keys.Enter) && intro > 2)
                            {
                                eindLift.Activate();
                            }
                            if (eindLift.Positie.Y < 100)
                            {
                                currentLevel = -1;
                                isNextLevel = 1;
                                intro = 0;
                            }
                            if (startLift.isActive)
                            {
                                rectList.Add(startLift.GetCollisionRectagle());
                            }
                            else
                            {
                                rectList.Add(eindLift.GetCollisionRectagle());
                            }
                            for (int i = 0; i < liftSides.Count; i++)
                            {
                                liftSides[i].Update(gameTime);
                                rectList.Add(liftSides[i].GetCollisionRectagle());
                            }
                            held.CheckEnviroments(rectList);

                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            List<BulletBlueprint> hi = new List<BulletBlueprint>();
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, hi);
                            camera.Position = new Vector2(0, 200);
                            break;
                        }
                    case 1:
                        {

                            eindLift.Activate(held.GetFeetCollisionRect());
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.GetRectangles();
                            rectList.Add(startLift.GetCollisionRectagle());
                            rectList.Add(eindLift.GetCollisionRectagle());
                            held.CheckEnviroments(rectList);

                            List<BulletBlueprint> enemyBullets = new List<BulletBlueprint>();
                            foreach (Enemy orihd in orihList)
                            {
                                orihd.CheckEnviroments(rectList);
                                orihd.SeePlayer(held.GetCollisionRectagle());
                                orihd.Update(gameTime, stateKey, held.GetBullets());
                                if (!held.isGrounded) //The player is allowed to stand on the enemy and can go through the enemy
                                {
                                    held.isGrounded = held.GetFeetCollisionRect().Intersects(orihd.GetCollisionRectagle());
                                }
                                enemyBullets.AddRange(orihd.GetBullets());
                            }

                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, enemyBullets);

                            if (eindLift.Positie.Y < 200)
                            {
                                currentLevel = -1;
                                isNextLevel = 2;
                            }
                            camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                            break;
                        }
                    case 2:
                        {
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.GetRectangles();
                            rectList.Add(startLift.GetCollisionRectagle());
                            rectList.Add(eindLift.GetCollisionRectagle());
                            held.CheckEnviroments(rectList);

                            List<BulletBlueprint> enemyBullets = new List<BulletBlueprint>();
                            foreach (Enemy orihd in orihList)
                            {
                                orihd.CheckEnviroments(rectList);
                                orihd.SeePlayer(held.GetCollisionRectagle());
                                orihd.Update(gameTime, stateKey, held.GetBullets());
                                if (!held.isGrounded)
                                {
                                    held.isGrounded = held.GetFeetCollisionRect().Intersects(orihd.GetCollisionRectagle());
                                }
                                enemyBullets.AddRange(orihd.GetBullets());
                            }

                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, enemyBullets);
                            eindLift.Activate(held.GetFeetCollisionRect());

                            if (eindLift.Positie.Y < 200)
                            {
                                currentLevel = -1;
                                isNextLevel = 3;
                            }
                            camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                            break;
                        }
                    case 3:
                        {
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.GetRectangles();
                            rectList.Add(startLift.GetCollisionRectagle());
                            rectList.Add(eindLift.GetCollisionRectagle());
                            held.CheckEnviroments(rectList);

                            boss.CheckEnviroments(rectList);
                            boss.Update(gameTime, stateKey, held.GetCollisionRectagle(), held.GetBullets());

                            if (!boss.IsAlive)
                            {
                                ui.StopTimer(gameTime);
                                showVictory = true;
                                enableUpdate = false;
                                ui.UpdateDeath(camPos, gameTime);
                            }

                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, boss.GetBullets());
                            camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                            break;
                        }
                    case 4:
                        {

                            if (stateKey.IsKeyDown(Keys.Enter) && lastEnter == 0)
                            {
                                intro++;
                                lastEnter = -1;
                            }
                            if (stateKey.IsKeyUp(Keys.Enter)) lastEnter = 0;
                            if (intro > 1)
                            {
                                LoadLevel0();
                                intro = 0;
                            }

                            break;
                        }
                }
            }

            ui.Update(gameTime, held, points);
            if(held.Lives < 1 && enableUpdate)
            {
                enableUpdate = false;
                GameOVer();
            }
            if (!enableUpdate  && !showVictory)
            {
                camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                ui.UpdateDeath(camPos, gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.Enter)) //Dit staat verkeerd
                {
                    LoadLevel0();
                    currentLevel = 0;
                    intro = 2;
                    enableUpdate = true;
                }
            }
            if(!enableUpdate && showVictory)
            {
                camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                ui.UpdateDeath(camPos, gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    currentLevel = -1;
                    isNextLevel = 4;
                    enableUpdate = true;
                    showVictory = false;
                }
            }
        }
        float rotation = 0;
        float zoom = 1-0.5f;
        Vector2 camPos = new Vector2();
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var viewMatrix = camera.GetViewMatrix();
            //_camera.Position = new Vector2(theHero.position.X - 200, theHero.position.Y - 300);// new Vector2(theHero.position.X + 200, theHero.position.Y+400);
            
            camera.Rotation = rotation;
            camera.Zoom = zoom;
            camPos = CameraPos(camera.Focus, held.GetCollisionRectagle());

            switch (currentLevel)
            {
                case -1:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(_afbeeldingBlokken[8], new Rectangle(300, 100, 600, 600), Color.White);
                        camPos = new Vector2(0, 0);
                        break;
                    }
                case 0:
                    {
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        switch (intro)
                        {
                            case 0:
                                spriteBatch.Draw(_afbeeldingBlokken[8], new Rectangle(500, 250, 600, 600), Color.White);
                                break;
                            case 1:
                                spriteBatch.Draw(_afbeeldingBlokken[9], new Rectangle(500, 250, 600, 600), new Rectangle(0,0, 800, 800), Color.White);
                                break;
                            case 2:
                                spriteBatch.Draw(_afbeeldingBlokken[9], new Rectangle(350, 50, 900, 900), new Rectangle(800, 0, 800, 800), Color.White);
                                break;
                            default:
                                spriteBatch.Draw(_afbeeldingBlokken[8], new Rectangle(500, 250, 600, 600), Color.White);
                                break;
                        }
                        for (int i = 0; i < liftSides.Count; i++)
                        {
                            liftSides[i].Draw(spriteBatch);
                        }
                        held.Draw(spriteBatch);
                        if (startLift.isActive)
                        {
                            startLift.Draw(spriteBatch);
                        }
                        else
                        {
                            eindLift.Draw(spriteBatch);
                        }
                        spriteBatch.DrawString(General.font, ("PRESS ENTER TO CONTINUE"), new Vector2(530,  850), Color.DarkRed);
                        break;
                    }
                case 1:
                    {

                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.3), -300, 7000, 2500), Color.White);
                        level.Draw(spriteBatch, 0, 0);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        foreach (Enemy orihd in orihList)
                        {
                            orihd.Draw(spriteBatch);
                        }
                        held.Draw(spriteBatch);
                        level.Draw(spriteBatch, 0, 0);
                        ui.ShowTime(spriteBatch, camPos);
                        break;
                    }
                case 2:
                    {
                        camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.3), -300, 7000, 2500), Color.White);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        foreach (Enemy orihd in orihList)
                        {
                            orihd.Draw(spriteBatch);
                        }
                        held.Draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);
                        ui.ShowTime(spriteBatch, camPos);

                        break;
                    }
                case 3:
                    {
                        camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.3), -300, 7000, 2500), Color.White);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        boss.Draw(spriteBatch);
                        held.Draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);
                        if (boss.isAlive)
                        {
                            ui.ShowTime(spriteBatch, camPos);
                        }
                        if (!boss.isAlive)
                        {
                            enableUpdate = false;
                            ui.ShowResultAnimated(spriteBatch);
                        }
                        break;
                    }
                case 4:
                    {
                        spriteBatch.Begin();
                        ui.ShowDemoEnd(spriteBatch, held);
                        break;
                    }
             }
            spriteBatch.Draw(_afbeeldingBlokken[5], mouse, Color.White);
            Console.WriteLine(held.Location.ToString());
            if (!enableUpdate)
            {
                if(showVictory)
                {
                    ui.ShowResultAnimated(spriteBatch);
                }
                else
                {
                    ui.ShowDeath(spriteBatch, camPos);
                }
                
            }
            if(isDebug)
            {
                spriteBatch.DrawString(General.font, ("DEBUG"), new Vector2(camPos.X, camPos.Y), Color.DeepSkyBlue);
            }
            spriteBatch.End();
            /*
            spriteBatch.Begin();
            spriteBatch.DrawString(General.font, ("Live Points: " + held.Lives.ToString()), (camPos + new Vector2(100, 100)), Color.NavajoWhite);
            spriteBatch.End();
            */

            Console.WriteLine(1000f/(gameTime.ElapsedGameTime.TotalMilliseconds));
            base.Draw(gameTime);
        }

        public Vector2 CameraPos(Rectangle camer, Rectangle item) //TODO: cameraBox voor smoothere camera
        {
            Vector2 newCampos = camPos + ((item.Location.ToVector2() - new Vector2(600,600)) - camPos) / 8;
            newCampos.X = (int)(newCampos.X);
            if(item.X < 400 || item.X > 9200)
            {
                newCampos.Y = 1000;
            }
            newCampos.Y = (int)(newCampos.Y);
            return newCampos;
        }

        public void LoadLevel0()
        {
            held.HealthUp();
            //TODO: explenation picture
            held.SetToStartPos(new Vector2(150, 1250));

            points = 0;
            zoom = 1;
            orihList = new List<Enemy>();
            camPos = new Vector2(0, 0);
            level = new LevelControl();
            liftSides = new List<LiftSide>();
            for(int i = 0; i < 1100; i += 200)
            {
                liftSides.Add(new LiftSide(4, new Vector2(0, i), false));
                liftSides.Add(new LiftSide(4, new Vector2(300, i), true));
            }
            startLift = new Lift(new Vector2(100, 1500), new Vector2(100, 700));
            startLift.isActive = true;
            eindLift = new Lift(new Vector2(100, 700), new Vector2(100, -100));
            currentLevel = 0;
            MediaPlayer.Play(music[0]);
        }
        public void LoadLevel1(GameTime gameTime) //TODO: Maak van level 1 een vriendelijke introductie level
        {
            held.HealthUp();
            zoom = 1 - 0.5f;
            currentLevel = 1;

            orihList = new List<Enemy>();

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;
            held.SetToStartPos(new Vector2(100, 2000));
            held.Reset();
            level = new LevelControl(_levelMaps[1], _afbeeldingBlokken, orihList);
            //level.CreateWorld(_afbeeldingBlok, Content.Load<Texture2D>("platform"));
            held.SetToStartPos(new Vector2(200, 2400));
            eindLift = new Lift(new Vector2(9400, 1700), new Vector2(9400, 100));
            MediaPlayer.Play(music[1]);
            ui.StartTimer(gameTime);
        }

        public void LoadLevel2(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 2;
            
            orihList = new List<Enemy>();
            
            held.SetToStartPos(new Vector2(200, 2400));
            level = new LevelControl(_levelMaps[2], _afbeeldingBlokken, orihList);

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;

            eindLift = new Lift(new Vector2(9400, 900*2), new Vector2(9400, 100));
            
        }

        public void LoadLevel3(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 3;
            boss = new BigBoy(4, new RobotMove(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], new Vector2(5200, 500));
            held.SetToStartPos(new Vector2(200, 2400));
            level = new LevelControl(_levelMaps[3], _afbeeldingBlokken, orihList);

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;

            eindLift = new Lift(new Vector2(9400, 900*2), new Vector2(9400, 100));
            MediaPlayer.Play(music[2]);
        }

        public void LoadResult()
        {
            intro = 1;
            MediaPlayer.Play(music[3]);
            if(points > 50)
            {
                held.MaxArms = 3;
            }
            if(points > 100)
            {
                held.MaxArms = 4;
            }
        }

        public void GameOVer()
        {
            camera.Position = CameraPos(camera.Focus, held.GetCollisionRectagle());
            enableUpdate = false;
            ui = new UI();
            ui.GameOver(camPos);
            MediaPlayer.Play(music[4]);
        }
    }
}
