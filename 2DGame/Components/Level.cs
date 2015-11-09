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

            int widthNeed = width / 16;
            int heightNeed = height / 16;

           

            /*for (int i=1; i<=widthNeed; i++)
            {
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 16, 16, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                barr.Initialize(barrierAnimation, new Vector2(i * 24, 8));
                walls.Add(barr);
            }

            for (int i=0; i<heightNeed; i++)
            {
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 16, 16, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                if(i!=0)
                    barr.Initialize(barrierAnimation, new Vector2(8, walls[i-1].Position.Y+16));
                else
                    barr.Initialize(barrierAnimation, new Vector2(8, 8));

                walls.Add(barr);
            }*/

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
