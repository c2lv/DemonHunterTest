using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using DemonHunterTestCode.Cards;

[HarmonyPatch(typeof(ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
public static class ArchaicToothDhPatch
{
	[HarmonyPostfix]
	private static void AddDhTranscendenceUpgrades(ref Dictionary<ModelId, CardModel> __result)
	{
		__result[((AbstractModel)ModelDb.Card<CoordinatedStrike>()).Id] = (CardModel)(object)ModelDb.Card<ExpendablePerformers>();
	}
}
