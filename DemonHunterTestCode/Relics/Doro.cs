using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Audio;
using DemonHunterTest.DemonHunterTestCode.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DemonHunterTest.DemonHunterTestCode.Relics
{
    [Pool(typeof(DemonHunterTestRelicPool))]
    public sealed class Doro : DemonHunterTestRelic
    {
        private bool _isShopMusicActive;

        public override RelicRarity Rarity => RelicRarity.Shop;

        public override async Task AfterObtained()
        {
            ApplyForCurrentRoom();
            if (Owner != null && Owner.Relics.OfType<Spiretaker>().Any() && !Owner.Relics.OfType<CarrotHamburg>().Any())
            {
                await MegaCrit.Sts2.Core.Commands.RelicCmd.Obtain<CarrotHamburg>(Owner);
            }
        }

        public override Task AfterRoomEntered(AbstractRoom room)
        {
            if (room?.RoomType == RoomType.Shop)
            {
                PlayShopMusic();
            }
            else if (_isShopMusicActive)
            {
                BgmController.Stop();
                _isShopMusicActive = false;
            }
            return Task.CompletedTask;
        }

        private void ApplyForCurrentRoom()
        {
            IRunState? state = base.Owner?.RunState;
            if (state?.CurrentRoom?.RoomType == RoomType.Shop)
            {
                PlayShopMusic();
            }
        }

        private void PlayShopMusic()
        {
            if (_isShopMusicActive)
            {
                return;
            }
            _isShopMusicActive = true;
            int idx = Owner.RunState.Rng.CombatEnergyCosts.NextInt(4) + 1;  // 1..4
            string path = $"shop_doro{idx}.mp3";
            BgmController.Play(path);
        }
    }
}
