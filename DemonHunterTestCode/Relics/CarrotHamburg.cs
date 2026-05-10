using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class CarrotHamburg : DemonHunterTestRelic
{
    private static readonly HashSet<string> EdibleRelicIds = new()
    {
        "MEAL_TICKET", "STRAWBERRY", "VENERABLE_TEA_SET", "PEAR", "LASTING_CANDY",
        "LUCKY_FISH", "MEAT_ON_THE_BONE", "LIZARD_TAIL", "MANGO", "ICE_CREAM",
        "LUNAR_PASTRY", "LEES_WAFFLE", "BREAD", "DRAGON_FRUIT", "FAKE_LEES_WAFFLE",
        "FAKE_MANGO", "TEA_OF_DISCOURTESY", "BONE_TEA", "CHOSEN_CHEESE",
        "FAKE_VENERABLE_TEA_SET", "BIG_MUSHROOM", "FRAGRANT_MUSHROOM", "DEMONHUNTERTEST-SOUR_GRAPES",
    };

    private bool _isSubscribed;

    public override RelicRarity Rarity => RelicRarity.Event;

    public override Task AfterObtained()
    {
        TrySubscribe();
        return Task.CompletedTask;
    }

    public override Task AfterRemoved()
    {
        if (_isSubscribed && Owner != null)
        {
            Owner.RelicObtained -= OnRelicObtained;
            _isSubscribed = false;
        }
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        TrySubscribe();
        return Task.CompletedTask;
    }

    private void TrySubscribe()
    {
        if (!_isSubscribed && Owner != null)
        {
            Owner.RelicObtained += OnRelicObtained;
            _isSubscribed = true;
        }
    }

    private void OnRelicObtained(RelicModel relic)
    {
        if (EdibleRelicIds.Contains(relic.Id.Entry))
        {
            Flash();
            TaskHelper.RunSafely(HealToMax());
        }
    }

    private async Task HealToMax()
    {
        if (Owner?.Creature != null)
        {
            int healAmount = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
            if (healAmount > 0)
            {
                await CreatureCmd.Heal(Owner.Creature, healAmount);
            }
        }
    }
}
