using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class UIManager : SingleToneMaker<UIManager>
{
    #region variables
    [Header("일시정지")]
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

    [Header("옵션")]
    [SerializeField]
    private bool mIsOption;
    [SerializeField]
    private GameObject mOptionPannel;
    [SerializeField]
    private GameObject mExitBtn;

    [Header("전투포기")]
    [SerializeField]
    private bool mIsGiveup;
    [SerializeField]
    private GameObject mGiveupPannel;

    [Header("능력치선택창")]
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

    [Header("게임오버")]
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

    [Header("테스트 로비")]
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


    [Header("스킬선택창")]
    [SerializeField]
    private GameObject mSkillSelectPannel;

    [Header("엔딩패널")]
    [SerializeField]
    private GameObject mFirstEndingPannel;
    [SerializeField]
    private GameObject mSecondEndingPannel;

    [Header("크래딧")]
    [SerializeField]
    private GameObject mCreditPannel;
    [SerializeField]
    private bool mIsOpenCreditPannel;
    [SerializeField]
    private int mCurrentNamePage;

    [Header("경고창")]
    [SerializeField]
    private GameObject mAlertEnterPannel;
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
            mGamePlayTimeText.text = (int)mGamePlayTime + "초";
        }
    }

    #region method
    private void ClickSound(LobbyMusicManager.AudioType _type) =>
        MusicManager.Instance.ClickUISound(_type);
    /*
     *  Todo : 만들다보니 비슷한 기능들이여서 추후에 리팩토링필요
     */
    public void OpenAlertEnterPannel(string _desc)
    {
        ClickSound(LobbyMusicManager.AudioType.Error);
        mAlertEnterPannel.transform.GetChild(2).GetComponent<Text>().text = _desc;
        mAlertEnterPannel.SetActive(true);
    }
    public void CloseAlertEnterPannel()
    {
        mAlertEnterPannel.SetActive(false);
    }
    public void ClickedGamePause()
    {
        // 일시정지 상태에서 클릭
        if (mIsPause)
        {
            // 진행
            ClickSound(LobbyMusicManager.AudioType.Cancel);
            GameRestart();
            mPausePannel.SetActive(false);
        }
        // 진행중인 상태에서 클릭
        else
        {
            // 일시정지
            ClickSound(LobbyMusicManager.AudioType.Choice);
            GamePause();
            mPausePannel.SetActive(true);
        }
        mIsPause = !mIsPause;
    }
    public void ClickedOption()
    {
        // 옵션창이 켜진 상태
        if (mIsOption)
        {
            ClickSound(LobbyMusicManager.AudioType.Cancel);
            mOptionPannel.SetActive(false);
            mExitBtn.SetActive(false);
        }
        // 옵션창이 꺼진 상태
        else
        {
            ClickSound(LobbyMusicManager.AudioType.Choice);
            mOptionPannel.SetActive(true);
            mExitBtn.SetActive(true);
        }
        mIsOption = !mIsOption;
    }
    public void ClickedGiveup()
    {
        // 옵션창이 켜진 상태
        if (mIsGiveup)
        {
            ClickSound(LobbyMusicManager.AudioType.Cancel);
            mGiveupPannel.SetActive(false);
        }
        // 옵션창이 꺼진 상태
        else
        {
            ClickSound(LobbyMusicManager.AudioType.Choice);
            mGiveupPannel.SetActive(true);
        }
        mIsGiveup = !mIsGiveup;
    }
    public void StatusSelectPannelOn()
    {
        // 일시 정지
        GamePause();
        ClickSound(LobbyMusicManager.AudioType.LevelUp);
        // TO-DO : 각 선택 섹션 별로 능력치 저장 후 랜덤하게 등장하게 구현
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
        ClickSound(LobbyMusicManager.AudioType.Choice);
        // 게임 재개
        GameRestart();
        mStatusSelectPannel.SetActive(false);
        mPausePannel.SetActive(false);
    }
    public void GameOverPannelOn()
    {
        // 일시 정지
        GamePause();

        // 게임오버 패널 오픈
        mGameOverPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGameOverPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "초";
        mGameOverPannel.SetActive(true);
    }
    public void GameOverFirstResurrectionPannelOn()
    {
        // 일시 정지
        GamePause();

        // 게임오버 패널 오픈
        mGaneOverFirstResurrectionPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = 
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGaneOverFirstResurrectionPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "초";
        // 광고 패스 구매한 경우
        if (mIsAdSkip)
        {
            mGaneOverFirstResurrectionPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text =
                "무료 부활";
        }
        // 미구매
        else
        {
            mGaneOverFirstResurrectionPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text =
                "광고 시청 후 무료 부활";
        }
        mGaneOverFirstResurrectionPannel.SetActive(true);
    }
    public void GameOverSecondResurrectionPannelOn()
    {
        // 일시 정지
        GamePause();

        // 게임오버 패널 오픈
        mGaneOverSecondResurrectionPannel.transform.GetChild(9).GetChild(1).GetComponent<Text>().text =
            PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond.ToString() + "개";
        mGaneOverSecondResurrectionPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
    PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold.ToString() + "g";
        mGaneOverSecondResurrectionPannel.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            ((int)(mGamePlayTime)).ToString() + "초";
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
        // TODO : 광고 시청 후 부활 가능
        // 광고 패스 구매 시
        if (PlayerManager.Instance.IsAdsPass)
        {
            Ressureection(mGaneOverFirstResurrectionPannel);
        }
        // 미구매시
        else
        {
            // 광고 재생 후 보상형으로 부활
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
            // 소지 다이아 부족
            mGaneOverSecondResurrectionPannel.transform.GetChild(6).gameObject.SetActive(true);
        }
    }
    public void ClickGameReload()
    {
        if (MapManager.Instance.CurrentMapType != MapManager.MapType.BossRelay)
        {
            if (SpawnManager.Instance.WaveCount >= 4)
            {
                // 첫번째 엔딩 출력
                mFirstEndingPannel.SetActive(true);
            }
            else
            {
                ClickRealGameReload();
            }
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
        // 스킬아이콘 변경 되는 곳
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
        ClickSound(LobbyMusicManager.AudioType.Choice);
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
                        coolTime += "[" + mGeneralSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "초]";
                    }
                    mSkillSelectPannel.transform.GetChild(6).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text =
                        mGeneralSkill[num].GetComponent<Skill>().Spec.EquipDesc + "\n" + "쿨타임 : " + coolTime;
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
                        coolTime += "[" + mUltimateSkill[num].GetComponent<Skill>().Spec.getSkillCoolTime()[i] + "초]";
                    }
                    mSkillSelectPannel.transform.GetChild(7).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text =
                        mUltimateSkill[num].GetComponent<Skill>().Spec.EquipDesc + "\n" + "쿨타임 : " + coolTime;
                }
                break;
        }
    }
    public void ClickGameStart()
    {
        if(mIsGSkillSelect)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentBaseSkill = mBaseSkill.GetComponent<Skill>();
            PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentDodgeSkill = mDodgeSkill.GetComponent<Skill>();
            // 플레이매니저에서 스타트 API호출
            PlayerManager.Instance.SettingGameStart();
            //GameRestart();
            mSkillSelectPannel.SetActive(false);
        }
        else
        {
            OpenAlertEnterPannel("최소 한개의 스킬을 선택하세요.");
        }
    }
    public void ClickCreditPannel()
    {
        mIsOpenCreditPannel = !mIsOpenCreditPannel;
        if (mIsOpenCreditPannel)
            ClickSound(LobbyMusicManager.AudioType.Choice);
        else
            ClickSound(LobbyMusicManager.AudioType.Cancel);
        mCreditPannel.SetActive(mIsOpenCreditPannel);
    }
    public void ClickNamePannel(bool _isOn)
    {
        ClickSound(LobbyMusicManager.AudioType.Choice);
        mCreditPannel.transform.GetChild(0).GetChild(0).gameObject.SetActive(_isOn);
        mCreditPannel.transform.GetChild(2).gameObject.SetActive(_isOn);
        mCreditPannel.transform.GetChild(1).GetChild(0).gameObject.SetActive(!_isOn);
        mCreditPannel.transform.GetChild(3).gameObject.SetActive(!_isOn);
    }
    public void ClickNextCreditNamePannel(bool _isNext)
    {
        if (_isNext)
            mCurrentNamePage++;
        else
            mCurrentNamePage--;
        switch (mCurrentNamePage)
        {
            case 0:
                mCreditPannel.transform.GetChild(2).GetChild(4).gameObject.SetActive(false);
                break;
            case 1:
                mCreditPannel.transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
                mCreditPannel.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
                break;
            case 2:
                mCreditPannel.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                break;
        }
        for (int i = 0; i < 3; i++)
            mCreditPannel.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
        mCreditPannel.transform.GetChild(2).GetChild(mCurrentNamePage).gameObject.SetActive(true);
    }

    // 테스트용
    public void ClickMapSelectBtn()
    {
        int enumCnt = Enum.GetValues(typeof(MapManager.MapType)).Length;
        int nextMap = ((int)MapManager.Instance.CurrentMapType + 1) % (enumCnt - 1);
        MapManager.Instance.CurrentMapType = (MapManager.MapType)Enum.Parse(typeof(MapManager.MapType), nextMap.ToString());
        mMapText.text = MapManager.Instance.CurrentMapType.ToString();
    }
    #endregion
}
