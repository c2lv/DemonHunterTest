using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class RhythmDancerRisaPower : DemonHunterTestPower
{
    private class Data
    {
        public int triggeredThisTurn;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    private bool CanTrigger(CardModel card)
    {
        return card.Owner.Creature == base.Owner && card.Type == CardType.Attack && GetInternalData<Data>().triggeredThisTurn < base.Amount;
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (!CanTrigger(card))
        {
            return playCount;
        }

        return playCount + 1;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (CanTrigger(card))
        {
            GetInternalData<Data>().triggeredThisTurn++;
        }

        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature == base.Owner)
        {
            GetInternalData<Data>().triggeredThisTurn = 0;
        }

        return Task.CompletedTask;
    }
}
