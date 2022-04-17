using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private string mSpawnType;
    public string MSpawnType
    {
        get { return mSpawnType; }
        set { mSpawnType = value; }
    }
    public enum SpawnType
    {
        General, // �Ϲ���
        SelfSpawn, // �ڱ����� ������
        RandomSpawn, // ����������
        ShortWide, // ª�����̵���
        LongWide, // ����̵���
    }
    [SerializeField]
    private VertualJoyStick mUJoySitick;
    private Vector3 mPlayer;
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
        if (isActive && (SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType) == SpawnType.LongWide)
        {
            mPlayer = GameObject.Find("fire").transform.position;
            mUJoySitick = GameObject.Find("Canvas").transform.Find("UltimateSkillJoyStick").GetComponent<VertualJoyStick>();
            Vector3 newPos = new Vector3(mUJoySitick.GetHorizontalValue() + mPlayer.x, mUJoySitick.GetVerticalValue() + mPlayer.y, 0);
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
        switch ((SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType))
        {
            case SpawnType.General:
                transform.position = mPlayer + (target - mPlayer).normalized;
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.SelfSpawn:
                transform.position = mPlayer;
                break;
            case SpawnType.RandomSpawn:
                float rH = UnityEngine.Random.Range(-3, 4);
                float rV = UnityEngine.Random.Range(-3, 4);
                Vector3 ranPos = new Vector3(rH, rV, 0);
                transform.position = mPlayer + ranPos;
                break;
            case SpawnType.ShortWide:
                // ���Ŀ� ��Ȯ�� ������ ���ؼ� ����
                transform.position = mPlayer + (target - mPlayer).normalized * (GetComponent<SpriteRenderer>().size.x * 0.6f);
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.LongWide:
                // ���Ŀ� ��Ȯ�� ������ ���ؼ� ����
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
