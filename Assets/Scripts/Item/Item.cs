using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int mHp;
    public int Hp
    {
        set { mHp = value; }
        get { return mHp; }
    }
    [SerializeField]
    private int mDamage;
    public int Damage
    {
        set { mDamage = value; }
        get { return mDamage; }
    }
    [SerializeField]
    private int mExp;
    [SerializeField]
    private int mGold;
    public int Gold
    {
        set { mGold = value; }
        get { return mGold; }
    }
    [SerializeField]
    float mScale;
    public float Scale
    {
        set {
            mScale = value;
            gameObject.GetComponent<Transform>().localScale = new Vector3(mScale, mScale, mScale);
        }
        get { return mScale; }
    }

    [SerializeField]
    private int mMustDrop;
    public int MustDrop
    {
        set
        {
            mMustDrop = value;
        }
        get { return mMustDrop; }
    }

    [SerializeField]
    private bool mIsMagnetOn;
    [SerializeField]
    private float mMagnetSpeed;
    [SerializeField]
    private float mDistanceStretch;
    [SerializeField]
    private int mMagnetDirection;

    // 플레이어 타겟으로 따라감
    private void FixedUpdate()
    {
        MagnetToPlayer();
    }

    public void SetTarget(bool _on, float _magnetSpeed, float _distanceStretch, int _magnetDirection)
    {
        mIsMagnetOn = _on;
        mMagnetSpeed = _magnetSpeed;
        mDistanceStretch = _distanceStretch;
        mMagnetDirection = _magnetDirection;
    }
    private void MagnetToPlayer()
    {
        if(mIsMagnetOn)
        {
            Vector3 mTarget = PlayerManager.Instance.Player.transform.position;
            Vector3 dir = mTarget - transform.position;
            float distance = Vector2.Distance(mTarget, transform.position); // Item과 Player사이의 거리
            float magnetDistanceStr = (mDistanceStretch / distance) * mMagnetSpeed; // 거리에 따른 가속효과
            //GetComponent<Rigidbody2D>().AddForce(magnetDistanceStr * (dir * mMagnetDirection), ForceMode2D.Force);
            transform.Translate(magnetDistanceStr * dir * Time.fixedDeltaTime);
        }
    }
    private void OnDisable()
    {
        mIsMagnetOn = false;
    }
}
