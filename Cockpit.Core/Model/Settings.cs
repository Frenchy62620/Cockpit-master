using System.Collections.Generic;
using System.Linq;

namespace Cockpit.Core.Model
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; }

        public List<string> RecentScripts { get; set; }

        public Settings()
        {

            RecentScripts = new List<string>();
        }


        public void AddRecentScript(string path)
        {
            if (path != null)
            {
                const int n = 10;
                RecentScripts.Remove(path);
                RecentScripts.Insert(0,path);
                if(RecentScripts.Count > n) 
                    RecentScripts.RemoveRange(n, RecentScripts.Count-n);
            }
        }
    }
}
