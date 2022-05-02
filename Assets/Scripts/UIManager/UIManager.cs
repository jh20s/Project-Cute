using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
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
    [SerializeField]
    private Image mFirstSelectImage;
    [SerializeField]
    private Image mSecondSelectImage;
    [SerializeField]
    private Image mThirdSelectImage;

    [Header("���ӿ���")]
    [SerializeField]
    private GameObject mGameOverPannel;
    [SerializeField]
    private GameObject mGaneOverResurrectionPannel;


    [Header("�׽�Ʈ �κ�")]
    [SerializeField]
    private GameObject mTestLobbyPannel;
    [SerializeField]
    private GameObject mBaseSkill;
    [SerializeField]
    private GameObject mDodgeSkill;
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
    private float mGamePlayTime;
    [SerializeField]
    private Text mGamePlayTimeText;

    [SerializeField]
    private Text mMapText;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GamePause();
        mIsPause = false;
        mIsOption = false;
        mIsGiveup = false;

        mGamePlayTime = 0f;

        mMapText.text = MapManager.Instance.CurrentMapType.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!mIsPause)
        {
            mGamePlayTime += Time.deltaTime;
            mGamePlayTimeText.text = string.Format("{0:N2}", mGamePlayTime);
        }
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

        mFirstSelectImage.sprite = LevelUpStatusManager.Instance.SelectSlotImage(1);
        mSecondSelectImage.sprite = LevelUpStatusManager.Instance.SelectSlotImage(2);
        mThirdSelectImage.sprite = LevelUpStatusManager.Instance.SelectSlotImage(3);

        mStatusSelectPannel.SetActive(true);
        mPausePannel.SetActive(true);
    }
    public void ClickedSelectStatus(int _num)
    {
        LevelUpStatusManager.Instance.SelectToStat(_num);

        // ���� �簳
        GameRestart();
        mStatusSelectPannel.SetActive(false);
        mPausePannel.SetActive(false);
    }
    public void GameOverPannelOn()
    {
        // �Ͻ� ����
        GamePause();

        // ���ӿ��� �г� ����
        mGameOverPannel.SetActive(true);
    }
    public void GameOverResurrectionPannelOn()
    {
        // �Ͻ� ����
        GamePause();

        // ���ӿ��� �г� ����
        mGaneOverResurrectionPannel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text =
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond.ToString() + "��";
        mGaneOverResurrectionPannel.SetActive(true);
    }
    public void ClickResurrectionButton()
    {
        if(PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond >= 30)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond -= 30;
            PlayerManager.Instance.ResurrectionPlayer();
            mGaneOverResurrectionPannel.SetActive(false);
            GameRestart();
        }
        else
        {
            // ���� ���̾� ����
            mGaneOverResurrectionPannel.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void ClickGameReload()
    {
        // �� ���ε�
        // ���� ������ init
        SpawnManager.Instance.init();
        SaveLoadManager.Instance.SaveWarEndData();
        GamePause();
        SceneManager.LoadScene("LobbyScene");
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
    public void SkillSelectUILoad(string _type)
    {
        mBaseSkill = SkillManager.Instance.FindBaseSkill(_type);
        mDodgeSkill = SkillManager.Instance.FindDodgeSkill(_type);
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
            mSkillInfoNameText.text = _skillList[_num].GetComponent<Skill>().Spec.EquipName;
            mSkillInfoTypeText.text = _skillList[_num].GetComponent<Skill>().Spec.TypeName;
            mSkillInfoCoolTimeText.text = "";
            for (int i = 0; i < _skillList[_num].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
            {
                mSkillInfoCoolTimeText.text += "[" + _skillList[_num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
            }
            mSkillInfoDescText.text = _skillList[_num].GetComponent<Skill>().Spec.EquipDesc;

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
    public void ClickMapSelectBtn()
    {
        int enumCnt = Enum.GetValues(typeof(MapManager.MapType)).Length;
        int nextMap = ((int)MapManager.Instance.CurrentMapType + 1) % (enumCnt-1);
        MapManager.Instance.CurrentMapType = (MapManager.MapType)Enum.Parse(typeof(MapManager.MapType), nextMap.ToString());
        mMapText.text = MapManager.Instance.CurrentMapType.ToString();
    }
    public void ClickGameStart()
    {
        if(mIsGSkillSelect && mIsUSkillSelect)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentBaseSkill = mBaseSkill.GetComponent<Skill>();
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentDodgeSkill = mDodgeSkill.GetComponent<Skill>();
            // �÷��̸Ŵ������� ��ŸƮ APIȣ��
            PlayerManager.Instance.SettingGameStart();
            GameRestart();
            mTestLobbyPannel.SetActive(false);
        }
    }
    #endregion
}
