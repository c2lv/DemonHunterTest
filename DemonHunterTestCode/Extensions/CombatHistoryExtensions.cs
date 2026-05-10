using System;
using System.Linq;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DemonHunterTest.DemonHunterTestCode.Extensions;

public static class CombatHistoryExtensions
{
    public static int CountCardPlaysWithKeyword(this CombatHistory history, CardKeyword keyword)
    {
        if (history == null) throw new ArgumentNullException(nameof(history));
        return history.Entries.OfType<CardPlayFinishedEntry>().Count(e => e.CardPlay.Card.Keywords.Contains(keyword));
    }

    public static int GetCardPlaysWithKeyword(this CombatManager manager, CardKeyword keyword)
    {
        if (manager == null) throw new ArgumentNullException(nameof(manager));
        return manager.History.CountCardPlaysWithKeyword(keyword);
    }

    public static int GetExhaustKeywordCardsPlayed(this CombatManager manager)
    {
        return manager.GetCardPlaysWithKeyword(CardKeyword.Exhaust);
    }
}
