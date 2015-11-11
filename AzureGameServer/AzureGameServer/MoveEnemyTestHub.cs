using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace AzureGameServer
{
    public class MoveEnemyTestHub : Hub
    {

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

        #endregion
    }
}