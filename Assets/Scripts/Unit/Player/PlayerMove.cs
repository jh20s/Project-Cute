using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IMove
{
    private bool DEBUG = false;

    private Animator mAnim;
    public Animator MAnim
    {
        get { return mAnim; }
        set { mAnim = value; }
    }

    [SerializeField]
    private VertualJoyStick mJoyStick;

    public struct Node
    {
        public bool isMove;
        public int value;
    }

    private Dictionary<Vector3, Node> mBfsMap = new Dictionary<Vector3, Node>();
    public Dictionary<Vector3, Node> BfsMap
    {
        get { return mBfsMap; }
    }

    private Vector3[] mVectorDir = new Vector3[8] { Vector3.up, Vector3.down, Vector3.left, Vector3.right,
            Vector3.up+ Vector3.left, Vector3.up+ Vector3.right, Vector3.down+ Vector3.left, Vector3.down+ Vector3.right};
    private float raydist = 0.49f;
    private Vector3 mRayPosition;


    void Awake()
    {
        mAnim = transform.GetChild(1).GetComponent<Animator>();
        mJoyStick = GameObject.Find("Canvas").transform.Find("JoyStick").GetComponent<VertualJoyStick>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();

        MovingStartegy();
    }


    protected override void UpdateMove()
    {
        if (mMoveable)
        {
            //TO-DO : �ڵ��� Ű�Է����� ��ȯ �ʿ�
            float h = mJoyStick.GetHorizontalValue();
            float v = mJoyStick.GetVerticalValue();
            if(h == 0 && v == 0)
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }

            //TO-DO : �̵������ ��� �̷������?
            // �¿� ����
            if (h < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (h > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            // ��ȭ���� ������ �ִϸ��̼� ���
            if (h != 0 || v != 0)
            {
                mAnim.SetFloat("RunState", 0.5f);
                mAnim.SetBool("Run", true);
            }
            else
            {
                mAnim.SetFloat("RunState", 0f);
                mAnim.SetBool("Run", false);
            }
            mDir = new Vector3(h, v, 0).normalized;
        }
    }

    private void FixedUpdate()
    {
        // ��� ���������� FixedUpdate����
        if(mMoveable)
            transform.Translate(mDir * mSpeed * Time.fixedDeltaTime);
    }

    public override void StopStiffTime(float _time)
    {
        base.StopStiffTime(_time);
        StartCoroutine(CoStiffAnimation(_time));
    }

    IEnumerator CoStiffAnimation(float _time)
    {
        mAnim.SetFloat("RunState", 1f);
        yield return new WaitForSeconds(_time);
        mAnim.SetFloat("RunState", 0f);
    }

    private void MovingStartegy()
    {
        mBfsMap.Clear();
        mBfsMap = new Dictionary<Vector3, Node>();
        MakeMap();
        GetBFS();
    }

    //MakeMap�� ���� ������� �ʿ��� BFS�� ���� �������� �ִܰŸ��� ���س��´�.
    //���Ͱ� ������ BFS���� Ȯ���Ͽ� 8���⿡�� �������� ���� ����� ��ġ�� üũ�Ҽ� ����
    private void GetBFS()
    {
        Dictionary<Vector3, bool> check = new Dictionary<Vector3, bool>();
        Queue<Vector3> q = new Queue<Vector3>();
        Vector3 startPos = Vector3Int.FloorToInt(transform.position);
        startPos.x += 0.5f;
        startPos.y += 0.5f;
        Node startnode = new Node();
        startnode.isMove = false;
        startnode.value = 0;

        q.Enqueue(startPos);
        mBfsMap[startPos] = startnode;
        while (q.Count != 0)
        {
            Vector3 nowPosition = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Vector3 nextPosition = nowPosition + mVectorDir[i];
                if (mBfsMap.ContainsKey(nextPosition))
                {
                    if (mBfsMap[nextPosition].isMove && !check.ContainsKey(nextPosition))
                    {
                        check[nextPosition] = true;
                        Node nextNode = mBfsMap[nextPosition];
                        nextNode.value = mBfsMap[nowPosition].value + 1;
                        mBfsMap[nextPosition] = nextNode;
                        q.Enqueue(nextPosition);
                    }
                }
            }
        }
    }

    //RayCast�� ��� �������� Sizeũ���� Vector3�� key�� ���� ��ųʸ��� �����.
    private void MakeMap()
    {
        //TO-DO frame drop�� ���� �� �ȿ��� ��� ���Ͱ� �Ѿƿ����� size �����ʿ�
        int Size = 25;
        int count = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Node node = new Node();
                //node.isMove = CustomRayCastManager.Instance.NomarlizeMoveableWithRay(transform.position, Size / 2 - i, Size / 2 - j, 0.49f, 0.49f, true, ref mRayPosition); 
                node.isMove = MoveableWithRay(Size / 2 - i, Size / 2 - j);
                node.value = 99999;
                mBfsMap[mRayPosition] = node;
                if (DEBUG)
                {
                    if (!node.isMove)
                    {
                        count++;
                    }
                }
            }
        }
        if (DEBUG)
        {
            Debug.Log("��ֹ� ����" + count);
            Debug.Log("MakeMap ���� mBfsMap��ũ��" + mBfsMap.Count);
        }
    }

    //object��ġ���� x,y�Ÿ���ŭ ���������� ��ֹ��� �ִ��� Ȯ��
    //Ÿ���� ���߾ӿ��� rayCaset�� �߻��Ͽ� ��ֹ��� �ִ��� üũ
    //��ֹ��� ������ true , ������ false�� ����
    private bool MoveableWithRay(int x, int y)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x += x;
        targetPosition.y += y;
        float distance = Vector3.Distance(targetPosition, transform.position);


        Vector3 dir = (targetPosition - transform.position).normalized * distance;
        Vector2 rayTargetPosition = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

        //rayTargetPosition�� ��ġ�� tile�� �Ѱ���� ��ġ������
        rayTargetPosition = Vector2Int.FloorToInt(rayTargetPosition);
        rayTargetPosition.x += 0.5f;
        rayTargetPosition.y += 0.5f;
        mRayPosition = rayTargetPosition;

        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D ray = Physics2D.Raycast(rayTargetPosition, mVectorDir[i], raydist, LayerMask.GetMask("Tilemap"));
            if (ray.collider != null)
            {
                if (DEBUG)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Vector3 debugVector = mVectorDir[j] * raydist;
                        Debug.DrawRay(new Vector2(rayTargetPosition.x, rayTargetPosition.y), debugVector, Color.red);
                    }
                }
                return false;
            }
        }
        return true;

    }

}
