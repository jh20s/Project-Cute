using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMove
{
    // Start is called before the first frame update
    void Start()
    {
        /*
         * TO-DO : MonsterManager�κ��� �о������ ���� �ʿ�
        */
        mSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * TO-DO : ���� ���갡 �پ������� MonsterMoveStrategy Ŭ������ �����
         *         Stragegy������ �����ؼ� �߰��Ұ�
         *         �켱 Player���� �ٰ����� ���¸����� ����.
        */
        MovingPattern1();
    }

    void MovingPattern1()
    {
        GameObject target = GameObject.Find("UnitRoot");
        if (target != null)
        {

            mDir = target.transform.position - transform.position;
            mDir.Normalize();
        }
        transform.position += mDir * mSpeed * Time.deltaTime;
    }
}
