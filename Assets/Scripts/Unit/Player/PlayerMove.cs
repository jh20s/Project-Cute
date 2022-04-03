using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IMove
{
    // Start is called before the first frame update
    void Start()
    {
        mSpeed = 10f;
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

        mDir.Set(h, v, 0);
        transform.position += mDir * mSpeed * Time.deltaTime;

        //TO-DO : ������ �����ӿ� ���� ī�޶� ����ٴϵ��� ���� �ʿ�
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

}
