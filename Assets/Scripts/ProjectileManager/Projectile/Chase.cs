using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : Projectile
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
    protected override void launchProjectile()
    {
        /*���� �ż���*/
        if (mIsActive)
        {
           Vector3 dir = target - transform.position;
            Debug.Log(dir);
            // ������ �Ϸ�Ǿ��µ��� �����ð��� ���Ҵٸ� �ٽ� ����
            if (dir.sqrMagnitude <= new Vector3(2, 2, 0).sqrMagnitude)
            {
                dir = MonsterManager.Instance.GetNearestMonsterPos(transform.position) - transform.position;
            }
               
            //angle = setAngle(dir);
            //Quaternion rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * spec.Speed);
            transform.Translate(dir.normalized * Time.deltaTime * spec.MoveSpeed);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        launchProjectile();
    }
    // �߻� ���⿡ ���� �߻�ü�� ȸ����Ű�� �Լ�
    public float setAngle(Vector3 dir)
    {
        // ���Ϸ���(0~360)
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    public override void setEnable(Vector3 _target, Vector3 _player, float _angle)
    {
        transform.position = _player + ranDir();
        target = _target;
        gameObject.SetActive(true);
        mIsActive = true;
    }

    public override void setDisable()
    {
        mIsActive = false;
        myPos = transform.position;
    }

    private Vector3 ranDir()
    {
        Vector3 dir = Vector3.zero;
        int ranDir = Random.Range(0, 4);
        switch (ranDir)
        {
            case 0:
                dir = Vector2.right;
                angle = 0f;
                break;
            case 1:
                dir = Vector2.left;
                angle = 180f;
                break;
            case 2:
                dir = Vector2.up;
                angle = 90f;
                break;
            case 3:
                dir = Vector2.down;
                angle = 270f;
                break;
        }
        return dir;
    }
    #endregion
}

