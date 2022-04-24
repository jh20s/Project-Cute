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
    public static void init()
    {
        addPassCount = 0;
        addProjectilesCount = 0;
        addScaleCoefficient = 1f;
    }
    // ���ӵ������� ���ؼ� Stay�� �ٲ�
    // ������ 0.02�ʴ����� �������� �ʹ� ���� ���� �켱 0.5�� ������ �ٲ�
    // ��Ʈ���� bool �� ���� ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(gameObject.tag + collision.tag);
        if ((gameObject.CompareTag("PlayerProjectile") && collision.gameObject.CompareTag("Monster"))) { 
            // ���� ����
            // -1 : ���� ����
            if (isActive)
            {
                if (Spec.MaxPassCount != -1)
                {
                    currentPassCount++;
                    // ���� ������ �������� ������ ��ġ���� Ŀ���� disable
                    if (currentPassCount > (Spec.MaxPassCount + GameObject.Find("PlayerObject").GetComponent<IAttack>().PassCount))
                    {
                        setDisable();
                        ObjectPoolManager.Instance.DisableGameObject(gameObject);
                    }
                }
                collision.gameObject.GetComponent<IStatus>().DamageHp = damage;
                // ���� �ð��� ������
                if(Spec.StiffTime + GameObject.Find("PlayerObject").GetComponent<IAttack>().StiffTime > 0)
                {
                    collision.gameObject.GetComponent<IMove>().StopStiffTime(
                        Spec.StiffTime + GameObject.Find("PlayerObject").GetComponent<IAttack>().StiffTime);
                }

                // �˹� ��ġ�� ������
                if(Spec.Knockback > 0)
                {
                    // ������Ÿ���� ����������� �÷��̾� �ݶ��̴� * ��ġ��ŭ �̵�
                    collision.gameObject.transform.Translate(
                        (transform.position - Target).normalized * 
                        GameObject.Find("PlayerObject").GetComponent<BoxCollider2D>().size.x * 
                        Spec.Knockback);
                }
            }
        }
        else if (gameObject.CompareTag("MonsterProjectile") && collision.gameObject.CompareTag("Player"))
        {
            if (isActive)
            {
                if (Spec.MaxPassCount != -1)
                {
                    currentPassCount++;
                    // ���� ������ �������� ������ ��ġ���� Ŀ���� disable
                    if (currentPassCount > (Spec.MaxPassCount))
                    {
                        setDisable();
                        ObjectPoolManager.Instance.DisableGameObject(gameObject);
                    }
                }
                
                collision.gameObject.GetComponent<IStatus>().DamageHp = damage;
            }
        }
    }

    #region method
    protected abstract void destroySelf();
    protected abstract void launchProjectile();
    public abstract void setEnable(Vector3 _target, Vector3 _player, float _angle);
    public abstract void setDisable();
    #endregion
}

