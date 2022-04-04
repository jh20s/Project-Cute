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
    private bool isActive = false;
    [SerializeField]
    private float angle = 0;
    #endregion
    #region method
    protected override void destroySelf()
    {
        /*생성후 파괴되는 매서드*/
        Destroy(gameObject, spec.SpawnTime);
    }
    protected override void launchProjectile()
    {
        /*추적 매서드*/
        if (isActive)
        {
            Vector3 dir = (target - transform.position).normalized;
            angle = setAngle(dir);
            Quaternion rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * spec.Speed);
            transform.Translate(dir * Time.deltaTime * spec.Speed);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        launchProjectile();
    }
    // 발사 방향에 따라 발사체를 회전시키는 함수
    public float setAngle(Vector3 dir)
    {
        // 오일러각(0~360)
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    public override void setEnable(Vector3 _target, Vector3 _player, float _angle)
    {
        transform.localScale = new Vector3(AddScaleCoefficient, AddScaleCoefficient, AddScaleCoefficient);
        transform.position = _player + ranDir();
        target = _target;
        transform.Rotate(0, 0, angle);
        gameObject.SetActive(true);
        isActive = true;
    }

    public override void setDisable()
    {
        transform.Rotate(0, 0, 0);
        gameObject.SetActive(false);
        isActive = false;
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

