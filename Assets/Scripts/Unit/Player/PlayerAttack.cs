using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerAttack : IAttack
{
    public GameObject GBtn;
    public GameObject UBtn;
    public VertualJoyStick Ujoystick;
    public VertualJoyStick Mjoystick;

    private GameObject mChargingBar;

    [SerializeField]
    private Image mGeneralSkillImg;
    [SerializeField]
    private Image mUltimateSkillImg;
    
    [SerializeField]
    private Skill currentGeneralSkill;
    public Skill CurrentGeneralSkill
    {
        get { return currentGeneralSkill; }
        set
        {
            currentGeneralSkill = value;
            
            mGeneralSkillImg.sprite = ProjectileManager.
                Instance.allProjectiles[currentGeneralSkill.Spec.getProjectiles()[0]].
                GetComponent<SpriteRenderer>().sprite;
        }
    }
    [SerializeField]
    private Skill currentUltimateSkill;
    public Skill CurrentUltimateSkill
    {
        get { return currentUltimateSkill; }
        set
        {
            currentUltimateSkill = value;
            mUltimateSkillImg.sprite = ProjectileManager.
                Instance.allProjectiles[currentUltimateSkill.Spec.getProjectiles()[0]].
                GetComponent<SpriteRenderer>().sprite;
        }
    }

    [SerializeField]
    private Vector3 mTarget;

    [SerializeField]
    private bool mGSkillUseable  = true;
    [SerializeField]
    private bool mUSkillUseable = true;

    [SerializeField]
    private bool mIsGameStart;

    public Text mTestDebugText;
    void Start()
    {
        // key : 스킬 게임오브젝트 value : 각 스킬의 발사체 오브젝트
        TileDict = new SkillDic();
        mChargingBar = GameObject.Find("ChargingBar").gameObject;
        mChargingBar.SetActive(false);
        mAutoAttackSpeed = 1f; //TO-DO 임시로 넣어놓음. 실제 공속은 무엇?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }
    void FixedUpdate()
    {
        mTestDebugText.text = "mAutoAttackCheckTime = " + mAutoAttackCheckTime.ToString() + "\n" 
            + "mAutoAttackSpeed = " + mAutoAttackSpeed.ToString();
        if (mAutoAttackCheckTime > mAutoAttackSpeed && mIsGameStart)
        {
            // 자동공격 매서드
            {
                launchProjectile(
                    CurrentBaseSkill,
                    0,
                    MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position),
                    firePosition.transform.position,
                    false);
            }
            mAutoAttackCheckTime = 0f;
        }
        mAutoAttackCheckTime += Time.fixedDeltaTime;
    }

    public void getProjectiles()
    {
        // 스킬 매니저를 통해 현재 장착중인 스킬을 받아온다
        TileDict.Clear();
        pushProjectile(CurrentBaseSkill);
        pushProjectile(CurrentGeneralSkill);
        pushProjectile(CurrentUltimateSkill);
        createObjectPool();
        mIsGameStart = true;
    }

    /*
     * 스킬 클릭 시 호출됩니다.
     */
    public void DragSkillBtn(GameObject _btn)
    {
        VertualJoyStick dir = _btn.GetComponent<VertualJoyStick>();
        mTarget = new Vector3(dir.GetHorizontalValue() + transform.position.x , dir.GetVerticalValue() + transform.position.y, 0);
    }
    public void clickGeneralSkillBtn()
    {
        if (mGSkillUseable)
        {
            mGSkillUseable = false;
            useSkill(GBtn, CurrentGeneralSkill);
            StartCoroutine(activeAnimation(CurrentUltimateSkill));
        }
    }
    public void clickUltimateSkillBtn()
    {
        if (mUSkillUseable)
        {
            mUSkillUseable = false;
            useSkill(UBtn, CurrentUltimateSkill);
            StartCoroutine(activeAnimation(CurrentUltimateSkill));
        }
    }
    /*
     * 스킬로 공격 시 애니메이션 재생
     * 공격 애니메이션 재생시 움직임 불가(변수로 제어)
     * Todo : PlayerManager쪽으로 애니메이션 코드 옮기기
     */
    IEnumerator activeAnimation(Skill _skill)
    {
        float num = 0f;
        switch (_skill.Spec.SkillWeaponType)
        {
            case "sw":
            case "sp":
                num = 0f;
                break;
            case "bg":
                num = 0.5f;
                break;
            case "st":
                num = 1f;
                break;
        }
        GetComponent<PlayerMove>().MMoveable = false;
        GetComponent<PlayerMove>().MAnim.SetFloat("AttackState", 0f);
        GetComponent<PlayerMove>().MAnim.SetFloat("NormalState", num);
        GetComponent<PlayerMove>().MAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f); // Todo : 추후에 스킬 데이터에서 대기시간을 받아 입력 
        GetComponent<PlayerMove>().MMoveable = true;
    }

    private void useSkill(GameObject _btn, Skill _skill)
    {
        int count = _skill.Spec.SkillClickCount;
        // 스킬의 지속시간이 있는 경우
        if (_skill.Spec.MSkillRunTime > 0)
        {
            int launchCount = _skill.Spec.SkillCount;
            // (익설티드 소드)
            if (launchCount > 1)
            {
                if (mTarget == Vector3.zero)
                    mTarget = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
                Vector3 firePos = firePosition.transform.position;
                // 마지막 클릭일 경우
                if (count - 1 == _skill.CurrentUseCount)
                {
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 2, mTarget, firePos, false);
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount = 0;
                }
                // 첫클릭 일 경우
                else if(_skill.CurrentUseCount == 0)
                {
                    // 두번째부턴 지속시간동안 일반 쿨타임코루틴 실행
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 1, mTarget, firePos, false);
                    StartCoroutine(WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                    // 첫 검기 발사 후 지속시간 쿨타임 코루틴 실행
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount++;
                    StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, _btn, _skill));
                }
                // 중간일 경우
                else
                {
                    _skill.CurrentUseCount++;
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 2, mTarget, firePos, false);
                    StartCoroutine(WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                }
            }
            // 지속 시간내에 일정량의 발사체가 발사되는 경우
            else if (launchCount == -1)
            {
                // Todo : 쿨타임동안에는 무한발사가 가능한 스킬의 경우 (오버래피드, 아포칼립스)
                // 터치패드의 방향으로 해당 시간동안 발사
                StartCoroutine(InfiniteLaunch(_skill));
            }
            // 한번 클릭에 지속시간동안 발사되는 경우
            else
            {
                launchSkill(_skill);
                StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, _btn, _skill));
            }
        }
        // 한번 클릭
        else if (count == 1)
        {
            launchSkill(_skill);
            StartCoroutine(
                WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
        }
        // 클릭횟수가 정해져 있는 경우
        else
        {
            // 마지막 클릭일 경우
            if (count - 1 == _skill.CurrentUseCount)
            {
                _skill.CurrentCoolTimeIndex++;
                launchSkill(_skill);
                StartCoroutine(
                    WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                _skill.CurrentCoolTimeIndex = 0;
                _skill.CurrentUseCount = 0;
            }
            else
            {
                _skill.CurrentUseCount++;
                launchSkill(_skill);
                StartCoroutine(
                    WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
            }
        }
    }
    // 시간내의 무한 발사인 경우
    // 지속 시간, 타겟
    IEnumerator InfiniteLaunch(Skill _skill)
    {
        // Todo : 무한발사 스킬 지속시간과 한발당 발사시간 데이터 받아오기
        // 나중에 이런스킬류가 더 생기게되면 테이블이 필요해요...
        int count = (int)(_skill.Spec.MSkillRunTime / 0.1f); //  지속 시간 / 한발당 발사시간
        StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, UBtn, _skill));
        Vector3 firePos;
        while (count != 0)
        {
            count--;
            if(mTarget == Vector3.zero)
                mTarget = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
            firePos = firePosition.transform.position;
            if(TileDict[_skill].Count > 1)
            {
                int ranNum = Random.Range(0, TileDict[_skill].Count);
                launchProjectile(_skill, ranNum, mTarget, firePos, false);
            }
            else
            {
                launchProjectile(_skill, 0, mTarget, firePos, false);
            }

            yield return new WaitForSeconds(0.2f); // 한발당 발사시간
        }
    }
    // 우선 발사체가 한개인 경우만 구현
    private void launchSkill(Skill _skill)
    {
        int count = _skill.Spec.SkillCount;
        int projectileCount = TileDict[_skill].Count;
        // 터치패드의 입력 방향으로 발사
        // 터치패드 방향 가져올 시 이미 정규화가 되어있는 상태라
        // 일정량을 곱해줘야 제대로된 방향으로 나감
        // 만약 터치패드의 입력이 없을 경우 근거리 몬스터로 발사
        if(mTarget == Vector3.zero)
        {
            mTarget = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
        }
        Vector3 firePos = firePosition.transform.position;
        // 발사체가 한개인 경우
        if(projectileCount <= 1)
        {
            // 한번 발사일 경우
            if (count == 1)
            {
                launchProjectile(_skill, 0, mTarget, firePos, false);
            }
            // 중복 발사일 경우
            else
            {
                StartCoroutine(multiLuanch(_skill, count, mTarget, firePos));
            }
        }
        // 발사체가 2개 이상인경우
        // 1. 첫번째 발사체가 나가고 그자리에서 스폰
        // 2. 나갈때 발사체가 2개 이상인 경우
        else
        {
            // 1. 첫번째 발사체가 나가고 그자리에서 스폰
            StartCoroutine(notSingleProjectileLaunch(_skill, mTarget, firePos));
        }
    }

    // 하드 코딩 상태(발사체 2가지만 구현)
    IEnumerator notSingleProjectileLaunch(Skill _skill, Vector3 _target, Vector3 _fire)
    {
        for(int i = 0; i < TileDict[_skill].Count; i++)
        {
            if(i > 0)
            {
                if(i == TileDict[_skill].Count - 1)
                {
                    launchProjectile(_skill, i, _target, firstProjectile.GetComponent<Projectile>().MyPos, false);
                }
                else
                {
                    launchProjectile(_skill, i, _target, firstProjectile.GetComponent<Projectile>().MyPos, true);
                }
            }
            else
            {
                launchProjectile(_skill, i, _target, _fire, true);
            }
            yield return new WaitWhile(() => firstProjectile.activeInHierarchy);
        }
    }


    IEnumerator WaitForCoolTime(GameObject _btn, float _cooltime, string _type)
    {
        // 방어 코드...
        switch (_type)
        {
            case "G":
                mGSkillUseable = false;
                break;
            case "U":
                mUSkillUseable = false;
                break;
        }
        float lefttime = _cooltime;
        while (lefttime > 0f)
        {
            lefttime -= Time.deltaTime;
            _btn.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount =( lefttime  / _cooltime);
            yield return new WaitForFixedUpdate();
        }
        _btn.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = 0;
        switch (_type)
        {
            case "G":
                mGSkillUseable = true;
                break;
            case "U":
                mUSkillUseable = true;
                break;
        }
    }

    IEnumerator WaitForChargingTime(GameObject _bar, float _spawnTime, GameObject _btn, Skill _skill)
    {
        // 궁극기 스킬 버튼이 터치패드로 변경(스킬클릭이 한번이거나 무한인경우만)
        if(_skill.Spec.SkillCount == 1 || _skill.Spec.SkillCount == -1)
        {
            //UBtn.SetActive(false);
            //Ujoystick.gameObject.SetActive(true);
        }
        _bar.SetActive(true);
        float lefttime = _spawnTime;
        while(lefttime > 0f)
        {
            lefttime -= Time.deltaTime;
            _bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            _bar.transform.GetChild(0).GetComponent<Image>().fillAmount = (lefttime / _spawnTime);
            yield return new WaitForFixedUpdate();
        }
        // 방어 코드...
        mUSkillUseable = false;
        _bar.SetActive(false);
        //Ujoystick.gameObject.SetActive(false);
        //UBtn.SetActive(true);
        if (_skill.CurrentCoolTimeIndex == 1)
            _skill.CurrentCoolTimeIndex++;
        StartCoroutine(
                   WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
        _skill.CurrentUseCount = 0;
        _skill.CurrentCoolTimeIndex = 0;
    }
}