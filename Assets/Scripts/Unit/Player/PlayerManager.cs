using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SingleToneMaker<PlayerManager>
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject mPlayer;
    public GameObject Player
    {
        get { return mPlayer; }
    }
    [SerializeField]
    private SpriteRenderer mWeaponSprite;

    // �÷��̾��� ��������Ʈ(�ڽ�Ƭ)
    [SerializeField]
    private List<SpriteRenderer> mCostumes;

    [SerializeField]
    private Text mTestCostumeRankText;

    [SerializeField]
    private bool mIsGameStart;

    [SerializeField]
    public Dictionary<int, int> mLevelData;
    public bool IsGameStart
    {
        get { return mIsGameStart; }
        set { mIsGameStart = value; }
    }
    [SerializeField]
    private bool mIsCutePotion = false;
    [SerializeField]
    private Image mSupplyImage;
    [SerializeField]
    private float mEndGoldRate;
    public float EndGoldRate => mEndGoldRate;
    [SerializeField]
    private int mAutoReviveValue;
    public int AutoReviveValue => mAutoReviveValue;
    [SerializeField]
    private float mStaffShieldTime;
    public float StaffShieldTime => mStaffShieldTime;
    [SerializeField]
    private float mMeleeDodgeCount;
    public float MeleeDodgeCount => mMeleeDodgeCount;
    [SerializeField]
    private bool mIsAdsPass;
    public bool IsAdsPass => mIsAdsPass;
    // DEBUG�� �� ���� ������Ʈ
    //public GameObject mSelectMap;

    private void Awake()
    {
        mLevelData = new Dictionary<int, int>();
        InitPlayerLevelData();
    }
    void Start()
    {
        IsGameStart = false; 
        mPlayer = GameObject.Find("PlayerObject");
        InitPlayerBaseStat();
        
    }

    public void AutoReviveValueDiscount()
    {
        mAutoReviveValue--;
    }
    private void InitPlayerBaseStat()
    {
        List<Dictionary<string, object>> playerBaseStatData = CSVReader.Read("CSVFile/PlayerBaseStat");
        for(int i = 0; i < playerBaseStatData.Count; i++)
        {
            mPlayer.GetComponent<IStatus>().CriticalChance = float.Parse(playerBaseStatData[i]["PlayerBaseCritChance"].ToString());
            mPlayer.GetComponent<IStatus>().CriticalDamage = float.Parse(playerBaseStatData[i]["PlayerBaseCritDamage"].ToString());
            mPlayer.GetComponent<IStatus>().AttackSpeed = float.Parse(playerBaseStatData[i]["PlayerBaseATKSPD"].ToString());
        }
        WarInfo loadInfo = SaveLoadManager.Instance.LoadPlayerWarData();

        mPlayer.GetComponent<IStatus>().Hp = loadInfo.WarHp + loadInfo.WarBuffHp;
        mPlayer.GetComponent<IStatus>().MaxHP = loadInfo.WarHp + loadInfo.WarBuffHp;
        mPlayer.GetComponent<IStatus>().BaseDamage = loadInfo.WarDamage;
        mPlayer.GetComponent<IStatus>().MoveSpeed = loadInfo.WarMoveSpeed * (1 + loadInfo.WarBuffSpeedRate * 0.01f);
        mPlayer.GetComponent<PlayerStatus>().Diamond = loadInfo.WarDiamond;
        mPlayer.GetComponent<PlayerStatus>().MagnetPower = loadInfo.WarMagnetPower;
        mEndGoldRate = loadInfo.WarGoldRate;
        mAutoReviveValue = loadInfo.WarRevuveValue;
        mStaffShieldTime = loadInfo.WarStaffShieldTime;
        mMeleeDodgeCount = loadInfo.WarMeleeDodgeCount;
        // ���, ���� ������(�ڽ�Ƭ�� ���� �ʿ� X)
        EquipmentManager.Instance.ChangeWeapon(loadInfo.WarWeaponName);
        EquipmentManager.Instance.ChangeCostume(loadInfo.WarCostumeShapeName);
        if (mPlayer.GetComponent<PlayerStatus>().PlayerCurrentWeapon.name.Substring(0, 2) == "sw" ||
            mPlayer.GetComponent<PlayerStatus>().PlayerCurrentWeapon.name.Substring(0, 2) == "sp")
        {
            mPlayer.GetComponent<IStatus>().AttackSpeed = 1f;
        }
        // ��ų �ر� ���� �ҷ�����
        foreach (string key in loadInfo.WarSkillLock.Keys)
        {
            SkillManager.Instance.FindSkill(key).GetComponent<Skill>().Spec.IsLocked = loadInfo.WarSkillLock[key];
        }
        // ��ų �� UI�� �ҷ�����
        UIManager.Instance.SkillSelectUILoad(loadInfo.WarWeaponName.Substring(0, 2));

        // �� ���� �ҷ�����
        if (loadInfo.IsBossRelay)
        {
            MapManager.Instance.CurrentMapType = MapManager.MapType.BossRelay;
            MapManager.Instance.MapSelect();
        }
        else
        {
            // �� ���� ����
            MapManager.Instance.RandMapSelect();
        }
        // ���� ���� Ÿ�Ժ� UI����
        switch (loadInfo.WarDeongunBuffType)
        {
            case DeongunStartManager.DeongunBuffType.PlayerBaseHP:
                mSupplyImage.sprite = Resources.Load<Sprite>("UI/WarUI/WarIcon/RecoverHP");
                break;
            case DeongunStartManager.DeongunBuffType.PlayerBaseSPD:
                mSupplyImage.sprite = Resources.Load<Sprite>("UI/WarUI/WarIcon/MoveSPD");
                break;
            case DeongunStartManager.DeongunBuffType.PlayerCostume:
                mSupplyImage.sprite 
                    = EquipmentManager.Instance.FindCostume(loadInfo.WarCostumeName).GetComponent<SpriteRenderer>().sprite;
                break;
        }
        // �Ϳ������� ���� ��� ����
        mIsCutePotion = loadInfo.IsWarCutePotion;
        // ���� �н� ����
        mIsAdsPass = loadInfo.IsAdsPass;
    }

    private void InitPlayerLevelData()
    {
        List<Dictionary<string, object>> levelData = CSVReader.Read("CSVFile/LevelData");
        for (int i = 0; i < levelData.Count; i++)
        {
            mLevelData[int.Parse(levelData[i]["Level"].ToString())] = int.Parse(levelData[i]["Exp"].ToString());
        }
    }

    public SpriteRenderer getPlayerWeaponSprite()
    {
        return mWeaponSprite;
    }

    public SpriteRenderer GetPlayerCostumeSpriteRenderer(int _num)
    {
        return mCostumes[_num];
    }

    public void SettingGameStart()
    {
        mPlayer.GetComponent<PlayerAttack>().getProjectiles();
        SpawnManager.Instance.InitAllSpawnData();

        string weaponType =
            mPlayer.GetComponent<PlayerStatus>()
            .PlayerCurrentWeapon.GetComponent<Weapon>().Spec.Type;
        weaponType = weaponType.Substring(0, 2);
        LevelUpStatusManager.Instance.SetSlots(weaponType);

        MusicManager.Instance.OnBackgroundMusic();
        UIManager.Instance.GameRestart();
        if (mIsCutePotion)
        {
            int exp = 0;
            for (int i = 1; i < 10; i++)
                exp += mLevelData[i];
            Player.GetComponent<PlayerStatus>().PlayerGetExp += exp;
        }
        IsGameStart = true;
    }

    public void ResurrectionPlayer()
    {
        mPlayer.GetComponent<PlayerStatus>().Resurrection();
    }
}
