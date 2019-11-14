using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace ArchesGenerators
{
    public class RefillCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "refillgen";

        public string Help => "refills generators";

        public string Syntax => "/rgen \n/rgen [radius]";

        public List<string> Aliases => new List<string> { "rgen" };

        public List<string> Permissions => new List<string> { "arches.refillgen" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 1 && uint.TryParse(command[1], out uint radius))
            {
                List<InteractableGenerator> generators = PowerTool.checkGenerators(caller is ConsolePlayer ? new Vector3(0, 0, 0) : ((UnturnedPlayer)caller).Position, caller is ConsolePlayer ? float.MaxValue : radius, ushort.MaxValue);
                foreach (InteractableGenerator generator in generators)
                {
                    if (generator == null || !generator.isRefillable)
                        continue;
                    generator.askFill((ushort)(generator.capacity - generator.fuel));
                    //EffectManager.sendEffect(Plugin.Instance.Configuration.Instance.SoundEffect, Plugin.Instance.Configuration.Instance.SoundEffectRadius, generator.transform.position);
                }
                UnturnedChat.Say(caller, "Generators refilled");
                return;
            }
            if (command.Length == 0)
            {
                List<InteractableGenerator> generators = PowerTool.checkGenerators(caller is ConsolePlayer ? new Vector3(0, 0, 0) : ((UnturnedPlayer)caller).Position, float.MaxValue, ushort.MaxValue);
                foreach (InteractableGenerator generator in generators)
                {
                    if (generator == null || !generator.isRefillable)
                        continue;
                    generator.askFill((ushort)(generator.capacity - generator.fuel));
                    //EffectManager.sendEffect(Plugin.Instance.Configuration.Instance.SoundEffect, Plugin.Instance.Configuration.Instance.SoundEffectRadius, generator.transform.position);
                }
                UnturnedChat.Say(caller, "Generators refilled");
                return;
            }
            UnturnedChat.Say(caller, $"Invalid command parameters, usage: {Syntax}");
        }
    }
}
