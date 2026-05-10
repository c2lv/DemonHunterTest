using System.Collections.Generic;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using DemonHunterTest.DemonHunterTestCode.Potions;
using DemonHunterTest.DemonHunterTestCode.Relics;
using DemonHunterTestCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Models.Potions;
using DemonHunterTest.DemonHunterTestCode.Extensions;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace DemonHunterTest.DemonHunterTestCode.Character;

public sealed class DemonHunterTest : PlaceholderCharacterModel
{
    public const string CharacterId = "DemonHunter";

    public static readonly Color Color = new("142e0b");

    public override string CustomVisualPath => "demon_hunter_character.tscn".ScenesPath();
    public override string CustomCharacterSelectBg => "demon_hunter_select_bg.tscn".ScenesPath();
    public override string CustomIconPath => "demon_hunter_icon.tscn".ScenesPath();
    public override string CustomMerchantAnimPath => "demon_hunter_merchant.tscn".ScenesPath();
    public override string CustomRestSiteAnimPath => "demon_hunter_rest.tscn".ScenesPath();
    public override string CustomArmPointingTexturePath => "multiplayer_hand_1.png".ImagePath();
    public override string CustomArmRockTexturePath => "multiplayer_hand_4.png".ImagePath();
    public override string CustomArmPaperTexturePath => "multiplayer_hand_2.png".ImagePath();
    public override string CustomArmScissorsTexturePath => "multiplayer_hand_3.png".ImagePath();
    public override string CustomEnergyCounterPath => "demon_hunter_energy_counter.tscn".ScenesPath();

    // 88 x 88 png 캐릭터 아이콘
    public override string CustomIconTexturePath => "character_icon_demonhunter.png".ImagePath();
    // 848 x 1253 png 캐릭터 선택화면 이미지
    public override string CustomCharacterSelectIconPath => "CustomCharacterSelectIcon.png".ImagePath();
    // 사일런트 리소스 활용
    public override string PlaceholderID => "silent";

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeDemonHunter>(),
        ModelDb.Card<StrikeDemonHunter>(),
        ModelDb.Card<StrikeDemonHunter>(),
        ModelDb.Card<StrikeDemonHunter>(),
        ModelDb.Card<DefendDemonHunter>(),
        ModelDb.Card<DefendDemonHunter>(),
        ModelDb.Card<DefendDemonHunter>(),
        ModelDb.Card<BlurDemonHunter>(),
        ModelDb.Card<BlurDemonHunter>(),
        ModelDb.Card<CoordinatedStrike>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<WarglaiveOfAzzinoth>()
    ];

    public override IReadOnlyList<PotionModel> StartingPotions =>
    [
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<DemonHunterTestCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<DemonHunterTestRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<DemonHunterTestPotionPool>();
}