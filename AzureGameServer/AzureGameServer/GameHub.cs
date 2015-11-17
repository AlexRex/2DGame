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
        static List<Vector2> barriersPosition;
        static List<int> barriersType;


        static int players = 0;
        static int player1Char = 5;
        static int player2Char = 5;

        static string player1;
        static string player2;

        static Random r = new Random();
        

        public override Task OnConnected()
        {
            players++;

            if(players == 1)
            {
                player1 = Context.ConnectionId;
            }

            if (players == 2)
            {
                player2 = Context.ConnectionId;
            }



            //Clients.All.playerConnecteds(players);
            
            //Need: Random collectable positions, random collectable types.

            collectablesPosition = new List<Vector2>();
            collectablesType = new List<int>();


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


            barriersPosition = new List<Vector2>();
            barriersType = new List<int>();


            float posXB;
            float posYB;
            int typeB;

            for (int i = 0; i < 10; i++)
            {
                posXB = r.Next(40, 1280 - 40);
                posYB = r.Next(40, 720 - 40);

                typeB = r.Next(0, 6);
                barriersPosition.Add(new Vector2(posXB, posYB));
                barriersType.Add(typeB);
            }






            try
            {
                Clients.All.sendCollectables(collectablesPosition, collectablesType);
                Clients.All.sendBarriers(barriersPosition, barriersType);
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

        public void selectedChar(int charSelection)
        {


            if(players == 1 && player1Char == 5 && player2Char == 5)
            {
                player1Char = charSelection;
                Clients.All.playerConnecteds(players);
            }
            else if(players == 2 && player1Char != 5 && player2Char == 5)
            {
                player2Char = charSelection;
                Clients.Caller.otherPlayerChar(player1Char);
                Clients.Others.otherPlayerChar(player2Char);
            }
            else if(players == 2 && player1Char == 5 && player2Char == 5 && Context.ConnectionId.Equals(player2))
            {
                player2Char = charSelection;
                Clients.All.playerConnecteds(players);
            }
            else if (players == 2 && player1Char == 5 && player2Char == 5 && Context.ConnectionId.Equals(player1))
            {
                player1Char = charSelection;
                Clients.All.playerConnecteds(players);
            }
            else if(players == 2 && player1Char == 5 && player2Char != 5)
            {
                player1Char = charSelection;
                Clients.Caller.otherPlayerChar(player2Char);
                Clients.Others.otherPlayerChar(player1Char);
            }
        }

        #endregion
    }
}