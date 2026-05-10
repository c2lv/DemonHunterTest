using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Audio;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DemonHunterTest.DemonHunterTestCode.Relics;

[Pool(typeof(DemonHunterTestRelicPool))]
public sealed class Spiretaker : DemonHunterTestRelic
{
    private static readonly string EnemyBgmPath = "combat_enemy_spiretaker.mp3";
    private static readonly string EliteBgmPath = "combat_elite_spiretaker.mp3";
    private static readonly string BossBgmPath = "combat_boss_spiretaker.mp3";

    public override RelicRarity Rarity => RelicRarity.Shop;

    public override async Task AfterObtained()
    {
        ApplyMusicForCurrentEncounter();
        if (Owner != null && Owner.Relics.OfType<Doro>().Any() && !Owner.Relics.OfType<CarrotHamburg>().Any())
        {
            await MegaCrit.Sts2.Core.Commands.RelicCmd.Obtain<CarrotHamburg>(Owner);
        }
    }

    public override Task BeforeCombatStart()
    {
        ApplyMusicForCurrentEncounter();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        CombatBgmController.RestoreDefaultMusic();
        return Task.CompletedTask;
    }

    private void ApplyMusicForCurrentEncounter()
    {
        IRunState? runState = base.Owner?.RunState;
        if (runState?.CurrentRoom == null)
        {
            return;
        }
        // Only apply music for combat rooms, not other room types
        if (!(runState.CurrentRoom is CombatRoom))
        {
            return;
        }
        RoomType roomType = runState.CurrentRoom.RoomType;
        Flash();
        CombatBgmController.PlayForRoom(roomType, EnemyBgmPath, EliteBgmPath, BossBgmPath);
    }
}