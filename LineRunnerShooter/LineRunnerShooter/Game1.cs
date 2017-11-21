using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LineRunnerShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        BigBoy boss;
        List<Texture2D> _afbeeldingBlokken;
        List<Texture2D> _afbeeldingEnemys;
        List<Texture2D> _levelMaps;
        int currentLevel;
        Level level;
        Hiro held;
        //Orih orih;
        List<Orih> orihList;
        MovingPlatform platform;
        MovingPlatform platform2;
        Lift startLift;
        Lift eindLift;
        List<LiftSide> liftSides;

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
            

            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
            currentLevel = 0;
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _afbeeldingBlokken = new List<Texture2D>();
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RetroBlock")); //0
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RotPlat")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("Lava"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("DreadBlock"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsCornerL"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsCornerR")); //5
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideL"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideM"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideR"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsStreight"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("intro")); //10

            _afbeeldingEnemys = new List<Texture2D>();
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyL"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyR"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("ARM"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("energyBall"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuyL"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuyR"));

            _levelMaps = new List<Texture2D>();
            _levelMaps.Add(Content.Load<Texture2D>("Map"));
            _levelMaps.Add(Content.Load<Texture2D>("Map2"));
            _levelMaps.Add(Content.Load<Texture2D>("Map3"));
            //held = new Hiro2(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 250, 1750);
            loadLevel0();

            // TODO: use this.Content to load your game content here TODO: slaag afbeeldingen op in variabelen

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

            base.Update(gameTime);

            List<Rectangle> rectList = new List<Rectangle>();
            switch (currentLevel)
            {
                case 0:
                    
                    {
                        if (stateKey.IsKeyDown(Keys.Enter) && !startLift.isActive)
                        {
                            eindLift.activate();
                        }
                        if (eindLift.Positie.Y < 100)
                        {
                            loadLevel1(gameTime);
                        }
                        if (startLift.isActive)
                        {
                            rectList.Add(startLift.getCollisionRectagle());
                        }
                        else
                        {
                            rectList.Add(eindLift.getCollisionRectagle());
                        }
                        for (int i =0; i < liftSides.Count; i++)
                        {
                            liftSides[i].Update(gameTime);
                            rectList.Add(liftSides[i].getCollisionRectagle());
                        }
                        held.checkEnviroments(rectList);

                        startLift.Update(gameTime, held);
                        eindLift.Update(gameTime, held);

                        held.Update(gameTime, stateKey, mouseState, held.Location);

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

                        foreach (Orih orihd in orihList)
                        {
                            orihd.checkEnviroments(rectList);
                            orihd.SeePlayer(held.getCollisionRectagle());
                            orihd.Update(gameTime, stateKey, held.getBulletsCollision(orihd.getCollisionRectagle()));
                            if (!held.isGrounded)
                            {
                                held.isGrounded = held.getFeetCollisionRect().Intersects(orihd.getCollisionRectagle());
                            }
                        }

                        level.Update(gameTime, held.getFeetCollisionRect());
                        held.Update(gameTime, stateKey, mouseState, held.Location);

                        if(eindLift.Positie.Y < 200)
                        {
                            loadLevel2(gameTime);
                        }
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

                        foreach (Orih orihd in orihList)
                        {
                            orihd.checkEnviroments(rectList);
                            orihd.SeePlayer(held.getCollisionRectagle());
                            orihd.Update(gameTime, stateKey, held.getBulletsCollision(orihd.getCollisionRectagle()));
                            if (!held.isGrounded)
                            {
                                held.isGrounded = held.getFeetCollisionRect().Intersects(orihd.getCollisionRectagle());
                            }
                            held.checkHit(orihd.getAttackRect());
                        }

                        level.Update(gameTime, held.getFeetCollisionRect());
                        held.Update(gameTime, stateKey, mouseState, held.Location);
                        eindLift.activate(held.getFeetCollisionRect());

                        if (eindLift.Positie.Y < 200)
                        {
                            loadLevel3(gameTime);
                        }
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
                        boss.Update(gameTime, stateKey, held._Position, held.getBulletsRect());

                        if (!boss.isAlive)
                        {
                            eindLift.activate(held.getFeetCollisionRect());
                        }

                        foreach(Rectangle br in boss.getBulletsRect())
                        {
                            held.checkHit(br);
                        }
                        

                        level.Update(gameTime, held.getFeetCollisionRect());
                        held.Update(gameTime, stateKey, mouseState, held.Location);


                        if (eindLift.Positie.Y < 200)
                        {
                            loadLevel0();
                        }
                        break;
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
            GraphicsDevice.Clear(Color.DarkBlue);

            var viewMatrix = camera.GetViewMatrix();
            //_camera.Position = new Vector2(theHero.position.X - 200, theHero.position.Y - 300);// new Vector2(theHero.position.X + 200, theHero.position.Y+400);
            
            camera.Rotation = rotation;
            camera.Zoom = zoom;
            
            switch (currentLevel)
            {
                case 0:
                    {
                        camera.Position = new Vector2(0, 200);
                        spriteBatch.Begin(transformMatrix: viewMatrix);
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
                        spriteBatch.End();
                        break;
                    }
                case 1:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        level.Draw(spriteBatch, 0, 0);
                        startLift.Draw(spriteBatch, 1);
                        eindLift.Draw(spriteBatch);
                        foreach (Orih orihd in orihList)
                        {
                            orihd.draw(spriteBatch);
                        }
                        held.draw(spriteBatch);
                        level.Draw(spriteBatch, 0, 0);
                        spriteBatch.End();
                        break;
                    }
                case 2:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        
                        startLift.Draw(spriteBatch, 1);
                        eindLift.Draw(spriteBatch);

                        foreach (Orih orihd in orihList)
                        {
                            orihd.draw(spriteBatch);
                        }
                        held.draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);

                        spriteBatch.End();
                        break;
                    }
                case 3:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        
                        startLift.Draw(spriteBatch, 1);
                        eindLift.Draw(spriteBatch);

                        boss.draw(spriteBatch);
                        held.draw(spriteBatch);

                        level.Draw(spriteBatch, 0, 0);
                        spriteBatch.End();
                        break;
                    }
            }

            // TODO: Add your drawing code here
            
            //Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Draw(gameTime);
        }

        public Vector2 cameraPos(Rectangle camer, Rectangle item) //TODO: cameraBox voor smoothere camera
        {
            if(camer.Intersects(item))
            {

            }
            return new Vector2(held.Location.X - 700, camPos.Y);
        }

        public void loadLevel0()
        {
            //TODO: explenation picture
            zoom = 1;
            orihList = new List<Orih>();
            held = new Hiro(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 150, 1000);
            level = new Level();
            liftSides = new List<LiftSide>();
            for(int i = 0; i < 1100; i += 200)
            {
                liftSides.Add(new LiftSide(_afbeeldingBlokken[10], new Vector2(0, i), false));
                liftSides.Add(new LiftSide(_afbeeldingBlokken[10], new Vector2(300, i), true));
            }
            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 1500), new Vector2(100, 700));
            startLift.isActive = true;
            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 700), new Vector2(100, -100));
            currentLevel = 0;
        }
        public void loadLevel1(GameTime gameTime)
        {

            zoom = 1 - 0.5f;
            currentLevel = 1;

            orihList = new List<Orih>();

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            level = new Level(_levelMaps[2], _afbeeldingBlokken);
            //level.CreateWorld(_afbeeldingBlok, Content.Load<Texture2D>("platform"));
            held = new Hiro(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 200, 1750);
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 1600));
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 1500));
            platform = new MovingPlatform(_afbeeldingBlokken[0], new Vector2(1500, 300));
            platform2 = new MovingPlatform(_afbeeldingBlokken[0], new Vector2(1550, 300));
            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(7400, 900), new Vector2(7400, 100));
        }

        public void loadLevel2(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 2;
            
            orihList = new List<Orih>();
            orihList.Add( new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 5100));
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 5000));
            held.setStartPos();
            level = new Level(_levelMaps[1], _afbeeldingBlokken);

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(7400, 900), new Vector2(7400, 100));

        }

        public void loadLevel3(GameTime gameTime)
        {
            zoom = 1 - 0.5f;
            currentLevel = 3;
            boss = new BigBoy(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 5200);
            held.setStartPos();
            level = new Level(_levelMaps[1], _afbeeldingBlokken);

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(7400, 900), new Vector2(7400, 100));
        }
    }
}
