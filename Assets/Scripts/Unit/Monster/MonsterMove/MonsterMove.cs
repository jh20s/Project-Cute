using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMove
{
    private GameObject target;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mSpeed = gameObject.GetComponent<IStatus>().MoveSpeed;
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
        if(target != null && mMoveable) { 
            transform.Translate(mDir * mSpeed * Time.deltaTime);
            int size = gameObject.GetComponent<IStatus>().Size;
            if(gameObject.transform.position.x < GameObject.Find("PlayerObject").transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-1*size, size, size);
            }
        }
    }
    public override void StopStiffTime(float _time)
    {
        base.StopStiffTime(_time);
    }
}
