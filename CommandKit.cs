using System.Collections.Generic;
using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace KitsLimiter
{
    public class CommandKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kit";

        public string Help => "Gives you items and money stored in kit";

        public string Syntax => "/kit [name]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "kitslimiter.kit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, $"Incorrect command syntax. Correct usage: {Syntax}", UnityEngine.Color.red);
                return;
            }
            string content = Plugin.Instance.Database.GetKitContent(command[0].Trim(), out string fullkitname, out decimal kitprice);
            if(content == null)
            {
                UnturnedChat.Say(caller, $"Kit: {command[0]} was not found!", UnityEngine.Color.red);
                return;
            }
            if(!R.Permissions.HasPermission(caller, $"kitslimiter.kit.{fullkitname.ToLower()}"))
            {
                UnturnedChat.Say(caller, $"You dont have permission to execute this kit!", UnityEngine.Color.red);
                return;
            }
            decimal playerB = Uconomy.Instance.Database.GetBalance(((UnturnedPlayer)caller).CSteamID.ToString());
            if (playerB < kitprice)
            {
                UnturnedChat.Say(caller, $"You need more: {kitprice - playerB}{Uconomy.Instance.Configuration.Instance.MoneySymbol} to get {fullkitname} KIT!", UnityEngine.Color.red);
                return;
            }
            if (!Plugin.Instance.GiveKit((UnturnedPlayer)caller, fullkitname, content, kitprice))
                UnturnedChat.Say(caller, $"Nothing to add from kit: {fullkitname}!");
        }
    }
}
