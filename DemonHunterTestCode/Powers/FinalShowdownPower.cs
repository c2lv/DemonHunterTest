using System;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class FinalShowdownPower : DemonHunterTestPower
{
    private const int DrawThreshold = 4;

    private class Data
    {
        public int cardsDrawnThisTurn;
        public bool triggeredThisTurn;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature != base.Owner)
        {
            return Task.CompletedTask;
        }

        Data data = GetInternalData<Data>();
        if (data.triggeredThisTurn)
        {
            return Task.CompletedTask;
        }

        data.cardsDrawnThisTurn++;
        if (data.cardsDrawnThisTurn < DrawThreshold)
        {
            return Task.CompletedTask;
        }

        data.triggeredThisTurn = true;
        ArgumentNullException.ThrowIfNull(base.Owner.Player);
        CardPile pile = PileType.Hand.GetPile(base.Owner.Player);
        CardModel? target = base.Owner.Player.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (target == null)
        {
            return Task.CompletedTask;
        }

        Flash();
        target.EnergyCost.AddThisCombat(-1, reduceOnly: true);
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature == base.Owner)
        {
            Data data = GetInternalData<Data>();
            data.cardsDrawnThisTurn = 0;
            data.triggeredThisTurn = false;
        }

        return Task.CompletedTask;
    }
}
