using _2DGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Games3D.Engines;
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


        Player player;

        //Barrier
        Texture2D barrierTexture;
        List<Barrier> barriers;

        TimeSpan barrierSpawnTime;
        TimeSpan previousBarrierSpawnTime;

        Random random;

        Camera _camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;


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

            player = new Player();

            barriers = new List<Barrier>();

            previousBarrierSpawnTime = TimeSpan.Zero;
            barrierSpawnTime = TimeSpan.FromSeconds(.5f);

            random = new Random();

            _camera = new Camera(GraphicsDevice.Viewport);

       

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



            //Load all the textures for all the characters

            Texture2D ballFireTexture;
            Texture2D squareTexture;

            ballFireTexture = Content.Load<Texture2D>("Sprites/ballfire");
            squareTexture = Content.Load<Texture2D>("Sprites/player2");

            List<Texture2D> charactersTexture = new List<Texture2D>();

            charactersTexture.Add(ballFireTexture);
            charactersTexture.Add(squareTexture);


            player.Initialize(charactersTexture, GraphicsDevice);


            barrierTexture = Content.Load<Texture2D>("Sprites/barrier");

            


            AddBarrier();




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

            player.Update(gameTime, GraphicsDevice, barriers);
            UpdateBarrier(gameTime);



            _camera.lookAt(player.Position);

            base.Update(gameTime);
        }

        private void AddBarrier()
        {
            Animation barrierAnimation = new Animation();

            barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 64, 64, 1, 30, Color.White, 1f, true);

            Vector2 position = new Vector2(random.Next(100, GraphicsDevice.Viewport.Width - 100),
                random.Next(100, GraphicsDevice.Viewport.Height - 100));


            Barrier barrier = new Barrier();

            barrier.Initialize(barrierAnimation, position);

            barriers.Add(barrier);
            Console.WriteLine("Barriers: {0}", barriers.Count);
        }

        private void UpdateBarrier(GameTime gameTime)
        {
            /*if(gameTime.TotalGameTime - previousBarrierSpawnTime > barrierSpawnTime)
             {
                 previousBarrierSpawnTime = gameTime.TotalGameTime;
                 AddBarrier();
             }*/

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

            player.Draw(spriteBatch);

            for (int i = 0; i < barriers.Count; i++)
            {
                barriers[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
