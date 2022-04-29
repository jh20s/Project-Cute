using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region variable
    [SerializeField]
    protected int damage;
    public virtual int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    [SerializeField]
    protected int currentPassCount;
    public int CurrentPassCount
    {
        get { return currentPassCount; }
        set { currentPassCount = value; }
    }
    [SerializeField]
    protected Vector3 myPos;
    public Vector3 MyPos
    {
        get { return myPos; }
    }

    //���� active�������� üũ
    [SerializeField]
    protected bool mIsActive = false;
    public bool IsActive
    {
        get { return mIsActive; }
        set { mIsActive = value; }
    }

    [SerializeField]
    private float mAttackSpeedCheckTime = 1;

    // �� ���ݴ� ũ��Ƽ�� ���������� üũ
    [SerializeField]
    private bool mIsCriticalDamage;
    public bool IsCriticalDamage
    {
        get { return mIsCriticalDamage; }
        set { mIsCriticalDamage = value; }
    }

    public abstract ProjectileSpec Spec
    {
        get;
        set;
    }
    public abstract Vector3 Target
    {
        get;
        set;
    }
    #endregion
    private void OnDisable()
    {
        mAttackSpeedCheckTime = 1;
    }
    protected virtual void FixedUpdate() 
    {
        if(mIsActive)
            mAttackSpeedCheckTime += Time.fixedDeltaTime;
    }
    // ���ӵ������� ���ؼ� Stay�� �ٲ�
    // ������ 0.02�ʴ����� �������� �ʹ� ���� ���� �켱 0.5�� ������ �ٲ�
    // ��Ʈ���� bool �� ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Player"))
        {
                // ���� ����
            // -1 : ���� ����
            if (mIsActive)
            {
                if (Spec.MaxPassCount != -1)
                {
                    currentPassCount++;
                    // ���� ������ �������� ������ ��ġ���� Ŀ���� disable
                    if (currentPassCount > (Spec.MaxPassCount + 
                        (collision.gameObject.CompareTag("Monster") ? GameObject.Find("PlayerObject").GetComponent<IAttack>().PassCount : 0)))
                    {
                        setDisable();
                        ObjectPoolManager.Instance.DisableGameObject(gameObject);
                    }
                }
                // ���� Ȯ��
                float tmpStiffTime = collision.gameObject.CompareTag("Monster") ? GameObject.Find("PlayerObject").GetComponent<IAttack>().StiffTime : 0f;
                if (Spec.StiffTime + tmpStiffTime > 0)
                {
                    // �⺻������ ���
                    if (Spec.Type == GameObject.Find("PlayerObject").GetComponent<IAttack>().CurrentBaseSkill.Spec.getProjectiles()[0])
                    {
                        collision.GetComponent<IMove>().StopStiffTime(Spec.StiffTime + tmpStiffTime);
                    }
                    // Ÿ ��ų�� ���
                    else
                        collision.GetComponent<IMove>().StopStiffTime(Spec.StiffTime);
                }
            }
        }
    }

    #region method
    protected abstract void launchProjectile();
    public abstract void setEnable(Vector3 _target, Vector3 _player, float _angle);
    public abstract void setDisable();
    public void setSize(Vector3 _size)
    {
        transform.localScale = _size;
    }
    public void setDisableWaitForTime(float _time)
    {
        StartCoroutine(CoDisableWaitForTime(_time));
    }
    IEnumerator CoDisableWaitForTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        if (gameObject.activeInHierarchy)
        {
            setDisable();
            ObjectPoolManager.Instance.DisableGameObject(gameObject);
        }
    }
    #endregion
}

