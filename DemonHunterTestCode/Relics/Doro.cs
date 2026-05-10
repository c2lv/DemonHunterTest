using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using DemonHunterTest.DemonHunterTestCode.Audio;
using DemonHunterTest.DemonHunterTestCode.Character;
using System;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DemonHunterTest.DemonHunterTestCode.Relics
{
    [Pool(typeof(DemonHunterTestRelicPool))]
    public sealed class Doro : DemonHunterTestRelic
    {
        private bool _isShopMusicActive;

        // Use Common rarity to avoid core shop-special handling that pollutes map/history
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
            else
            {
                // 숍 음악 종료 시 단순 Stop이 아닌 기본 음악까지 복원
                if (_isShopMusicActive)
                {
                    CombatBgmController.RestoreDefaultMusic();
                }
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
            var rnd = new Random();
            int idx = rnd.Next(1, 5); // 1..4
            string path = $"shop_doro{idx}.mp3";
            CombatBgmController.Play(path);
        }
    }
}
