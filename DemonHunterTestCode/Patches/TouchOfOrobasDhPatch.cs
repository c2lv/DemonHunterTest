using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using DemonHunterTest.DemonHunterTestCode.Relics;

[HarmonyPatch(typeof(TouchOfOrobas), "RefinementUpgrades", MethodType.Getter)]
public static class TouchOfOrobasDhPatch
{
	[HarmonyPostfix]
	private static void AddDhRefinementUpgrades(ref Dictionary<ModelId, RelicModel> __result)
	{
		__result[ModelDb.Relic<WarglaiveOfAzzinoth>().Id] = ModelDb.Relic<WarglaivesOfAzzinoth>();
	}
}
