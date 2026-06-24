using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class BloodFiendPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != base.Owner)
        {
            return;
        }

        Flash();
        ICombatState? tempCombatState = base.CombatState;
        if (tempCombatState != null)
        {
            CombatState combatState = (CombatState)tempCombatState;
            Rupture rupture = combatState.CreateCard<Rupture>(player);
            await CardCmd.AutoPlay(choiceContext, rupture, player.Creature, AutoPlayType.Default, skipXCapture: true);
        }

        await CreatureCmd.Damage(choiceContext, base.Owner, 4m, ValueProp.Unblockable | ValueProp.Unpowered, base.Owner, null);
    }
}
