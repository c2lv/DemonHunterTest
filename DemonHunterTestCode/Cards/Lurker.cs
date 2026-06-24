using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class Lurker : DemonHunterTestCard
{
    private const int HandMax = 10;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ThornsPower>(999m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromPower<ThornsPower>(),
        HoverTipFactory.FromCard<BlurDemonHunter>(false)
    };

    public Lurker()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ICombatState? tempCombatState = base.CombatState;
        ArgumentNullException.ThrowIfNull(tempCombatState);
        CombatState combatState = (CombatState)tempCombatState;

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        foreach (Creature enemy in combatState.HittableEnemies)
        {
            await PowerCmd.Apply<ThornsPower>(choiceContext, enemy, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, this);
        }
        ArgumentNullException.ThrowIfNull(base.Owner.PlayerCombatState);
        int handCount = base.Owner.PlayerCombatState.AllCards.Count((CardModel c) => c.Pile != null && c.Pile.Type == PileType.Hand);
        int cardsToAdd = HandMax - handCount;
        for (int i = 0; i < cardsToAdd; i++)
        {
            BlurDemonHunter blurCard = combatState.CreateCard<BlurDemonHunter>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(blurCard, PileType.Hand, null);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
