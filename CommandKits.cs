using System.Collections.Generic;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Chat;

namespace KitsLimiter
{
    public class CommandKits : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kits";

        public string Help => "Lists available kits";

        public string Syntax => "/kits";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "kitslimiter.kits" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 0)
            {
                UnturnedChat.Say(caller, $"Incorrect command syntax. Correct usage: {Syntax}", UnityEngine.Color.red);
                return;
            }
            List<Permission> permissions = R.Permissions.GetPermissions(caller);
            if (permissions == null || permissions.Count == 0)
            {
                UnturnedChat.Say(caller, $"You are not in any of group!", UnityEngine.Color.red);
                return;
            }
            string kits = "";
            foreach (Permission p in permissions)
            {
                string[] parts = p.Name.Split('.');
                if (parts.Length == 3 && parts[0].ToLower() == "kitslimiter" && parts[1].ToLower() == "kit")
                    kits += $"kit.{parts[2]} ";
            }
            if (kits == "")
                UnturnedChat.Say(caller, $"You don't have permission to use any kit!", true);
            else
                UnturnedChat.Say(caller, $"Available kits: {kits.TrimEnd()}", true);
        }
    }
}
