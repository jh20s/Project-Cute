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
        //TO-DO 모든 몬스터는 공속이 다 1f?
        mAutoAttackSpeed = 1f;
        mAutoAttackCheckTime = 0f;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        // 몬스터의 데미지 설정을 위한 override
        base.Start();
        mIsUsingSkill = false;


        tempTime = 0f;

        if (UseSkillCheck())
        {
            MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);


            firePosition = transform.Find("FirePosition").gameObject;
            TileDict = new SkillDic();
            mMonsterSkill = SkillManager.Instance.FindMonsterSkill(md.monsterType);
            for (int i = 0; i < mMonsterSkill.Count; i++)
            {
                pushProjectile(mMonsterSkill[i].GetComponent<Skill>());
            }
            //TO-DO 어디서 objectpool을 하는게 맞을지 고민. 여기서 하면 각 몬스터가 소환될때마다 create를 하는 문제가 발생한다.
            //objectpool 이 같은 object가 들어올때 재생성은 안하긴 하지만 1회만 호출하고싶은데 그런경우는 어디서 해야 옳을지 고민이필요.
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
             * Boss는 스킬이 4가지, Range는 스킬이 1가지라 일단은 이부분을 attack에 반영해서 코드를 짤것임. 이부분을
             * SkillAttackPower
             * SkillAttackPowerRange
             * SkillAttackAnimation
             * 이것들을 1,2,3,4로 둔게 아니라 /로 나눠서 한다음 list를 받는 형태로해서 각각 이 값에 대해 list형태로 받아서 해야 작업이 편할것으로 보임
             * 일단 현재 형태를 다 뒤집어 엎기는 어렵기 때문이 grade에 따라 따로 작업을 하는걸로 하고 이후 수정 
             */
            if (UseSkillCheck())
            {
                //몬스터 스킬 랜덤선택 => 거리비교를 통해 사용해야할 스킨 번호를 메서드로 리턴
                int skillNum = GetSkillNumber();
                Debug.Log("사용스킬num :"+ skillNum);
                //스킬 사용
                if (skillNum != -1)
                {
                    StartCoroutine(SkillStopTime(skillNum));
                }
            }
            tempTime = tempTileMax;
        }
        
            


            tempTime = Mathf.Max(tempTime - Time.deltaTime, 0);
    }
    
    // -1 시 사용 가능스킬이 없음
    //랜덤으로 사용사능한 스킬번호 return 
    private int GetSkillNumber()
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);

        if (md.monsterGrade == MonsterManager.MonsterGrade.Boss) { 
            //사용 가능한 스킬을 찾는다
            List<int> skillNumber = new List<int>();
            for (int i = 0; i < md.skillAttackPowerRange.Count; i++)
            {
                if (Mathf.Abs(Vector3.Distance(GameObject.Find("PlayerObject").transform.position, gameObject.transform.position)) <= md.skillAttackPowerRange[i])
                {
                    skillNumber.Add(i);
                }
            }
            //스킬을 리스트에 대해 랜덤을 돌린다
            //리턴한다.
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


    private IEnumerator SkillStopTime(int _skillNum)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(gameObject.name);
        Skill skill = mMonsterSkill[_skillNum].GetComponent<Skill>();
        SkillLaunchType skillLaunchType = (SkillLaunchType)Enum.Parse(typeof(SkillLaunchType), skill.Spec.SkillLaunchType);
        gameObject.GetComponent<MonsterStatus>().BaseDamage = md.skillAttackPower[_skillNum];

        //애니메이션에서 스킬 발사 이전모션까지는 대기
        yield return new WaitForSeconds(skill.Spec.SkillStartTime);


        //Base데미지 설정
        gameObject.GetComponent<MonsterStatus>().BaseDamage = md.skillAttackPower[_skillNum];

        //스킬발사
        FireSkillLaunchType(skillLaunchType, skill, skill.Spec.SkillCount,
                            GameObject.Find("PlayerObject").transform.position, firePosition.transform.position, false);

        //스킬사용 동안 멈춤
        // 무브스피드를 조정하는 것이아니라 bool형변수로 컨트롤해야합니다.
        // 초기 설정되었던 보스몬스터의 이속이 스킬을 사용한 이후 1로 변경이 됩니다.
        //gameObject.GetComponent<IStatus>().MoveSpeed = 0;
        gameObject.GetComponent<IMove>().MMoveable = false;
        transform.GetComponent<Animator>().SetTrigger(MonsterManager.Instance.GetMonsterData(gameObject.name).skillAttackAnimation[_skillNum]);
        mIsUsingSkill = true;
        yield return new WaitForSeconds(skill.Spec.SkillStopTime);

        //다시 움직이는 애니메이션으로 동작
        transform.GetComponent<Animator>().SetTrigger("Walk");
        //gameObject.GetComponent<IStatus>().MoveSpeed = 1;
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
