using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonsterAttack : IAttack
{
    protected int mCloseAttackPower;
    protected int mCloseAttackRange;
    protected int mStandoffAttackPower;
    protected int mStandoffAttackRange;


    private float tempTime = 0f;
    private float tempTileMax = 2f;

    private bool mIsUsingSkill;

    [SerializeField]
    private List<GameObject> mMonsterSkill = new List<GameObject>();

    void Awake()
    {
        //TO-DO ��� ���ʹ� ������ �� 1f?
        mAutoAttackSpeed = 1f;
        mAutoAttackCheckTime = 0f;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        // ������ ������ ������ ���� override
        base.Start();
        mIsUsingSkill = false;

        if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
        {
            firePosition = transform.Find("FirePosition").gameObject;
            TileDict = new SkillDic();
            mMonsterSkill = SkillManager.Instance.FindMonsterSkill("Slime");
            for (int i = 0; i < mMonsterSkill.Count; i++)
            {
                pushProjectile(mMonsterSkill[i].GetComponent<Skill>());
            }
            //TO-DO ��� objectpool�� �ϴ°� ������ ���. ���⼭ �ϸ� �� ���Ͱ� ��ȯ�ɶ����� create�� �ϴ� ������ �߻��Ѵ�.
            //objectpool �� ���� object�� ���ö� ������� ���ϱ� ������ 1ȸ�� ȣ���ϰ������ �׷����� ��� �ؾ� ������ ������ʿ�.
            createObjectPool();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UseSkil();

        mAutoAttackCheckTime = Mathf.Max(mAutoAttackCheckTime - Time.deltaTime, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (mAutoAttackCheckTime.Equals(0f))
            {
                GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().DamageHp = mCloseAttackPower;
                mAutoAttackCheckTime = mAutoAttackSpeed;
            }
        }
    }

    private void UseSkil()
    {
        if (!mIsUsingSkill && tempTime.Equals(0f))
        {
            if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
            {
                //���� ��ų �������� => �Ÿ��񱳸� ���� ����ؾ��� ��Ų ��ȣ�� �޼���� ����
                MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);

                //��ų ���
                int skillNum = GetSkillNumber(md);
                if (skillNum != -1)
                {
                    //skillNum = 2;//TEMP
                    Debug.Log("������ų �ѹ�: " + skillNum);
                    Skill skill = mMonsterSkill[skillNum].GetComponent<Skill>();
                    SkillLaunchType skillLaunchType = (SkillLaunchType)Enum.Parse(typeof(SkillLaunchType), skill.Spec.SkillLaunchType);
                    int count = skill.Spec.SkillCount;
                    FireSkillLaunchType(skillLaunchType, skill, count,
                            GameObject.Find("PlayerObject").transform.position, firePosition.transform.position, false);
                    StartCoroutine(SkillStopTime(skill.Spec.SkilStopTime, skillNum));

                }
            }
            tempTime = tempTileMax;
        }
        tempTime = Mathf.Max(tempTime - Time.deltaTime, 0);
    }
    
    private int GetSkillNumber(MonsterManager.MonsterData _md)
    {
        //��� ������ ��ų�� ã�´�
        List<int> skillNumber = new List<int>();
        for (int i = 0; i < _md.skillAttackPowerRange.Count; i++)
        {
            if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= _md.skillAttackPowerRange[i])
            {
                skillNumber.Add(i);
            }
        }
        //��ų�� ����Ʈ�� ���� ������ ������
        //�����Ѵ�.
        return skillNumber.Count == 0 ? -1 : skillNumber[UnityEngine.Random.Range(0, skillNumber.Count)];
    }


    private IEnumerator SkillStopTime(float _time, int _skillNum)
    {
        gameObject.GetComponent<IStatus>().MoveSpeed = 0;
        transform.GetComponent<Animator>().SetTrigger(MonsterManager.Instance.GetMonsterData(gameObject.name).skillAttackAnimation[_skillNum]);
        mIsUsingSkill = true;
        yield return new WaitForSeconds(_time);
        transform.GetComponent<Animator>().SetTrigger("Walk");
        gameObject.GetComponent<IStatus>().MoveSpeed = 1;
        mIsUsingSkill = false;
    }



    public int CloseAttackPower
    {
        get { return mCloseAttackPower; }
        set { mCloseAttackPower = value; }
    }
    public int CloseAttackRange
    {
        get { return mCloseAttackRange; }
        set { mCloseAttackRange = value; }
    }
    public int StandoffAttackPower
    {
        get { return mStandoffAttackPower; }
        set { mStandoffAttackPower = value; }
    }
    public int StandoffAttackRange
    {
        get { return mStandoffAttackRange; }
        set { mStandoffAttackRange = value; }
    }
}
