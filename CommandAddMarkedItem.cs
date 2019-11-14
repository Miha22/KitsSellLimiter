using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace KitsLimiter
{
    public class CommandAddMarkedItem : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "givemarkeditem";

        public string Help => "Gives item that may have some limitations e.g non-tradable";

        public string Syntax => "/givem [player] [id]\n/givem [player] [id] [amount]";

        public List<string> Aliases => new List<string> { "givem" };

        public List<string> Permissions => new List<string> { "kitslimiter.givemarked" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 3)
            {
                UnturnedChat.Say(caller, $"Incorrect command syntax. Correct usage: {Syntax}", UnityEngine.Color.red);
                return;
            }
            if (!PlayerTool.tryGetSteamPlayer(command[0], out SteamPlayer steamPlayer))
            {
                UnturnedChat.Say(caller, $"Player was not found", UnityEngine.Color.red);
                return;
            }
            if (!ushort.TryParse(command[1], out ushort id) || !ushort.TryParse(command[1], out ushort amount))
            {
                UnturnedChat.Say(caller, "id or amount is not a number", UnityEngine.Color.red);
                return;
            }
            ItemAsset item = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            if (item == null || item.isPro)
            {
                UnturnedChat.Say(caller, "Item was not found or is Pro", UnityEngine.Color.red);
                return;
            }
            Plugin.Instance.GiveMarkedGun(UnturnedPlayer.FromSteamPlayer(steamPlayer).Player, id);
            //Console.WriteLine("Added marked item");
        }
    }
}
