using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField]
    private float mMagnetPower;
    [SerializeField]
    private float mMagnetSpeed;
    [SerializeField]
    private float mDistanceStretch; // �Ÿ��� ���� ���� ȿ��
    [SerializeField]
    private int mMagnetDirection; // �η� 1 ô�� -1
    void Start()
    {
        gameObject.GetComponent<PlayerEventHandler>().registerMagnetPowerbserver(RegisterMagentPowerObserver);
        mMagnetSpeed = 1f;
        mDistanceStretch = 4f;
        mMagnetDirection = 1;
    }

    private void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(transform.position, mMagnetPower, LayerMask.GetMask("Item"))) {
            Collider2D[] colArray =  Physics2D.OverlapCircleAll(transform.position, mMagnetPower, LayerMask.GetMask("Item"));
            foreach(Collider2D obj in colArray)
            {
                obj.GetComponent<Item>().SetTarget(true, mMagnetSpeed, mDistanceStretch, mMagnetDirection);
                //obj.GetComponent<Transform>().Translate(mMagnetSpeed * dir * Time.deltaTime);
            }
        }
        
    }


    public void RegisterMagentPowerObserver(float _magnetPower)
    {
        mMagnetPower = _magnetPower + 1f;
    }
}
