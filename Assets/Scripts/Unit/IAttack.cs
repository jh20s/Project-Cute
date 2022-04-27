using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{

    public enum SkillLaunchType
    {
        //��Ƽ�� ����
        MULTISHOT,
        //projectile�� 1�����ϰ�� 
        NORMAL,
        //����������
        THROW
    }
    // ���� ������
    [SerializeField]
    private int mObjectDamage;

    // �߻�ü ���� ��
    [SerializeField]
    private int mProjectileCount;

    // �߻�ü ���� ������
    [SerializeField]
    private float mProjectileScale;
    public float ProjectileScale
    {
        get { return mProjectileScale; }
    }

    // �⺻ ���� �ð� �߰���
    [SerializeField]
    private float mStiffTime;
    public float StiffTime
    {
        get { return mStiffTime; }
    }

    // �⺻ ���� Ƚ��
    [SerializeField]
    protected int mRAttackCount;

    // �⺻ ���� ���� ��
    [SerializeField]
    private int mPassCount;
    public int PassCount
    {
        get { return mPassCount; }
    }

    // ���� ��� �� ��ų�� �߻�ü 2�� �̻��ε�
    // ù�߻�ü�� disable�� position���� enable
    protected GameObject firstProjectile;


    //TO-Do value�� List<Projectile>�� ���� 
    protected SkillDic TileDict;
    public GameObject firePosition;

    [SerializeField]
    protected float mAutoAttackSpeed;
    protected float mAutoAttackCheckTime;

    [SerializeField]
    private Skill currentBaseSkill;
    public Skill CurrentBaseSkill
    {
        get { return currentBaseSkill; }
        set
        {
            currentBaseSkill = value;
        }
    }


    // Start is called before the first frame update
     protected virtual void Start()
    {
        // key : ��ų ���ӿ�����Ʈ value : �� ��ų�� �߻�ü ������Ʈ
        gameObject.GetComponent<IEventHandler>().registerAttackSpeedObserver(RegisterAttackSpeedObserver);
        gameObject.GetComponent<IEventHandler>().registerAttackPointObserver(RegisterAttackPointObserver);
        gameObject.GetComponent<IEventHandler>().registerProjectileCountObserver(RegisterProjectileCountObserver);
        gameObject.GetComponent<IEventHandler>().registerProjectileScaleObserver(RegisterProjectileScaleObserver);
        gameObject.GetComponent<IEventHandler>().registerStiffTimeObserver(RegisterStiffTimeObserver);
        gameObject.GetComponent<IEventHandler>().registerRAttackCountObserver(RegisterRAttackCountObserver);
        gameObject.GetComponent<IEventHandler>().registerPassCountObserver(RegisterPassCountObserver);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegisterAttackSpeedObserver(float _attackSpeed, GameObject _obj)
    {
        mAutoAttackSpeed = _attackSpeed;
    }

    private void RegisterAttackPointObserver(int _attackPoint, GameObject _obj)
    {
        mObjectDamage = _attackPoint;
    }
    private void RegisterProjectileCountObserver(int _count, GameObject _obj)
    {
        mProjectileCount = _count;
    }
    private void RegisterProjectileScaleObserver(float _scale, GameObject _obj)
    {
        mProjectileScale= _scale;
    }
    private void RegisterStiffTimeObserver(float _time, GameObject _obj)
    {
        mStiffTime = _time;
    }
    private void RegisterRAttackCountObserver(int _count, GameObject _obj)
    {
        mRAttackCount = _count;
    }
    private void RegisterPassCountObserver(int _count, GameObject _obj)
    {
        mPassCount = _count;
    }


    public void setTileDict(Skill _skill, List<Projectile> _projectiles)
    {
        TileDict.Add(_skill, _projectiles);
    }

    // �޾ƿ� ��ų�� �߻�ü ����Ʈ�� �߻�ü�Ŵ����� ���� �޾ƿ´�
    //  <��ų, �߻�ü����Ʈ> Ÿ���� Dic�� �߰�
    protected void pushProjectile(Skill _skill)
    {
        List<Projectile> newList = new List<Projectile>();
        for (int i = 0; i < _skill.Spec.getProjectiles().Count; i++)
        {
            string projectile = _skill.Spec.getProjectiles()[i];
            Projectile newProjectile = ProjectileManager.Instance.allProjectiles[projectile].GetComponent<Projectile>();
            newList.Add(newProjectile);
        }
        setTileDict(_skill, newList);
    }



    // ���������� �߻� �ڷ�ƾ���� ȣ���ϴ� �Լ�
    /*
    * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
    * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
     * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
    * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
    */
    protected void launchProjectile(Skill mSkill, int mProjectileIndex, Vector3 mTargetPos, Vector3 mFirePos, bool mNotSingle)
    {
        // �⺻���ݸ� ���� ����
        int launchCount;
        if (mSkill.Spec.Type == "B")
            launchCount = TileDict[mSkill][mProjectileIndex].Spec.Count + mProjectileCount;
        else
            launchCount = TileDict[mSkill][mProjectileIndex].Spec.Count;
        int angle = TileDict[mSkill][mProjectileIndex].Spec.Angle;
        // ������ ���� ��� �ѹ߸� �߻�
        for (int i = 0; i < launchCount; i++)
        {
            LaunchCorutines(
                (launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i),
                TileDict[mSkill][mProjectileIndex].gameObject.name,
                mTargetPos,
                mFirePos, mNotSingle);
        }
    }

    /*
    * _angle : �߰� ���� �����Դϴ�.
    * _name : �ش� �߻�ü������Ʈ�� name�Դϴ�.
    */
    protected void LaunchCorutines(float _angle, string _name, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(_name);
        if (_notSingle) firstProjectile = obj;
        float keepTime = obj.GetComponent<Projectile>().Spec.SpawnTime;
        Vector3 size = new Vector3(
            obj.GetComponent<Projectile>().Spec.ProjectileSizeX * (1 + mProjectileScale),
            obj.GetComponent<Projectile>().Spec.ProjectileSizeY * (1 + mProjectileScale),
            1);
        obj.GetComponent<Projectile>().setSize(size);
        setProjectileData(ref obj);
        obj.GetComponent<Projectile>().CurrentPassCount = 0;
        obj.GetComponent<Projectile>().setEnable(_targetPos, _firePos, _angle);
        obj.GetComponent<Projectile>().setDisableWaitForTime(keepTime);
    }


    protected IEnumerator multiLuanch(Skill _skill, int _count, Vector3 _target, Vector3 _fire)
    {
        for (int i = 0; i < _count; i++)
        {
            // �⺻������ ��� �߰��Ǿ�����, ���� ���⿡�� ������ ���׸� �����ϱ�����
            // ���⼭ �� �߻��, firePosition�� ����
            _fire = firePosition.transform.position;
            launchProjectile(_skill, 0, _target, _fire, false);
            yield return new WaitForSeconds(_skill.Spec.SkillCountTime);
        }
    }

    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        // �߻�ü�� �߻� �ɶ� ���� ������ �����ϴ� apiȣ��(ũ��Ƽ�� ���� ��ȯ)
        obj.GetComponent<Projectile>().IsCriticalDamage = GetComponent<IStatus>().AttackPointSetting(gameObject);
        obj.GetComponent<Projectile>().Damage = 
            (int)(mObjectDamage * obj.GetComponent<Projectile>().Spec.ProjectileDamage);
    }

    protected void createObjectPool()
    {
        foreach (Skill key in TileDict.Keys)
        {
            for (int i = 0; i < TileDict[key].Count; i++)
            {
                ObjectPoolManager.Instance.CreateDictTable(TileDict[key][i].gameObject, 10, 10);
            }
        }
    }


    protected void FireSkillLaunchType(SkillLaunchType _enum, Skill _skill, int _count, Vector3 _target, Vector3 _fire, bool _notSingle)
    {
        switch (_enum)
        {
            case SkillLaunchType.MULTISHOT:
                StartCoroutine(multiLuanch(_skill, _count, _target, _fire));
                break;
            case SkillLaunchType.NORMAL:
                launchProjectile(_skill, 0, _target, _fire, false);
                break;
            case SkillLaunchType.THROW:
                launchProjectile(_skill, 0, _target, _fire, false);
                break;
            default:
                Debug.Log("�߸��� EnumŸ�� " + _enum.ToString() + "�� ���Խ��ϴ�");
                break;
        }
    }
}
