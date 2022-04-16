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
    private GameObject mStatusPannel;
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
        mProjectileCountText.text = "+" + Projectile.AddProjectilesCount.ToString() + "��";
        mProjectileScaleText.text = "+" + (Projectile.AddScaleCoefficient - 1.0f).ToString() + "%";
        mPassCountText.text = "+" + Projectile.AddPassCount.ToString() + "����";
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
            mStatusPannel.SetActive(false);
        }
        // �������� ���¿��� Ŭ��
        else
        {
            // �Ͻ�����
            GamePause();
            mPausePannel.SetActive(true);
            mStatusPannel.SetActive(true);
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
        mFirstSelectText.text = "�߻�ü ���� ���� +1";
        mSecondSelectText.text = "�߻�ü ���� ���� +10%";
        mThirdSelectText.text = "���� ���� �� ���� +1";
        mStatusSelectPannel.SetActive(true);
    }
    public void ClickedSelectStatus(int _num)
    {
        // ���Ŀ� PlayerStatus�� ���Ͽ� ��ġ ����
        switch (_num)
        {
            case 0:
                ProjectileManager.Instance.AddProjectilesCount(1);
                break;
            case 1:
                ProjectileManager.Instance.AddProjectilesScale(0.1f);
                break;
            case 2:
                ProjectileManager.Instance.AddPassCount(1);
                break;
        }

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
        // ���� ������ ���ε� �Ұ�...
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // �켱 ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    private void GamePause()
    {
        Time.timeScale = 0;
        mPauseBtn.SetActive(false);
        mBackGroundPannel.SetActive(true);
    }
    private void GameRestart()
    {
        Time.timeScale = 1;
        mPauseBtn.SetActive(true);
        mBackGroundPannel.SetActive(false);
    }
    public void ClickWeaponSelect(string _type)
    {
        int ran = Random.Range(0, 5);
        List<GameObject> newWeaponList = EquipmentManager.Instance.FindWepaonList(_type);
        EquipmentManager.Instance.ChangeWeapon(newWeaponList[ran].GetComponent<Weapon>().Spec.Type);
        mBaseSkill = SkillManager.Instance.FindBaseSkill(_type);
        mGeneralSkill = SkillManager.Instance.FindGeneralSkill(_type);
        mUltimateSkill = SkillManager.Instance.FindUltimateSkill(_type);
        for (int i = 0; i < mGeneralSkillBtn.Count; i++)
        {
            mGeneralSkillBtn[i].transform.GetChild(0).GetComponent<Image>().sprite =
                ProjectileManager.
                Instance.allProjectiles[mGeneralSkill[i].GetComponent<Skill>().Spec.getProjectiles()[0]].
                GetComponent<SpriteRenderer>().sprite;
        }
        for (int i = 0; i < mUltimateSkillBtn.Count; i++)
        {
            mUltimateSkillBtn[i].transform.GetChild(0).GetComponent<Image>().sprite =
                ProjectileManager.
                Instance.allProjectiles[mUltimateSkill[i].GetComponent<Skill>().Spec.getProjectiles()[0]].
                GetComponent<SpriteRenderer>().sprite;
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
        if(mIsGSkillSelect && mIsUSkillSelect)
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
