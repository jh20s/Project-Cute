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
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private float angle = 0;
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
    public float setAngle(Vector3 dir)
    {
        // ���Ϸ���(0~360)
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    // ��Ȱ��ȭ
    public override void setDisable()
    {
        transform.Rotate(0, 0, 0);
        gameObject.SetActive(false);
        isActive = false;
    }

    // Ȱ��ȭ
    /*
     * _target : ��ġ�е� �Է¹��� ���� or �ٰŸ� ������ ��ġ ����
     * _player : ���� player�� ��ġ ����(�߻�ü�� ������ġ)
     */
    public override void setEnable(Vector3 _target, Vector3 _player, float _angle)
    {
        transform.localScale = new Vector3(AddScaleCoefficient, AddScaleCoefficient, AddScaleCoefficient);
        transform.position = _player;
        target = _target;
        angle = setAngle(target - _player) + _angle;
        transform.Rotate(0, 0, angle);
        gameObject.SetActive(true);
        isActive = true;
    }
    void Start()
    {
#if DEBUG
#endif
    }
    void Update()
    {
        launchProjectile();
    }
    #endregion
}
