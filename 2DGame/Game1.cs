using Components;
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

        Level level;

        Player player;
        Enemy enemy;
        ConnectionTest con; 

        //Menus
        InitialMenu initMenu;
        CharacterSelection charMenu;
        public int playerChar;

        //Barrier
        Texture2D barrierTexture;
        List<Barrier> barriers;

        Texture2D background;
        Texture2D backgroundMenu;

        SpriteFont menuFont;
        SpriteFont screenFont;

        TimeSpan barrierSpawnTime;
        TimeSpan previousBarrierSpawnTime;

        private Vector2 bPo;
        Random random;

        Camera _camera;

        public Color bgColor;
        public Color blackColor = new Color(51, 51, 51);

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

            bgColor = new Color(0, 167, 254);

            //State of the game.
            GameState = STATE.InitialMenu;

            playerChar = 0;

            //Create the two players
            player = new Player();
            enemy = new Enemy();

            level = new Level();

            //Create the menu
            initMenu = new InitialMenu();
            charMenu = new CharacterSelection();



            //List for barriers; Helpers for spawning new barriers each .5 seconds
            barriers = new List<Barrier>();
            previousBarrierSpawnTime = TimeSpan.Zero;
            barrierSpawnTime = TimeSpan.FromSeconds(.5f);
            //Barreir position
            bPo = new Vector2(8, 8);
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

            menuFont = Content.Load<SpriteFont>("SpriteFont/Animatic");
            screenFont = Content.Load<SpriteFont>("SpriteFont/AnimaticSmall");


            barrierTexture = Content.Load<Texture2D>("Sprites/WallTile");

            //Background
            background = Content.Load<Texture2D>("Backgrounds/background0");
            backgroundMenu = Content.Load<Texture2D>("Backgrounds/backgroundMenu");
            //Console.WriteLine(background);

            initMenu.Initialize(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), menuFont, this);
            charMenu.Initialize(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), menuFont, this);



            level.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, barrierTexture);


            con.Initialize(enemy, player, barrierTexture, level);




            // TODO: use this.Content to load your game content here
        }

        public void initializeUsers()
        {
            //Load all the textures for all the characters

            Texture2D ballFireTexture;
            Texture2D squareTexture;

            ballFireTexture = Content.Load<Texture2D>("Sprites/ballfire");
            squareTexture = Content.Load<Texture2D>("Sprites/player2");

            List<Texture2D> charactersTexture = new List<Texture2D>();

            charactersTexture.Add(ballFireTexture);
            charactersTexture.Add(squareTexture);


            player.Initialize(charactersTexture, GraphicsDevice, con, enemy, playerChar);
            enemy.Initialize(charactersTexture, GraphicsDevice, con, player, playerChar);


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
                    player.Update(gameTime, level.getWalls(), level.getCollectables());
                }

                if (enemy.Active)
                    enemy.Update(gameTime, level.getWalls(), level.getCollectables());

                level.Update(gameTime);

                //Uncomment for adding barriers
                //UpdateBarrier(gameTime); 


                _camera.lookAt(player.Position);

               // con.Update();

                
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

            barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 16, 16, 1, 30, Color.White, 1f, true);

            Vector2 position = bPo;


            Barrier barrier = new Barrier();

            barrier.Initialize(barrierAnimation, position, true);

            barriers.Add(barrier);
            Console.WriteLine("Barriers: {0}", barriers.Count);
            bPo.X += 16;
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

            GraphicsDevice.Clear(bgColor);

            var viewMatrix = _camera.GetViewMatrix();


            spriteBatch.Begin(transformMatrix: viewMatrix);

            if(GameState == STATE.InGame)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                level.Draw(spriteBatch);


                if(player.Active)
                    player.Draw(spriteBatch);
                if (enemy.Active)
                    enemy.Draw(spriteBatch);

                for (int i = 0; i < barriers.Count; i++)
                {
                    barriers[i].Draw(spriteBatch);
                }


                //Draw all variables

                spriteBatch.DrawString(screenFont, "Player Health: " + player.character.Health, new Vector2(1300, 70), Color.White);
                spriteBatch.DrawString(screenFont, "Player Score: " + player.score, new Vector2(1300, 100), Color.White);
                spriteBatch.DrawString(screenFont, "Player Speed: " + player.character.Speed, new Vector2(1300, 130), Color.White);
                spriteBatch.DrawString(screenFont, "Player Strength: "+player.character.Strength, new Vector2(1300, 160), Color.White);
                spriteBatch.DrawString(screenFont, "Player Ammunition: " + player.character.Ammunition, new Vector2(1300, 190), Color.White);

                spriteBatch.DrawString(screenFont, "Enemy Health: " + enemy.character.Health, new Vector2(1300, 250), Color.Red);
                spriteBatch.DrawString(screenFont, "Enemy Score: " + enemy.score, new Vector2(1300, 280), Color.Red);
                spriteBatch.DrawString(screenFont, "Enemy Speed: " + enemy.character.Speed, new Vector2(1300, 310), Color.Red);
                spriteBatch.DrawString(screenFont, "Enemy Strength: " + enemy.character.Strength, new Vector2(1300, 340), Color.Red);
                spriteBatch.DrawString(screenFont, "Enemy Ammunition: " + enemy.character.Ammunition, new Vector2(1300, 370), Color.Red);
            }

            else if(GameState == STATE.InitialMenu)
            {
                spriteBatch.Draw(backgroundMenu, new Vector2(0, 0), Color.White);

                initMenu.Draw(spriteBatch);
            }
            else if (GameState == STATE.CharacterSelectionMenu)
            {
                spriteBatch.Draw(backgroundMenu, new Vector2(0, 0), Color.White);

                charMenu.Draw(spriteBatch);
            }


            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
