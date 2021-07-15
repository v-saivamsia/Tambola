using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Constants
{
    public class AvailableWinningWays
    {
        public List<string> list  = new List<string>() { "Four Corners", "Early Five","Top Row","Middle Row","Bottom Row","Housie"};
        public Dictionary<string,int> dictionary = new Dictionary<string, int>();
        public AvailableWinningWays()
        {
            int i = 0;
            foreach(string s in list)
                dictionary.Add(s, i++);
        }
    }
}
