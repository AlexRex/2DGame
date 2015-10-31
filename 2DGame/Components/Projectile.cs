using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace _2DGame.Components
{
    public class Projectile
    {

        float speed;
        public Vector2 Position;
        Animation sprite;

        int direction;

        public bool Active;

        public float Damage;

        TimeSpan spawnDuration;
        Stopwatch lifeTime;

        public Projectile(){}

        public void Initialize(Vector2 initialPosition, int shootDirection, Texture2D texture)
        {
            speed = 20f;

            direction = shootDirection;

            Console.WriteLine("new projectile");
            Position = initialPosition;

            sprite = new Animation();

            sprite.Initialize(texture, Position, 64, 64, 3, 30, Color.White, 1, true);

            Damage = 15f;
            Active = true;

            spawnDuration = TimeSpan.FromSeconds(.5f);

            lifeTime = new Stopwatch();
            lifeTime.Start();

        }

        public void Update(GameTime gameTime)
        {
            switch (direction)
            {
                case 0:
                    sprite.Position.X += speed;
                    break;
                case 1:
                    sprite.Position.X += -speed;
                    break;
                case 2:
                    sprite.Position.Y += -speed;
                    break;
                case 3:
                    sprite.Position.Y += +speed;
                    break;
                default:
                    sprite.Position.X = 0;
                    sprite.Position.Y = 0;
                    break;
            }

           
           // Console.WriteLine(lifeTime.Elapsed.TotalSeconds);

            if (lifeTime.Elapsed.TotalSeconds >= spawnDuration.TotalSeconds)
            {
                Active = false;
                lifeTime.Stop();
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

    }
}
