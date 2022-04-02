using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : Projectile
{
    #region variable
    [SerializeField]
    private ProjectileSpec spec = new ProjectileSpec();
    public override ProjectileSpec Spec
    {
        get { return spec; }
        set { spec = value; }
    }

    [SerializeField]
    private Vector3 target;
    public override Vector3 Target
    {
        get { return target; }
        set { target = value; }
    }

    private bool isActive = false;
    #endregion
    #region method
    protected override void destroySelf()
    {
        /*������ �ı��Ǵ� �ż���*/
        Destroy(gameObject, Spec.SpawnTime);
    }
    protected override void launchProjectile()
    {
        if (isActive)
        {
            // Rotate�� ȸ�� ���ױ� ������ ���������� ������ ����
            transform.Translate(Vector2.right * Time.deltaTime * spec.Speed);
        }
    }
    // �߻� ���⿡ ���� �߻�ü�� ȸ����Ű�� �Լ�
    public void setAngle()
    {
        // ���Ϸ���(0~360)
        float angle = Quaternion.FromToRotation(
            new Vector3(1, 0, 0), target.normalized).eulerAngles.z;
        transform.Rotate(0, 0, angle);
    }
    // ��Ȱ��ȭ
    public void setDisable()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    // Ȱ��ȭ
    /*
     * _target : ��ġ�е� �Է¹��� ���� or �ٰŸ� ������ ��ġ ����
     * _player : ���� player�� ��ġ ����
     */
    public void setEnable(Vector3 _target, Vector3 _player)
    {
        transform.position = _player;
        target = _target;
        setAngle();
        gameObject.SetActive(true);
        isActive = true;

    }
    void Start()
    {
#if DEBUG
        target = new Vector3(50, -10, 0);
#endif
        setDisable();
    }
    // Update is called once per frame
    void Update()
    {
        launchProjectile();
    }
    #endregion
}
