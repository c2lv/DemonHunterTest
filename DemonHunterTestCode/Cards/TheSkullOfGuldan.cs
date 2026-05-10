using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using DemonHunterTest.DemonHunterTestCode.Cards;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Extensions;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class TheSkullOfGuldan : DemonHunterTestCard
{
    public override string PortraitPath => "the_skull_of_guldan.png".ImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new IntVar("Cost", 1m)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromKeyword(DHKeyWords.Outcast)
    };

    public TheSkullOfGuldan()
        : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TheSkullOfGuldanPower>(base.Owner.Creature, base.DynamicVars["Cost"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
