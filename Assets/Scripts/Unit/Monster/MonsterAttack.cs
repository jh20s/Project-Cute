using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonsterAttack : IAttack
{
    private bool DEBUG = false;

    protected int mCloseAttackPower;
    protected int mCloseAttackRange;
    protected int mStandoffAttackPower;
    protected int mStandoffAttackRange;


    private float tempTime = 0f;
    private float tempTileMax = 3f;

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


        tempTime = 0f;

        if (UseSkillCheck())
        {
            MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);


            mFirePosition = transform.Find("FirePosition").gameObject;
            TileDict = new SkillDic();
            mMonsterSkill = SkillManager.Instance.FindMonsterSkill(md.monsterType);
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
                GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().CloseDamaged = mCloseAttackPower;
                mAutoAttackCheckTime = mAutoAttackSpeed;
            }
        }
    }

    private void UseSkil()
    {
        if (!mIsUsingSkill && tempTime.Equals(0f))
        {
            /*
             * 
             * TO-DO 
             * Boss�� ��ų�� 4����, Range�� ��ų�� 1������ �ϴ��� �̺κ��� attack�� �ݿ��ؼ� �ڵ带 ©����. �̺κ���
             * SkillAttackPower
             * SkillAttackPowerRange
             * SkillAttackAnimation
             * �̰͵��� 1,2,3,4�� �а� �ƴ϶� /�� ������ �Ѵ��� list�� �޴� ���·��ؼ� ���� �� ���� ���� list���·� �޾Ƽ� �ؾ� �۾��� ���Ұ����� ����
             * �ϴ� ���� ���¸� �� ������ ����� ��Ʊ� ������ grade�� ���� ���� �۾��� �ϴ°ɷ� �ϰ� ���� ���� 
             */
            if (UseSkillCheck())
            {
                //���� ��ų �������� => �Ÿ��񱳸� ���� ����ؾ��� ��Ų ��ȣ�� �޼���� ����
                int skillNum = GetSkillNumber();
                Debug.Log("��뽺ųnum :"+ skillNum);
                //��ų ���
                if (skillNum != -1)
                {
                    StartCoroutine(UseSkill(skillNum));
                }
            }
            tempTime = tempTileMax;
        }
        
            


            tempTime = Mathf.Max(tempTime - Time.deltaTime, 0);
    }
    
    // -1 �� ��� ���ɽ�ų�� ����
    //�������� ������� ��ų��ȣ return 
    private int GetSkillNumber()
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);

        if (md.monsterGrade == MonsterManager.MonsterGrade.Boss) { 
            //��� ������ ��ų�� ã�´�
            List<int> skillNumber = new List<int>();
            for (int i = 0; i < md.skillAttackPowerRange.Count; i++)
            {
                if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= md.skillAttackPowerRange[i])
                {
                    skillNumber.Add(i);
                }
            }
            //��ų�� ����Ʈ�� ���� ������ ������
            //�����Ѵ�.
            return skillNumber.Count == 0 ? -1 : skillNumber[UnityEngine.Random.Range(0, skillNumber.Count)];
        }
        else if(md.monsterGrade == MonsterManager.MonsterGrade.Range)
        {
            if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= md.skillAttackPowerRange[0])
            {
                return 0;
            }
        }
        return -1;
    }

    private bool UseSkillCheck()
    {
        return gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss
                    || gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Range;
    }


    //_skillNum���� ��ϵ� ��ų�� ����մϴ�
    //�ִϸ��̼��� event�۾��� �����ϱ� ���ؼ� �ִϸ��̼��� ��ų�� �������ϴ� ����� �ڵ�� �����ϵ��� �Ǿ��ֽ��ϴ�
    //1. ��ų ����� ����   -> TO DO : projectile�� �ѱ�� �ִ� ����� ������ Ȯ���غ����� ���� �ʿ� ���� ��ų�� ������ ���������� �� ��ų BaseDamage�� �����Ǵ� ���װ� �߻��� �� ����
    //2. ��ų ���� ���㼳��
    //3. ��ų ��� ����.
    //4. ��� �� ��ų�� �ߵ��Ǵ����� �������� wait
    //5. ��ų�߻�
    //6. ��ų ���۽ð� ���� ����
    //7. �ٽ� �����̴� �ִϸ��̼� ����
    private IEnumerator UseSkill(int _skillNum)
    {
        if (DEBUG)
            Debug.Log(gameObject.name + "�� " + _skillNum + "��° ��ų������");
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);
        Skill skill = mMonsterSkill[_skillNum].GetComponent<Skill>();

        //1. ��ų ����� ����
        SkillLaunchType skillLaunchType = (SkillLaunchType)Enum.Parse(typeof(SkillLaunchType), skill.Spec.SkillLaunchType);
        gameObject.GetComponent<MonsterStatus>().BaseDamage = md.skillAttackPower[_skillNum];

        //2. ��ų ���� ���㼳��
        gameObject.GetComponent<IMove>().MMoveable = false;

        if (DEBUG)
            Debug.Log(gameObject.name + " ���ð�" + skill.Spec.SkillStartTime);

        //3. ��ų ��� ����.
        transform.GetComponent<Animator>().SetTrigger(MonsterManager.Instance.GetMonsterData(gameObject.name).skillAttackAnimation[_skillNum]);

        //4. ��� �� ��ų�� �ߵ��Ǵ����� �������� wait
        yield return new WaitForSeconds(skill.Spec.SkillStartTime);

        if (DEBUG)
            Debug.Log(gameObject.name + " ��ų�߻�");


        //5. ��ų�߻�
        FireSkillLaunchType(skillLaunchType, skill, skill.Spec.SkillCount,
                            GameObject.Find("PlayerObject").transform.position, mFirePosition.transform.position, false);
        mIsUsingSkill = true;

        if (DEBUG)
            Debug.Log(gameObject.name + " ��ų���۵��� ����ð� :" + skill.Spec.SkillStopTime);


        //6. ��ų ���۽ð����� ����
        yield return new WaitForSeconds(skill.Spec.SkillStopTime);

        if (DEBUG)
            Debug.Log(gameObject.name + " Walk�� ��ȯ");

        //7. �ٽ� �����̴� �ִϸ��̼� ����
        transform.GetComponent<Animator>().SetTrigger("Walk");
        gameObject.GetComponent<IMove>().MMoveable = true;
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
