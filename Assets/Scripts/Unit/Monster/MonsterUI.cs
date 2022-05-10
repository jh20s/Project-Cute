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
    void Start()
    {
    }

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

        //TO-DO ���� �̹����� ���� 2���� �̿ܿ� ��� �ϵ��ڵ��Ǿ��ִ»���
        //���߿� ���͸��� �̹����� �����ϰԵȴٸ� CSV�� ������ �ʿ�
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
