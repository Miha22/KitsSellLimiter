using System.Collections.Generic;
using System.IO;
using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;

namespace KitsLimiter
{
    public class CommandLoadKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public string Name => "loadkit";

        public string Help => "Loads kit(-s) in database";

        public string Syntax => "/loadkit [kitname]\n/loadkit all";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "kitslimiter.loadkit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1 || new DirectoryInfo(Plugin.kitPath).GetFiles().Length == 0)
            {
                Logger.LogError($"No JSON kits in {Plugin.kitPath} or Incorrect command syntax. Correct usage: {Syntax}");
                return;
            }
            FileInfo[] files = new DirectoryInfo(Plugin.kitPath).GetFiles();
            if (command[0].ToLower() == "all")
            {
                foreach (FileInfo file in files)
                {
                    using StreamReader sr = file.OpenText();
                    string notFound = Plugin.Instance.TryLoadKit(sr.ReadToEnd(), file.Name);
                    if (notFound != "")
                        Logger.LogWarning($"[NOT FOUND IDS]: {notFound} in {file.Name} KIT");
                }
            }
            foreach (FileInfo file in files)
            {
                if(file.Name.ToLower() == command[0].Trim().ToLower())
                {
                    using StreamReader sr = file.OpenText();
                    string notFound = Plugin.Instance.TryLoadKit(sr.ReadToEnd(), file.Name);
                    if (notFound != "")
                        Logger.LogWarning($"[NOT FOUND IDS]: {notFound} in {file.Name} KIT");
                    return;
                }
            }
            string names = "";
            foreach (var file in files)
                names += $"{file.Name} ";

            Logger.LogError($"Kit {command[0].Trim()} was not found!, available kits: {names.TrimEnd()}");
            //Console.WriteLine("Added marked item");
        }
    }
}
