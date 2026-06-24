using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class PatchesThePilot : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(6)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Illidari>(base.IsUpgraded)
    };

    public PatchesThePilot()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ICombatState? tempCombatState = base.CombatState;
        if (tempCombatState == null)
        {
            return;
        }

        CombatState combatState = (CombatState)tempCombatState;
        List<Illidari> generated = Illidari.Create(base.Owner, base.DynamicVars.Cards.IntValue, combatState).ToList();
        IReadOnlyList<CardPileAddResult> results = await CardPileCmd.AddGeneratedCardsToCombat(generated, PileType.Draw, base.Owner, CardPilePosition.Random);
        if (base.IsUpgraded)
        {
            foreach (CardPileAddResult result in results)
            {
                CardCmd.Upgrade(result.cardAdded);
            }
        }

        CardCmd.PreviewCardPileAdd(results);
    }

    protected override void OnUpgrade()
    {
    }
}
