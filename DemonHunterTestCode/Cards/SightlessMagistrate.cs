using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class SightlessMagistrate : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("CostThreshold", 5m)
    ];

    public SightlessMagistrate()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int threshold = base.DynamicVars["CostThreshold"].IntValue;
        int totalCost = 0;
        while (totalCost < threshold)
        {
            CardModel? drawn = await CardPileCmd.Draw(choiceContext, base.Owner);
            if (drawn == null)
            {
                break;
            }

            totalCost += drawn.EnergyCost.GetAmountToSpend();
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["CostThreshold"].UpgradeValueBy(2m);
    }
}
