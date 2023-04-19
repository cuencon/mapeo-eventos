using Buzz.TxLeague.Women.Config.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzz.TxLeague.Women.Config
{
    public static class CheckEventsFromOtherLeagues
    {
        public static void Check(string date)
        {            
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
            string strOutput = ssh.QuickCommand($"echo \"Qev5?AA.\" | sudo -S -p  \"\" journalctl -p 4 -u dgs-consumer-v2@1.service -u dgs-consumer-v2@2.service -u dgs-consumer-v2@3.service -u dgs-consumer-v2@4.service -u dgs-consumer-v2@5.service -u dgs-consumer-v2@6.service --since \"{date} 00:00:00\" | grep 'Config with LeagueI'\n", "ansi");
            if (ssh.LastMethodSuccess != true)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            WriteLog.WriteToFile($"===========================> {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")} <=========================== ");
            WriteLog.WriteToFile($"================================> BEGIN <============================================== ");
            WriteLog.WriteToFile(strOutput);
            WriteLog.WriteToFile($"================================> END <============================================== ");

            Console.WriteLine("---- Successfully saved to Log.txt ----");
            Console.WriteLine("---- FINISH ----");      
        }
    }
}
