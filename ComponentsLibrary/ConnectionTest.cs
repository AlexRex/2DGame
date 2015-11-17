using Components;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Components
{
    public class ConnectionTest
    {

        // IN the hub make the variables static for cross-hub stateless

        public enum STATE
        {
            Connecting,
            WaitingPlayers,
            AllPlayersConnected,
            AllCharactersSelected,
            AllPlayersReady
        }

        public STATE GameState;


        static IHubProxy proxy;
        Enemy enemy;
        Player player;
        HubConnection connection;

        public int enemyChar;
        public int playerChar;

        Texture2D barrierTexture;
        Texture2D collectableTexture;
        Level level;

        public void Initialize(Enemy enemy, Player player, Texture2D barrierTexture, Texture2D collectableTexture, Level level)
        {
            this.enemy = enemy;
            this.player = player;
            this.barrierTexture = barrierTexture;
            this.collectableTexture = collectableTexture;
            this.level = level;
            enemyChar = 5;
            playerChar = 5;


            GameState = STATE.Connecting;

            connection = new HubConnection("http://localhost:9685");
            //connection = new HubConnection("http://2dgameserver.azurewebsites.net/");
            proxy = connection.CreateHubProxy("GameHub");
            ServicePointManager.DefaultConnectionLimit = 10;

            //connection.Received += Connection_Received;

            Action<float, float> EnemyPosReceived = received_enemy_position;
            proxy.On("sendPosition", EnemyPosReceived);

            Action EnemyActiveReceived = received_enemy_active;
            proxy.On("sendActive", EnemyActiveReceived);

            Action<int> enemyShoot = received_enemy_shoot;
            proxy.On("sendShoot", enemyShoot);

            Action<List<Vector2>, List<int>> getCollectables = received_collectables;
            proxy.On("sendCollectables", getCollectables);

            Action<List<Vector2>, List<int>> getBarriers = received_barriers;
            proxy.On("sendBarriers", getBarriers);

            Action<float, int, int, float, int> enemyVariables = received_enemy_variables;
            proxy.On("sendVariables", enemyVariables);

            Action<int> playerConnecteds = conected_players;
            proxy.On("playerConnecteds", playerConnecteds);

            Action<int> otherPlayerChar = received_enemy_char;
            proxy.On("otherPlayerChar", otherPlayerChar);

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







        #region Initialize

        private void received_collectables(List<Vector2> collectablesPosition, List<int> type)
        {
            //Console.WriteLine("got collectables");
            List<Collectable> collectables = new List<Collectable>();
            for(int i=0; i<collectablesPosition.Count; i++)
            {
                Animation collectableAnimation = new Animation();
                collectableAnimation.Initialize(collectableTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Collectable coll = new Collectable();

                coll.Initialize(new Vector2(collectablesPosition[i].X, collectablesPosition[i].Y), type[i]);

                coll.setAnimation(collectableAnimation);

                collectables.Add(coll);
            }

            level.setCollectables(collectables);
        }

        private void received_barriers(List<Vector2> barriersPosition, List<int> type)
        {
            //Console.WriteLine("got collectables");
            List<Barrier> barriers = new List<Barrier>();
            for (int i = 0; i < barriersPosition.Count; i++)
            {
                Animation barrierAnimation = new Animation();
                barrierAnimation.Initialize(barrierTexture, Vector2.Zero, 32, 32, 1, 30, Color.White, 1f, true);

                Barrier barr = new Barrier();

                barr.Initialize(barrierAnimation, new Vector2(barriersPosition[i].X, barriersPosition[i].Y), true, false);


                barriers.Add(barr);
            }

            level.setBarriers(barriers);
        }



        private void conected_players(int players)
        {
            Console.WriteLine(players);
            if(players == 1 && playerChar != 5){
                GameState = STATE.WaitingPlayers;
            }
            else if (players == 2 && enemyChar == 5 && playerChar != 5)
            {
                GameState = STATE.WaitingPlayers;
            }



        }

        public void selectCharacter(int active)
        {
            playerChar = active;
            if (connection.State == ConnectionState.Connected)
                proxy.Invoke("selectedChar", active);
        }

        private void received_enemy_char(int enemyChar)
        {
            this.enemyChar = enemyChar;
            GameState = STATE.AllCharactersSelected;
            
        }

        #endregion

        #region UpdateEnemy
        //Update Enemy
        private void received_enemy_position(float x, float y)
        {
           // Console.WriteLine("SERVER: enemyRec position Y: {0}", enemyRec.Position.Y);
            enemy.Position.X = x;
            enemy.Position.Y = y;
        }

        private void received_enemy_active()
        {
            enemy.Active = !enemy.Active;
        }

        private void received_enemy_shoot(int dir)
        {
            //Console.WriteLine("received shoot: {0}", dir);
            enemy.Shoot(dir);
        }

        private void received_enemy_variables(float speed, int strength, int ammunition, float health, int score)
        {
            enemy.character.Speed = speed;
            enemy.character.Strength = strength;
            enemy.character.Ammunition = ammunition;
            enemy.character.Health = health;
            enemy.score = score;
        }

        #endregion

        #region UpdatePlayer

        // Update player
        public void Update()
        {
            if (connection.State == ConnectionState.Connected)
                proxy.Invoke("UpdatePlayerPosition", player.Position.X, player.Position.Y);
        }

        public void Shoot(int direction)
        {
            //Console.WriteLine("sended shoot");

            if (connection.State == ConnectionState.Connected)
                proxy.Invoke("Shoot", direction);
        }

        public void UpdateActive()
        {
            Console.WriteLine("active");
            if (connection.State == ConnectionState.Connected)
                proxy.Invoke("UpdatePlayerActive");
        }

        internal void UpdateCharVariables(float speed, int strength, int ammunition, float health, int score)
        {
            if (connection.State == ConnectionState.Connected)
                proxy.Invoke("UpdateCharVariables", speed, strength, ammunition, health, score);
        }

        #endregion
    }
}
