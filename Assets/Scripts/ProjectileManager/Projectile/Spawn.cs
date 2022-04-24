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
        ReverseSpawn, //��ǥ ������Ʈ�� ���� ����������
    }
    [SerializeField]
    private VertualJoyStick mUJoySitick;
    [SerializeField]
    private Vector3 mPlayer;
    [SerializeField]
    private Vector3 newPos = Vector3.zero;
    [SerializeField]
    private Vector3 mJoyStickPos = Vector3.zero;
    [SerializeField]
    float scale;
    [SerializeField]
    float baseX, baseY;
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
            Vector3 joystickPos = new Vector3(mUJoySitick.GetHorizontalValue(), mUJoySitick.GetVerticalValue(), 0);
            // ��ġ�е� �Է��� ���� ���
            if (joystickPos != Vector3.zero)
                mJoyStickPos = joystickPos;
            // ��ġ�е� �Է��� ���� ���
            else
                mJoyStickPos = mJoyStickPos == Vector3.zero ? Vector3.right : mJoyStickPos;
            newPos = mJoyStickPos + mPlayer;
            angle = setAngle(newPos - mPlayer);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position = mPlayer;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        baseX = transform.localScale.x;
        baseY = transform.localScale.y;
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
        float scale = GameObject.Find("PlayerObject").GetComponent<IAttack>().ProjectileScale;
        transform.localScale = new Vector3(baseX + scale, baseY + scale, scale);
        target = _target;
        mPlayer = _player;
        scale = AddScaleCoefficient - 1;
        // �������� ���
        switch ((SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType))
        {
            case SpawnType.General:
                transform.position = mPlayer;
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
                transform.position = mPlayer;
                angle = setAngle(target - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.ReverseSpawn:
                transform.position = mPlayer;
                if (gameObject.transform.position.x < GameObject.Find("PlayerObject").transform.position.x)
                {
                    gameObject.transform.localScale = new Vector3(baseX, baseY, baseX);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(-baseX, baseY, baseX);
                }
                break;
        }
        gameObject.SetActive(true);
        isActive = true;
    }

    public override void setDisable()
    {
        mJoyStickPos = Vector3.zero;
        newPos = Vector3.zero;
        isActive = false;
    }
    #endregion
}
