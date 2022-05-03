using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonsterAttack : IAttack
{
    private bool DEBUG = false;
    [SerializeField]
    private int DEBUG_SKILL_NUMBER=-1;

    [SerializeField]
    protected int mCloseAttackPower;

    public int CloseAttackPower
    {
        get { return mCloseAttackPower; }
        set { mCloseAttackPower = value; }
    }


    [SerializeField]
    protected float mBerserkerModeScale;
    public float BerserkerModeScale
    {
        get { return mBerserkerModeScale; }
        set { mBerserkerModeScale = value; }
    }


    private float tempTime = 0f;
    private float tempTileMax = 3f;

    private bool mIsUsingSkill;

    [SerializeField]
    private List<GameObject> mMonsterSkill = new List<GameObject>();

    [SerializeField]
    private List<float> mSkillCheckCoolTime;

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

        mBerserkerModeScale = 1f;

        tempTime = 0f;

        //��Ÿ�� �迭 ����
        if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
            mSkillCheckCoolTime = new List<float> { 0f, 0f, 0f, 0f };
        else if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Range)
            mSkillCheckCoolTime = new List<float> { 0f };

        if (UseSkillCheck())
        {
            MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);


            mFirePosition = transform.Find("FirePosition").gameObject;
            TileDict = new SkillDic();
            mMonsterSkill = SkillManager.Instance.FindMonsterSkill(md.monsterType);
            for (int i = 0; i < mMonsterSkill.Count; i++)
            {
                PushProjectile(mMonsterSkill[i].GetComponent<Skill>());
            }
            //TO-DO ��� objectpool�� �ϴ°� ������ ���. ���⼭ �ϸ� �� ���Ͱ� ��ȯ�ɶ����� create�� �ϴ� ������ �߻��Ѵ�.
            //objectpool �� ���� object�� ���ö� ������� ���ϱ� ������ 1ȸ�� ȣ���ϰ������ �׷����� ��� �ؾ� ������ ������ʿ�.
            createObjectPool();
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        UseSkil();

        mAutoAttackCheckTime = Mathf.Max(mAutoAttackCheckTime - Time.deltaTime, 0);

        for(int i = 0; i < mSkillCheckCoolTime.Count; i++)
        {
            mSkillCheckCoolTime[i] = Mathf.Max(mSkillCheckCoolTime[i] - Time.deltaTime, 0f);
        }

        base.FixedUpdate();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (mAutoAttackCheckTime.Equals(0f))
            {
                GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().CloseDamaged = (int)((float)mCloseAttackPower * BerserkerModeScale);
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
        int skillNumber = -1;
        if (md.monsterGrade == MonsterManager.MonsterGrade.Boss) {
            int priorityValue = 99;
            for (int i = 0; i < md.skillAttackPowerRange.Count; i++)
            {
                if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= md.skillAttackPowerRange[i]
                        && mSkillCheckCoolTime[i].Equals(0f)
                        && mMonsterSkill[i].GetComponent<Skill>().Spec.SkillPriority < priorityValue)
                {
                    priorityValue = mMonsterSkill[i].GetComponent<Skill>().Spec.SkillPriority;
                    skillNumber = i;
                }
            }
            if(skillNumber!= -1)
            {
                mSkillCheckCoolTime[skillNumber] = mMonsterSkill[skillNumber].GetComponent<Skill>().Spec.getSkillCoolTime()[0];
            }
        }
        else if(md.monsterGrade == MonsterManager.MonsterGrade.Range)
        {
            if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= md.skillAttackPowerRange[0]
                && mSkillCheckCoolTime[0].Equals(0f))
            {
                skillNumber = 0;
            }
        }
        //inspectorâ���� Ư����ų ����׽� Ư����ų�� �������� �� �� ���
        if (DEBUG_SKILL_NUMBER != -1)
        {
            return DEBUG_SKILL_NUMBER;
        }
        return skillNumber;
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
        gameObject.GetComponent<MonsterStatus>().BaseDamage = (int)((float)md.skillAttackPower[_skillNum] * BerserkerModeScale);

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
                            GameObject.Find("PlayerObject").transform.position, mFirePosition.transform.position, false, skill.Spec.SkillStopTime);
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

}
