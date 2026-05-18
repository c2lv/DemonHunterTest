using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

/// <summary>
/// 매 턴 시작 시 카드 1장을 버립니다.
/// </summary>
public sealed class NethrandamusPower : DemonHunterTestPower
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.None;

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		Flash();
		CardModel? cardModel = (await CardSelectCmd.FromHandForDiscard(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this)).FirstOrDefault();
		if (cardModel != null)
		{
			await CardCmd.Discard(choiceContext, cardModel);
		}
	}
}
