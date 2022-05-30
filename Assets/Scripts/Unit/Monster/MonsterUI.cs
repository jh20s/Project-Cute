using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterUI : IUI
{
    private string ResourceString = "UI\\UIAsset\\";
    private string BossIconName1 = "Boss_Icon";
    private string BossIconName2 = "Boss_Icon02";
    private string Icon;
    // Start is called before the first frame update
    private void OnEnable()
    {
        mStatusObject = GameObject.Find("MonsterStatusObject");
        mHpBar = mStatusObject.transform.Find("HpBar");
        gameObject.GetComponent<MonsterEventHandler>().registerHpObserver(RegisterHpObserver);
    }

    private void RegisterHpObserver(int _hp, GameObject _obj)
    {
        int hp = gameObject.GetComponent<MonsterStatus>().Hp;
        int maxHp = gameObject.GetComponent<MonsterStatus>().MaxHP;
        mHpBar.transform.Find("Hp").GetComponent<Image>().fillAmount = ((float)hp / (float)maxHp);
        mHpBar.transform.Find("HpText").GetComponent<Text>().text = _hp.ToString();
        mHpBar.gameObject.SetActive(true);

        //TO-DO 몬스터 이미지가 현재 2가지 이외에 없어서 하드코딩되어있는상태
        //나중에 몬스터마다 이미지를 관리하게된다면 CSV로 관리가 필요
        ChangeMonsterImage(gameObject.GetComponent<MonsterStatus>().IsBerserker);


        if (hp <= 0)
        {
            mHpBar.gameObject.SetActive(false); ;
        }
    }

    public void ChangeMonsterImage(bool _isBerserker)
    {
        //if (gameObject.GetComponent<MonsterStatus>().IsBerserker)
        if(_isBerserker)
        {
            Icon = BossIconName2;
        }
        else
        {
            Icon = BossIconName1;
        }
        mHpBar.transform.Find("MonsterImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(ResourceString + Icon);
    }
}
