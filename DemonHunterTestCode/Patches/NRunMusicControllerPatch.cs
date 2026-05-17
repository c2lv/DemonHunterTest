using System;
using System.Collections.Generic;
using DemonHunterTest.DemonHunterTestCode.Audio;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DemonHunterTest.DemonHunterTestCode.Patches;

[HarmonyPatch(typeof(NRunMusicController))]
public static class NRunMusicControllerPatch
{
    private static readonly AccessTools.FieldRef<NRunMusicController, string> _currentTrackRef =
        AccessTools.FieldRefAccess<NRunMusicController, string>("_currentTrack");

    private static readonly AccessTools.FieldRef<NRunMusicController, Node> _proxyRef =
        AccessTools.FieldRefAccess<NRunMusicController, Node>("_proxy");

    private static readonly AccessTools.FieldRef<NRunMusicController, IRunState> _runStateRef =
        AccessTools.FieldRefAccess<NRunMusicController, IRunState>("_runState");

    private static readonly Dictionary<NRunMusicController, AudioStreamPlayer?> _players = new();

    [HarmonyPatch("PlayCustomMusic")]
    [HarmonyPrefix]
    public static bool PlayCustomMusic_Prefix(NRunMusicController __instance, string customMusic)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customMusic))
            {
                return true;
            }

            string normalized = customMusic;

            if (ResourceLoader.Exists(normalized, "") || normalized.StartsWith(MainFile.ResPath))
            {
                __instance.Call("_stopMusic");

                if (_players.TryGetValue(__instance, out var existing) && existing != null)
                {
                    if (GodotObject.IsInstanceValid(existing))
                    {
                        existing.Stop();
                        existing.QueueFree();
                    }
                }

                string resolved = normalized;

                MainFile.Logger.Info($"[NRunMusicControllerPatch] Playing mod-local directly via AudioStreamPlayer: {resolved}");
                var stream = ResourceLoader.Load<AudioStream>(resolved);
                if (stream is AudioStreamMP3 mp3) mp3.Loop = true;
                else if (stream is AudioStreamOggVorbis ogg) ogg.Loop = true;
                else if (stream is AudioStreamWav wav) wav.LoopMode = AudioStreamWav.LoopModeEnum.Forward;

                var player = new Godot.AudioStreamPlayer
                {
                    Stream = stream,
                    VolumeDb = Mathf.LinearToDb(BgmController.Volume),
                    Bus = "Music"
                };
                __instance.AddChild(player);
                player.Play();
                _players[__instance] = player;

                return false;
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"[NRunMusicControllerPatch] PlayCustomMusic patch failed: {ex}");
        }

        return true;
    }

    [HarmonyPatch("StopCustomMusic")]
    [HarmonyPrefix]
    public static bool StopCustomMusic_Prefix(NRunMusicController __instance)
    {
        try
        {
            if (_players.TryGetValue(__instance, out var player) && player != null)
            {
                if (GodotObject.IsInstanceValid(player))
                {
                    player.Stop();
                    player.QueueFree();
                }
                _players.Remove(__instance);
            }

            try
            {
                string current = _currentTrackRef(__instance);
                if (!string.IsNullOrEmpty(current))
                {
                    var proxy = _proxyRef(__instance);
                    proxy?.Call("update_music", current);
                    proxy?.Call("update_global_parameter", "Progress", 7f);
                }
            }
            catch (Exception ex)
            {
                MainFile.Logger.Info($"[NRunMusicControllerPatch] StopCustomMusic restore failed: {ex}");
            }

            return false;
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"[NRunMusicControllerPatch] StopCustomMusic patch failed: {ex}");
        }

        return true;
    }

    [HarmonyPatch("UpdateMusic")]
    [HarmonyPrefix]
    public static bool UpdateMusic_Prefix()
    {
        if (!DemonHunterTest.DemonHunterTestCode.Audio.BgmController.IsOverrideActive)
        {
            return true;
        }

        if (RunManager.Instance != null && !RunManager.Instance.IsInProgress)
        {
            MainFile.Logger.Info("[NRunMusicControllerPatch] Run is no longer in progress (e.g. player died). Stopping custom BGM.");
            DemonHunterTest.DemonHunterTestCode.Audio.BgmController.Stop();
            return true;
        }

        MainFile.Logger.Info($"[NRunMusicControllerPatch] Skipping default BGM update due to relic override.");
        return false;
    }

    [HarmonyPatch(nameof(NRunMusicController.UpdateTrack), new Type[] { })]
    [HarmonyPrefix]
    public static bool UpdateTrack_Prefix(NRunMusicController __instance)
    {
        if (DemonHunterTest.DemonHunterTestCode.Audio.BgmController.IsOverrideActive)
        {
            return false;
        }

        IRunState? runState = _runStateRef(__instance);
        if (runState?.CurrentRoom?.RoomType != RoomType.RestSite)
        {
            return true;
        }

        try
        {
            __instance.UpdateAmbience();

            Node? proxy = _proxyRef(__instance);
            proxy?.Call("update_global_parameter", "Progress", 3f);
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"[NRunMusicControllerPatch] RestSite UpdateTrack safe-path failed: {ex}");
        }

        return false;
    }
}

[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Combat.CombatManager), "HandlePlayerDeath")]
public static class CombatManagerDeathPatch
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        MainFile.Logger.Info("[CombatManagerDeathPatch] Player death detected. Stopping custom BGM.");
        DemonHunterTest.DemonHunterTestCode.Audio.BgmController.Stop();
    }
}

[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NGameOverScreen), "_Ready")]
public static class GameOverScreenReadyPatch
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        MainFile.Logger.Info("[GameOverScreenReadyPatch] GameOverScreen _Ready detected. Stopping custom BGM.");
        DemonHunterTest.DemonHunterTestCode.Audio.BgmController.Stop();
    }
}
