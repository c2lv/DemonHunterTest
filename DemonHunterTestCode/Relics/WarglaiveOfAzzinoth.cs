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
            // 같은 팀에서 같은 캐릭터(일리단)를 가진 살아있는 플레이어 찾기
            var allyWithSameCharacter = combatState.Players
                .FirstOrDefault(p => p.Creature.Side == side && p.Creature.IsAlive && p.Character.Id == base.Owner.Character.Id);

            // 첫 번째 일리단만 가시 적용 (중복 방지)
            if (allyWithSameCharacter == base.Owner)
            {
                Flash();
                IEnumerable<Creature> targets = from c in combatState.GetOpponentsOf(base.Owner.Creature)
                                                where c.IsAlive
                                                select c;
                await PowerCmd.Apply<ThornsPower>(targets, base.DynamicVars["ThornsPower"].BaseValue, null, null);
            }
            
            // Regen은 항상 자신에게 적용
            await PowerCmd.Apply<RegenPower>(base.Owner.Creature, base.DynamicVars["RegenPower"].BaseValue, base.Owner.Creature, null);
        }
    }
}
