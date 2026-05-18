using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

/// <summary>
/// 내 턴 시작 시 무작위 아이언클래드(Ironclad) 카드 1장을 손으로 가져옵니다.
/// </summary>
public sealed class ProsecutorMeltranixPower : DemonHunterTestPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(player, ModelDb.CardPool<IroncladCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), 1, player.RunState.Rng.CombatCardGeneration);
        foreach (CardModel item in distinctForCombat)
        {
            await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, player);
        }
    }
}
