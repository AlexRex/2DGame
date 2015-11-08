using _2DGame.Components;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace _2DGame
{
    class ConnectionTest
    {

        static IHubProxy proxy;
        Enemy enemy;
        Player player;
        HubConnection connection;

        public async void Initialize(Enemy enemy, Player player)
        {
            this.enemy = enemy;
            this.player = player;
            //connection = new HubConnection("http://localhost:9685");
            connection = new HubConnection("http://2dgameserver.azurewebsites.net/");
            proxy = connection.CreateHubProxy("MoveEnemyTestHub");
            ServicePointManager.DefaultConnectionLimit = 10;

            //connection.Received += Connection_Received;

            Action<float, float> EnemyReceived = received_enemy;
            proxy.On("sendEnemy", EnemyReceived);



            Console.WriteLine("SERVER: Waiting for connection");
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
                Console.WriteLine("--SERVER: CONNECTED--");
            }
        }

        private void received_enemy(float enemyRec, float y)
        {
           // Console.WriteLine("SERVER: enemyRec position Y: {0}", enemyRec.Position.Y);
            enemy.Position.X = enemyRec;
            enemy.Position.Y = y;
        }



        private void Connection_Received(string obj)
        {
            Console.WriteLine("Message Received {0}", obj);
        }

        public void Update()
        {
                proxy.Invoke("UpdatePlayer", player.Position.X, player.Position.Y);
        }
    }
}
