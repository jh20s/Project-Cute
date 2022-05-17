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
    public GameObject StatusSelectPannel
    {
        get { return mStatusSelectPannel; }
    }
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
    private GameObject mGaneOverFirstResurrectionPannel;
    [SerializeField]
    private GameObject mGaneOverSecondResurrectionPannel;
    [SerializeField]
    private bool mIsAdSkip;
    public bool IsAdSkip
    {
        get => mIsAdSkip;
        set
        {
            mIsAdSkip = value;
        }
    }

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
    [SerializeField]
    private bool mIsGSkillSelect = false;
    [SerializeField]
    private float mGamePlayTime;
    public float GamePlayerTime => mGamePlayTime;
    [SerializeField]
    private Text mGamePlayTimeText;

    [SerializeField]
    private Text mMapText;


    [Header("��ų����â")]
    [SerializeField]
    private GameObject mSkillSelectPannel;

    [Header("�����г�")]
    [SerializeField]
    private GameObject mFirstEndingPannel;
    [SerializeField]
    private GameObject mSecondEndingPannel;
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
        AdmobManager.Instance.registerEndRewardObserver(RegisterEndRewardObserver);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!mIsPause)
        {
            mGamePlayTime += Time.deltaTime;
            mGamePlayTimeText.text = (int)mGamePlayTime + "��";
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
        mGameOverPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGameOverPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "��";
        mGameOverPannel.SetActive(true);
    }
    public void GameOverFirstResurrectionPannelOn()
    {
        // �Ͻ� ����
        GamePause();

        // ���ӿ��� �г� ����
        mGaneOverFirstResurrectionPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = 
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGaneOverFirstResurrectionPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "��";
        // ���� �н� ������ ���
        if (mIsAdSkip)
        {
            mGaneOverFirstResurrectionPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text =
                "���� ��Ȱ";
        }
        // �̱���
        else
        {
            mGaneOverFirstResurrectionPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text =
                "���� ��û �� ���� ��Ȱ";
        }
        mGaneOverFirstResurrectionPannel.SetActive(true);
    }
    public void GameOverSecondResurrectionPannelOn()
    {
        // �Ͻ� ����
        GamePause();

        // ���ӿ��� �г� ����
        mGaneOverSecondResurrectionPannel.transform.GetChild(9).GetChild(1).GetComponent<Text>().text =
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond.ToString() + "��";
        mGaneOverSecondResurrectionPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
    PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGaneOverSecondResurrectionPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "��";
        mGaneOverSecondResurrectionPannel.SetActive(true);
    }
    private void RegisterEndRewardObserver(bool _isEnd)
    {
        if (_isEnd)
        {
            PlayerManager.Instance.ResurrectionPlayer();
            mGaneOverFirstResurrectionPannel.SetActive(false);
            GameRestart();
        }
    }
    public void Ressureection(GameObject _pannel)
    {
        PlayerManager.Instance.ResurrectionPlayer();
        _pannel.SetActive(false);
        GameRestart();
    }
    public void ClickFirstResurrectionButton()
    {
        // TODO : ���� ��û �� ��Ȱ ����
        // ���� �н� ���� ��
        if (mIsAdSkip)
        {
            Ressureection(mGaneOverFirstResurrectionPannel);
        }
        // �̱��Ž�
        else
        {
            // ���� ��� �� ���������� ��Ȱ
            AdmobManager.Instance.Show();
        }
    }
    public void ClickSecondResurrectionButton()
    {
        if(PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond >= 6)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond -= 6;
            Ressureection(mGaneOverSecondResurrectionPannel);
        }
        else
        {
            // ���� ���̾� ����
            mGaneOverSecondResurrectionPannel.transform.GetChild(6).gameObject.SetActive(true);
        }
    }
    public void ClickGameReload()
    {
        if(SpawnManager.Instance.WaveCount >= 4)
        {
            // ù��° ���� ���
            mFirstEndingPannel.SetActive(true);
        }
        else
        {
            ClickRealGameReload();
        }
    }
    public void ClickRealGameReload()
    {
        SaveLoadManager.Instance.SaveWarEndData();
        SpawnManager.Instance.init();
        GamePause();
        SceneManager.LoadScene("LobbyScene");
    }
    public void OpenSecondEndingPannel()
    {
        GamePause();
        mSecondEndingPannel.SetActive(true);
    }
    private void GamePause()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        mPauseBtn.SetActive(false);
        mBackGroundPannel.SetActive(true);
    }
    public void GameRestart()
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

        mSkillSelectPannel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text
            = PlayerManager.Instance.Player.GetComponent<PlayerStatus>().PlayerCurrentWeapon.Spec.TypeName;
        // ��ų������ ���� �Ǵ� ��
        for (int i = 0; i < mGeneralSkillBtn.Count; i++)
        {
            Sprite icon = Resources.Load<Sprite>("UI/SkillIcon/" + mGeneralSkill[i].GetComponent<Skill>().name);
            mGeneralSkillBtn[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
            if (!mGeneralSkill[i].GetComponent<Skill>().Spec.IsLocked)
            {
                mSkillSelectPannel.transform.GetChild(6).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
                mSkillSelectPannel.transform.GetChild(6).GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
                mGeneralSkillBtn[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                mGeneralSkillBtn[i].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }
                
        }
        for (int i = 0; i < mUltimateSkillBtn.Count; i++)
        {
            Sprite icon = Resources.Load<Sprite>("UI/SkillIcon/" + mUltimateSkill[i].GetComponent<Skill>().name);
            mUltimateSkillBtn[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
            if (!mUltimateSkill[i].GetComponent<Skill>().Spec.IsLocked)
            {
                mSkillSelectPannel.transform.GetChild(7).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
                mSkillSelectPannel.transform.GetChild(7).GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
                mUltimateSkillBtn[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                mUltimateSkillBtn[i].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    public void ClickSkillSelectBtn(string _type)
    {
        string type = _type.Substring(0, 1);
        int num = int.Parse(_type.Substring(1, 1));
        string coolTime = "";
        Sprite icon = null;
        switch (type)
        {
            case "G":
                if (!mGeneralSkill[num].GetComponent<Skill>().Spec.IsLocked)
                {
                    icon = Resources.Load<Sprite>("UI/SkillIcon/" + mGeneralSkill[num].GetComponent<Skill>().name);
                    PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentGeneralSkill = mGeneralSkill[num].GetComponent<Skill>();
                    mSkillSelectPannel.transform.GetChild(6).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
                    mSkillSelectPannel.transform.GetChild(6).GetChild(0).GetComponent<Text>().text 
                        = mGeneralSkill[num].GetComponent<Skill>().Spec.EquipName;
                    for (int i = 0; i < mGeneralSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
                    {
                        coolTime += "[" + mGeneralSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
                    }
                    mSkillSelectPannel.transform.GetChild(6).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text =
                        mGeneralSkill[num].GetComponent<Skill>().Spec.EquipDesc + "\n" + "��Ÿ�� : " + coolTime;
                    mIsGSkillSelect = true;
                }
                break;
            case "U":
                if (!mUltimateSkill[num].GetComponent<Skill>().Spec.IsLocked)
                {
                    icon = Resources.Load<Sprite>("UI/SkillIcon/" + mUltimateSkill[num].GetComponent<Skill>().name);
                    PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentUltimateSkill = mUltimateSkill[num].GetComponent<Skill>();
                    mSkillSelectPannel.transform.GetChild(7).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
                    mSkillSelectPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text 
                        = mUltimateSkill[num].GetComponent<Skill>().Spec.EquipName; ;
                    for (int i = 0; i < mUltimateSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime().Count; i++)
                    {
                        coolTime += "[" + mUltimateSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "��]";
                    }
                    mSkillSelectPannel.transform.GetChild(7).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text =
                        mUltimateSkill[num].GetComponent<Skill>().Spec.EquipDesc + "\n" + "��Ÿ�� : " + coolTime;
                }
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
        if(mIsGSkillSelect)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentBaseSkill = mBaseSkill.GetComponent<Skill>();
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentDodgeSkill = mDodgeSkill.GetComponent<Skill>();
            // �÷��̸Ŵ������� ��ŸƮ APIȣ��
            PlayerManager.Instance.SettingGameStart();
            //GameRestart();
            mSkillSelectPannel.SetActive(false);
        }
        else
        {
            
        }
    }
    #endregion
}
