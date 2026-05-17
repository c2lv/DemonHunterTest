using System.IO;
using MegaCrit.Sts2.Core.Nodes.Audio;
using Godot;
using MegaCrit.Sts2.Core.Rooms;

namespace DemonHunterTest.DemonHunterTestCode.Audio;

public static class BgmController
{
    private static string? _currentPath;
    private static bool _isOverrideActive;

    public static bool IsOverrideActive => _isOverrideActive;

    public static readonly float Volume = 0.2f;  // (0.0f = silent, 1.0f = full volume)

    public static void PlayForRoom(RoomType roomType, string enemyBgmPath, string eliteBgmPath, string bossBgmPath)
    {
        string? musicPath = roomType switch
        {
            RoomType.Monster => enemyBgmPath,
            RoomType.Elite => eliteBgmPath,
            RoomType.Boss => bossBgmPath,
            _ => null,
        };
        if (musicPath != null)
        {
            Play(musicPath);
        }
    }

    public static void Play(string musicPath)
    {
        if (string.IsNullOrWhiteSpace(musicPath))
        {
            return;
        }
        if (_currentPath == musicPath && _isOverrideActive)
        {
            return;
        }
        Stop();
        _isOverrideActive = true;
        string resolvedPath = Path.Join(MainFile.ResPath, "audio", musicPath);
        if (!ResourceLoader.Exists(resolvedPath)) MainFile.Logger.Info("Could not find music path: " + resolvedPath);
        NRunMusicController.Instance?.PlayCustomMusic(resolvedPath);
        _currentPath = resolvedPath;
    }

    public static void Stop()
    {
        if (_isOverrideActive)
        {
            NRunMusicController.Instance?.StopCustomMusic();
        }
        _currentPath = null;
        _isOverrideActive = false;
    }
}