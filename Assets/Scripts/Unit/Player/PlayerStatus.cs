using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : IStatus
{

    public Text hpUI;


    // Start is called before the first frame update
    void Start()
    {
        //TO-DO : �ӽ÷� ü���� 100���� ����. ��ȹ �������� Ȯ�� �� ���� �ʿ�
        mHp = 100;
        hpUI.text = "User HP : " + Hp;
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * TO-DO : HP�� �ް��� Delegate�� ���� ���߿� ������� UIManager�� ������ ü�»��¸� ��������
         */
         

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

    public override int Hp
    {
        /*
         *  TO-DO :player Attack���� �־ ����ȭ�� �Ǵ��� Ȯ���ʿ�
         */
        set {
            mHp = Mathf.Max(0, value);
            hpUI.text = "User HP : " + mHp;
        }
    }
}