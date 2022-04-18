using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region variable
    // �߰� ���� ������
    [SerializeField]
    private static int addPassCount;
    public static int AddPassCount
    {
        get { return addPassCount; }
        set { addPassCount = value; }
    }
    // �߰� �߻�ü ����
    [SerializeField]
    private static int addProjectilesCount;
    public static int AddProjectilesCount
    {
        get { return addProjectilesCount; }
        set { addProjectilesCount = value; }
    }
    // �߰� �߻�ü ����
    [SerializeField]
    private static float addScaleCoefficient = 1f;
    public static float AddScaleCoefficient
    {
        get { return addScaleCoefficient; }
        set { addScaleCoefficient = value; }
    }
    [SerializeField]
    protected int damage;
    public virtual int Damage
    {
        get { return damage; }
        set { damage = value;}
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
    protected bool isActive = false;


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

    //void OnTrrigerEnter2D(Collider2D collision)
    //{
    //    if (gameObject.CompareTag("PlayerProjectile") && collision.gameObject.CompareTag("Enemy"))
    //    {
    //        collision.gameObject.GetComponent<IStatus>().Hp -= damage;
    //        ObjectPoolManager.Instance.DisableGameObject(gameObject);
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("PlayerProjectile") && collision.gameObject.CompareTag("Monster"))
        {
            // ���� ����
            // -1 : ���� ����
            if (isActive) {
                if (Spec.MaxPassCount != -1)
                {
                    currentPassCount++;
                    // ���� ������ �������� ������ ��ġ���� Ŀ���� disable
                    if (currentPassCount > (Spec.MaxPassCount + addPassCount))
                    {
                        setDisable();
                        ObjectPoolManager.Instance.DisableGameObject(gameObject);
                    }
                }
                collision.gameObject.GetComponent<IStatus>().DamageHp = damage;
            }
        }
        else if (gameObject.CompareTag("MonsterProjectile") && collision.gameObject.CompareTag("Player"))
        {
            // TO-DO : �÷��̾ �߻�ü�� �´� ó��
        }
    }

    #region method
    protected abstract void destroySelf();
    protected abstract void launchProjectile();
    public abstract void setEnable(Vector3 _target, Vector3 _player, float _angle);
    public abstract void setDisable();
    #endregion
}

