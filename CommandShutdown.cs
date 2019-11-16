using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using UnityEngine;

namespace KitsLimiter
{
    public class CommandShutdown : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "ss";

        public string Help => "Shutdowns server";

        public string Syntax => "/ss";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "kitslimiter.ss" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Provider.onServerShutdown.Invoke();
            Provider.shutdown(10, "extreme shutdown");
            Application.Quit();
        }
    }
}
