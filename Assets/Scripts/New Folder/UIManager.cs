using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion
}
