using System.Threading.Tasks;
using DemonHunterTest.DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class OutcastTriggerPower : DemonHunterTestPower
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner == base.Owner.Player && cardPlay.Card.Keywords.Contains(DHKeyWords.Outcast) && cardPlay.IsLastInSeries)
		{
			await CreatureCmd.Damage(
				context,
				base.CombatState.HittableEnemies,
				base.Amount,
				ValueProp.Unpowered,
				base.Owner,
				null
			);
		}
	}
}
