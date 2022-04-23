using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : SingleToneMaker<UIManager>
{
    #region variables
    [Header("�Ͻ�����")]
    [SerializeField]
    private bool mIsPause;
    [SerializeField]
    private GameObject mPauseBtn;
    [SerializeField]
    private GameObject mPausePannel;
    [SerializeField]
    private GameObject mBackGroundPannel;
    [SerializeField]
    private Text mProjectileCountText;
    [SerializeField]
    private Text mProjectileScaleText;
    [SerializeField]
    private Text mPassCountText;

    [Header("�ɼ�")]
    [SerializeField]
    private bool mIsOption;
    [SerializeField]
    private GameObject mOptionPannel;
    [SerializeField]
    private GameObject mExitBtn;

    [Header("��������")]
    [SerializeField]
    private bool mIsGiveup;
    [SerializeField]
    private GameObject mGiveupPannel;

    [Header("�ɷ�ġ����â")]
    [SerializeField]
    private GameObject mStatusSelectPannel;
    [SerializeField]
    private Text mFirstSelectText;
    [SerializeField]
    private Text mSecondSelectText;
    [SerializeField]
    private Text mThirdSelectText;

    [Header("���ӿ���")]
    [SerializeField]
    private GameObject mGameOverPannel;


    [Header("�׽�Ʈ �κ�")]
    [SerializeField]
    private GameObject mTestLobbyPannel;
    [SerializeField]
    private GameObject mBaseSkill;
    [SerializeField]
    private List<GameObject> mGeneralSkillBtn = new List<GameObject>();
    [SerializeField]
    private List<GameObject> mGeneralSkill = new List<GameObject>();
    [SerializeField]
    private List<GameObject> mUltimateSkillBtn = new List<GameObject>();
    [SerializeField]
    private List<GameObject> mUltimateSkill = new List<GameObject>();
    [SerializeField]
    private GameObject mSkillInfoPannel;
    [SerializeField]
    private Text mSkillInfoNameText;
    [SerializeField]
    private Text mSkillInfoTypeText;
    [SerializeField]
    private Text mSkillInfoCoolTimeText;
    [SerializeField]
    private Text mSkillInfoDescText;
    private bool mIsGSkillSelect = false;
    private bool mIsUSkillSelect = false;
    [SerializeField]
    private GameObject mMinWeaponImage;
    [SerializeField]
    private GameObject mMaxWeaponImage;
    [SerializeField]
    private GameObject mMinCostumeImage;
    [SerializeField]
    private GameObject mMaxCostumeImage;
    [SerializeField]
    private List<GameObject> mSelectWeaponList;
    [SerializeField]
    private List<GameObject> mSelectCostumeList;
    [SerializeField]
    private List<GameObject> mWeaponButton = new List<GameObject>();
    [SerializeField]
    private List<GameObject> mCostumeButton = new List<GameObject>();
    private bool mIsSelectWeapon = false;
    private bool mIsSelectCostume = false;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GamePause();
        mIsPause = false;
        mIsOption = false;
        mIsGiveup = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region method
    /*
     *  Todo : ����ٺ��� ����� ��ɵ��̿��� ���Ŀ� �����丵�ʿ�
     */
    public void ClickedGamePause()
    {
        // �Ͻ����� ���¿��� Ŭ��
        if (mIsPause)
        {
            // ����
            GameRestart();
            mPausePannel.SetActive(false);
        }
        // �������� ���¿��� Ŭ��
        else
        {
            // �Ͻ�����
            GamePause();
            mPausePannel.SetActive(true);
        }
        mIsPause = !mIsPause;
    }
    public void ClickedOption()
    {
        // �ɼ�â�� ���� ����
        if (mIsOption)
        {
            mOptionPannel.SetActive(false);
            mExitBtn.SetActive(false);
        }
        // �ɼ�â�� ���� ����
        else
        {
            mOptionPannel.SetActive(true);
            mExitBtn.SetActive(true);
        }
        mIsOption = !mIsOption;
    }
    public void ClickedGiveup()
    {
        // �ɼ�â�� ���� ����
        if (mIsGiveup)
        {
            mGiveupPannel.SetActive(false);
        }
        // �ɼ�â�� ���� ����
        else
        {
            mGiveupPannel.SetActive(true);
        }
        mIsGiveup = !mIsGiveup;
    }
    public void StatusSelectPannelOn()
    {
        // �Ͻ� ����
        GamePause();
        // TO-DO : �� ���� ���� ���� �ɷ�ġ ���� �� �����ϰ� �����ϰ� ����
        mFirstSelectText.text = LevelUpStatusManager.Instance.SelectStatus(1);
        mSecondSelectText.text = LevelUpStatusManager.Instance.SelectStatus(2);
        mThirdSelectText.text = LevelUpStatusManager.Instance.SelectStatus(3);
        mStatusSelectPannel.SetActive(true);
    }
    public void ClickedSelectStatus(int _num)
    {
        LevelUpStatusManager.Instance.SelectToStat(_num);

        // ���� �簳
        GameRestart();
        mStatusSelectPannel.SetActive(false);
    }
    public void GameOverPannelOn()
    {
        // �Ͻ� ����
        GamePause();

        // ���ӿ��� �г� ����
        mGameOverPannel.SetActive(true);
    }
    public void ClickGameReload()
    {
        // �� ���ε�
        // ���� ������ init
        SpawnManager.Instance.init();
        Projectile.init();
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    private void GamePause()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        mPauseBtn.SetActive(false);
        mBackGroundPannel.SetActive(true);
    }
    private void GameRestart()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        mPauseBtn.SetActive(true);
        mBackGroundPannel.SetActive(false);
    }
    public void ClickWeaponSelect(string _type)
    {
        mIsGSkillSelect = false;
        mIsUSkillSelect = false;
        mIsSelectCostume = false;
        mIsSelectWeapon = false;
        for (int i = 0; i < mGeneralSkillBtn.Count; i++)
        {
            mGeneralSkillBtn[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        for (int i = 0; i < mUltimateSkillBtn.Count; i++)
        {
            mUltimateSkillBtn[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        for (int i = 0; i < mWeaponButton.Count; i++)
        {
            mWeaponButton[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        for (int i = 0; i < mCostumeButton.Count; i++)
        {
            mCostumeButton[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        mSelectWeaponList = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>().FindWepaonList(_type);
        mSelectCostumeList = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>().FindCostumeList(_type);

        mMinWeaponImage.GetComponent<Image>().sprite = mSelectWeaponList[0].GetComponent<SpriteRenderer>().sprite;
        mMaxWeaponImage.GetComponent<Image>().sprite = mSelectWeaponList[4].GetComponent<SpriteRenderer>().sprite;

        mMinCostumeImage.GetComponent<Image>().sprite 
            = mSelectCostumeList.Find((item) => item.name == "cstbgstswsp01").GetComponent<SpriteRenderer>().sprite;
        mMaxCostumeImage.GetComponent<Image>().sprite 
            = mSelectCostumeList.Find((item) => item.name == "cstbgstswsp02").GetComponent<SpriteRenderer>().sprite;
        mBaseSkill = SkillManager.Instance.FindBaseSkill(_type);
        mGeneralSkill = SkillManager.Instance.FindGeneralSkill(_type);
        mUltimateSkill = SkillManager.Instance.FindUltimateSkill(_type);

        // ��ų������ ���� �Ǵ� ��
        for (int i = 0; i < mGeneralSkillBtn.Count; i++)
        {
            Sprite icon = Resources.Load<Sprite>("UI/SkillIcon/" + mGeneralSkill[i].GetComponent<Skill>().name);
            mGeneralSkillBtn[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
        }
        for (int i = 0; i < mUltimateSkillBtn.Count; i++)
        {
            Sprite icon = Resources.Load<Sprite>("UI/SkillIcon/" + mUltimateSkill[i].GetComponent<Skill>().name);
            mUltimateSkillBtn[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
        }
    }

    public void ClickWeaponButton(string _type)
    {
        int idx = int.Parse(_type.Substring(0, 1));
        int buttonidx = int.Parse(_type.Substring(1, 1));
        EquipmentManager.Instance.ChangeWeapon(mSelectWeaponList[idx].name);
        for(int i = 0; i < mWeaponButton.Count; i++)
        {
            if (i == buttonidx)
                mWeaponButton[i].transform.GetChild(1).gameObject.SetActive(true);
            else
               mWeaponButton[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        mIsSelectWeapon = true;
    }
    public void ClickComstumeButton(string _name)
    {
        int buttonidx = int.Parse(_name.Substring(12)) -  1;
        EquipmentManager.Instance.ChangeCostume(_name);
        for (int i = 0; i < mCostumeButton.Count; i++)
        {
            if (i == buttonidx)
                mCostumeButton[i].transform.GetChild(1).gameObject.SetActive(true);
            else
                mCostumeButton[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        mIsSelectCostume = true;
    }
    public void OverSkillSelectBtn(string _type)
    {
        string type = _type.Substring(0, 1);
        int num = int.Parse(_type.Substring(1, 1));
        switch (type)
        {
            case "G":
                SettingInfoPannel(mGeneralSkill, num);
                break;
            case "U":
                SettingInfoPannel(mUltimateSkill, num);
                break;
        }
    }
    public void OutSKillSelectBtn()
    {
        mSkillInfoPannel.SetActive(false);
    }
    private void SettingInfoPannel(List<GameObject> _skillList, int _num)
    {
        if(_skillList.Count > 0)
        {
            mSkillInfoNameText.text = _skillList[_num].GetComponent<Skill>().Spec.Name;
            mSkillInfoTypeText.text = _skillList[_num].GetComponent<Skill>().Spec.TypeName;
            mSkillInfoCoolTimeText.text = "";
            for (int i = 0; i < _skillList[_num].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
            {
                mSkillInfoCoolTimeText.text += "[" + _skillList[_num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
            }
            mSkillInfoDescText.text = _skillList[_num].GetComponent<Skill>().Spec.Desc;

            mSkillInfoPannel.SetActive(true);
        }
    }
    public void ClickSkillSelectBtn(string _type)
    {
        string type = _type.Substring(0, 1);
        int num = int.Parse(_type.Substring(1, 1));
        switch (type)
        {
            case "G":
                GameObject.Find("PlayerObject").GetComponent<PlayerAttack>().CurrentGeneralSkill = mGeneralSkill[num].GetComponent<Skill>();
                for(int i = 0; i < mGeneralSkillBtn.Count; i++)
                {
                    if (i == num)
                        mGeneralSkillBtn[i].transform.GetChild(1).gameObject.SetActive(true);
                    else
                        mGeneralSkillBtn[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                mIsGSkillSelect = true;
                break;
            case "U":
                GameObject.Find("PlayerObject").GetComponent<PlayerAttack>().CurrentUltimateSkill = mUltimateSkill[num].GetComponent<Skill>();
                for (int i = 0; i < mUltimateSkill.Count; i++)
                {
                    if (i == num)
                        mUltimateSkillBtn[i].transform.GetChild(1).gameObject.SetActive(true);
                    else
                        mUltimateSkillBtn[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                mIsUSkillSelect = true;
                break;
        }
    }
    public void ClickGameStart()
    {
        if(mIsGSkillSelect && mIsUSkillSelect && mIsSelectWeapon && mIsSelectCostume)
        {
            GameObject.Find("PlayerObject").GetComponent<PlayerAttack>().CurrentBaseSkill = mBaseSkill.GetComponent<Skill>();
            // �÷��̸Ŵ������� ��ŸƮ APIȣ��
            PlayerManager.Instance.SettingGameStart();
            GameRestart();
            mTestLobbyPannel.SetActive(false);
        }
    }
    #endregion
}
