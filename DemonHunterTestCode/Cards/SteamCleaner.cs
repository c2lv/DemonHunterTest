using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SteamCleaner : DemonHunterTestCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    };

    public SteamCleaner()
        : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        PileType[] piles = [PileType.Draw, PileType.Discard];
        foreach (PileType pileType in piles)
        {
            CardPile? pile = pileType.GetPile(base.Owner);
            if (pile == null)
            {
                continue;
            }

            HashSet<string> seen = new HashSet<string>();
            List<CardModel> toExhaust = new List<CardModel>();
            foreach (CardModel card in pile.Cards.ToList())
            {
                if (!seen.Add(card.Id.Entry))
                {
                    toExhaust.Add(card);
                }
            }

            foreach (CardModel card in toExhaust)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
