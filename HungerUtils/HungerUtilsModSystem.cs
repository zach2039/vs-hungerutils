using HungerUtils.ModCommands;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace HungerUtils
{
    public class HungerUtilsModSystem : ModSystem
    {
        public override void StartServerSide(ICoreServerAPI api)
        {
            HungerCommands.Register(api);
            Mod.Logger.Notification("[HungerUtils] Loaded server-side");
        }
    }
}
