using _2DGame.Components;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DGame
{
    class ConnectionTest
    {

        static IHubProxy proxy;
        Enemy enemy;
        Player player;
        HubConnection connection;

        public void Initialize(Enemy enemy, Player player)
        {
            this.enemy = enemy;
            this.player = player;
            connection = new HubConnection("http://localhost:9685/");
            proxy = connection.CreateHubProxy("MoveEnemyTestHub");

           // connection.Received += Connection_Received;

            Action<Player> PlayerReceived = received_player;
            proxy.On("sendPlayer", PlayerReceived);

            Console.WriteLine("Waiting for connection");
            try
            {
                connection.Start().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (connection.State == ConnectionState.Connected)
            {
                Console.WriteLine("invoking");
                proxy.Invoke("UpdatePlayer", player);
                //proxy.Invoke("GetPoint");
            }
        }

        private void received_player(Player obj)
        {
            Console.WriteLine(obj.Position.Y);
            enemy.Position.Y = obj.Position.Y;
        }


        private void Connection_Received(string obj)
        {
            Console.WriteLine("Message Received {0}", obj);
        }

        public void Update()
        {
            if(connection.State == ConnectionState.Connected)
                proxy.Invoke("UpdatePlayer", player);
        }
    }
}
