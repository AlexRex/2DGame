using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DGame.Menus
{
    class InitialMenu
    {

        int active;

        Game1 game;

        Vector2 screenDimension;

        SpriteFont spFont;

        KeyboardState oldState;

        public void Initialize(Vector2 screenDimension, SpriteFont font, Game1 game)
        {
            this.screenDimension = screenDimension;
            spFont = font;

            this.game = game;

            active = 0;

            oldState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            handleInput(gameTime);
        }

        private void handleInput(GameTime gameTime)
        {
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
                game.GameState = Game1.STATE.CharacterSelectionMenu;
            }

            if (active > 2)
            {
                active = 2;
            }
            else if(active < 0)
            {
                active = 0;
            }

            oldState = kbState;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int posX = 150;
            int posY = 380;
            if (active == 0)
            {
                spriteBatch.DrawString(spFont, "Play", new Vector2(posX, posY), Color.Black);
                spriteBatch.DrawString(spFont, "Login", new Vector2(posX, posY + 60), Color.White);
                spriteBatch.DrawString(spFont, "Register", new Vector2(posX, posY + 120), Color.White);
            }
            else if(active == 1)
            {
                spriteBatch.DrawString(spFont, "Play", new Vector2(posX, posY), Color.White);
                spriteBatch.DrawString(spFont, "Login", new Vector2(posX, posY + 60), Color.Black);
                spriteBatch.DrawString(spFont, "Register", new Vector2(posX, posY + 120), Color.White);
            }
            else if (active == 2)
            {
                spriteBatch.DrawString(spFont, "Play", new Vector2(posX, posY), Color.White);
                spriteBatch.DrawString(spFont, "Login", new Vector2(posX, posY + 60), Color.White);
                spriteBatch.DrawString(spFont, "Register", new Vector2(posX, posY + 120), Color.Black);

            }

        }
    }
}
