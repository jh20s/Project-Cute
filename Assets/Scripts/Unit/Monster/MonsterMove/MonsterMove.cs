using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMove
{
    [SerializeField]
    private GameObject mTarget;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //mTarget = GameObject.Find("UnitRoot");
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
        //MovingPattern2();
        
    }
    IEnumerator SetDir()
    {
        if(mDir != Vector3.zero)
        {
            yield return new WaitForSeconds(1f);
            mDir = Vector3.zero;
        }
    }
    //�� ���þ��� distance�θ� �̵��ϴ� ����
    void MovingPattern1()
    {
        mTarget = GameObject.Find("UnitRoot");
        if (mTarget != null)
        {
            
           

            mDir = mTarget.transform.position - transform.position;
            mDir.Normalize();
        }
    }
    //���� ���� 8���⿡���ؼ� ���� ���� ���� ��ȸ�ϴ� �˰���
    public void MovingPattern2()
    {
        if (mDir == Vector3.zero)
        {
            bool[] arr = new bool[8];
            float minDistance = float.MaxValue;
            int guessDir = 0;

            for (int i = 0; i < 8; i++)
            {
                if (MoveableWithRay(i))
                {
                    Vector3 dir = new Vector3(mRayDr[i], mRayDc[i], 0).normalized + gameObject.transform.position;
                    float distance = Vector3.Distance(dir, PlayerManager.Instance.Player.transform.position);
                    if (minDistance > distance)
                    {
                        guessDir = i;
                        minDistance = distance;
                    }
                }
            }
            mDir = new Vector3(mRayDr[guessDir], mRayDc[guessDir], 0);
            mDir.Normalize();
            StartCoroutine(SetDir());
            Debug.DrawRay(transform.position, mDir, Color.red);
        }
    }

    private void FixedUpdate()
    {
        // ��� ���������� FixedUpdate ����
        if(mTarget != null && mMoveable) { 
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
    // ����, ��, �»�
    private int[] mRayDr = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };
    private int[] mRayDc = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };

    public bool MoveableWithRay(int i )
    {
        Vector3 dir = new Vector3(mRayDr[i], mRayDc[i], 0).normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Tilemap"));
        if (ray.collider != null)
        {
            return false;
        } 
        else
            return true;
    }
}
