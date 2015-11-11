using Components;
using Microsoft.AspNet.SignalR;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace AzureGameServer
{
    public class GameHub : Hub
    {
        static List<Vector2> collectablesPosition;
        static List<int> collectablesType;

        public override Task OnConnected()
        {
            //Need: Random collectable positions, random collectable types.

            collectablesPosition = new List<Vector2>();
            collectablesType = new List<int>();


            Random r = new Random();
            float posX;
            float posY;
            int type;

            for (int i = 0; i < 10; i++)
            {
                posX = r.Next(40, 1280 - 40);
                posY = r.Next(40, 720 - 40);

                type = r.Next(0, 6);
                collectablesPosition.Add(new Vector2(posX, posY));
                collectablesType.Add(type);
            }
            try
            {
                Clients.All.sendCollectables(collectablesPosition, collectablesType);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return base.OnConnected();
        }


        #region UpdateMethods

        public void UpdatePlayerPosition(float x, float y)
        {
            Clients.Others.sendPosition(x, y);
        }


        public void UpdatePlayerActive()
        {
            Clients.Others.sendActive();
        }

        public void Shoot(int direction)
        {
            Clients.Others.sendShoot(direction);
        }

        public void UpdateCharVariables(float speed, int strength, int ammunition, float health, int score)
        {
            Clients.Others.sendVariables(speed, strength, ammunition, health, score);
        }

        #endregion
    }
}