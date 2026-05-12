using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using DemonHunterTest.DemonHunterTestCode.Powers;
using DemonHunterTest.DemonHunterTestCode.Character;
using BaseLib.Utils;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class CrystalPrison : DemonHunterTestRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;

    public override async Task BeforeCombatStart()
    {
        ICombatState? combatState = base.Owner?.Creature.CombatState;
        if (combatState != null && base.Owner != null)
        {
            Flash();
            foreach (Creature enemy in combatState.GetOpponentsOf(base.Owner.Creature))
            {
                if (enemy.IsAlive)
                {
                    await PowerCmd.Apply<ImprisonmentPower>(new ThrowingPlayerChoiceContext(), new[] { enemy }, 2, base.Owner.Creature, null);
                }
            }
        }
    }
}
