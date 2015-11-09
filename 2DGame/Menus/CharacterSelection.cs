using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DGame.Menus
{
    class CharacterSelection
    {
        int active;

        Game1 game;

        Vector2 screenDimension;

        SpriteFont spFont;

        public KeyboardState oldState;
        bool firstTime;


        public void Initialize(Vector2 screenDimension, SpriteFont font, Game1 game)
        {
            this.screenDimension = screenDimension;
            spFont = font;

            this.game = game;

            active = 0;

            oldState = Keyboard.GetState();
            firstTime = true;
        }

        public void Update(GameTime gameTime)
        {
            handleInput(gameTime);

        }

        private void handleInput(GameTime gameTime)
        {
            if (firstTime)
                oldState = Keyboard.GetState();

            var kbState = Keyboard.GetState();


            if (kbState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                active--;
            }

            else if (kbState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                active++;
            }

            else if (kbState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                game.playerChar = active;
                game.bgColor = game.blackColor;
                game.initializeUsers();
                game.GameState = Game1.STATE.InGame;
                
            }

            if (active > 1)
            {
                active = 1;
            }
            else if (active < 0)
            {
                active = 0;
            }

            oldState = kbState;
            if (firstTime)
                firstTime = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int posX = 150;
            int posY = 380;
            if (active == 0)
            {
                spriteBatch.DrawString(spFont, "Char1", new Vector2(posX, posY), Color.Black);
                spriteBatch.DrawString(spFont, "Char2", new Vector2(posX, posY + 60), Color.White);
            }
            else if (active == 1)
            {
                spriteBatch.DrawString(spFont, "Char1", new Vector2(posX, posY), Color.White);
                spriteBatch.DrawString(spFont, "Char2", new Vector2(posX, posY + 60), Color.Black);
            }


        }
    }
}
