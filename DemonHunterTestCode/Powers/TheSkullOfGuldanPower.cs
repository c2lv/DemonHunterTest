using System;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Cards;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class TheSkullOfGuldanPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Keywords.Contains(DHKeyWords.Outcast))
        {
            modifiedCost = Math.Max(0m, originalCost - base.Amount);
            return true;
        }
        return false;
    }
}
