using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace ArchesGenerators
{
    public class TurnOnOff : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "turngen4";

        public string Help => "turns on/off generators";

        public string Syntax => "/tgen4 [on/off]";

        public List<string> Aliases => new List<string> { "tgen4" };

        public List<string> Permissions => new List<string> { "arches.turngen4" };


        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 1 && command[0].ToLower() == "on" || command[0].ToLower() == "off")
            {
                List<InteractableGenerator> generators = PowerTool.checkGenerators(new Vector3(100, 10, 100), 10000f, 1000);
                //System.Console.WriteLine(generators);
                System.Console.WriteLine($"Generators found: {(generators == null ? "0 (null)" : generators.Count.ToString())}");
                if(command[0] == "on")
                {
                    foreach (InteractableGenerator generator in generators)
                    {
                        if (generator == null || generator.isPowered)
                            continue;
                        //generator.use();
                        generator.updatePowered(true);
                        System.Console.WriteLine("turning on generator");
                        EffectManager.sendEffect(Plugin.Instance.Configuration.Instance.SoundEffect, Plugin.Instance.Configuration.Instance.SoundEffectRadius, generator.transform.position);
                    }
                }
                else
                {
                    foreach (InteractableGenerator generator in generators)
                    {
                        if (generator == null || !generator.isPowered)
                            continue;
                        //generator.use();
                        generator.updatePowered(false);
                        System.Console.WriteLine("turning off generator");
                        EffectManager.sendEffect(Plugin.Instance.Configuration.Instance.SoundEffect, Plugin.Instance.Configuration.Instance.SoundEffectRadius, generator.transform.position);
                    }
                }
                    

                UnturnedChat.Say(caller, $"All Generators {command[0].ToUpper()}");
                return;
            }
            UnturnedChat.Say(caller, $"Invalid command parameters, usage: {Syntax}");
        }
    }
}
