using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class MetamorphosisDH : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
            new IntVar("Turn", 2m),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { 
        HoverTipFactory.FromPower<ThornsPower>() 
    };

    public MetamorphosisDH()
        : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ICombatState? combatState = base.CombatState;
        if (combatState == null) return;

        // Apply DoubleDamagePower to self for 2 turns (doubles attack card damage)
        await PowerCmd.Apply<DoubleDamagePower>(choiceContext, base.Owner.Creature, base.DynamicVars["Turn"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<DoubleDamageThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Turn"].BaseValue, base.Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
