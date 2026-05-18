using BaseLib.Abstracts;
using DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Extensions;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public class ChaosStrikePower : TemporaryStrengthPower, ICustomPower
{
	public override AbstractModel OriginModel => ModelDb.Card<ChaosStrike>();

	public string? CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
	public string? CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
	public string? CustomBigBetaIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
