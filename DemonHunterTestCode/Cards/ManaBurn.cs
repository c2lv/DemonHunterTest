using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using DemonHunterTest.DemonHunterTestCode.Powers;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class ManaBurn : DemonHunterTestCard
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new EnergyVar(2),
		new PowerVar<LoseEnergyNextTurnPower>(1m)
	];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { base.EnergyHoverTip };

	public ManaBurn()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		await PowerCmd.Apply<LoseEnergyNextTurnPower>(base.Owner.Creature, base.DynamicVars["LoseEnergyNextTurnPower"].IntValue, base.Owner.Creature, this);
    }
	protected override void OnUpgrade()
	{
		base.DynamicVars.Energy.UpgradeValueBy(1m);
	}
}