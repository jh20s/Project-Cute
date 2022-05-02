using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{

    private bool DEBUG = true;

    public enum SkillLaunchType
    {
        //��Ƽ�� ����
        MULTISHOT,
        //projectile�� 1�����ϰ�� 
        NORMAL,
        //����������
        THROW,
        //��Ƽ ��ġ�� ex �巡�� 2����ų
        LAUNCHMULTISHOT,
        //NORMAL������ �����̸� �ξ� �߻��Ѵ�.
        DELAYSHOT,
        //DELAYSHOT�� ��Ƽ������ �߻��Ѵ�.
        DELAYMULTISHOT,
        //Ư�� ��ġ�� ����
        RUSH,
        //waring projectile�������� ����
        WARINGSHOT
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
    protected GameObject mFirstProjectile;


    protected SkillDic TileDict;
    public GameObject mFirePosition;

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

    [SerializeField]
    private bool mIsRush;
    [SerializeField]
    private Vector3 mRushDir;
    [SerializeField]
    private float mRushSpeed;


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

    virtual protected void  FixedUpdate()
    {

        if (mIsRush)
        {
            transform.Translate(mRushDir * mRushSpeed * Time.fixedDeltaTime);
        }

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


    public void SetTileDict(Skill _skill, List<Projectile> _projectiles)
    {
        TileDict.Add(_skill, _projectiles);
    }

    // �޾ƿ� ��ų�� �߻�ü ����Ʈ�� �߻�ü�Ŵ����� ���� �޾ƿ´�
    //  <��ų, �߻�ü����Ʈ> Ÿ���� Dic�� �߰�
    protected void PushProjectile(Skill _skill)
    {
        List<Projectile> newList = new List<Projectile>();
        for (int i = 0; i < _skill.Spec.getProjectiles().Count; i++)
        {
            string projectile = _skill.Spec.getProjectiles()[i];
            Projectile newProjectile = ProjectileManager.Instance.allProjectiles[projectile].GetComponent<Projectile>();
            newList.Add(newProjectile);
        }
        SetTileDict(_skill, newList);
    }



    // ���������� �߻� �ڷ�ƾ���� ȣ���ϴ� �Լ�
    /*
    * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
    * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
     * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
    * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
    * 
    * return List<gameObject> : �߻�� projectile�� Gameobject list�� ��ȯ
    */
    protected List<GameObject> launchProjectile(Skill _skill, int _projectileIndex, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
        List<GameObject> objList = new List<GameObject>();
        // �⺻���ݸ� ���� ����
        int launchCount;
        if (_skill.Spec.Type == "B")
            launchCount = TileDict[_skill][_projectileIndex].Spec.Count + mProjectileCount;
        else
            launchCount = TileDict[_skill][_projectileIndex].Spec.Count;
        int angle = TileDict[_skill][_projectileIndex].Spec.Angle;
        // ������ ���� ��� �ѹ߸� �߻�
        for (int i = 0; i < launchCount; i++)
        {
            objList.Add(
                LaunchCorutines(
                (launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i),
                TileDict[_skill][_projectileIndex].gameObject.name,
                _targetPos,
                _firePos, _notSingle)
                );
        }
        return objList;
    }

    /*
    * _angle : �߰� ���� �����Դϴ�.
    * _name : �ش� �߻�ü������Ʈ�� name�Դϴ�.
    * return GameObject : �߻�� projectile Gameobject�� ��ȯ
    */
    protected GameObject LaunchCorutines(float _angle, string _name, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(_name);
        if (_notSingle) mFirstProjectile = obj;
        float keepTime = obj.GetComponent<Projectile>().Spec.SpawnTime;
        setProjectileData(ref obj);
        obj.GetComponent<Projectile>().CurrentPassCount = 0;
        obj.GetComponent<Projectile>().setEnable(_targetPos, _firePos, _angle);
        obj.GetComponent<Projectile>().setDisableWaitForTime(keepTime);
        return obj;
    }

    //������Ÿ���� ProjectileDelayTime��ŭ ��ٷȴٰ� projectile�� �߻�ȴ�.
    protected IEnumerator DelayLaunchProjectile(Skill _skill, int _projectileIndex, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
        // �⺻���ݸ� ���� ����
        int launchCount = TileDict[_skill][_projectileIndex].Spec.Count;
        int angle = TileDict[_skill][_projectileIndex].Spec.Angle;
        if (DEBUG)
            Debug.Log(_projectileIndex + "��° projectile��" + TileDict[_skill][_projectileIndex].gameObject.name + "�� " + launchCount + "��, " + angle + "������ �߻���");
        
        for (int i = 0; i < launchCount; i++)
        {
            LaunchCorutines(
                 (launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i),
                 TileDict[_skill][_projectileIndex].gameObject.name,
                 _targetPos,
                 _firePos, _notSingle);
            yield return new WaitForSeconds(TileDict[_skill][_projectileIndex].Spec.ProjectileDelayTime);
        }
    }


    protected IEnumerator multiLuanch(Skill _skill, int _count, Vector3 _target, Vector3 _fire)
    {
        for (int i = 0; i < _count; i++)
        {
            // �⺻������ ��� �߰��Ǿ�����, ���� ���⿡�� ������ ���׸� �����ϱ�����
            // ���⼭ �� �߻��, firePosition�� ����
            _fire = mFirePosition.transform.position;
            launchProjectile(_skill, 0, _target, _fire, false);
            yield return new WaitForSeconds(_skill.Spec.SkillCountTime);
        }
    }

    //_skill�� _projectileIndex��° ��ų�� ��Ƽ������ �����ϴ�.
    //_obj�� position���� ��ų�� �߻�˴ϴ�.
    protected IEnumerator multiLuanch(Skill _skill, int _projectileIndex, int _count, Vector3 _target, GameObject _obj)
    {
        for (int i = 0; i < _count; i++)
        {
            launchProjectile(_skill, _projectileIndex, _target, _obj.GetComponent<Transform>().position, false);
            yield return new WaitForSeconds(_skill.Spec.SkillCountTime);
        }
    }

    //ù��° �߻�� projectile�������� multiLuanch�� �߻�ȴ�
    //�巡�� 2�� ��ų ����
    private void LaunchInMultilaunchSkil(Skill _skill, int _count, Vector3 _target, Vector3 _fire)
    {
        //ù��° ������Ÿ���� �޿� �߻�ȴ�.
        List<GameObject> projectileList = launchProjectile(_skill, 0, _target, _fire, true);

        //�ι�° ������ Ÿ���� ù��° ������ Ÿ�� �������� multi launch�� �߻��Ѵ�
        //�巡�� 2����ų������ �̻����� 1������ �������
        //TO-DO : �ٸ� ������Ÿ�ϵ� �߻��ؾ��ϴ� ��Ȳ�̶�� for������ ��� ���¿� ���� �߰����� �ʿ�
        StartCoroutine(multiLuanch(_skill, 1, _count, _target, projectileList[0]));

    }

    //��ġ�� �̻��� �������� ��Ƽ ��ġ���� ������.
    //������Ʈ 0�� ��ų ����
    protected IEnumerator DelayMultiLuanch(Skill _skill, int _projectileIndex, int _count, Vector3 _target, Vector3 _fire)
    {
        for (int i = 0; i < _count; i++)
        {
            StartCoroutine(DelayLaunchProjectile(_skill, _projectileIndex, _target, _fire, false));
            yield return new WaitForSeconds(_skill.Spec.SkillCountTime);
        }
    }

    //_time �ð����� gameobject�� position���� _target���� �̵�
    protected IEnumerator RushAndLuanch(Skill _skill, int _projectileIndex, Vector3 _target, float _time)
    {
        mIsRush = true;
        mRushDir = _target - transform.position;
        mRushDir.Normalize();
        mRushSpeed = _skill.Spec.SKillRushSpeed;

        if (DEBUG)
        {
            Debug.Log("mRushDir : " + mRushDir + ",�Ÿ� : " + Vector3.Distance(_target, transform.position) + ",mRushSpeed : " + mRushSpeed);
        }
        yield return new WaitForSeconds(_time);

        launchProjectile(_skill, _projectileIndex, _target, gameObject.transform.position, false);
        mIsRush = false;
    }


    //Waring projectile ���� ������ �߻�ü�� ������� �߻��
    IEnumerator WaringAndLuanch(Skill _skill, Vector3 _target, Vector3 _fire)
    {
        GameObject obj = launchProjectile(_skill, 0, _target, _fire, true)[0];
        if (DEBUG)
        {
            Debug.Log("ù��° waring�� ��ġ : " + obj.transform.position+" ,target�� ��ġ :"+ _target+",fireposition�� ��ġ :"+_fire);
        }

        for (int i = 1; i < TileDict[_skill].Count; i++)
        {
            yield return new WaitWhile(() => obj.activeInHierarchy);
            if (DEBUG)
            {
                Debug.Log(i+ "��° ��ų�� �߻�, target�� ��ġ: "+ _target+", fireposition�� ��ġ: "+ obj.transform.position);
            }
            Vector3 nextFire = obj.transform.position;
            obj = launchProjectile(_skill, i, _target, nextFire, false)[0];
            if (DEBUG)
            {
                Debug.Log("�� ���� obj�� ��ġ"+obj.transform.position);
            }

        }
    }


    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        // �߻�ü�� �߻� �ɶ� ���� ������ �����ϴ� apiȣ��(ũ��Ƽ�� ���� ��ȯ)
        obj.GetComponent<Projectile>().IsCriticalDamage = GetComponent<IStatus>().AttackPointSetting(gameObject);
        obj.GetComponent<Projectile>().Damage = 
            (int)(mObjectDamage * obj.GetComponent<Projectile>().Spec.ProjectileDamage);

        Vector3 size = new Vector3(
            obj.GetComponent<Projectile>().Spec.ProjectileSizeX * (1 + mProjectileScale),
            obj.GetComponent<Projectile>().Spec.ProjectileSizeY * (1 + mProjectileScale),
            1);
        obj.GetComponent<Projectile>().setSize(size);

        //caller pos ��ġ ����
        obj.GetComponent<Projectile>().CallerPos = gameObject.transform.position;


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


    protected void FireSkillLaunchType(SkillLaunchType _enum, Skill _skill, int _count, Vector3 _target, Vector3 _fire, bool _notSingle, float _time)
    {
        if (DEBUG)
            Debug.Log(gameObject.name + "�� " + _enum.ToString() + "Ÿ���� "+ _skill.name+ "�� "+_count+"�� ����մϴ�");
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
            case SkillLaunchType.LAUNCHMULTISHOT:
                LaunchInMultilaunchSkil(_skill, _count, _target, _fire);
                break;
            case SkillLaunchType.DELAYSHOT:
                StartCoroutine(DelayLaunchProjectile(_skill, 0, _target, _fire, _notSingle));
                break;
            case SkillLaunchType.DELAYMULTISHOT:
                StartCoroutine(DelayMultiLuanch(_skill, 0, _count, _target, _fire));
                break;
            case SkillLaunchType.RUSH:
                StartCoroutine(RushAndLuanch(_skill, 0, _target, _time));
                break;
            case SkillLaunchType.WARINGSHOT:
                StartCoroutine(WaringAndLuanch(_skill, _target, _fire));
                break;
            default:
                Debug.Log("�߸��� EnumŸ�� " + _enum.ToString() + "�� ���Խ��ϴ�");
                break;
        }
    }
}
