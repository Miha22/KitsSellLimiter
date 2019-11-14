using System;
using System.Collections.Generic;
using System.IO;
using fr34kyn01535.Uconomy;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace KitsLimiter
{
    public class Plugin : RocketPlugin<MyConfig>
    {
        internal static Plugin Instance;
        internal static JsonSchema Schema;
        internal Uconomy Uconomy;
        static readonly string kitPath = $@"Plugins\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Kits";

        static Plugin()
        {
            FileStream file = new FileInfo(kitPath + @"/Example.json").Open(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            Dictionary<ushort, ushort> items = new Dictionary<ushort, ushort>
            {
                { 363, 1 },
                { 6, 5 },
                { 253, 1 },
                { 81, 3 },
                { 15, 2 },
                { 1010, 1 },
                { 1011, 1 },
                { 1012, 1 },
                { 1013, 1 }
            };
            Kit kit = new Kit { Name = "KitStart", Category = null, Priority = 0, Cost = 25.0f, CoolDown = 300, Money = 100, Items = items };
            string json = JsonConvert.SerializeObject(kit, Formatting.Indented);
            using StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
            using TextReader reader = File.OpenText(@"c:\schema\Person.json");
            Schema = JsonSchema.Read(new JsonTextReader(reader));
        }

        protected override void Load()
        {
            Instance = this;
            Uconomy = new Uconomy();
            if (!System.IO.Directory.Exists(kitPath))
                System.IO.Directory.CreateDirectory(kitPath);
            DirectoryInfo directory = new DirectoryInfo(kitPath);
            if (directory.Attributes == FileAttributes.ReadOnly)
                directory.Attributes &= ~FileAttributes.ReadOnly;

        }

        internal bool GiveMarkedGun(Player player, ushort id)
        {
            //player.inventory.forceAddItem(new MItem(id), true);
            ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            Item item = new Item(id, EItemOrigin.ADMIN);
            if (itemAsset == null || item.state.Length < 16 || itemAsset.isPro)
                return false;


            //byte[] newState = new byte[item.state.Length + 1];
            //newState[newState.Length - 1] = 22;
            //for (byte i = 0; i < item.state.Length; i++)
            //    newState[i] = item.state[i];
            //item.state = newState;
            //Console.WriteLine("Marked state: ");
            //byte num = 0;
            //foreach (byte bite in item.state)
            //{
            //    Console.WriteLine($"{num++}. {bite}");
            //}
            //Console.WriteLine();
            item.state[12] = 22;
            player.inventory.forceAddItem(item, true);
            return true;
        }

        internal bool LoadKitToDB(string kit)
        {

        }

        //private void Events_OnPlayerConnected(UnturnedPlayer player)
        //{
        //    player.Player.inventory.onInventoryAdded += Events_InventoryAdded;
        //    //player.Player.inventory.onInventoryRemoved += Events_InventoryRemoved;
        //    //player.Player.inventory.onInventoryUpdated += Events_InventoryRemoved;
        //}

        //private void Events_InventoryAdded(byte page, byte index, ItemJar jar)
        //{
        //    byte num = 0;
        //    foreach (byte bite in jar.item.state)
        //    {
        //        Console.WriteLine($"{num++}. {bite}");
        //    }
        //}

        internal bool GetKit(UnturnedPlayer player, string kitname, string content)
        {
            string[] items = content.Trim().Split(' ');
            if (items == null || items.Length == 0)
                return false;
            decimal money = 0;
            foreach (string item in items)
            {
                ushort count = 1;
                ushort id = 0;

                if (item.Substring(0, 1) == "c")
                    money += Uconomy.Database.IncreaseBalance(player.CSteamID.ToString(), decimal.Parse(item.Substring(2)));
                else if (!ushort.TryParse(item, out id))
                {
                    string[] c = item.Split('/');
                    id = ushort.Parse(c[0]);
                    count = ushort.Parse(c[1]);
                }
                Item kit = new Item(id, EItemOrigin.ADMIN);
                kit.state[12] = 22;
                for (ushort i = 0; i < count; i++)
                    player.Player.inventory.forceAddItemAuto(kit, true, true, true);
            }
            UnturnedChat.Say(player, $"You have received kit: {kitname}" + (money == 0 ? "!" : $" with {money} money!"), true);

            return true;
        }

        internal bool GetContent(Kit kit)
        {

        }

        protected override void Unload()
        {
            //Logger.Log("ArchesGenerators Unloaded!");
        }
    }
}
