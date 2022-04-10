using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : IStatus
{

    public Text hpUI;
    public Text mExpUI;
    private int mPlayerExp;
    private int mPlayerExpMax;

    // Start is called before the first frame update
    void Start()
    {
        //TO-DO : �÷��̾� ���ݵ� �ϵ��ڵ�. csv���� ������ �����ʿ�.
        mHp = 100;
        mPlayerExp = 0;
        mPlayerExpMax = 100;
        hpUI.text = "Player HP : " + Hp;
        mExpUI.text = "Player EXP : " + mPlayerExp + "/" + mPlayerExpMax;
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * TO-DO : HP�� �ް��� Delegate�� ���� ���߿� ������� UIManager�� ������ ü�»��¸� ��������
         */
        hpUI.text = "User HP : " + mHp;
        
        if (Input.GetKey(KeyCode.Z))
        {
            StartCoroutine(InvincibilityCorutine(3f));
        }
        

    }


    public void registerMonsterHp(int _hp, GameObject _obj)
    {
        Debug.Log("registerMonsterHp");
        if (_hp <= 0)
        {
            MonsterManager.MonsterGrade md = _obj.GetComponent<MonsterStatus>().MonsterGrade;
            if(md == MonsterManager.MonsterGrade.Boss)
            {
                mPlayerExp += (int)(0.7 * mPlayerExpMax);
            }
            else if(md == MonsterManager.MonsterGrade.Normal)
            {
                mPlayerExp += 1;
            }
            mExpUI.text = "Player EXP : " + mPlayerExp + "/" + mPlayerExpMax;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    
}
