using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DGame.Components
{
    class Level
    {
        int width;
        int height;
        List<Barrier> walls;

        public Level() {}

        public void Initialize(int width, int height, Texture2D barrierTexture)
        {
            this.width = width;
            this.height = height;
            walls = new List<Barrier>();

            int widthNeed = width / 32;
            int heightNeed = height / 32;



            for (int i = 0; i < widthNeed; i++)
            {

                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                if (i != 0)
                    barr.Initialize(barrierAnimation, new Vector2(walls[i - 1].Position.X + 32, 16), false);
                else
                    barr.Initialize(barrierAnimation, new Vector2(16, 16), false);

                walls.Add(barr);

            }

            for (int i = 0; i < widthNeed; i++)
            {

                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                if (i != 0)
                    barr.Initialize(barrierAnimation, new Vector2(walls[i - 1].Position.X + 32, heightNeed* 32), false);
                else
                    barr.Initialize(barrierAnimation, new Vector2(16, heightNeed* 32), false);

                walls.Add(barr);

            }

            for(int i = 0; i < heightNeed-1; i++)
            {
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                int counter = walls.Count;

                Barrier barr = new Barrier();
                if (i != 0 && i != heightNeed)
                {
                    barr.Initialize(barrierAnimation, new Vector2(16, walls[counter-1].Position.Y + 32), false);
                    //Console.WriteLine(barr.Position);
                }
                else if (i == 0)
                {
                    barr.Initialize(barrierAnimation, new Vector2(16, 48), false);
                   // Console.WriteLine(barr.Position);
                   // Console.WriteLine(walls.Count);
                }

                counter++;

                walls.Add(barr);

            }

            for (int i = 0; i < heightNeed - 1; i++)
            {
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                int counter = walls.Count;

                Barrier barr = new Barrier();
                if (i != 0 && i != heightNeed)
                {
                    barr.Initialize(barrierAnimation, new Vector2(widthNeed * 32 - 16, walls[counter - 1].Position.Y + 32), false);
                    Console.WriteLine(barr.Position);
                }
                else if (i == 0)
                {
                    barr.Initialize(barrierAnimation, new Vector2(widthNeed * 32 - 16, 48), false);
                    Console.WriteLine(barr.Position);
                    Console.WriteLine(walls.Count);
                }

                counter++;

                walls.Add(barr);

            }

        }

        public void Update(GameTime gameTime)
        {
            for(int i=0; i<walls.Count; i++)
            {
                if (walls[i].Active)
                    walls[i].Update(gameTime);
                else
                    walls.RemoveAt(i);
            }
        }

        public List<Barrier> getWalls()
        {
            return walls;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].Active)
                    walls[i].Draw(spriteBatch);
            }
        }

    }
}
