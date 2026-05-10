using System;
using System.Collections.Generic;
using System.Linq;
using BaseLib.Abstracts;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Cards;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class LeaderOfTheIllidari : DemonHunterTestRelic
{
    private const string _extraDamageKey = "ExtraDamage";

    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>()
    {
        new DynamicVar(_extraDamageKey, 3m)
    };

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (cardSource == null)
        {
            return 0m;
        }
        if (cardSource.Id.Entry == null || !cardSource.Id.Entry.Contains("Illidari", StringComparison.OrdinalIgnoreCase))
        {
            return 0m;
        }
        if (dealer != base.Owner.Creature && cardSource.Owner != base.Owner)
        {
            return 0m;
        }
        return base.DynamicVars["ExtraDamage"].BaseValue;
    }
}