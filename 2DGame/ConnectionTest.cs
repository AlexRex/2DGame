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

            //connection.Received += Connection_Received;

            Action<Enemy> EnemyReceived = received_enemy;
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

        private void received_enemy(Enemy enemyRec)
        {
            Console.WriteLine("SERVER: enemyRec position Y: {0}", enemyRec.Position.Y);
            //enemy.Position.Y = enemyRec.Position.Y;
        }



        private void Connection_Received(string obj)
        {
            Console.WriteLine("Message Received {0}", obj);
        }

        public void Update()
        {
            if(connection.State == ConnectionState.Connected)
            {
                proxy.Invoke("UpdatePlayer", player);
            }
        }
    }
}
