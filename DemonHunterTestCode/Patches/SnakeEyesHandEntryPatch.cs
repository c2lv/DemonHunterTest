using HarmonyLib;
using DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Patches;

[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Add), new[] { typeof(CardModel), typeof(CardPile), typeof(CardPilePosition), typeof(AbstractModel), typeof(bool) })]
public static class SnakeEyesHandEntryPatch
{
    [HarmonyPostfix]
    public static void Postfix(CardModel card, CardPile newPile)
    {
        if (card is SnakeEyesDH snakeEyes && newPile.Type == PileType.Hand)
        {
            snakeEyes.RandomizeEnergy();
        }
    }
}
