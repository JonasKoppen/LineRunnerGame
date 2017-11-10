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
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RetroBlock")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("RotPlat")); 
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("Lava"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("DreadBlock"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsCornerL"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsCornerR"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideL"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideM"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsSideR"));
            _afbeeldingBlokken.Add(Content.Load<Texture2D>("platformsStreight"));

            _afbeeldingEnemys = new List<Texture2D>();
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyL"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("SmallGuyR"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("ARM"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("energyBall"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuyL"));
            _afbeeldingEnemys.Add(Content.Load<Texture2D>("BigGuyR"));

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
            if (stateKey.IsKeyDown(Keys.F1))
            {
                camPos.X -= 1;
            }
            if (stateKey.IsKeyDown(Keys.F2))
            {
                camPos.X += 1;
            }
            if (stateKey.IsKeyDown(Keys.F3))
            {
                rotation += .1f;
            }
            if (stateKey.IsKeyDown(Keys.F4))
            {
                rotation -= .1f;
            }
            if (stateKey.IsKeyDown(Keys.F5))
            {
                zoom += .1f;
            }
            if (stateKey.IsKeyDown(Keys.F6))
            {
                zoom -= .1f;
            }
            base.Update(gameTime);
            switch (currentLevel)
            {
                case 0:
                    {
                        if (stateKey.IsKeyDown(Keys.Enter))
                        {
                            loadLevel1(gameTime);
                        }
                        break;
                    }
                case 1:
                    {
                        held.PlatformUpdate(startLift.Update(gameTime, held.getFeetCollisionRect()));
                        held.PlatformUpdate(eindLift.Update(gameTime, held.getFeetCollisionRect()));

                        held.PlatformUpdate(platform.Update(gameTime, (held.getFeetCollisionRect()), 1));
                        held.PlatformUpdate(platform2.Update(gameTime, (held.getFeetCollisionRect())));

                        held.isGrounded = level.checkCollision(held.getFeetCollisionRect());
                       

                        foreach (Orih orihd in orihList)
                        {
                            orihd.isGrounded = level.checkCollision(orihd.getFeetCollisionRect());
                            orihd.canLeft = level.checkCollision(orihd.getLeftCollision());
                            orihd.canRight = level.checkCollision(orihd.getRightCollision());
                            orihd.SeePlayer(held.getCollisionRectagle());
                            orihd.Update(gameTime, stateKey, held.getBulletsCollision(orihd.getCollisionRectagle()));
                        }

                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(platform.getCollisionRectagle());
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(platform2.getCollisionRectagle());
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(startLift.getCollisionRectagle());
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(eindLift.getCollisionRectagle());
                            eindLift.activate(held.getFeetCollisionRect());
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
                        held.PlatformUpdate(startLift.Update(gameTime, held.getFeetCollisionRect()));
                        held.PlatformUpdate(eindLift.Update(gameTime, held.getFeetCollisionRect()));

                        held.isGrounded = level.checkCollision(held.getFeetCollisionRect());


                        foreach (Orih orihd in orihList)
                        {
                            orihd.isGrounded = level.checkCollision(orihd.getFeetCollisionRect());
                            orihd.canLeft = level.checkCollision(orihd.getLeftCollision());
                            orihd.canRight = level.checkCollision(orihd.getRightCollision());
                            orihd.SeePlayer(held.getCollisionRectagle());
                            orihd.Update(gameTime, stateKey, held.getBulletsCollision(orihd.getCollisionRectagle()));
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(startLift.getCollisionRectagle());
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(eindLift.getCollisionRectagle());
                            eindLift.activate(held.getFeetCollisionRect());
                        }

                        level.Update(gameTime, held.getFeetCollisionRect());
                        held.Update(gameTime, stateKey, mouseState, held.Location);
                        

                        if (eindLift.Positie.Y < 200)
                        {
                            loadLevel3(gameTime);
                        }
                        break;
                    }
                case 3:
                    {
                        held.PlatformUpdate(startLift.Update(gameTime, held.getFeetCollisionRect()));
                        held.PlatformUpdate(eindLift.Update(gameTime, held.getFeetCollisionRect()));

                        held.isGrounded = level.checkCollision(held.getFeetCollisionRect());

                        boss.canLeft = level.checkCollision(boss.getLeftCollision());
                        boss.canRight = level.checkCollision(boss.getRightCollision());
                        boss.isGrounded = level.checkCollision(boss.getFeetCollisionRect());
                        boss.Update(gameTime, stateKey, held._Position, held.getBulletsRect());

                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(startLift.getCollisionRectagle());
                        }
                        if (!held.isGrounded)
                        {
                            held.isGrounded = held.getFeetCollisionRect().Intersects(eindLift.getCollisionRectagle());
                        }
                        if (!boss.isAlive)
                        {
                            eindLift.activate(held.getFeetCollisionRect());
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var viewMatrix = camera.GetViewMatrix();
            //_camera.Position = new Vector2(theHero.position.X - 200, theHero.position.Y - 300);// new Vector2(theHero.position.X + 200, theHero.position.Y+400);
            
            camera.Rotation = rotation;
            camera.Zoom = zoom;
            
            switch (currentLevel)
            {
                case 0:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(_afbeeldingBlokken[1], new Rectangle(500, 500, 100, 50), Color.White);
                        spriteBatch.End();
                        break;
                    }
                case 1:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        level.Draw(spriteBatch, 0, 0);
                        platform.Draw(spriteBatch, 1);
                        platform2.Draw(spriteBatch, 1);
                        startLift.Draw(spriteBatch, 1);
                        eindLift.Draw(spriteBatch);
                        foreach (Orih orihd in orihList)
                        {
                            orihd.draw(spriteBatch);
                        }
                        held.draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case 2:
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
                        spriteBatch.End();
                        break;
                    }
                case 3:
                    {
                        camera.Position = cameraPos(camera.Focus, held.getCollisionRectagle());
                        spriteBatch.Begin(transformMatrix: viewMatrix);
                        level.Draw(spriteBatch, 0, 0);
                        startLift.Draw(spriteBatch, 1);
                        eindLift.Draw(spriteBatch);
                        boss.draw(spriteBatch);
                        held.draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
            }

            // TODO: Add your drawing code here
            
            //Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Draw(gameTime);
        }

        public Vector2 cameraPos(Rectangle camer, Rectangle item)
        {
            if(camer.Intersects(item))
            {

            }
            return new Vector2(held.Location.X - 700, camPos.Y);
        }

        public void loadLevel0()
        {
            orihList = new List<Orih>();
            level = new Level();
            currentLevel = 0;
        }
        public void loadLevel1(GameTime gameTime)
        {
            
            currentLevel = 1;

            orihList = new List<Orih>();

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            level = new Level(Content.Load<Texture2D>("Map"), _afbeeldingBlokken);
            //level.CreateWorld(_afbeeldingBlok, Content.Load<Texture2D>("platform"));
            held = new Hiro(_afbeeldingEnemys[0], _afbeeldingEnemys[1], new MovePlayer(), _afbeeldingEnemys[2], _afbeeldingEnemys[3], 250, 1750);
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 1600));
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 1500));
            platform = new MovingPlatform(_afbeeldingBlokken[0], new Vector2(1500, 300));
            platform2 = new MovingPlatform(_afbeeldingBlokken[0], new Vector2(1550, 300));
            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(3000, 950), new Vector2(3000, 100));
        }

        public void loadLevel2(GameTime gameTime)
        {
            currentLevel = 2;
            
            orihList = new List<Orih>();
            orihList.Add( new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 5100));
            orihList.Add(new Orih(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 5000));
            held.setStartPos();
            level = new Level(Content.Load<Texture2D>("Map2"), _afbeeldingBlokken);

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(7400, 900), new Vector2(7400, 100));

        }

        public void loadLevel3(GameTime gameTime)
        {
            currentLevel = 3;
            boss = new BigBoy(_afbeeldingEnemys[4], _afbeeldingEnemys[5], new RobotMove(), _afbeeldingEnemys[3], 4700);
            held.setStartPos();
            level = new Level(Content.Load<Texture2D>("Map2"), _afbeeldingBlokken);

            startLift = new Lift(_afbeeldingBlokken[0], new Vector2(100, 2000), new Vector2(100, 950));
            startLift.isActive = true;

            eindLift = new Lift(_afbeeldingBlokken[0], new Vector2(7400, 900), new Vector2(7400, 100));
        }
    }
}
