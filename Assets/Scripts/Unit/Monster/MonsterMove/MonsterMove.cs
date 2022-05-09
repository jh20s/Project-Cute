using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : IMove
{
    private bool DEBUG = true;
    private Vector3[] mVectorDir = new Vector3[8] { Vector3.up, Vector3.down, Vector3.left, Vector3.right,
            Vector3.up+ Vector3.left, Vector3.up+ Vector3.right, Vector3.down+ Vector3.left, Vector3.down+ Vector3.right};

    [SerializeField]
    private GameObject mTarget;

    [SerializeField]
    private bool mIsDie;
    public bool IsDie
    {
        set { mIsDie = value; }
        get { return mIsDie; }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mSpeed = gameObject.GetComponent<IStatus>().MoveSpeed;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<MonsterEventHandler>().registerIsDieObserver(registerMonsterDie);
    }
    

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(PlayerManager.Instance.Player.transform.position, transform.position) < 0.5f)
        {
            MovingPattern1();
        }
        else { 
            //������ ��� BFS�˰��� ���
            if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss || SpawnManager.Instance.WaveCount >=2)
            {
                MovingPattern2();
            }
            else
            {
                MovingPattern1();
            }
        }
    }

    private void FixedUpdate()
    {
        // ��� ���������� FixedUpdate ����
        if (mMoveable && !mIsDie) {
            transform.Translate(mDir * mSpeed * Time.deltaTime);
            int size = gameObject.GetComponent<IStatus>().Size;
            if (gameObject.transform.position.x < PlayerManager.Instance.Player.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-1 * size, size, size);
            }
        }
    }

    public void registerMonsterDie(bool _die, GameObject _obj)
    {
        mIsDie = _die;
    }

    public override void StopStiffTime(float _time)
    {
        base.StopStiffTime(_time);
    }

    //�÷��̾�� ���� ��ü�� ���� ���͹������� �����̴� ��ã�� �˰���
    void MovingPattern1()
    {
        mDir = PlayerManager.Instance.Player.transform.position - transform.position;
        mDir.Normalize();
    }


    //Player�� ������Ʈ�� BFS��带 Ȯ���Ͽ� ���� ����ġ�� ���������� �̵�
    public void MovingPattern2()
    {
        mTarget = PlayerManager.Instance.Player;
        int value = 99999;
        Vector3 dir = Vector3.right;

        //8���⿡ ���� PlayerBfs���� ���� ����ġ�� ���� ������ �̵�
        for (int i = 0; i < 8; i++)
        {
            Vector3 nextPosition = Vector3Int.FloorToInt(transform.position) + mVectorDir[i];
            nextPosition.x += 0.5f;
            nextPosition.y += 0.5f;
            if (!PlayerManager.Instance.Player.GetComponent<PlayerMove>().BfsMap.ContainsKey(nextPosition))
                continue;
            if(PlayerManager.Instance.Player.GetComponent<PlayerMove>().BfsMap[nextPosition].isMove
                    && PlayerManager.Instance.Player.GetComponent<PlayerMove>().BfsMap[nextPosition].value< value)
            {
                value = PlayerManager.Instance.Player.GetComponent<PlayerMove>().BfsMap[nextPosition].value;
                dir = nextPosition;

            }
        }
        mDir = dir - transform.position;
        mDir.Normalize();

        //Player�� BFSŽ�� ���� ���̸�  Moving Pattern1�� ����
        if (value == 99999) {
            MovingPattern1();
        }

    }
}
