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
    private float angle = 0;
    #endregion
    #region method
    protected override void destroySelf()
    {
        /*������ �ı��Ǵ� �ż���*/
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
        isActive = false;
        myPos = transform.position;
    }

    // Ȱ��ȭ
    /*
     * _target : ��ġ�е� �Է¹��� ���� or �ٰŸ� ������ ��ġ ����
     * _player : ���� player�� ��ġ ����(�߻�ü�� ������ġ)
     */
    public override void setEnable(Vector3 _target, Vector3 _player, float _angle)
    {
        float scale = GameObject.Find("PlayerObject").GetComponent<IAttack>().ProjectileScale + 1f;
         transform.localScale = new Vector3(scale, scale, scale);
         transform.position = _player;
         target = _target;
         angle = setAngle(target - _player) + _angle;
         transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
         gameObject.SetActive(true);
         isActive = true;
    }
    void Start()
    {
#if DEBUG
#endif
    }
    void FixedUpdate()
    {
        launchProjectile();
    }
    #endregion
}
