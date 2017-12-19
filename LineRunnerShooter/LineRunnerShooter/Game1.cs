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
    /// </summary>
    /// 
    //http://rbwhitaker.wikidot.com/monogame-drawing-text-with-spritefonts Text in the game
    public class Game1 : Game
    {
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
        List<Orih> orihList;
 
        Lift startLift;
        Lift eindLift;
        List<LiftSide> liftSides;
        Vector2 mouse;

        UI ui;

        int points;

        public static SpriteFont font;

        bool enableUpdate;

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

            _afbeeldingBlokken = new List<Texture2D>();
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformSpritSheet2")); //0
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RotPlat")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("lava2"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("DreadBlock"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("liftSideIntro")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("CrossHair")); //5
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("Lift")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RatchetBackgrond"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("introTitle"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("introExplain")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("UI")); //10
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("Complete"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("target")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("ChallangeFailed"));


            _afbeeldingEnemys = new List<Texture2D>();
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyL")); //0
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyR"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("ARM"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("energyBall"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuy"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuyR")); //5
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("TestGuy")); 
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("TestGuyHDR"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SpShSmallGuy"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("Fireball"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("FireballUp"));


            _levelMaps = new List<Texture2D>();
            _levelMaps.Add(Content.Load<Texture2D>("Map"));
            _levelMaps.Add(Content.Load<Texture2D>("Map2"));
            _levelMaps.Add(Content.Load<Texture2D>("Map3"));
            _levelMaps.Add(Content.Load<Texture2D>("Map4"));

            music = new List<Song>();
            music.Add(Content.Load<Song>("introS"));
            music.Add(Content.Load<Song>("level"));
            music.Add(Content.Load<Song>("boss"));
            music.Add(Content.Load<Song>("CompleteS"));
            music.Add(Content.Load<Song>("failed"));


            General._afbeeldingBlokken = _afbeeldingBlokken;
            General._afbeeldingEnemys = _afbeeldingEnemys;
            General._levelMaps = _levelMaps;
            General.r = new Random();
            //held = new Hiro2(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 250, 1750);
            ui = new UI();
            loadLevel0();

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
            
            /*
            if (stateKey.IsKeyDown(Keys.F1))
            {
                camPos.X -= 1;
            }
            if (stateKey.IsKeyDown(Keys.F2))
            {
                camPos.X += 1;
            }
            */
            

            if (stateKey.IsKeyDown(Keys.F1))
            {
                loadLevel1(gameTime);
            }
            if (stateKey.IsKeyDown(Keys.F2))
            {
                loadLevel2(gameTime);
            }
            if (stateKey.IsKeyDown(Keys.F3))
            {
                loadLevel3(gameTime);
            }
            if (stateKey.IsKeyDown(Keys.F9))
            {
                isNextLevel = 4;
                currentLevel = -1;
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

            base.Update(gameTime);

            List<Rectangle> rectList = new List<Rectangle>();
            if (enableUpdate)
            {
                points += level.getPoints();
                switch (currentLevel)
                {
                    case -1: //Loading screen, 
                        {
                            orihList = new List<Orih>();
                            camPos = new Vector2(0, 0);
                            currentLevel = isNextLevel;
                            isNextLevel = 0;
                            switch (currentLevel)
                            {
                                case 0:
                                    {
                                        loadLevel0();
                                        break;
                                    }
                                case 1:
                                    {
                                        loadLevel1(gameTime);
                                        break;
                                    }
                                case 2:
                                    {
                                        loadLevel2(gameTime);
                                        break;
                                    }
                                case 3:
                                    {
                                        loadLevel3(gameTime);
                                        break;
                                    }
                                case 4:
                                    {
                                        ui.stopTimer(gameTime);
                                        loadResult();
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
                                eindLift.activate();
                            }
                            if (eindLift.Positie.Y < 100)
                            {
                                currentLevel = -1;
                                isNextLevel = 1;
                            }
                            if (startLift.isActive)
                            {
                                rectList.Add(startLift.getCollisionRectagle());
                            }
                            else
                            {
                                rectList.Add(eindLift.getCollisionRectagle());
                            }
                            for (int i = 0; i < liftSides.Count; i++)
                            {
                                liftSides[i].Update(gameTime);
                                rectList.Add(liftSides[i].getCollisionRectagle());
                            }
                            held.checkEnviroments(rectList);

                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            List<BulletBlueprint> hi = new List<BulletBlueprint>();
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, hi);
                            camera.Position = new Vector2(0, 200);
                            break;
                        }
                    case 1:
                        {

                            eindLift.activate(held.getFeetCollisionRect());
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.getRectangles();
                            rectList.Add(startLift.getCollisionRectagle());
                            rectList.Add(eindLift.getCollisionRectagle());
                            held.checkEnviroments(rectList);

                            List<BulletBlueprint> enemyBullets = new List<BulletBlueprint>();
                            foreach (Orih orihd in orihList)
                            {
                                orihd.checkEnviroments(rectList);
                                orihd.SeePlayer(held.getCollisionRectagle());
                                orihd.Update(gameTime, stateKey, held.getBullets());
                                if (!held.isGrounded) //The player is allowed to stand on the enemy and can go through the enemy
                                {
                                    held.isGrounded = held.getFeetCollisionRect().Intersects(orihd.getCollisionRectagle());
                                }
                                enemyBullets.AddRange(orihd.getBullets());
                            }

                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, enemyBullets);

                            if (eindLift.Positie.Y < 200)
                            {

                                currentLevel = -1;
                                isNextLevel = 2;
                            }
                            camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                            break;
                        }
                    case 2:
                        {
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.getRectangles();
                            rectList.Add(startLift.getCollisionRectagle());
                            rectList.Add(eindLift.getCollisionRectagle());
                            held.checkEnviroments(rectList);

                            List<BulletBlueprint> enemyBullets = new List<BulletBlueprint>();
                            foreach (Orih orihd in orihList)
                            {
                                orihd.checkEnviroments(rectList);
                                orihd.SeePlayer(held.getCollisionRectagle());
                                orihd.Update(gameTime, stateKey, held.getBullets());
                                if (!held.isGrounded)
                                {
                                    held.isGrounded = held.getFeetCollisionRect().Intersects(orihd.getCollisionRectagle());
                                }
                                enemyBullets.AddRange(orihd.getBullets());
                            }

                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, enemyBullets);
                            eindLift.activate(held.getFeetCollisionRect());

                            if (eindLift.Positie.Y < 200)
                            {
                                currentLevel = -1;
                                isNextLevel = 3;
                            }
                            camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                            break;
                        }
                    case 3:
                        {
                            startLift.Update(gameTime, held);
                            eindLift.Update(gameTime, held);

                            rectList = level.getRectangles();
                            rectList.Add(startLift.getCollisionRectagle());
                            rectList.Add(eindLift.getCollisionRectagle());
                            held.checkEnviroments(rectList);

                            boss.checkEnviroments(rectList);
                            boss.Update(gameTime, stateKey, held.getCollisionRectagle(), held.getBullets());

                            if (!boss.isAlive)
                            {
                                eindLift.activate(held.getFeetCollisionRect());
                            }


                            level.Update(gameTime, held);
                            held.Update(gameTime, stateKey, mouseState, camera.Position, mouse, boss.getBullets());


                            if (eindLift.Positie.Y < 200)
                            {
                                currentLevel = -1;
                                isNextLevel = 4;
                            }
                            camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                            break;
                        }
                    case 4:
                        {
                            break;
                        }
                }
            }
            ui.Update(gameTime, held, points);
            if(held.Lives < 4 && enableUpdate)
            {
                enableUpdate = false;
                gameOVer();
            }
            if (!enableUpdate)
            {
                camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                ui.updateDeath(camPos, gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.Enter)) //Dit staat verkeerd
                {
                    loadLevel0();
                    currentLevel = 0;
                    enableUpdate = true;
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
            camPos = cameraPos(camera.Focus, held.getCollisionRectagle());

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
                                spriteBatch.Draw(_afbeeldingBlokken[9], new Rectangle(500, 250, 600, 600), Color.White);
                                break;
                            default:
                                spriteBatch.Draw(_afbeeldingBlokken[8], new Rectangle(500, 250, 600, 600), Color.White);
                                break;
                        }
                        for (int i = 0; i < liftSides.Count; i++)
                        {
                            liftSides[i].Draw(spriteBatch);
                        }
                        held.draw(spriteBatch);
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
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.25), -300, 6500, 2500), Color.White);
                        level.Draw(spriteBatch, 0, 0);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        foreach (Orih orihd in orihList)
                        {
                            orihd.draw(spriteBatch);
                        }
                        held.draw(spriteBatch);
                        level.Draw(spriteBatch, 0, 0);
                        ui.showTime(spriteBatch, camPos);
                        break;
                    }
                case 2:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.25), -300, 6500, 2500), Color.White);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        foreach (Orih orihd in orihList)
                        {
                            orihd.draw(spriteBatch);
                        }
                        held.draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);
                        ui.showTime(spriteBatch, camPos);

                        break;
                    }
                case 3:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        spriteBatch.Draw(_afbeeldingBlokken[7], new Rectangle(Convert.ToInt16(camPos.X * 0.25), -300, 6500, 2500), Color.White);
                        startLift.Draw(spriteBatch);
                        eindLift.Draw(spriteBatch);

                        boss.draw(spriteBatch);
                        held.draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);
                        ui.showTime(spriteBatch, camPos);
                        break;
                    }
                case 4:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(General._afbeeldingBlokken[11], new Rectangle(0, 0, 1280, 720), Color.White);
                        ui.showResult(spriteBatch);
                        break;
                    }
             }
            spriteBatch.Draw(_afbeeldingBlokken[5], mouse, Color.White);
            Console.WriteLine(held.Location.ToString());
            if (!enableUpdate)
            {
                ui.showDeath(spriteBatch);
                
            }
            spriteBatch.End();
            /*
            spriteBatch.Begin();
            spriteBatch.DrawString(General.font, ("Live Points: " + held.Lives.ToString()), (camPos + new Vector2(100, 100)), Color.NavajoWhite);
            spriteBatch.End();
            */

            //Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Draw(gameTime);
        }

        public Vector2 cameraPos(Rectangle camer, Rectangle item) //TODO: cameraBox voor smoothere camera
        {
            Vector2 newCampos = camPos + ((item.Location.ToVector2() - new Vector2(600,600)) - camPos) / 8;
            newCampos.X = (int)(newCampos.X);
            if(item.X < 400 || item.X > 7100)
            {
                newCampos.Y = 1000;
            }
            newCampos.Y = (int)(newCampos.Y);
            return newCampos;
        }

        public void loadLevel0()
        {
            //TODO: explenation picture
            zoom = 1;
            orihList = new List<Orih>();
            camPos = new Vector2(0, 0);
            held = new Hiro2(8, new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], new Vector2(100,800));
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
        public void loadLevel1(GameTime gameTime) //TODO: Maak van level 1 een vriendelijke introductie level
        {

            zoom = 1 - 0.5f;
            currentLevel = 1;

            orihList = new List<Orih>();

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;

            level = new LevelControl(_levelMaps[1], _afbeeldingBlokken, orihList);
            //level.CreateWorld(_afbeeldingBlok, Content.Load<Texture2D>("platform"));
            held = new Hiro2(8, new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], new Vector2(200,3000));
            held.setToStartPos(new Vector2(200, 2400));
            eindLift = new Lift(new Vector2(7400, 1700), new Vector2(7400, 100));
            MediaPlayer.Play(music[1]);
            ui.startTimer(gameTime);
        }

        public void loadLevel2(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 2;
            
            orihList = new List<Orih>();
            
            held.setToStartPos(new Vector2(200, 2400));
            level = new LevelControl(_levelMaps[2], _afbeeldingBlokken, orihList);

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;

            eindLift = new Lift(new Vector2(7400, 900*2), new Vector2(7400, 100));
            
        }

        public void loadLevel3(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 3;
            boss = new BigBoy(4, new RobotMove(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], new Vector2(5200, 500));
            held.setToStartPos(new Vector2(200, 2400));
            level = new LevelControl(_levelMaps[3], _afbeeldingBlokken, orihList);

            startLift = new Lift(new Vector2(100, 2700), new Vector2(100, 950*2));
            startLift.isActive = true;

            eindLift = new Lift(new Vector2(7400, 900*2), new Vector2(7400, 100));
            MediaPlayer.Play(music[2]);
        }

        public void loadResult()
        {
            MediaPlayer.Play(music[3]);
        }

        public void gameOVer()
        {
            camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
            enableUpdate = false;
            ui = new UI();
            ui.gameOver(camPos);
            MediaPlayer.Play(music[4]);
        }
    }
}
