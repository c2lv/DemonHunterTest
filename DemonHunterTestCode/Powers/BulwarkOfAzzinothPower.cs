using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

/// <summary>
/// 내 턴 종료 시 이번 턴에 받은 피해량만큼 방어도를 얻습니다.
/// </summary>
public sealed class BulwarkOfAzzinothPower : DemonHunterTestPower
{
    private int _damageTakenThisTurn = 0;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.None;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            _damageTakenThisTurn = 0;
        }
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Owner && result.UnblockedDamage > 0)
        {
            _damageTakenThisTurn += result.UnblockedDamage;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(base.Owner) && _damageTakenThisTurn > 0)
        {
            Flash();
            await CreatureCmd.GainBlock(base.Owner, _damageTakenThisTurn, ValueProp.Move, null);
        }
    }
}
