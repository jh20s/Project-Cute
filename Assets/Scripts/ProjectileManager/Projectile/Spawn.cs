using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : Projectile
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
    [SerializeField]
    private SpawnType mSpawnType;
    public SpawnType MSpawnType
    {
        get { return mSpawnType; }
        set { mSpawnType = value; }
    }
    public enum SpawnType
    {
        General, // 0 - �Ϲ���
        SelfSpawn, // 1 - �ڱ����� ������
        RendomSpawn, // 2 - ����������
        ShortWide, // 3 - ª�����̵���
        LongWide, // 4 - ����̵���
    }
    private VertualJoyStick mUJoySitick;
    private Vector3 mPlayer;
    private bool isChange;
    #endregion
    #region method
    protected override void destroySelf()
    {
        /*������ �ı��Ǵ� �ż���*/
    }
    protected override void launchProjectile()
    {
        /*���� �ż���*/
        // ����Ű�� ���� ��ġ�� �ٲ�� ���
        if (isActive && mSpawnType == SpawnType.LongWide)
        {
            mPlayer = GameObject.Find("fire").transform.position;
            mUJoySitick = GameObject.Find("JoyStick").transform.Find("UltimateJoyStick").GetComponent<VertualJoyStick>();
            Vector3 newPos = new Vector3(mUJoySitick.GetHorizontalValue() * 5, mUJoySitick.GetVerticalValue() * 5, 0);
            if(newPos != Vector3.zero)
            {
                angle = setAngle(newPos - mPlayer);
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.position = mPlayer + (newPos - mPlayer).normalized * (GetComponent<SpriteRenderer>().size.x * 1.5f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        launchProjectile();
    }
    public float setAngle(Vector3 dir)
    {
        // ���Ϸ���(0~360)
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    // ������ Ÿ�ٹ��� �������͸�ŭ �������� ����
    // �ڱ����� ����
    // ���� ������(player�ֺ��� ������ ũ���� ������ �����ϰ� ����)
    // �Ϲ������� ���������� ������ ������ �ʿ�(projectileSpec���� ������ ����)
    public override void setEnable(Vector3 _target, Vector3 _player, float _angle)
    {
        transform.localScale = new Vector3(AddScaleCoefficient, AddScaleCoefficient, AddScaleCoefficient);
        target = _target;
        mPlayer = GameObject.Find("fire").transform.position;
        // �������� ���
        switch (mSpawnType)
        {
            case SpawnType.General:
                transform.position = mPlayer + (target - mPlayer).normalized;
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.SelfSpawn:
                transform.position = mPlayer;
                break;
            case SpawnType.RendomSpawn:
                float rH = Random.Range(-3, 4);
                float rV = Random.Range(-3, 4);
                Vector3 ranPos = new Vector3(rH, rV, 0);
                transform.position = mPlayer + ranPos;
                break;
            case SpawnType.ShortWide:
                transform.position = mPlayer + (target - mPlayer).normalized * (GetComponent<SpriteRenderer>().size.x);
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.LongWide:
                transform.position = mPlayer + (target - mPlayer).normalized * (GetComponent<SpriteRenderer>().size.x * 1.5f);
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
        }
        gameObject.SetActive(true);
        isActive = true;
    }

    public override void setDisable()
    {
        isActive = false;
    }
    #endregion
}
