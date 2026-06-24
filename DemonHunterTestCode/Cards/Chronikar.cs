using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class Chronikar : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Turns", 3m),
        new PowerVar<StrengthPower>(3m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<StrengthPower>()
    };

    public Chronikar()
        : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        decimal strengthAmount = base.DynamicVars.Strength.BaseValue;
        int turns = base.DynamicVars["Turns"].IntValue;

        await PowerCmd.Apply<ChronikarStrengthPower>(choiceContext, base.Owner.Creature, strengthAmount, base.Owner.Creature, this);

        if (turns > 1)
        {
            await PowerCmd.Apply<ChronikarPower>(choiceContext, base.Owner.Creature, turns - 1, base.Owner.Creature, this);
            base.Owner.Creature.GetPower<ChronikarPower>()?.SetGrantedStrength(strengthAmount);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Strength.UpgradeValueBy(2m);
    }
}
