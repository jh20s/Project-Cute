using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class LobbyPlayerInfo
{
    #region variable
    #region TIME
    [SerializeField]
    private int mDate;
    public int Date
    {
        get => mDate;
        set
        {
            mDate = value;
        }
    }
    #endregion
    #region BASE_STAT
    // ±âº» ½ºÅÝ
    [SerializeField]
    private int mBaseHp;
    public int BaseHp
    {
        get { return mBaseHp; }
        set
        { 
            mBaseHp = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }

    [SerializeField]
    private int mBaseATK;
    public int BaseATK
    {
        get { return mBaseATK; }
        set 
        { 
            mBaseATK = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }

    [SerializeField]
    private float mBaseCriticalChance;
    public float BaseCriticalChance
    {
        get { return mBaseCriticalChance; }
        set 
        { 
            mBaseCriticalChance = value;
        }
    }

    [SerializeField]
    private float mBaseCriticalDamage;
    public float BaseCriticalDamage
    {
        get { return mBaseCriticalDamage; }
        set { mBaseCriticalDamage = value; }
    }

    [SerializeField]
    private float mBaseSPD;
    public float BaseSPD
    {
        get { return mBaseSPD; }
        set 
        { 
            mBaseSPD = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }

    [SerializeField]
    private float mBaseATKSPD;
    public float BaseATKSPD
    {
        get { return mBaseATKSPD; }
        set
        { 
            mBaseATKSPD = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #region TRAINNING_STAT
    [SerializeField]
    private List<int> mTrainingLevelList;
    public List<int> TrainingLevelList
    {
        get { return mTrainingLevelList; }
        set 
        { 
            mTrainingLevelList = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private int mTrainingHp;
    public int TrainingHp
    {
        get { return mTrainingHp; }
        set 
        { 
            mTrainingHp = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private int mTrainingATK;
    public int TrainingATK
    {
        get { return mTrainingATK; }
        set 
        { 
            mTrainingATK = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private float mTrainingAddDamage;
    public float TrainingAddDamage
    {
        get { return mTrainingAddDamage; }
        set 
        { 
            mTrainingAddDamage = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private int mTrainingMagnetPower;
    public int TrainingMagnetPower
    {
        get { return mTrainingMagnetPower; }
        set
        {
            mTrainingMagnetPower = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private float mTrainingGoldPower;
    public float TrainingGoldPower
    {
        get { return mTrainingGoldPower; }
        set
        {
            mTrainingGoldPower = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private int mTrainingRevive;
    public int TrainingRevive
    {
        get { return mTrainingRevive; }
        set
        {
            mTrainingRevive = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private float mTrainingShieldTime;
    public float TrainingShieldTime
    {
        get { return mTrainingShieldTime; }
        set
        {
            mTrainingShieldTime = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private float mTrainingDodgeTime;
    public float TrainingDodgeTime
    {
        get { return mTrainingDodgeTime; }
        set
        {
            mTrainingDodgeTime = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #region EQUIP_STAT
    [SerializeField]
    private float mMoveSpeedRate;
    public float MoveSpeedRate
    {
        get { return mMoveSpeedRate; }
        set 
        {
            mMoveSpeedRate = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerEventHendler>().ChangeMoveSpeed(mBaseSPD * mMoveSpeedRate);
        }
    }
    #endregion
    #region EQUIP
    [SerializeField]
    private string mCurrentWeaponName;
    public string CurrentWeaponName
    {
        get { return mCurrentWeaponName; }
        set 
        { 
            mCurrentWeaponName = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private string mCurrentCostumeName;
    public string CurrentCostumeName
    {
        get { return mCurrentCostumeName; }
        set 
        { 
            mCurrentCostumeName = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private string mCurrentCostumeShapeName;
    public string CurrentCostumeShapeName
    {
        get { return mCurrentCostumeShapeName; }
        set
        {
            mCurrentCostumeShapeName = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #region GOODS
    [SerializeField]
    private int mGold;
    public int Gold
    {
        get { return mGold; }
        set 
        { 
            mGold = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
            if (GameObject.Find("LobbyPlayer") != null)
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerEventHendler>().ChangeGoods(mGold, mDiamond);
        }
    }
    [SerializeField]
    private int mDiamond;
    public int Diamond
    {
        get { return mDiamond; }
        set
        {
            mDiamond = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
            if(GameObject.Find("LobbyPlayer") != null)
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerEventHendler>().ChangeGoods(mGold, mDiamond);
        }
    }
    [SerializeField]
    private int mCutePotionCount;
    public int CutePotionCount
    {
        get { return mCutePotionCount; }
        set 
        { 
            mCutePotionCount = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private int mDailyAdCount;
    public int DailyAddCount
    {
        get => mDailyAdCount;
        set
        {
            mDailyAdCount = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #region UNLOCK_INFO
    [SerializeField]
    private StringBoolean mWeaponlock;
    public StringBoolean Weaponlock
    {
        get { return mWeaponlock; }
        set 
        {
            mWeaponlock = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private StringBoolean mCostumelock;
    public StringBoolean Costumelock
    {
        get { return mCostumelock; }
        set
        {
            mCostumelock = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    [SerializeField]
    private StringBoolean mSkilllock;
    public StringBoolean Skilllock
    {
        get { return mSkilllock; }
        set
        { 
            mSkilllock = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #region ADPASS
    [SerializeField]
    private bool mIsAdsPass;
    public bool IsAdsPass
    {
        get => mIsAdsPass;
        set
        {
            mIsAdsPass = value;
            SaveLoadManager.Instance.SavePlayerInfoFile(this);
        }
    }
    #endregion
    #endregion
}
