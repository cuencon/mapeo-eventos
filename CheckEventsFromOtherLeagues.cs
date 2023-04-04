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

            // Start a shell session.
            // (The QuickShell method was added in Chilkat v9.5.0.65)
            int channelNum = ssh.QuickShell();
            if (channelNum < 0)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            Chilkat.StringBuilder sbCommands = new Chilkat.StringBuilder();
            sbCommands.Append($"echo {password} | sudo -S -p  ls;");
                        
            // Send the commands..
            success = ssh.ChannelSendString(channelNum, sbCommands.GetAsString(), "ansi");
            if (success != true)
            {
                Console.WriteLine(ssh.LastErrorText);
                return;
            }

            success = ssh.ChannelSendEof(channelNum);

            // Receive output up to our marker.
            success = ssh.ChannelReceiveUntilMatch(channelNum, "THIS IS THE END OF THE SCRIPT", "ansi", true);

            // Close the channel.
            // It is important to close the channel only after receiving the desired output.
            success = ssh.ChannelSendClose(channelNum);

            // Get any remaining output..
            success = ssh.ChannelReceiveToClose(channelNum);

            // Get the complete output for all the commands in the session.
            Console.WriteLine("--- output ----");
            Console.WriteLine(ssh.GetReceivedText(channelNum, "ansi"));
            
        }
    }
}
