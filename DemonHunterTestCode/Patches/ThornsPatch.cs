using HarmonyLib;
using DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using DemonHunterTest.DemonHunterTestCode.Powers;
using System.Threading.Tasks;

namespace DemonHunterTest.DemonHunterTestCode.Patches;

[HarmonyPatch(typeof(ThornsPower), nameof(ThornsPower.BeforeDamageReceived))]
    public static class BlurThornsPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ThornsPower __instance, Creature target, Creature? dealer, ref Task __result)
        {
            if (dealer != null && 
            target == __instance.Owner && 
            dealer.HasPower<BlurDemonHunterPower>())
            {
                __result = Task.CompletedTask;
                return false; // Skip the original BeforeDamageReceived
            }
            return true;
        }
    }