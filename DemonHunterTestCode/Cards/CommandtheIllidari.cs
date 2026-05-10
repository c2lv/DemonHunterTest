using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class CommandTheIllidari : DemonHunterTestCard
{
    public CommandTheIllidari()
        : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null)
        {
            return;
        }

        var owner = base.Owner;
        var matched = new List<CardModel>();
        PileType[] piles = new[] { PileType.Exhaust, PileType.Discard, PileType.Draw };
        foreach (var p in piles)
        {
            var pile = p.GetPile(owner);
            if (pile == null)
            {
                continue;
            }

            foreach (var card in pile.Cards.Where(c => MatchesIllidari(c)).ToList())
            {
                matched.Add(card);
            }
        }

        foreach (var card in matched)
        {
            var addResult = await CardPileCmd.Add(card, PileType.Hand);
            if (addResult.success && addResult.cardAdded.Pile?.Type == PileType.Hand)
            {
                addResult.cardAdded.SetToFreeThisTurn();
            }
        }
    }

    private static bool MatchesIllidari(CardModel card)
    {
        if (card == null)
        {
            return false;
        }
        try
        {
            if (card.Id.Entry.Contains("Illidari", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        catch
        {
            Log.Error("Error checking card name for Command the Illidari: " + card.Id.Entry);
        }

        return false;
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
