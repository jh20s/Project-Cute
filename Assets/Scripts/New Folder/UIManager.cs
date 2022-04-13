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
    #endregion
    // Start is called before the first frame update
    void Start()
    {
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
            Time.timeScale = 1;
            mPausePannel.SetActive(false);
            mStatusPannel.SetActive(false);
            mPauseBtn.SetActive(true);
            mBackGroundPannel.SetActive(false);
        }
        // �������� ���¿��� Ŭ��
        else
        {
            // �Ͻ�����
            Time.timeScale = 0;
            mPausePannel.SetActive(true);
            mStatusPannel.SetActive(true);
            mPauseBtn.SetActive(false);
            mBackGroundPannel.SetActive(true);
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
        Time.timeScale = 0;
        mPauseBtn.SetActive(false);
        mBackGroundPannel.SetActive(true);
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
        Time.timeScale = 1;
        mPauseBtn.SetActive(true);
        mBackGroundPannel.SetActive(false);
        mStatusSelectPannel.SetActive(false);
    }

    public void GameOverPannelOn()
    {
        // �Ͻ� ����
        Time.timeScale = 0;
        mPauseBtn.SetActive(false);
        mBackGroundPannel.SetActive(true);

        // ���ӿ��� �г� ����
        mGameOverPannel.SetActive(true);
    }

    public void ClickGameReload()
    {
        // �� ���ε�
        SceneManager.LoadScene(0);
    }
    #endregion
}
