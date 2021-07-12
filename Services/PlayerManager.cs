using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Services
{
    public class PlayerManager
    {
        public HashSet<string> Players { get;}
        public PlayerManager()
        {
            Players = new HashSet<string>();
        }
    }
}
