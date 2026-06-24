using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class RagingFelscreamerPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != base.Owner || !MatchesIllidari(card) || !IsInPlayableLocation(card))
        {
            return false;
        }

        modifiedCost = 0m;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == base.Owner && MatchesIllidari(cardPlay.Card) && IsInPlayableLocation(cardPlay.Card))
        {
            await PowerCmd.Decrement(this);
        }
    }

    private static bool IsInPlayableLocation(CardModel card)
    {
        return card.Pile?.Type is PileType.Hand or PileType.Play;
    }

    private static bool MatchesIllidari(CardModel card)
    {
        return card.Id.Entry.Contains("Illidari", StringComparison.OrdinalIgnoreCase);
    }
}
