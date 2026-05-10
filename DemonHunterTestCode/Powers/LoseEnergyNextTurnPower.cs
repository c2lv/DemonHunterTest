using System.Threading.Tasks;
using BaseLib.Abstracts;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class LoseEnergyNextTurnPower : DemonHunterTestPower
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override async Task AfterEnergyReset(Player player)
	{
		if (player != base.Owner.Player)
		{
			return;
		}

		await PlayerCmd.LoseEnergy(base.Amount, player);
		await PowerCmd.Remove(this);
	}
}
