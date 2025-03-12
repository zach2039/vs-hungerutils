using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace HungerUtils.ModCommands
{
    public static class HungerCommands
    {
        private static string[] hungerUtilValidAttributes =
        {
            "Saturation",
            "SaturationLossDelayFruit",
            "SaturationLossDelayVegetable",
            "SaturationLossDelayProtein",
            "SaturationLossDelayGrain",
            "SaturationLossDelayDairy",
            "MaxSaturation",
            "FruitLevel",
            "VegetableLevel",
            "ProteinLevel",
            "GrainLevel",
            "DairyLevel"
        };

        public static void Register(ICoreServerAPI api)
        {
            api.ChatCommands
                .Create("hungerutils")
                .WithDescription("Configuration utils for player hunger.")
                .RequiresPrivilege("controlserver")
                .WithArgs(
                    api.ChatCommands.Parsers.Word("playerName"),
                    api.ChatCommands.Parsers.WordRange("hungerUtilAttribute", hungerUtilValidAttributes),
                    api.ChatCommands.Parsers.OptionalFloat("setValue", float.NaN)
                    )
                .HandleWith((args) => OnHungerUtilsCommand(api, args));
        }

        private static IServerPlayer GetPlayerByName(ICoreServerAPI api, string playerName)
        {
            foreach (IServerPlayer player in api.World.AllOnlinePlayers)
            {
                if (player.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                {
                    return player;
                }
            }
            return null;
        }

        private static TextCommandResult OnHungerUtilsCommand(ICoreServerAPI api, TextCommandCallingArgs args)
        {
            string playerName = args[0] as string;
            string hungerUtilAttribute = args[1] as string;
            float setValue = (float)args[2];

            IServerPlayer targetPlayer;

            if (string.IsNullOrEmpty(playerName))
            {
                targetPlayer = args.Caller.Player as IServerPlayer;
            }
            else
            {
                targetPlayer = GetPlayerByName(api, playerName);
                if (targetPlayer == null)
                {
                    return TextCommandResult.Error($"Player '{playerName}' not found.");
                }
            }

            var behaviorHunger = targetPlayer.Entity.GetBehavior<EntityBehaviorHunger>();
            if (behaviorHunger == null) return TextCommandResult.Error("Hunger behavior not found.");


            if (float.IsNaN(setValue))
            {
                // Get value instead of set
                PropertyInfo propertyInfo = typeof(EntityBehaviorHunger).GetProperty(hungerUtilAttribute);

                if (propertyInfo != null)
                {
                    float retValue = (float)propertyInfo.GetValue(behaviorHunger);
                    return TextCommandResult.Success($"Value for '{hungerUtilAttribute}' is {retValue} for player '{targetPlayer.PlayerName}'.");
                }
                else
                {
                    return TextCommandResult.Error($"Could not locate EntityBehaviorHunger property '{hungerUtilAttribute}' for Player '{playerName}'.");
                }
            }
            else
            {
                // Set value
                PropertyInfo propertyInfo = typeof(EntityBehaviorHunger).GetProperty(hungerUtilAttribute);

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(behaviorHunger, setValue);
                    return TextCommandResult.Success($"'{hungerUtilAttribute}' set to {setValue} for player '{targetPlayer.PlayerName}'.");
                }
                else
                {
                    return TextCommandResult.Error($"Could not locate EntityBehaviorHunger property '{hungerUtilAttribute}' for Player '{playerName}'.");
                }
            }
        }
    }
}
