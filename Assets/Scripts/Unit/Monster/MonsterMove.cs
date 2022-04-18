using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMove
{
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        /*
         * TO-DO : MonsterManager�κ��� �о������ ���� �ʿ�
        */
    }

    // Update is called once per frame
    void Update()
    {
        mSpeed = GetComponent<MonsterStatus>().Speed;
        /*
         * TO-DO : ���� ���갡 �پ������� MonsterMoveStrategy Ŭ������ �����
         *         Stragegy������ �����ؼ� �߰��Ұ�
         *         �켱 Player���� �ٰ����� ���¸����� ����.
        */
        MovingPattern1();
    }

    void MovingPattern1()
    {
        target = GameObject.Find("UnitRoot");
        if (target != null)
        {

            mDir = target.transform.position - transform.position;
            mDir.Normalize();
        }
    }

    private void FixedUpdate()
    {
        // ��� ���������� FixedUpdate ����
        if(target != null)
            transform.Translate(mDir * mSpeed * Time.deltaTime);
    }
}
