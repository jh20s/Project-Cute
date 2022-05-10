using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbyPlayerAchievementData
{
    // ��ü���� ���� �޼���
    [SerializeField]
    private List<Tuple<string, Achievement.AState>> mProgress;
    public List<Tuple<string, Achievement.AState>> Progress
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
    private List<List<int>> mBossKillToWeapon;
    public List<List<int>> BossKillToWeapon
    {
        get => mBossKillToWeapon;
        set
        {
            mBossKillToWeapon = value;
        }
    }
    // ���� óġ(�ڽ�Ƭ)
    [SerializeField]
    private List<List<int>> mBossKillToCostume;
    public List<List<int>> BossKillToCostume
    {
        get => mBossKillToCostume;
        set
        {
            mBossKillToCostume = value;
        }
    }

    // ���̺� Ŭ����(����)
    [SerializeField]
    private List<List<int>> mWaveClearToWeapon;
    public List<List<int>> WaveClearToWeapon
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
