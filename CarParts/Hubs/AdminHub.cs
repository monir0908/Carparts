using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CarParts.Hubs
{
    [HubName("adminHub")]
    public class AdminHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}