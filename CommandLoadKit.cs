using System.Collections.Generic;
using System.IO;
using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;

namespace KitsLimiter
{
    public class CommandLoadKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "loadkit";

        public string Help => "Loads kit(-s) in database";

        public string Syntax => "/loadkit [kitname]\n/loadkit all";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "kitslimiter.loadkit" };

        internal static CommandLoadKit Instance;

        public CommandLoadKit()
        {
            Instance = this;
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (command.Length != 1 || new DirectoryInfo(Plugin.kitPath).GetFiles().Length == 0)
            {
                Logger.LogError($"No JSON kits in {Plugin.kitPath} or Incorrect command syntax. Correct usage: {Syntax}");
                return;
            }
            FileInfo[] files = new DirectoryInfo(Plugin.kitPath).GetFiles();
            //foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
            //{
            //    if (file.Name == "Config.json")
            //        continue;
            //    using (StreamReader sr = file.OpenText())
            //    {
            //        //System.Console.WriteLine($"IN USING: {sr.ReadToEnd()}");
            //        string notFound = TryLoadKit(sr.ReadToEnd(), file.Name);
            //        if (!string.IsNullOrEmpty(notFound))
            //            Console.WriteLine($"[NOT FOUND IDS]: {notFound} in {file.Name} KIT\nKit was loaded in DataBase");
            //        sr.Close();
            //    }
            //}
            if (command[0].ToLower() == "all")
            {
                foreach (FileInfo file in files)
                {
                    using (StreamReader sr = file.OpenText())
                    {
                        //System.Console.WriteLine($"IN USING: {sr.ReadToEnd()}");
                        string notFound = Plugin.Instance.TryLoadKit(sr.ReadToEnd(), file.Name);
                        if (!string.IsNullOrEmpty(notFound))
                            Logger.LogWarning($"[NOT FOUND IDS]: {notFound} in {file.Name} KIT\nKit was loaded in DataBase");
                        sr.Close();
                    }
                }
                return;
            }
            foreach (FileInfo file in files)
            {
                //System.Console.WriteLine(file.Name.Split('.')[0].ToLower());
                //System.Console.WriteLine(command[0].Trim().ToLower());
                if(file.Name.Split('.')[0].ToLower() == command[0].Trim().ToLower())
                {
                    using (StreamReader sr = file.OpenText())
                    {
                        //System.Console.WriteLine($"IN USING: {sr.ReadToEnd()}");
                        string notFound = Plugin.Instance.TryLoadKit(sr.ReadToEnd(), file.Name);
                        //System.Console.WriteLine(sr.ReadToEnd());
                        if (!string.IsNullOrEmpty(notFound))
                            Logger.LogWarning($"[NOT FOUND IDS]: {notFound.TrimEnd()} in {file.Name} KIT\nKit was loaded in DataBase");
                        return;
                    }
                }
            }
            string names = "";
            foreach (var file in files)
                names += $"{file.Name.Split('.')[0]} ";

            Logger.LogError($"Kit {command[0].Trim()} was not found!, available kits: {names.TrimEnd()}");
            //Console.WriteLine("Added marked item");
        }
    }
}
