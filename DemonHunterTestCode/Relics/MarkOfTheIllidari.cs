using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using DemonHunterTest.DemonHunterTestCode.Character;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using DemonHunterTest.DemonHunterTestCode.Cards;
using MegaCrit.Sts2.Core.Helpers;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class MarkOfTheIllidari : DemonHunterTestRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override bool ShowCounter => true;

    public override int DisplayAmount
    {
        get
        {
            if (!IsActivating)
            {
                return DemonHunterTest_OutcastPlayed % base.DynamicVars.Cards.IntValue;
            }
            return base.DynamicVars.Cards.IntValue;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new CardsVar(3),
        new EnergyVar(1)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>
    {
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromKeyword(DHKeyWords.Outcast)
    };

    private int _outcastPlayed;
    private bool _isActivating;

    private bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            UpdateDisplay();
        }
    }

    [SavedProperty]
    public int DemonHunterTest_OutcastPlayed
    {
        get => _outcastPlayed;
        set
        {
            AssertMutable();
            _outcastPlayed = value;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (IsActivating)
        {
            base.Status = RelicStatus.Normal;
        }
        else
        {
            int intValue = base.DynamicVars.Cards.IntValue;
            base.Status = ((DemonHunterTest_OutcastPlayed % intValue == intValue - 1) ? RelicStatus.Active : RelicStatus.Normal);
        }
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner && cardPlay.Card.Keywords.Contains(DHKeyWords.Outcast))
        {
            DemonHunterTest_OutcastPlayed++;
            int intValue = base.DynamicVars.Cards.IntValue;
            if (CombatManager.Instance.IsInProgress && DemonHunterTest_OutcastPlayed % intValue == 0)
            {
                await TaskHelper.RunSafely(DoActivateVisuals());
                await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            }
        }
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
}
