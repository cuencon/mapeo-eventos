using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzz.TxLeague.Women.Config.Utils
{
    public static class SyncEventsToDGS
    {
        public static void RecreateGamesBulkTask(string arrayOfEventIds)
        {            
            var eventsIdsArray = arrayOfEventIds.Replace(",", " ");
            var hostname = "10.0.0.181";
            var username = "sysusr";
            var password = "Qev5?AA.";
            int port = 22;

            Chilkat.Ssh ssh = new Chilkat.Ssh();

            bool success = ssh.Connect(hostname, port);
            if (success != true)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            // Authenticate using login/password:
            success = ssh.AuthenticatePw(username, password);
            if (success != true)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            // Send some commands and get the output.
            string strOutput = ssh.QuickCommand($"cd Test/LineshouseSnapshot; for i in {eventsIdsArray}; do dotnet Ls-dgs-sync.dll $i; done", "ansi");
            if (ssh.LastMethodSuccess != true)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            Console.WriteLine("---- Events Pull ----");
            Console.WriteLine(strOutput);       
        }
    }
}