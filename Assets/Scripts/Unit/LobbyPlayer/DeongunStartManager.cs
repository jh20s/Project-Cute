using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DeongunStartManager : SingleToneMaker<DeongunStartManager>
{
    [SerializeField]
    private WarInfo mWarInfo = new WarInfo();
    [SerializeField]
    private GameObject mSurvivalModeCheck;
    [SerializeField]
    private bool mIsClickedSurvivalModeCheck;
    [SerializeField]
    private GameObject mBossModeCheck;
    [SerializeField]
    private bool mIsClickedBossModeCheck;

    [SerializeField]
    private GameObject mCutePotionCheck;
    [SerializeField]
    private bool mIsCutePotionCheck;

    [SerializeField]
    private GameObject mBossPrologPannel;

    [SerializeField]
    private DeongunBuffType mCurrentBuffType = DeongunBuffType.None;
    public DeongunBuffType CurrentBuffType
    {
        get { return mCurrentBuffType; }
    }
    [SerializeField]
    private int mCurrentBuffValue;
    [SerializeField]
    private string mCostumeName;
    public string CosumeName
    {
        get { return mCostumeName; }
    }
    [SerializeField]
    private string mBuffDesc;
    public string BuffDesc
    {
        get { return mBuffDesc; }
    }
    public enum DeongunBuffType
    {
        None,
        PlayerBaseSPD,
        PlayerBaseHP,
        PlayerCostume,
    }
    public struct DeongunBuff
    {
        public DeongunBuffType mType;
        public int mIntValue;
        public string mStringValue;
        public string mDesc;
    }
    public enum GameMode
    {
        None,
        SurvivalMode,
        BossMode
    }
    private void Awake()
    {
        AdmobManager.Instance.registerEndRewardObserver(RegisterEndRewardObserver);
;    }
    // ������ ���� ������ �̺�Ʈ�� �޴� �Լ�
    private void RegisterEndRewardObserver(bool _isEnd)
    {
        if (_isEnd)
        {
            DrawBuff();
            LobbyUIManager.Instance.OpenDoengunPannel();
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.DailyAddCount--;
        }
    }
    // ����Ÿ�Կ� �´� �������� ����
    private List<DeongunBuff> MakeDeongunBuffList()
    {
        List<Dictionary<string, object>> buffData = CSVReader.Read("CSVFile/SupplyData");
        List<DeongunBuff> buffList = new List<DeongunBuff>();
        LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        for(int i = 0; i < buffData.Count; i++)
        {
            DeongunBuff buff = new DeongunBuff();
            buff.mType = (DeongunBuffType)Enum.Parse(typeof(DeongunBuffType), buffData[i]["SupplyBuffType"].ToString());
            buff.mIntValue = int.Parse(buffData[i]["SupplyIntValue"].ToString());
            buff.mStringValue = buffData[i]["SupplyStringValue"].ToString();
            buff.mDesc = buffData[i]["SupplyDesc"].ToString();
            string weaponType = buffData[i]["SupplyWeaponType"].ToString();

            // �������� ������ ����Ÿ���̸�
            if (weaponType.Contains(info.CurrentWeaponName.Substring(0, 2)))
            {
                int count = int.Parse(buffData[i]["SupplyChance"].ToString());
                for (int j = 0; j < count; j++)
                    buffList.Add(buff);
            }
        }
        return buffList;
    }
    // ����Ÿ�� �̱�
    public void DrawBuff()
    {
        List<DeongunBuff> buffList = MakeDeongunBuffList();
        int ran = UnityEngine.Random.Range(0, buffList.Count);
        DeongunBuff selectBuff = buffList[ran];

        mCurrentBuffType = selectBuff.mType;
        mCurrentBuffValue = selectBuff.mIntValue;
        mCostumeName = selectBuff.mStringValue;
        mBuffDesc = selectBuff.mDesc;
    }
    // ���� ����
    public void ResetBuff()
    {
        mCurrentBuffType = DeongunBuffType.None;

    }
    public void ClickSirvivalModeButton()
    {
        mIsClickedSurvivalModeCheck = !mIsClickedSurvivalModeCheck;
        mSurvivalModeCheck.transform.GetChild(0).gameObject.SetActive(mIsClickedSurvivalModeCheck);
        if (mIsClickedBossModeCheck)
        {
            mIsClickedBossModeCheck = false;
            mBossModeCheck.transform.GetChild(0).gameObject.SetActive(mIsClickedBossModeCheck);
        }
        mWarInfo.WarMode = mIsClickedSurvivalModeCheck ? GameMode.SurvivalMode : GameMode.None;
    }
    public void ClickBossModeButton()
    {
        mIsClickedBossModeCheck = !mIsClickedBossModeCheck;
        mBossModeCheck.transform.GetChild(0).gameObject.SetActive(mIsClickedBossModeCheck);
        if (mIsClickedSurvivalModeCheck)
        {
            mIsClickedSurvivalModeCheck = false;
            mSurvivalModeCheck.transform.GetChild(0).gameObject.SetActive(mIsClickedSurvivalModeCheck);
            if(mIsCutePotionCheck)
                ClickCutePotion();
        }   
        mWarInfo.WarMode = mIsClickedBossModeCheck ? GameMode.BossMode : GameMode.None;
    }
    public void ClickCutePotion()
    {
        if (mIsClickedSurvivalModeCheck)
        {
            mIsCutePotionCheck = !mIsCutePotionCheck;
            mCutePotionCheck.transform.GetChild(0).gameObject.SetActive(mIsCutePotionCheck);
        }
        else
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("���� ��� ���� �ÿ��� �Ϳ������� ������ ��밡���մϴ�.");
            mIsCutePotionCheck = false;
            mCutePotionCheck.transform.GetChild(0).gameObject.SetActive(mIsCutePotionCheck);
        }

    }
    public void ClickGameStart()
    {
        if(mWarInfo.WarMode == GameMode.None)
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("��� ������ �ùٸ��� �ʽ��ϴ�.");
        }
        else if(mWarInfo.WarMode == GameMode.SurvivalMode)
        {
            LobbyPlayerDataSend();
        }
        else
        {
            mBossPrologPannel.SetActive(true);
        }
    }

    public void LobbyPlayerDataSend()
    {
        LobbyPlayerInfo playerData = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        if (mWarInfo.WarMode == GameMode.BossMode)
            mWarInfo.IsBossRelay = true;
        else
            mWarInfo.IsBossRelay = false;

        mWarInfo.WarDeongunBuffType = mCurrentBuffType;
        mWarInfo.WarCostumeName = playerData.CurrentCostumeName;
        mWarInfo.WarCostumeShapeName = playerData.CurrentCostumeShapeName;
        string originCostumeName = playerData.CurrentCostumeName;
        if (mCurrentBuffType != DeongunBuffType.None)
        {
            switch (mCurrentBuffType)
            {
                case DeongunBuffType.PlayerBaseHP:
                    mWarInfo.WarBuffHp = mCurrentBuffValue;
                    break;
                case DeongunBuffType.PlayerBaseSPD:
                    mWarInfo.WarBuffSpeedRate = mCurrentBuffValue;
                    break;
                case DeongunBuffType.PlayerCostume:
                    mWarInfo.WarCostumeName = mCostumeName;
                    mWarInfo.WarCostumeShapeName = mCostumeName;
                    break;
            }
        }
        mWarInfo.WarHp = playerData.BaseHp + playerData.TrainingHp
            - EquipmentManager.Instance.FindCostume(originCostumeName).GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP)
            + EquipmentManager.Instance.FindCostume(mWarInfo.WarCostumeName).GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
        mWarInfo.WarDamage = (int)((playerData.BaseATK + playerData.TrainingATK) * playerData.TrainingAddDamage);
        mWarInfo.WarMoveSpeed = playerData.BaseSPD * (playerData.MoveSpeedRate
            - EquipmentManager.Instance.FindCostume(originCostumeName).GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) / 100f
            + EquipmentManager.Instance.FindCostume(mWarInfo.WarCostumeName).GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) / 100f);
        mWarInfo.WarDiamond = playerData.Diamond;
        mWarInfo.WarWeaponName = playerData.CurrentWeaponName;
        mWarInfo.WarSkillLock = playerData.Skilllock;
        mWarInfo.IsWarCutePotion = mIsCutePotionCheck;
        mWarInfo.WarMagnetPower = playerData.TrainingMagnetPower;
        mWarInfo.WarGoldRate = playerData.TrainingGoldPower;
        mWarInfo.WarRevuveValue = playerData.TrainingRevive;
        mWarInfo.WarStaffShieldTime = playerData.TrainingShieldTime;
        mWarInfo.WarMeleeDodgeCount = playerData.TrainingDodgeTime;
        SaveLoadManager.Instance.SavePlayerWarData(mWarInfo);
        if (mIsCutePotionCheck)
            playerData.CutePotionCount--;
        // Ŀ���͸���¡ ���� ����
        CustomizingManager.Instance.SaveShowHelmet();
        // ���� �� �ε�
        SceneManager.LoadScene("SampleScene");
    }
}
