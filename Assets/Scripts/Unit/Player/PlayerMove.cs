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
    [SerializeField]
    private VertualJoyStick mJoyStick;
    void Awake()
    {
        mAnim = transform.GetChild(1).GetComponent<Animator>();
        mJoyStick = GameObject.Find("Canvas").transform.Find("JoyStick").GetComponent<VertualJoyStick>();
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
}
