using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class Level
    {
        int width;
        int height;
        List<Barrier> walls;
        List<Collectable> collectables;
        List<Barrier> barriers;

        public Level() {}

        public void Initialize(int width, int height, Texture2D barrierTexture)
        {
            this.width = width;
            this.height = height;

            collectables = new List<Collectable>();
            walls = new List<Barrier>();
            barriers = new List<Barrier>();


            createWalls(barrierTexture);
            //createCollectables(barrierTexture);

            
        }

        private void createCollectables(Texture2D barrierTexture)
        {
            //Add collectables to the map

            Random r = new Random();
            float posX;
            float posY;

            for (int i = 0; i < 10; i++)
            {
                posX = r.Next(40, width - 40);
                posY = r.Next(40, height - 40);
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                //Collectable coll = new Collectable();

                //coll.Initialize(new Vector2(posX, posY));

                //collectables.Add(coll);
            }

        }

        private void createWalls(Texture2D barrierTexture)
        {



            int widthNeed = width / 32;
            int heightNeed = height / 32;


            for (int i = 0; i < widthNeed; i++)
            {

                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                if (i != 0)
                    barr.Initialize(barrierAnimation, new Vector2(walls[i - 1].Position.X + 32, 16), false, true);
                else
                    barr.Initialize(barrierAnimation, new Vector2(16, 16), false, true);

                walls.Add(barr);

            }

            for (int i = 0; i < widthNeed; i++)
            {

                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();
                if (i != 0)
                    barr.Initialize(barrierAnimation, new Vector2(walls[i - 1].Position.X + 32, heightNeed * 32), false, true);
                else
                    barr.Initialize(barrierAnimation, new Vector2(16, heightNeed * 32), false, true);

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
                    barr.Initialize(barrierAnimation, new Vector2(16, walls[counter - 1].Position.Y + 32), false, true);
                    //Console.WriteLine(barr.Position);
                }
                else if (i == 0)
                {
                    barr.Initialize(barrierAnimation, new Vector2(16, 48), false, true);
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
                    barr.Initialize(barrierAnimation, new Vector2(widthNeed * 32 - 16, walls[counter - 1].Position.Y + 32), false, true);
                }
                else if (i == 0)
                {
                    barr.Initialize(barrierAnimation, new Vector2(widthNeed * 32 - 16, 48), false, true);
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

            for(int i=0; i<collectables.Count; i++)
            {
                if (collectables[i].Active)
                    collectables[i].Update(gameTime);
                else
                    collectables.RemoveAt(i);
            }

            for (int i = 0; i < barriers.Count; i++)
            {
                if (barriers[i].Active)
                    barriers[i].Update(gameTime);
                else
                    barriers.RemoveAt(i);
            }

        }

        public List<Barrier> getWalls()
        {
            return walls;
        }

        public List<Barrier> getBarriers()
        {
            return barriers;
        }

        public List<Collectable> getCollectables()
        {
            return collectables;
        }

        public void setCollectables(List<Collectable> collectables)
        {
            this.collectables = collectables;
        }

        public void setBarriers(List<Barrier> barriers)
        {
            this.barriers = barriers;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].Active)
                    walls[i].Draw(spriteBatch);
            }

            for (int i = 0; i < collectables.Count; i++)
            {
                if (collectables[i].Active)
                    collectables[i].Draw(spriteBatch);
            }

            for (int i = 0; i < barriers.Count; i++)
            {
                if (barriers[i].Active)
                    barriers[i].Draw(spriteBatch);
            }
        }

    }
}
