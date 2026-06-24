using System.Collections.Generic;
using System.Threading.Tasks;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DemonHunterTestCode.Cards;

public sealed class Pochita : DemonHunterTestCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public Pochita()
        : base(-1, CardType.Power, CardRarity.None, TargetType.None, showInCardLibrary: false)
    {
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}
