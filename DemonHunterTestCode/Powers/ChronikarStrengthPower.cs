using BaseLib.Abstracts;
using DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class ChronikarStrengthPower : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<Chronikar>();
}
