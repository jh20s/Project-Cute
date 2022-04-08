using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IMove
{
    private Animator mAnim;
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
        //TO-DO : �ڵ��� Ű�Է����� ��ȯ �ʿ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        
        //TO-DO : �̵������ ��� �̷������?
        // �¿� ����
        if(h < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (h > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // ��ȭ���� ������ �ִϸ��̼� ���
        if(h != 0 || v != 0)
        {
            mAnim.SetBool("isRun", true);
        }
        else
        {
            mAnim.SetBool("isRun", false);
        }

        mDir = new Vector3(h, v, 0);
        transform.Translate(mDir.normalized * mSpeed * Time.deltaTime);

        //TO-DO : ������ �����ӿ� ���� ī�޶� ����ٴϵ��� ���� �ʿ�
        //Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        //if (pos.x < 0f) pos.x = 0f;
        //if (pos.x > 1f) pos.x = 1f;
        //if (pos.y < 0f) pos.y = 0f;
        //if (pos.y > 1f) pos.y = 1f;
        //transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

}
