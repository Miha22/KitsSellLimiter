using System.Collections.Generic;
using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;

namespace KitsLimiter
{
    public class CommandDeleteKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "deletekit";

        public string Help => "Deletes kit from database";

        public string Syntax => "/deletekit [kitname]";

        public List<string> Aliases => new List<string> { "delkit" };

        public List<string> Permissions => new List<string> { "kitslimiter.deletekit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                Logger.LogError($"Incorrect command syntax. Correct usage: {Syntax}");
                return;
            }
            if (Plugin.Instance.Database.DeleteKit(command[0].ToLower()))
            {
                Logger.Log($"Successfully deleted kit: {command[0]}");
            }
            else
                Logger.LogError($"Cannot find kit: {command[0]}!");
        }
    }
}
