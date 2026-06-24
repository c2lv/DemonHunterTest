using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class MechathunPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != base.Owner)
        {
            return;
        }

        bool drawEmpty = !PileType.Draw.GetPile(player).Cards.Any();
        bool handEmpty = !PileType.Hand.GetPile(player).Cards.Any();
        bool discardEmpty = !PileType.Discard.GetPile(player).Cards.Any();
        if (!drawEmpty || !handEmpty || !discardEmpty)
        {
            return;
        }

        Flash();
        await CreatureCmd.Kill(base.CombatState.Enemies.Where((Creature c) => c.IsAlive).ToList(), force: true);
    }
}
