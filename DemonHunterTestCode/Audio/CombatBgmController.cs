using System.IO;
using BaseLib.Audio;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Rooms;
using DemonHunterTest.DemonHunterTestCode;

namespace DemonHunterTest.DemonHunterTestCode.Audio;

public static class CombatBgmController
{
    private static AudioStreamPlayer? _currentPlayer;

    private static string? _currentPath;

    private static string ResolveAudioPath(string path)
    {
        return ResourceLoader.Exists(path, "") ? path : Path.Join(MainFile.ResPath, "audio", path);
    }

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

    public static void Play(string musicPath, float volumeMult = 0.5f)
    {
        if (string.IsNullOrWhiteSpace(musicPath))
        {
            return;
        }
        if (_currentPath == musicPath && _currentPlayer != null)
        {
            return;
        }
        Stop();
        // StopMusic()은 UnloadActBanks()를 호출해 오디오 뱅크를 영구 언로드하므로 절대 사용 금지.
        // 게임 Proxy에 stop_music 신호만 보내 사운드만 정지한다.
        NRunMusicController.Instance?.StopCustomMusic();
        string resolved = ResolveAudioPath(musicPath);
        var sound = new ModSound(resolved, ModAudio.SoundType.Music);
        // ModAudio expects a dB add and then multiplies the dB by the mult.
        // Convert the desired linear multiplier into dB and pass it as volumeAdd, leaving mult=1.
        float volumeAddDb = Mathf.LinearToDb(volumeMult);
        _currentPlayer = ModAudio.PlaySound(sound, volumeAddDb, 1f, 0f, 1f);
        _currentPath = musicPath;
    }

    public static void Stop()
    {
        if (_currentPlayer != null)
        {
            // 이미 해제된 Godot 노드에 접근하면 무음 크래시가 발생하므로 유효성 검사
            if (GodotObject.IsInstanceValid(_currentPlayer))
            {
                _currentPlayer.Stop();
                Node? parent = _currentPlayer.GetParent();
                if (parent != null && GodotObject.IsInstanceValid(parent))
                {
                    parent.RemoveChild(_currentPlayer);
                }
            }
            _currentPlayer = null;
        }
        _currentPath = null;
    }

    public static void RestoreDefaultMusic()
    {
        Stop();
        // UpdateMusic()은 새 뱅크 로드 + 트랙 처음부터 재시작 (액트 전환용).
        // 방 이동 복원 시에는 UpdateTrack()으로 현재 방에 맞는 트랙 상태만 복원한다.
        NRunMusicController.Instance?.UpdateTrack();
    }
}