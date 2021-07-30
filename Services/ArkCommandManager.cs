using CCbot.Models;
using CCbot.Services;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCbot.Services
{
    public class ArkCommandManager
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly Config ConfigCommands;
        private readonly FileService FileManager;
        private readonly RconManager RconManager;
         
        public ArkCommandManager(DiscordClient bot, RconManager rconManager, Config config, FileService fileManager)
        {
            Bot = bot;
            RconManager = rconManager;
            Config = config;
            FileManager = fileManager;
            Config = fileManager.GetConfig();
        }
       
        public async Task<string> RconSendCommand(string command, string servername)
        {
            int serverid = -1;
            for (int i = 0; i < Config.Servers.Count; i++)
            {
                if (Config.Servers[i].ServerName == servername)
                {
                    serverid = i;
                }
            }
            var result = await RconManager.RconCommand(command, serverid);
            return result;
        }
    }
}
