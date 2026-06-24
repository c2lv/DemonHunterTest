using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DemonHunterTest.DemonHunterTestCode.Powers;

public sealed class ChronikarPower : DemonHunterTestPower
{
    private class Data
    {
        public decimal strengthGranted;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public void SetGrantedStrength(decimal amount)
    {
        GetInternalData<Data>().strengthGranted = amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != base.Owner || base.Amount <= 0)
        {
            return;
        }

        Data data = GetInternalData<Data>();
        await PowerCmd.Apply<ChronikarStrengthPower>(choiceContext, base.Owner, data.strengthGranted, base.Owner, null);
        await PowerCmd.TickDownDuration(this);
    }
}
