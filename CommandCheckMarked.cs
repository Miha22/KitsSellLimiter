using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace KitsLimiter
{
    public class CommandCheckMarked : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "checkm";

        public string Help => "Checks your inventory for marked items";

        public string Syntax => "/checkm";

        public List<string> Aliases => new List<string> { "chm" };

        public List<string> Permissions => new List<string> { "kitslimiter.checkmarked" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 0)
            {
                UnturnedChat.Say(caller, $"Incorrect command syntax. Correct usage: {Syntax}", UnityEngine.Color.red);
                return;
            }

            List<Item> items = new List<Item>();
            foreach (Items page in ((UnturnedPlayer)caller).Player.inventory.items)
            {
                if (page == null || !page.items.Any())
                    continue;
                foreach (ItemJar itemJar in page.items)
                {
                    if (itemJar.item.state.Length == 18 && itemJar.item.state[12] == 22)
                        items.Add(itemJar.item);
                }

            }

            //IEnumerable<Item> items =
            //                    from page in ((UnturnedPlayer)caller).Player.inventory.items
            //                    from itemJar in page.items
            //                    where page != null || page.items.Count != 0
            //                    where itemJar.item is MItem
            //                    select itemJar.item;
            //IEnumerable<Item> items = ((UnturnedPlayer)caller).Player.inventory.items.SelectMany(u => u.items,
            //                (it, ij) => new { Items = it, ItemJar = ij })
            //              .Where(it => (it.Items != null || it.Items.items.Count != 0) && it.ItemJar.item is MItem)
            //              .Select(it => it.ItemJar.item);
            //
            if (items == null || !items.Any())
            {
                UnturnedChat.Say(caller, $"You don't have any marked items", true);
                return;
            }
            foreach (Item item in items)
            {
                UnturnedChat.Say(caller, $"Marked: {((ItemAsset)Assets.find(EAssetType.ITEM, item.id)).itemName}  {item.quality}%:Quality {item.state[10]}:Bullets", true);
            }
            //for (byte i = 0; i < player.Player.inventory.items.Length; i++)//7, becuase there are currently 7 types of wear where player possibly can store items: EItemType.HAT/PANTS/SHIRT/MASK/BACKPACK/VEST/GLASSES
            //{
            //    if (player.Player.inventory.items[i] == null)
            //        continue;
            //    byte itemsCount = player.Inventory.getItemCount(i);
            //    for (byte index = 0; index < itemsCount; index++)
            //    {
            //        player.Inventory.sendUpdateQuality(i, player.Inventory.getItem(i, index).x, player.Inventory.getItem(i, index).y, 100);
            //    }
            //}
        }
    }
}
