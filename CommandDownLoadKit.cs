using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

namespace KitsLimiter
{
    public class CommandDownLoadKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "downloadkit";

        public string Help => "DownLoads kit(-s) from database";

        public string Syntax => "/downloadkit [kitname]\n/downloadkit all";

        public List<string> Aliases => new List<string> { "dloadkit" };

        public List<string> Permissions => new List<string> { "kitslimiter.downloadkit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                Logger.LogError($"Incorrect command syntax. Correct usage: {Syntax}");
                return;
            }
            if (!Plugin.Instance.Database.SelectKitNames(out string results))
            {
                UnturnedChat.Say(caller, $"No kits in database", UnityEngine.Color.red);
                return;
            }
            System.Console.WriteLine($"res?: {results == null} {results == ""} {results.Length}");
            if (command[0].ToLower().Trim() == "all")
            {
                foreach (string kit in results.TrimEnd().Split(' '))
                {
                    Plugin.Instance.Database.GetKitContent(kit.Split('.')[1], out string fullkitname, out decimal kitprice, out string content);
                    System.Console.WriteLine(kit.Split('.')[1]);
                    System.Console.WriteLine(fullkitname == null);
                    System.Console.WriteLine(content == null);
                    string[] items = content.Trim().Split(' ');
                    System.Console.WriteLine(items == null);
                    Dictionary<ushort, ushort> ids = new Dictionary<ushort, ushort>();
                    int money = 0;
                    foreach (string item in items)
                    {
                        ushort count = 1;
                        ushort id = 0;
                        System.Console.WriteLine(0);
                        if (item.Substring(0, 1) == "c")
                        {
                            money += int.Parse(item.Substring(2));
                            continue;
                        }
                            
                        System.Console.WriteLine(1);
                        if (!ushort.TryParse(item, out id))
                        {
                            string[] c = item.Split('/');
                            id = ushort.Parse(c[0]);
                            //System.Console.WriteLine(3);
                            count = ushort.Parse(c[1]);
                            //System.Console.WriteLine(4);
                        }
                        ids.Add(id, count);
                    }
                    FileStream file = new FileInfo(Plugin.kitPath + $@"/{fullkitname}.json").Open(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    System.Console.WriteLine(5);
                    Kit jsonKit = new Kit { Name = fullkitname, Category = null, Priority = 0, Cost = kitprice, CoolDown = 0, Money = money, Items = ids };
                    string json = JsonConvert.SerializeObject(jsonKit, Formatting.Indented);
                    System.Console.WriteLine(6);
                    using StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(json);
                }
                Logger.Log("Loaded MySQL kits in jsons!");
                return;
            }
            foreach (string kit in results.TrimEnd().ToLower().Split(' '))
            {
                if(kit == command[0].ToLower().Trim())
                {
                    Plugin.Instance.Database.GetKitContent(kit.Split('.')[1], out string fullkitname, out decimal kitprice, out string content);
                    string[] items = content.Trim().Split(' ');
                    Dictionary<ushort, ushort> ids = new Dictionary<ushort, ushort>();
                    int money = 0;
                    foreach (string item in items)
                    {
                        ushort count = 1;
                        ushort id = 0;
                        //System.Console.WriteLine();
                        if (item.Substring(0, 1) == "c")
                            money += int.Parse(item.Substring(2));
                        if (!ushort.TryParse(item, out id))
                        {
                            string[] c = item.Split('/');
                            id = ushort.Parse(c[0]);
                            count = ushort.Parse(c[1]);
                        }
                        ids.Add(id, count);
                    }
                    FileStream file = new FileInfo(Plugin.kitPath + $@"/{fullkitname}.json").Open(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Kit jsonKit = new Kit { Name = fullkitname, Category = null, Priority = 0, Cost = kitprice, CoolDown = 0, Money = money, Items = ids };
                    string json = JsonConvert.SerializeObject(jsonKit, Formatting.Indented);
                    using StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(json);
                    Logger.Log($"Loaded MySQL kit: {fullkitname} in jsons!");
                    return;
                }
            }
            Logger.LogError($"Kit {command[0].Trim()} was not found!, available kits: {results.TrimEnd()}");
        }
    }
}
