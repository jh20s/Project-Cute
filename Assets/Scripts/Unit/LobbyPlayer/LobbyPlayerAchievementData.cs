using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LobbyPlayerAchievementData
{
    // ��ü���� ���� �޼���
    [SerializeField]
    private StringState mProgress;
    public StringState Progress
    {
        get => mProgress;
        set
        {
            mProgress = value;
        }
    }
    // ��� Ŭ����
    [SerializeField]
    private List<int> mModeClear;
    public List<int> ModeClear
    {
        get => mModeClear;
        set
        {
            mModeClear = value;
        }
    }

    // óġ(����)
    [SerializeField]
    private List<int> mKillToWeapon;
    public List<int> KillToWeapon
    {
        get => mKillToWeapon;
        set
        {
            mKillToWeapon = value;
        }
    }

    // óġ(��ų)
    [SerializeField]
    private StringInt mKillToSkill;
    public StringInt KillToSkill
    {
        get => mKillToSkill;
        set
        {
            mKillToSkill = value;
        }
    }

    // ���� �ð�(����)
    [SerializeField]
    private List<int> mTimeToWeapon;
    public List<int> TimeToWeapon
    {
        get => mTimeToWeapon;
        set
        {
            mTimeToWeapon = value;
        }
    }
    // ���� �ð�(�ڽ�Ƭ)
    [SerializeField]
    private List<int> mTimeToCostume;
    public List<int> TimeToCostume
    {
        get => mTimeToCostume;
        set
        {
            mTimeToCostume = value;
        }
    }

    // ���� óġ(����)
    [SerializeField]
    private List<IntInt> mBossKillToWeapon;
    public List<IntInt> BossKillToWeapon
    {
        get => mBossKillToWeapon;
        set
        {
            mBossKillToWeapon = value;
        }
    }
    // ���� óġ(�ڽ�Ƭ)
    [SerializeField]
    private List<IntInt> mBossKillToCostume;
    public List<IntInt> BossKillToCostume
    {
        get => mBossKillToCostume;
        set
        {
            mBossKillToCostume = value;
        }
    }

    // ���̺� Ŭ����(����)
    [SerializeField]
    private List<IntInt> mWaveClearToWeapon;
    public List<IntInt> WaveClearToWeapon
    {
        get => mWaveClearToWeapon;
        set
        {
            mWaveClearToWeapon = value;
        }
    }

    // ���� ��� Ŭ����
    [SerializeField]
    private List<int> mBossModeWaveClear;
    public List<int> BossModeWaveClear
    {
        get => mBossModeWaveClear;
        set
        {
            mBossModeWaveClear = value;
        }
    }
}
