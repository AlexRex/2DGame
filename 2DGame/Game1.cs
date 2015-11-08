using _2DGame.Components;
using _2DGame.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace _2DGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum STATE
        {
            LoginMenu,
            InitialMenu,
            CharacterSelectionMenu,
            InGame
        }

        public STATE GameState;

        Player player;
        Enemy enemy;
        ConnectionTest con; 

        //Menus
        InitialMenu initMenu;
        CharacterSelection charMenu;

        //Barrier
        Texture2D barrierTexture;
        List<Barrier> barriers;

        Texture2D background;
        SpriteFont messageFont;

        TimeSpan barrierSpawnTime;
        TimeSpan previousBarrierSpawnTime;

        private Vector2 bPo;
        Random random;

        Camera _camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;


            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            graphics.ApplyChanges();

            IsMouseVisible = true;


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

            //State of the game.
            GameState = STATE.InitialMenu;

            

            //Create the two players
            player = new Player();
            enemy = new Enemy();

            //Create the menu
            initMenu = new InitialMenu();
            charMenu = new CharacterSelection();


            //List for barriers; Helpers for spawning new barriers each .5 seconds
            barriers = new List<Barrier>();
            previousBarrierSpawnTime = TimeSpan.Zero;
            barrierSpawnTime = TimeSpan.FromSeconds(.5f);
            //Barreir position
            bPo = new Vector2(16, 16);
            //Random number for the position of the new barriers (not using now)
            random = new Random();

            //New camera object
            _camera = new Camera(GraphicsDevice.Viewport);

            //Create a connection with the server (actual: localhost)
            con = new ConnectionTest();

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

            messageFont = Content.Load<SpriteFont>("SpriteFont/MessgaeFont");


            //Background
            background = Content.Load<Texture2D>("Backgrounds/backgroundTest");
            //Console.WriteLine(background);
            

            //Load all the textures for all the characters

            Texture2D ballFireTexture;
            Texture2D squareTexture;

            ballFireTexture = Content.Load<Texture2D>("Sprites/ballfire");
            squareTexture = Content.Load<Texture2D>("Sprites/player2");

            List<Texture2D> charactersTexture = new List<Texture2D>();

            charactersTexture.Add(ballFireTexture);
            charactersTexture.Add(squareTexture);


            player.Initialize(charactersTexture, GraphicsDevice, con, enemy);
            enemy.Initialize(charactersTexture, GraphicsDevice, con, player);



            initMenu.Initialize(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), messageFont, this);
            charMenu.Initialize(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), messageFont, this);



            barrierTexture = Content.Load<Texture2D>("Sprites/barrier");
            AddBarrier();

            con.Initialize(enemy, player);




            // TODO: use this.Content to load your game content here
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
        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if(GameState == STATE.InGame)
            {
                if (player.Active)
                {
                    player.Update(gameTime, barriers);
                }

                if (enemy.Active)
                    enemy.Update(gameTime, barriers);

                //Uncomment for adding barriers
                //UpdateBarrier(gameTime); 

                _camera.lookAt(player.Position);

                con.Update();

                
            }
            else if(GameState == STATE.InitialMenu)
            {
                initMenu.Update(gameTime);
            }
            else if(GameState == STATE.CharacterSelectionMenu)
            {
                charMenu.Update(gameTime);
            }


            base.Update(gameTime);
        }


        private void AddBarrier()
        {

            Animation barrierAnimation = new Animation();

            barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

            Vector2 position = bPo;


            Barrier barrier = new Barrier();

            barrier.Initialize(barrierAnimation, position);

            barriers.Add(barrier);
           // Console.WriteLine("Barriers: {0}", barriers.Count);
            bPo.Y += 32;
        }

        private void UpdateBarrier(GameTime gameTime)
        {
            if(gameTime.TotalGameTime - previousBarrierSpawnTime > barrierSpawnTime)
             {
                 previousBarrierSpawnTime = gameTime.TotalGameTime;
                 AddBarrier();
             }

            for (int i = barriers.Count - 1; i >= 0; i--)
            {
                barriers[i].Update(gameTime);
                if (barriers[i].Active == false)
                {
                    barriers.RemoveAt(i);
                }
            }
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var viewMatrix = _camera.GetViewMatrix();


            spriteBatch.Begin(transformMatrix: viewMatrix);

            if(GameState == STATE.InGame)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                if(player.Active)
                    player.Draw(spriteBatch);
                if (enemy.Active)
                    enemy.Draw(spriteBatch);

                for (int i = 0; i < barriers.Count; i++)
                {
                    barriers[i].Draw(spriteBatch);
                }
            }

            else if(GameState == STATE.InitialMenu)
            {
                initMenu.Draw(spriteBatch);
            }
            else if (GameState == STATE.CharacterSelectionMenu)
            {
                charMenu.Draw(spriteBatch);
            }


            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
