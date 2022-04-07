using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{


    #region variable
    // �߰� ���� ������
    private static int addPassCount;
    public static int AddPassCount
    {
        get { return addPassCount; }
        set { addPassCount = value; }
    }
    // �߰� �߻�ü ����
    private static int addProjectilesCount;
    public static int AddProjectilesCount
    {
        get { return addProjectilesCount; }
        set { addProjectilesCount = value; }
    }
    private static float addScaleCoefficient = 1f;
    public static float AddScaleCoefficient
    {
        get { return addScaleCoefficient; }
        set { addScaleCoefficient = value; }
    }
    protected int damage;
    public virtual int Damage
    {
        get { return damage; }
        set { damage = value;}
    }
    [SerializeField]
    protected int currentPassCount;

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
        if (gameObject.CompareTag("PlayerProjectile") && collision.gameObject.CompareTag("Enemy"))
        {
            // ���� ����
            // -1 : ���� ����
            if(Spec.MaxPassCount != -1)
            {
                currentPassCount++;
                // ���� ������ �������� ������ ��ġ���� ���ų� Ŀ���� disable
                if (currentPassCount >= (Spec.MaxPassCount + addPassCount) - 1)
                {
                    currentPassCount = 0;
                    setDisable();
                    ObjectPoolManager.Instance.DisableGameObject(gameObject);
                }
            }
            collision.gameObject.GetComponent<IStatus>().Hp -= damage;
            MessageBoxManager.Instance.createMessageBox(MessageBoxManager.BoxType.MonsterDamage, damage.ToString(), collision.gameObject.transform.position);
        }
    }

    #region method
    protected abstract void destroySelf();
    protected abstract void launchProjectile();
    public abstract void setEnable(Vector3 _target, Vector3 _player, float _angle);
    public abstract void setDisable();
    #endregion
}

