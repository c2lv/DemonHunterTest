using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DemonHunterTestCode.Cards;

[Pool(typeof(DemonHunterTestCardPool))]
public sealed class DealWithADevil : DemonHunterTestCard
{
    private sealed class DevilNameVar : DynamicVar
    {
        public DevilNameVar()
            : base("Devil", 0m)
        {
        }

        public override string ToString()
        {
            CardModel? card = _owner as CardModel;
            int turnNumber = card?.Owner?.PlayerCombatState?.TurnNumber ?? 0;
            return (turnNumber % 3) switch
            {
                0 => ModelDb.Card<Pochita>().TitleLocString.GetFormattedText(),
                1 => ModelDb.Card<Makima>().TitleLocString.GetFormattedText(),
                _ => ModelDb.Card<PowerTheBloodFiend>().TitleLocString.GetFormattedText(),
            };
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DevilNameVar()
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.FromCard<Pochita>(false),
        HoverTipFactory.FromCard<Makima>(false),
        HoverTipFactory.FromCard<PowerTheBloodFiend>(false)
    };

    public DealWithADevil()
        : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        PochitaPower? pochita = base.Owner.Creature.GetPower<PochitaPower>();
        if (pochita != null)
        {
            await PowerCmd.Remove(pochita);
        }

        MakimaPower? makima = base.Owner.Creature.GetPower<MakimaPower>();
        if (makima != null)
        {
            await PowerCmd.Remove(makima);
        }

        BloodFiendPower? bloodFiend = base.Owner.Creature.GetPower<BloodFiendPower>();
        if (bloodFiend != null)
        {
            await PowerCmd.Remove(bloodFiend);
        }

        int turnNumber = base.Owner.PlayerCombatState?.TurnNumber ?? 0;
        switch (turnNumber % 3)
        {
            case 0:
                await PowerCmd.Apply<PochitaPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
                break;
            case 1:
                await PowerCmd.Apply<MakimaPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
                break;
            default:
                await PowerCmd.Apply<BloodFiendPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
                break;
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
