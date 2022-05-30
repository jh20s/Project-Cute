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

    public void RegisterGoldObserver(int _gold)
    {
        mGoldText.text = _gold + "g";
        MusicManager.Instance.ClickUISound(LobbyMusicManager.AudioType.Purchase);
    }
    private void RegisterLevelObserver(int _level)
    { 
        mLevelBar.transform.Find("LevelText").GetComponent<Text>().text = "Lv." + _level;
        mPauseLevelText.text = _level.ToString();
    }

    private void RegisterExpObserver()
    {
        int playerExp = gameObject.GetComponent<PlayerStatus>().PlayerExp;
        int playerExpMax = gameObject.GetComponent<PlayerStatus>().PlayerMaxExp;
        mExpBar.transform.Find("Exp").GetComponent<Image>().fillAmount = ((float)playerExp / (float)playerExpMax);
        mExpBar.transform.Find("ExpText").GetComponent<Text>().text = playerExp.ToString()+"/"+ playerExpMax.ToString();
    }

    private void RegisterHpObserver(int _hp, GameObject _obj)
    {
        int hp = gameObject.GetComponent<PlayerStatus>().Hp;
        int maxHp = gameObject.GetComponent<PlayerStatus>().MaxHP;
        mHpBar.transform.Find("Hp").GetComponent<Image>().fillAmount = ((float)hp / (float)maxHp);
        mHpBar.transform.Find("HpText").GetComponent<Text>().text = _hp.ToString();

        // 플레이어 사망시 게임오버 UI 팝업 API호출
        // TO-DO : UIManager에 이벤트 등록 하기
        if (_hp <= 0)
        {
            if (PlayerManager.Instance.AutoReviveValue > 0)
            {
                PlayerManager.Instance.AutoReviveValueDiscount();
                GetComponent<PlayerStatus>().Resurrection();
                GetComponent<PlayerStatus>().DieCount--;
            }
            else
            {
                switch (GetComponent<PlayerStatus>().DieCount)
                {
                    case 0:
                        UIManager.Instance.GameOverFirstResurrectionPannelOn();
                        break;
                    case 1:
                        // 다이아 부활 가능한 패널
                        UIManager.Instance.GameOverSecondResurrectionPannelOn();
                        break;
                    case 2:
                        // 부활 불가능한 패널
                        UIManager.Instance.GameOverPannelOn();
                        break;
                }
            }

        }
            
    }
}
