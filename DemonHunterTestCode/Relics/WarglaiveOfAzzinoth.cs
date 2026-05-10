using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using DemonHunterTest.DemonHunterTestCode.Character;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class WarglaiveOfAzzinoth : DemonHunterTestRelic
{

    public override RelicRarity Rarity => RelicRarity.Starter;

    // Thorns를 적들에게 부여하고, 자신에게 Regen을 부여하는 복합 유물
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<ThornsPower>(1m),
        new PowerVar<RegenPower>(1m)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<ThornsPower>(),
        HoverTipFactory.FromPower<RegenPower>()
    };

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Creature.Side)
        {
            Flash();
            IEnumerable<Creature> targets = from c in combatState.GetOpponentsOf(base.Owner.Creature)
                                            where c.IsAlive
                                            select c;
            await PowerCmd.Apply<ThornsPower>(targets, base.DynamicVars["ThornsPower"].BaseValue, null, null);
            await PowerCmd.Apply<RegenPower>(base.Owner.Creature, base.DynamicVars["RegenPower"].BaseValue, base.Owner.Creature, null);
        }
    }
}
