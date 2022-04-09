using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IMove
{
    private Animator mAnim;
    public Animator MAnim
    {
        get { return mAnim; }
        set { mAnim = value; }
    }
    private bool mMoveable = true;
    public bool MMoveable
    {
        get { return mMoveable; }
        set { mMoveable = value; }
    }
    void Awake()
    {
        mAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }


    protected override void UpdateMove()
    {
        if (mMoveable)
        {
            //TO-DO : �ڵ��� Ű�Է����� ��ȯ �ʿ�
            float h = VertualJoyStick.Instance.GetHorizontalValue();
            float v = VertualJoyStick.Instance.GetVerticalValue();
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

            mDir = new Vector3(h, v, 0);
            transform.Translate(mDir.normalized * mSpeed * Time.deltaTime);
        }
        else
        {
            mAnim.SetFloat("RunState", 0f);
            mAnim.SetBool("Run", false);
        }
    }

}
