using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : IUI
{

    [SerializeField]
    private Transform mLevelBar;
    [SerializeField]
    private Transform mExpBar;
    [SerializeField]
    private Text mGoldText;
    [SerializeField]
    private Text mPauseLevelText;

    

    void Awake()
    {
        gameObject.GetComponent<PlayerEventHandler>().registerLevelObserver(RegisterLevelObserver);
        gameObject.GetComponent<PlayerEventHandler>().registerHpObserver(RegisterHpObserver);
        gameObject.GetComponent<PlayerEventHandler>().registerExpObserver(RegisterExpObserver);
        gameObject.GetComponent<PlayerEventHandler>().registerGoldObserver(RegisterGoldObserver);
        mStatusObject = GameObject.Find("PlayerStatusObject");
        mLevelBar = mStatusObject.transform.Find("LevelBar");
        mHpBar = mStatusObject.transform.Find("HpBar");
        mExpBar = mStatusObject.transform.Find("ExpBar");
        mGoldText = GameObject.Find("Gold").transform.GetChild(2).GetComponent<Text>();
        mPauseLevelText = GameObject.Find("Canvas").transform.Find("PausePannel").transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterGoldObserver(int _gold)
    {
        mGoldText.text = _gold + " gold";
    }
    private void RegisterLevelObserver(int _level)
    {

        mLevelBar.transform.Find("LevelText").GetComponent<Text>().text = "Lv." + _level;
        mPauseLevelText.text = _level.ToString();
        // ������ �� ���ݼ���â �˾��ϴ� API ȣ��
        // TO-DO : UIManager�� �̺�Ʈ ��� �ϱ�
        if (_level != 1)
            UIManager.Instance.StatusSelectPannelOn();
    }

    private void RegisterExpObserver(int _exp)
    {
        int playerExp = gameObject.GetComponent<PlayerStatus>().PlayerExp;
        int playerExpMax = gameObject.GetComponent<PlayerStatus>().PlayerMaxExp;
        mExpBar.transform.Find("Exp").GetComponent<Image>().fillAmount = ((float)playerExp / (float)playerExpMax);
        mExpBar.transform.Find("ExpText").GetComponent<Text>().text = _exp.ToString()+"/"+gameObject.GetComponent<PlayerStatus>().PlayerMaxExp;
    }

    private void RegisterHpObserver(int _hp, GameObject _obj)
    {
        int hp = gameObject.GetComponent<PlayerStatus>().Hp;
        int maxHp = gameObject.GetComponent<PlayerStatus>().MaxHP;
        mHpBar.transform.Find("Hp").GetComponent<Image>().fillAmount = ((float)hp / (float)maxHp);
        mHpBar.transform.Find("HpText").GetComponent<Text>().text = _hp.ToString();

        // �÷��̾� ����� ���ӿ��� UI �˾� APIȣ��
        // TO-DO : UIManager�� �̺�Ʈ ��� �ϱ�
        if (_hp <= 0)
        {
            if (GetComponent<PlayerStatus>().IsFirstDie)
            {
                // ��Ȱ ������ �г�
                UIManager.Instance.GameOverResurrectionPannelOn();
            }
            else
            {
                // ��Ȱ �Ұ����� �г�
                UIManager.Instance.GameOverPannelOn();
            }
        }
            
    }

}
