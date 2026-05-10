using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class AceHunterKreen : DemonHunterTestCard
{
	public AceHunterKreen()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.None)
	{
	}

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { 
        HoverTipFactory.FromPower<ThornsPower>()
    };

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		CombatState? combatState = base.CombatState;
		ArgumentNullException.ThrowIfNull(combatState);
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		foreach (Creature enemy in combatState.HittableEnemies)
		{
			await PowerCmd.SetAmount<ThornsPower>(enemy, 0m, base.Owner.Creature, this);
		}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
