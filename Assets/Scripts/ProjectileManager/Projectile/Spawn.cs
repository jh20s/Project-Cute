using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : Projectile
{
    private bool DEBUG = false;

    #region variable
    [SerializeField]
    private ProjectileSpec spec = new ProjectileSpec();
    public override ProjectileSpec Spec
    {
        get { return spec; }
        set { spec = value; }
    }
    [SerializeField]
    private Vector3 mTarget;
    public override Vector3 Target
    {
        get { return mTarget; }
        set { mTarget = value; }
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
        TargetSpawn, //Ÿ�� ��ġ�� ����
        NormalizeSpawn,//firePosition���� �������͸�ŭ�ָ��� ȸ������ �ʰ� ����
    }
    [SerializeField]
    private VertualJoyStick mUJoySitick;
    [SerializeField]
    private Vector3 mPlayer;
    [SerializeField]
    private Vector3 newPos = Vector3.zero;
    [SerializeField]
    private Vector3 mJoyStickPos = Vector3.zero;
    #endregion
    #region method
    protected override void launchProjectile()
    {
        if (gameObject.layer == LayerMask.NameToLayer("projectile")) { 
            /*���� �ż���*/
            // ����Ű�� ���� ��ġ�� �ٲ�� ���
            if (mIsActive && (SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType) == SpawnType.LongWide)
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
            if(mIsActive && (SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType) == SpawnType.SelfSpawn)
            {
                mPlayer = GameObject.Find("fire").transform.position;
                transform.position = mPlayer;
            }
        }
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
    public override void setEnable(Vector3 _target, Vector3 _fire, float _angle)
    {
        if (DEBUG)
            Debug.Log("spawn�� spawnType: "+ mSpawnType.ToString()+", enable target: " + _target + ",fire: " + _fire + ",angle: " + _angle);

        mTarget = _target;
        mPlayer = _fire;
        // �������� ���
        switch ((SpawnType)Enum.Parse(typeof(SpawnType), mSpawnType))
        {
            case SpawnType.General:
                transform.position = mPlayer;
                angle = setAngle(mTarget - mPlayer) + _angle;
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
                angle = setAngle(mTarget - mPlayer) + _angle;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpawnType.ReverseSpawn:
                transform.position = mPlayer;
                float sizeX = transform.localScale.x;
                float sizeY = transform.localScale.y;
                //TO-DO : monsterAttack�� �������� ����� �ڵ�� player�� ����Ϸ��� ������ �ʿ�
                if (CallerPos.x < PlayerManager.Instance.Player.transform.position.x)
                {
                    transform.localScale = new Vector3(sizeX, sizeY, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-sizeX, sizeY, 1f);
                }
                break;
            //TO-DO �������͸� ��� �޴°� ������? �̰Ͷ��� ������ �ְ� data�����̵Ǿ�� ����
            case SpawnType.NormalizeSpawn:
                Vector3 dir = mTarget - mPlayer;
                dir.Normalize();
                transform.position = mPlayer + dir*2;
                break;
            case SpawnType.TargetSpawn:
                transform.position = mTarget;
                break;
        }
        if (DEBUG)
            Debug.Log("spawn object positon: " + transform.position);

        gameObject.SetActive(true);
        mIsActive = true;
    }

    public override void setDisable()
    {
        mJoyStickPos = Vector3.zero;
        newPos = Vector3.zero;
        mIsActive = false;
    }
    #endregion
}
