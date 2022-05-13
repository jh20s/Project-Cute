using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUIManager : SingleToneMaker<LobbyUIManager>
{
    // Start is called before the first frame update
    #region Pannel
    [Header("�Ͻ�����")]
    [SerializeField]
    private GameObject mBackGroundPannel;
    [SerializeField]
    private bool mIsPause = false;

    [Header("�޴�")]
    [SerializeField]
    private GameObject mMenuPannel;
    [SerializeField]
    private bool mIsOpenMenuPannel = false;

    [Header("��ųâ")]
    [SerializeField]
    private GameObject mSkillPannel;
    [SerializeField]
    private bool mIsOpenSkillPannel = false;

    [Header("��ų����â")]
    [SerializeField]
    private GameObject mSkillInfoPannel;

    [Header("����â")]
    [SerializeField]
    private GameObject mAchivePannel;
    [SerializeField]
    private bool mIsOpenAchivePannel = false;

    [Header("��������Ʈâ")]
    [SerializeField]
    private GameObject mDailyQuestPannel;
    [SerializeField]
    private bool mIsOpenDailyQuestPannel = false;

    [Header("�ɼ�â")]
    [SerializeField]
    private GameObject mOptionPannel;
    [SerializeField]
    private bool mIsOpenOptionPannel = false;

    [Header("��������â")]
    [SerializeField]
    private GameObject mGameExitPannel;
    [SerializeField]
    private bool mIsOpenGameExitPannel = false;

    [Header("â���г�")]
    [SerializeField]
    private GameObject mWareHousePannel;
    [SerializeField]
    private bool mIsOpenWareHousePannel = false;

    [Header("���â")]
    [SerializeField]
    private GameObject mAlertEnterPannel;
    [SerializeField]
    private GameObject mAlertEnterExitPannel;

    [Header("���� ����")]
    [SerializeField]
    private GameObject mSupportItemPannel;
    [SerializeField]
    private GameObject mDeongunStartPannel;

    [Header("�÷��̾� ����â")]
    [SerializeField]
    private GameObject mPlayerInfoPannel;
    [SerializeField]
    private bool mIsOpenPlayerInfoPannel = false;

    [Header("BM����")]
    [SerializeField]
    private GameObject mBMPannel;
    [SerializeField]
    private bool mIsOpenBMPannel = false;

    [Header("����ǰ����")]
    [SerializeField]
    private GameObject mSupplyShopPannel;
    [SerializeField]
    private bool mIsOpenSupplyShopPannel = false;
    [SerializeField]
    private GameObject mBuyAlertPannel;
    [SerializeField]
    private List<GameObject> mGoodsImageList = new List<GameObject>();
    [SerializeField]
    private List<int> mPriceList = new List<int>();
    [SerializeField]
    private int mBuyNum;
    [SerializeField]
    private GameObject mBoxAnimationPannel;
    [SerializeField]
    private GameObject mGachaItemListPannel;

    [Header("�Ʒ�")]
    [SerializeField]
    private GameObject mTrainingPannel;
    [SerializeField]
    private GameObject mTrainingAlertPannel;
    [SerializeField]
    private int mSelectCost;
    [SerializeField]
    private TrainingManager.TrainingType mSelectType;

    [Header("���̵�")]
    [SerializeField]
    private GameObject mGuidePannel;
    [SerializeField]
    private bool mIsOpenGuidePannel;
    #endregion
    void Start()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public void OpenAlertEnterPannel(string _desc)
    {
        mAlertEnterPannel.transform.GetChild(2).GetComponent<Text>().text = _desc;
        mAlertEnterPannel.SetActive(true);
    }
    public void CloseAlertEnterPannel()
    {
        mAlertEnterPannel.SetActive(false);
    }
    public void OpenAlertEnterExitPannel(string _desc)
    {
        mAlertEnterExitPannel.transform.GetChild(2).GetComponent<Text>().text = _desc;
        mAlertEnterExitPannel.SetActive(true);
    }
    public void EnterAlertEnterExitPannel()
    {

    }
    public void CloseAlertEnterExitPannel()
    {
        mAlertEnterExitPannel.SetActive(false);
    }
    private void GamePause()
    {
        Time.timeScale = Convert.ToInt32(mIsPause);
        mIsPause = !mIsPause;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        mBackGroundPannel.SetActive(mIsPause);
    }
    public void ClickMenuButton()
    {
        mIsOpenMenuPannel = !mIsOpenMenuPannel;
        mMenuPannel.SetActive(mIsOpenMenuPannel);
    }
    public void ClickSkillButton()
    {
        GamePause();
        mIsOpenSkillPannel = !mIsOpenSkillPannel;
        if (mIsOpenSkillPannel)
        {
            for (int i = 0; i < 4; i++)
            {
                string weaponType = mSkillPannel.transform.GetChild(i + 5).name;
                List<GameObject> generalSkill = SkillManager.Instance.FindGeneralSkill(weaponType);
                for (int j = 0; j < generalSkill.Count; j++)
                {
                    mSkillPannel.transform.GetChild(i + 5).GetChild(j + 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("UI/SkillIcon/" + generalSkill[j].name);
                    mSkillPannel.transform.GetChild(i + 5).GetChild(j + 1).GetChild(1)
                        .gameObject.SetActive(generalSkill[j].GetComponent<Skill>().Spec.IsLocked);
                }
                List<GameObject> ultimateSkill = SkillManager.Instance.FindUltimateSkill(weaponType);
                for (int j = 0; j < ultimateSkill.Count; j++)
                {
                    mSkillPannel.transform.GetChild(i + 5).GetChild(j + 4).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("UI/SkillIcon/" + ultimateSkill[j].name);
                    mSkillPannel.transform.GetChild(i + 5).GetChild(j + 4).GetChild(1)
                        .gameObject.SetActive(ultimateSkill[j].GetComponent<Skill>().Spec.IsLocked);
                }
            }
        }
        mSkillPannel.SetActive(mIsOpenSkillPannel);
    }
    public void ClickAchiveButton()
    {
        GamePause();
        mIsOpenAchivePannel = !mIsOpenAchivePannel;
        if (mIsOpenAchivePannel)
        {
            AchievementManager.Instance.UpdateState();
        }
        mAchivePannel.SetActive(mIsOpenAchivePannel);
    }
    public void ClickDailyQuestButton()
    {
        GamePause();
        mIsOpenDailyQuestPannel = !mIsOpenDailyQuestPannel;
        mDailyQuestPannel.SetActive(mIsOpenDailyQuestPannel);
    }
    public void ClickOptionButton()
    {
        GamePause();
        mIsOpenOptionPannel = !mIsOpenOptionPannel;
        mOptionPannel.SetActive(mIsOpenOptionPannel);
    }
    public void ClickGameExitButton()
    {
        GamePause();
        mIsOpenGameExitPannel = !mIsOpenGameExitPannel;
        mGameExitPannel.SetActive(mIsOpenGameExitPannel);
    }
    public void ClickInteractionButton()
    {
        LobbyPlayerMove player = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerMove>();
        LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        // �÷��̾ â��� ��ȣ�ۿ����� �̶��
        if (player.IsTriggerInWareHouse)
        {
            GamePause();
            mIsOpenWareHousePannel = !mIsOpenWareHousePannel;
            mWareHousePannel.SetActive(mIsOpenWareHousePannel);
        }
        // �÷��̾ ���� �Ա��� ��ȣ�ۿ� ����� �̶��
        if (player.IsTriggerInStartZone)
        {
            if(info.CurrentWeaponName == "" || info.CurrentCostumeName == "")
            {
                OpenAlertEnterPannel("����� �ڽ�Ƭ�� �������� ������ ������ ������ �� �����ϴ�.");
            }
            else
            {
                GamePause();
                mSupportItemPannel.transform.GetChild(8).GetChild(1).GetComponent<Text>().text =
                    info.DailyAddCount.ToString();
                mSupportItemPannel.SetActive(true);
            }
        }
        // �÷��̾ ����ǰ������ ��ȣ�ۿ� ����� �̶��
        if (player.IsTriggerInSupplyZone)
        {
            GamePause();
            mSupplyShopPannel.SetActive(true);
        }
        // �÷��̾ �Ʒñ����� ��ȣ�ۿ� ����� �̶��
        if (player.IsTriggerInTrainingZone)
        {
            GamePause();
            SetTrainingPannel();
        }
    }
    public void SetTrainingPannel()
    {
        TrainingManager.Instance.UpdateStstus();
        mTrainingPannel.SetActive(true);
    }
    public void CloseTrainingPannel()
    {
        mTrainingPannel.SetActive(false);
        GamePause();
    }
    public void ClickTrainingButton()
    {

        TrainingManager.Training training = TrainingManager.Instance.TrainingSet[TrainingManager.Instance.CurrentSelectType];
        if(training.mMax == training.mCurrentValue)
        {
            //���
            OpenAlertEnterPannel("���� ������ �Ʒ��� �̹� �ִ�ġ �Դϴ�.");
        }
        else
        {
            mSelectType = TrainingManager.Instance.CurrentSelectType;
            // ���� ��ġ
            mTrainingAlertPannel.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text =
                training.mLevel.ToString();
            mTrainingAlertPannel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text =
                training.mName + " +" + training.mCurrentValue.ToString() + training.mUnit;
            // ���� ��ġ
            mTrainingAlertPannel.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text =
                (training.mLevel + 1).ToString();
            mTrainingAlertPannel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text =
                training.mName + " +" +TrainingManager.Instance.NextLevelValue(mSelectType) + training.mUnit;
            // ���
            mSelectCost = training.mCurrentCost;
            mTrainingAlertPannel.transform.GetChild(6).GetChild(2).GetComponent<Text>().text = training.mCurrentCost.ToString();

            mTrainingAlertPannel.SetActive(true);
        }
    }
    public void CloseTrainingAlertPannel()
    {
        mTrainingAlertPannel.SetActive(false);
    }
    public void ClickTrainging()
    {
        LobbyPlayerInfo playerInfo = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        if (playerInfo.Gold >= mSelectCost)
        {
            playerInfo.Gold -= mSelectCost;
            TrainingManager.Instance.LevelUpTraining(mSelectType);
            SetTrainingPannel();
            mTrainingAlertPannel.SetActive(false);

        }
        else
        {
            OpenAlertEnterPannel("��尡 �����մϴ�.");
        }
    }
    public void ClickAdvButton(bool _ok)
    {
        mDeongunStartPannel.transform.GetChild(3).GetChild(6).gameObject.SetActive(true);
        mDeongunStartPannel.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        // ���� ���� �� ���
        if (_ok)
        {
            // TODO : ���� Ʋ��, ���� ���� ���� ����, ���� Ƚ�� ����
            // ���� ���� ���� Ƚ�� üũ �ϱ�
            if(GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.DailyAddCount > 0)
            {
                AdmobManager.Instance.Show(AdmobManager.AdType.Supply);
                mDeongunStartPannel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                OpenAlertEnterPannel("���� ���� Ƚ���� ��� �����Ǿ����ϴ�.\n����9�ÿ� Ƚ���� �����˴ϴ�.");
            }
        }
        // ���� ���� ���� ���
        else
        {
            OpenDoengunPannel();
        }
    }
    public void OpenDoengunPannel()
    {
        mSupportItemPannel.SetActive(false);

        // ���� ���� UI
        switch (DeongunStartManager.Instance.CurrentBuffType)
        {
            case DeongunStartManager.DeongunBuffType.None:
                break;
            case DeongunStartManager.DeongunBuffType.PlayerBaseHP:
                mDeongunStartPannel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = Resources.Load<Sprite>("UI/WarUI/WarIcon/RecoverHP");
                break;
            case DeongunStartManager.DeongunBuffType.PlayerBaseSPD:
                mDeongunStartPannel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = Resources.Load<Sprite>("UI/WarUI/WarIcon/MoveSPD");
                break;
            case DeongunStartManager.DeongunBuffType.PlayerCostume:
                mDeongunStartPannel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = EquipmentManager.Instance.FindCostume(DeongunStartManager.Instance.CosumeName)
                    .GetComponent<SpriteRenderer>().sprite;
                break;
        }
        mDeongunStartPannel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text
            = DeongunStartManager.Instance.BuffDesc;
        // �Ϳ������� ���� UI
        LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        if (info.CutePotionCount > 0)
        {
            mDeongunStartPannel.transform.GetChild(3).GetChild(6).gameObject.SetActive(false);
            mDeongunStartPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
                info.CutePotionCount.ToString();
        }
        mDeongunStartPannel.SetActive(true);
    }
    public void CloseDeongunStartPannel()
    {
        GamePause();
        mDeongunStartPannel.SetActive(false);
        // TODO : ����ǰ��ִ� ���� ���� ����
        DeongunStartManager.Instance.ResetBuff();
    }
    public void CloseSupplyShopPannel()
    {
        GamePause();
        mSupplyShopPannel.SetActive(false);
    }
    public void ClickBuyAlertButton(int _num)
    {
        mBuyNum = _num;
        mBuyAlertPannel.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite =
            mGoodsImageList[_num].GetComponent<Image>().sprite;

        mBuyAlertPannel.SetActive(true);
    }
    public void CloseBuyAlertButton()
    {
        mBuyAlertPannel.SetActive(false);
    }
    public void ClickBuyButton()
    {
        LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        // �ִϸ��̼� �ʿ�
        if (info.Diamond >= mPriceList[mBuyNum])
        {
            info.Diamond -= mPriceList[mBuyNum];
            StartCoroutine(BoxAnimaion());
            CloseBuyAlertButton();
        }
        // ���� �Ұ���
        else
        {
            OpenAlertEnterPannel("���̾Ƹ�尡 �����մϴ�.");
        }
    }
    IEnumerator BoxAnimaion()
    {
        GamePause();
        mBoxAnimationPannel.SetActive(true);
        mBoxAnimationPannel.transform.GetChild(mBuyNum).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        mBoxAnimationPannel.transform.GetChild(mBuyNum).gameObject.SetActive(false);
        mBoxAnimationPannel.SetActive(false);
        GamePause();
        // ����Ȯ�� â ����
        ItemDraw();
    }
    public void ItemDraw()
    {
        Transform item = null;
        mGachaItemListPannel.transform.GetChild(3).gameObject.SetActive(false);
        mGachaItemListPannel.transform.GetChild(4).gameObject.SetActive(false);
        switch (mBuyNum)
        {
            case 0: // �ѹ� �̱�
                item = mGachaItemListPannel.transform.GetChild(3);
                item.gameObject.SetActive(true);
                item.GetChild(1).GetChild(0).gameObject.SetActive(false);
                item.GetChild(1).GetChild(1).gameObject.SetActive(false);
                item.GetChild(1).GetChild(2).gameObject.SetActive(false);
                string name = GachaManager.Instance.OneItemDraw();
                if (name == GachaManager.Instance.CutePotion)
                {
                    item.GetChild(0).GetChild(0).GetComponent<Text>().text = "�Ϳ������� ����";
                    item.GetChild(1).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UIAsset/Cute_Potion");
                    item.GetChild(5).GetChild(0).GetComponent<Text>().text = "���� ���� �� ����10���� ������ �� �ִ�.";
                }
                else
                {
                    GameObject costume = EquipmentManager.Instance.FindCostume(name);
                    item.GetChild(0).GetChild(0).GetComponent<Text>().text = costume.GetComponent<Costume>().Spec.EquipName;
                    item.GetChild(6).gameObject.SetActive(true);
                    item.GetChild(6).GetChild(0).GetComponent<Text>().text = costume.GetComponent<Costume>().Spec.EquipRankDesc;
                    switch(name.Substring(3, name.Length - 5))
                    {
                        case "bgst":
                            item.GetChild(6).GetChild(1).GetComponent<Text>().text = "������ �Ǵ� ���� ���⿡ ���� �����մϴ�.";
                            break;
                        case "swsp":
                            item.GetChild(6).GetChild(1).GetComponent<Text>().text = "�ҵ� �Ǵ� ���Ǿ� ���⿡ ���� �����մϴ�.";
                            break;
                        case "bgstswsp":
                            item.GetChild(6).GetChild(1).GetComponent<Text>().text = "��� ���⿡ ���� �����մϴ�.";
                            break;
                    }
                    item.GetChild(1).GetChild(costume.GetComponent<Costume>().Spec.Rank - 1).gameObject.SetActive(true);
                    item.GetChild(1).GetChild(3).GetComponent<Image>().sprite = costume.GetComponent<SpriteRenderer>().sprite;
                    int hp = costume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
                    if (hp > 0)
                        item.GetChild(2).GetChild(0).GetComponent<Text>().text = "HP : +" + hp.ToString();
                    int spd = costume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD);
                    if (spd > 0)
                        item.GetChild(3).GetChild(0).GetComponent<Text>().text = "SPD : +" + spd.ToString();
                    item.GetChild(5).GetChild(0).GetComponent<Text>().text = costume.GetComponent<Costume>().Spec.EquipDesc;
                }
                break;
            case 1: // 10�� �̱�
                item = mGachaItemListPannel.transform.GetChild(4);
                item.gameObject.SetActive(true);
                List<string> nameList = GachaManager.Instance.TenItemDraw();
                for(int i = 0; i < nameList.Count; i++)
                {
                    item.GetChild(i).GetChild(0).gameObject.SetActive(false);
                    item.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    item.GetChild(i).GetChild(2).gameObject.SetActive(false);
                    if (nameList[i] == GachaManager.Instance.CutePotion)
                    {
                        item.GetChild(i).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UIAsset/Cute_Potion");
                    }
                    else
                    {
                        GameObject costume = EquipmentManager.Instance.FindCostume(nameList[i]);
                        item.GetChild(i).GetChild(costume.GetComponent<Costume>().Spec.Rank - 1).gameObject.SetActive(true);
                        item.GetChild(i).GetChild(3).GetComponent<Image>().sprite = costume.GetComponent<SpriteRenderer>().sprite;
                    }
                }
                break;
        }
        mGachaItemListPannel.SetActive(true);
    }
    public void CloseGachItemListPannel()
    {
        mGachaItemListPannel.SetActive(false);
    }
    public void ClickPlayerInfoPannel()
    {
        mIsOpenPlayerInfoPannel = !mIsOpenPlayerInfoPannel;
        // �÷��̾��� ���� �޾ƿ���
        if (mIsOpenPlayerInfoPannel)
        {
            LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
            // TODO : �������� �ҷ�����
            mPlayerInfoPannel.transform.GetChild(7).GetChild(1).GetComponent<Text>().text 
                = (info.BaseHp + info.TrainingHp).ToString();
            mPlayerInfoPannel.transform.GetChild(7).GetChild(3).GetComponent<Text>().text 
                = Mathf.Ceil((info.BaseATK + info.TrainingATK) * info.TrainingAddDamage).ToString();
            mPlayerInfoPannel.transform.GetChild(7).GetChild(5).GetComponent<Text>().text 
                = (info.BaseSPD * info.MoveSpeedRate).ToString();

            if (info.CurrentWeaponName != "")
                mPlayerInfoPannel.transform.GetChild(8).GetChild(1).GetChild(0).GetComponent<Image>().sprite
                    = EquipmentManager.Instance.FindWeapon(info.CurrentWeaponName).GetComponent<SpriteRenderer>().sprite;
            if (info.CurrentCostumeName != "")
                mPlayerInfoPannel.transform.GetChild(9).GetChild(1).GetChild(0).GetComponent<Image>().sprite
                    = EquipmentManager.Instance.FindCostume(info.CurrentCostumeName).GetComponent<SpriteRenderer>().sprite;
            if (info.CurrentCostumeShapeName != "")
                mPlayerInfoPannel.transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite
                    = EquipmentManager.Instance.FindCostume(info.CurrentCostumeShapeName).GetComponent<SpriteRenderer>().sprite;
        }
        mPlayerInfoPannel.SetActive(mIsOpenPlayerInfoPannel);
    }
    public void ClickSkillButton(string _type)
    {
        string weaponType = _type.Substring(0, 2);
        string skillType = _type.Substring(2, 1);
        int skillNum = int.Parse(_type.Substring(3, 1));
        List<GameObject> skillList = null;
        if(skillType == "g")
        {
            skillList = SkillManager.Instance.FindGeneralSkill(weaponType);
            Skill skill = skillList[skillNum].GetComponent<Skill>();
            string coolTime = "";
            mSkillInfoPannel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("UI/SkillIcon/" + skillList[skillNum].name);
            mSkillInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = skill.Spec.EquipName;
            for (int i = 0; i < skillList[skillNum].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
            {
                coolTime += "[" + skillList[skillNum].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
            }
            mSkillInfoPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text 
                = skill.Spec.EquipDesc + "\n" + "��Ÿ�� : " + coolTime;
        }
        else
        {
            skillList = SkillManager.Instance.FindUltimateSkill(weaponType);
            Skill skill = skillList[skillNum].GetComponent<Skill>();
            string coolTime = "";
            mSkillInfoPannel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("UI/SkillIcon/" + skillList[skillNum].name);
            mSkillInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = skill.Spec.EquipName;
            for (int i = 0; i < skillList[skillNum].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
            {
                coolTime += "[" + skillList[skillNum].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
            }
            mSkillInfoPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text 
                = skill.Spec.EquipDesc + "\n" + "��Ÿ�� : " + coolTime;
        }
        mSkillInfoPannel.SetActive(true);
    }
    public void CloseSkillInfoPannel()
    {
        mSkillInfoPannel.SetActive(false);
    }
    public void ClickBMButton()
    {
        mIsOpenBMPannel = !mIsOpenBMPannel;
        if (mIsOpenBMPannel)
        {
            mBMPannel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text =
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.Diamond.ToString();
        }
        mBMPannel.SetActive(mIsOpenBMPannel);
    }
    public void ClickGuideButton()
    {
        mIsOpenGuidePannel = !mIsOpenGuidePannel;
        mGuidePannel.SetActive(mIsOpenGuidePannel);
    }
    public void ClickNextGuide(bool _isNext)
    {
        mGuidePannel.transform.GetChild(1).gameObject.SetActive(!_isNext);
        mGuidePannel.transform.GetChild(2).gameObject.SetActive(_isNext);
        mGuidePannel.transform.GetChild(3).gameObject.SetActive(!_isNext);
        mGuidePannel.transform.GetChild(4).gameObject.SetActive(_isNext);
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }


    // �׽�Ʈ��
    public void ClickUnlock()
    {
        LobbyPlayerInfo mInfo = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        List<string> nameList = new List<string>();
        // ���� �ر� ���� ����
        foreach (string key in mInfo.Weaponlock.Keys)
        {
            nameList.Add(key);
            EquipmentManager.Instance.FindWeapon(key).GetComponent<Weapon>().IsLocked = false;
            WareHouseManager.Instance.ChangeWeaponUnlock(key, false);
        }
        for (int i = 0; i < nameList.Count; i++)
            mInfo.Weaponlock[nameList[i]] = false;
        nameList.Clear();
        //// �ڽ�Ƭ �ر� ���� ����
        //foreach (string key in mInfo.Costumelock.Keys)
        //{
        //    nameList.Add(key);
        //    EquipmentManager.Instance.FindCostume(key).GetComponent<Costume>().IsLocked = false;
        //    WareHouseManager.Instance.ChangeCostumeUnlock(key, false);
        //}
        //for (int i = 0; i < nameList.Count; i++)
        //    mInfo.Costumelock[nameList[i]] = false;
        //nameList.Clear();
        // ��ų �ر� ���� ����
        foreach (string key in mInfo.Skilllock.Keys)
        {
            nameList.Add(key);
            SkillManager.Instance.FindSkill(key).GetComponent<Skill>().Spec.IsLocked = false;
        }
        for (int i = 0; i < nameList.Count; i++)
            mInfo.Skilllock[nameList[i]] = false;
        nameList.Clear();
        OpenAlertEnterPannel("��� ����, ��ų�� �رݵǾ����ϴ�.");
    }
}
