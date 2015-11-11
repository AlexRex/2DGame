using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DGame.Components
{
    class Character 
    {
        public int Strength { get; set; }

        public int Ammunition { get; set; }

        public float Speed { get; set; }

        public float Health { get; set; }


        private Animation animation;

        public int Width { get { return Animation.FrameWidth; }  }
        public int Height { get { return Animation.FrameHeight; } }

        public Vector2 Position { get { return Animation.Position; } }

        public Animation Animation
        {
            get { return animation; }
        }

        public void Initialize(List<Texture2D> charactersTexture, int type, GraphicsDevice graphicsDevice)
        {

            animation = new Animation();
            Health = 100f;


            switch (type)
            {
                case 0:
                    Strength = 30;
                    Ammunition = 100;
                    Speed = 10.0f;
                    animation.Initialize(charactersTexture.ElementAt(type), Vector2.Zero, 32, 32, 1, 100, Color.White, 1f, true);
                    break;
                case 1:
                    Strength = 15;
                    Ammunition = 10;
                    Speed = 20.0f;
                    animation.Initialize(charactersTexture.ElementAt(type), Vector2.Zero, 32, 32, 1, 100, Color.White, 1f, true);
                    break;

                default:
                    Strength = 100;
                    Ammunition = 100;
                    Speed = 1.0f;
                    animation.Initialize(charactersTexture.ElementAt(type), Vector2.Zero, 32, 32, 1, 100, Color.White, 1f, true);
                    break;

            }

            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        public void UpdatePosition(Vector2 position)
        {
            if(position!=null)
                animation.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }
       

    }

}
