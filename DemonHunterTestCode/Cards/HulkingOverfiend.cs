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
public sealed class HulkingOverfiend : DemonHunterTestCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Battlefiend>(false)
    };

    public HulkingOverfiend()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    private const int HandMax = 10;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        PileType[] piles = [PileType.Exhaust, PileType.Discard, PileType.Draw];
        List<CardModel> found = new List<CardModel>();
        foreach (PileType pileType in piles)
        {
            CardPile? pile = pileType.GetPile(base.Owner);
            if (pile != null)
            {
                found.AddRange(pile.Cards.Where((CardModel c) => c is Battlefiend));
            }
        }

        int handCount = base.Owner.PlayerCombatState?.AllCards.Count((CardModel c) => c.Pile != null && c.Pile.Type == PileType.Hand) ?? 0;
        foreach (CardModel card in found)
        {
            if (handCount >= HandMax)
            {
                break;
            }

            CardPileAddResult result = await CardPileCmd.Add(card, PileType.Hand);
            if (result.success)
            {
                handCount++;
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
