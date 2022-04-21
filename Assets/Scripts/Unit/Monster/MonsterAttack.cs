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

    [SerializeField]
    private List<GameObject> mMonsterSkill = new List<GameObject>();

    void Awake()
    {
        //TO-DO ��� ���ʹ� ������ �� 1f?
        mAutoAttackSpeed = 1f;
        mAutoAttackCheckTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss) {
            firePosition = transform.Find("FirePosition").gameObject;
            TileDict = new SkillDic();
            mMonsterSkill = SkillManager.Instance.FindMonsterSkill("Slime");
            for(int i=0; i<mMonsterSkill.Count; i++)
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
        if (tempTime.Equals(0f)) {
            if (gameObject.GetComponent<MonsterStatus>().MonsterGrade == MonsterManager.MonsterGrade.Boss)
            {
                int tempRandNum = UnityEngine.Random.Range(0, 2);
                Debug.Log("�������� "+ tempRandNum);
                Skill skill = mMonsterSkill[tempRandNum].GetComponent<Skill>();
                SkillLaunchType skillLaunchType =(SkillLaunchType)Enum.Parse(typeof(SkillLaunchType), skill.Spec.SkillLaunchType);
                int count = skill.Spec.SkillCount;
                FireSkillLaunchType(skillLaunchType, skill, count,
                        GameObject.Find("PlayerObject").transform.position, firePosition.transform.position, false);
            }
            tempTime = tempTileMax;
        }
        tempTime = Mathf.Max(tempTime - Time.deltaTime, 0);

        mAutoAttackCheckTime = Mathf.Max(mAutoAttackCheckTime - Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

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

    void OnCollisionExit2D(Collision2D collision)
    {

    }


    public int CloseAttackPower {
        get { return mCloseAttackPower;}
        set { mCloseAttackPower = value;}
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
