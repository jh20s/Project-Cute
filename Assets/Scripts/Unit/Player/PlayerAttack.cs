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
    public Vector3 Target
    {
        get { return mTarget; }
    }

    [SerializeField]
    private bool mGSkillUseable  = true;
    [SerializeField]
    private bool mUSkillUseable = true;

    [SerializeField]
    private bool mIsGameStart;

    public Text mTestDebugText;
    void Start()
    {
        // key : ��ų ���ӿ�����Ʈ value : �� ��ų�� �߻�ü ������Ʈ
        TileDict = new SkillDic();
        mChargingBar = GameObject.Find("ChargingBar").gameObject;
        mChargingBar.SetActive(false);
        mAutoAttackSpeed = GetComponent<PlayerStatus>().PlayerATKSPD; //TO-DO �ӽ÷� �־����. ���� ������ ����?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }
    private void Update()
    {
        mTestDebugText.text = "���ݷ� = " + (GetComponent<IStatus>().BaseDamage + GetComponent<IStatus>().getCurrentWeponeDamage()).ToString() + "\n"
            + "���ݼӵ� = " + mAutoAttackSpeed.ToString() + "\n" + "�̵��ӵ� = " + (GetComponent<IStatus>().Speed * GetComponent<IStatus>().SpeedRate).ToString();
    }
    void FixedUpdate()
    {
        if (mAutoAttackCheckTime > mAutoAttackSpeed && mIsGameStart)
        {
            // �ڵ����� �ż���
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
        // ��ų �Ŵ����� ���� ���� �������� ��ų�� �޾ƿ´�
        TileDict.Clear();
        pushProjectile(CurrentBaseSkill);
        pushProjectile(CurrentGeneralSkill);
        pushProjectile(CurrentUltimateSkill);
        createObjectPool();
        mIsGameStart = true;
    }

    /*
     * ��ų Ŭ�� �� ȣ��˴ϴ�.
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
            StartCoroutine(activeAnimation(CurrentGeneralSkill));
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
     * ��ų�� ���� �� �ִϸ��̼� ���
     * ���� �ִϸ��̼� ����� ������ �Ұ�(������ ����)
     * Todo : PlayerManager������ �ִϸ��̼� �ڵ� �ű��
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
        Debug.Log(_skill.gameObject.name);
        if(_skill.gameObject.name != "Exalted Sword")
            GetComponent<PlayerMove>().MMoveable = false;
        GetComponent<PlayerMove>().MAnim.SetFloat("AttackState", 0f);
        GetComponent<PlayerMove>().MAnim.SetFloat("NormalState", num);
        GetComponent<PlayerMove>().MAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f); // Todo : ���Ŀ� ��ų �����Ϳ��� ���ð��� �޾� �Է� 
        GetComponent<PlayerMove>().MMoveable = true;
    }

    private void useSkill(GameObject _btn, Skill _skill)
    {
        // �ش� ��ų�� �����ɷ��� �ִٸ� ����
        if (_skill.SkillBuffType != Skill.ESkillBuffType.None)
        {
            _skill.BuffOn();
        }
        int count = _skill.Spec.SkillClickCount;
        // ��ų�� ���ӽð��� �ִ� ���
        if (_skill.Spec.MSkillRunTime[0] > 0)
        {
            int launchCount = _skill.Spec.SkillCount;
            // (�ͼ�Ƽ�� �ҵ�)
            if (launchCount > 1)
            {
                if (mTarget == Vector3.zero)
                    mTarget = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
                Vector3 firePos = firePosition.transform.position;
                // ������ Ŭ���� ���
                if (count - 1 == _skill.CurrentUseCount)
                {
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 2, mTarget, firePos, false);
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount = 0;
                }
                // ùŬ�� �� ���
                else if(_skill.CurrentUseCount == 0)
                {
                    // �ι�°���� ���ӽð����� �Ϲ� ��Ÿ���ڷ�ƾ ����
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 1, mTarget, firePos, false);
                    StartCoroutine(WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                    // ù �˱� �߻� �� ���ӽð� ��Ÿ�� �ڷ�ƾ ����
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount++;
                    StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime[0], _btn, _skill));
                }
                // �߰��� ���
                else
                {
                    _skill.CurrentUseCount++;
                    launchProjectile(_skill, 0, mTarget, firePos, false);
                    launchProjectile(_skill, 2, mTarget, firePos, false);
                    StartCoroutine(WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                }
            }
            // ���� �ð����� �������� �߻�ü�� �߻�Ǵ� ���
            else if (launchCount == -1)
            {
                // Todo : ��Ÿ�ӵ��ȿ��� ���ѹ߻簡 ������ ��ų�� ��� (�������ǵ�, ����Į����)
                // ��ġ�е��� �������� �ش� �ð����� �߻�
                StartCoroutine(InfiniteLaunch(_skill));
            }
            // �ѹ� Ŭ���� ���ӽð����� �߻�Ǵ� ���
            else
            {
                launchSkill(_skill);
                StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime[0], _btn, _skill));
            }
        }
        // �ѹ� Ŭ��
        else if (count == 1)
        {
            launchSkill(_skill);
            StartCoroutine(
                WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
        }
        // Ŭ��Ƚ���� ������ �ִ� ���
        else
        {
            // ������ Ŭ���� ���
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
    // �ð����� ���� �߻��� ���
    // ���� �ð�, Ÿ��
    IEnumerator InfiniteLaunch(Skill _skill)
    {
        // Todo : ���ѹ߻� ��ų ���ӽð��� �ѹߴ� �߻�ð� ������ �޾ƿ���
        // ���߿� �̷���ų���� �� ����ԵǸ� ���̺��� �ʿ��ؿ�...
        int count = (int)(_skill.Spec.MSkillRunTime[0] / _skill.Spec.MSkillRunTime[1]); //  ���� �ð� / �ѹߴ� �߻�ð�
        StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime[0], UBtn, _skill));
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

            yield return new WaitForSeconds(_skill.Spec.MSkillRunTime[1]); // �ѹߴ� �߻�ð�
        }

    }
    // �켱 �߻�ü�� �Ѱ��� ��츸 ����
    private void launchSkill(Skill _skill)
    {
        int count = _skill.Spec.SkillCount;
        int projectileCount = TileDict[_skill].Count;
        // ��ġ�е��� �Է� �������� �߻�
        // ��ġ�е� ���� ������ �� �̹� ����ȭ�� �Ǿ��ִ� ���¶�
        // �������� ������� ����ε� �������� ����
        // ���� ��ġ�е��� �Է��� ���� ��� �ٰŸ� ���ͷ� �߻�
        if(mTarget == Vector3.zero)
        {
            mTarget = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
        }
        Vector3 firePos = firePosition.transform.position;
        // �߻�ü�� �Ѱ��� ���
        if(projectileCount <= 1)
        {
            // �ѹ� �߻��� ���
            if (count == 1)
            {
                launchProjectile(_skill, 0, mTarget, firePos, false);
            }
            // �ߺ� �߻��� ���
            else
            {
                StartCoroutine(multiLuanch(_skill, count, mTarget, firePos));
            }
        }
        // �߻�ü�� 2�� �̻��ΰ��
        // 1. ù��° �߻�ü�� ������ ���ڸ����� ����
        // 2. ������ �߻�ü�� 2�� �̻��� ���
        else
        {
            // 1. ù��° �߻�ü�� ������ ���ڸ����� ����
            StartCoroutine(notSingleProjectileLaunch(_skill, mTarget, firePos));
        }
    }

    // �ϵ� �ڵ� ����(�߻�ü 2������ ����)
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
        float lefttime = _cooltime;
        while (lefttime > 0f)
        {
            // ��� �ڵ�...
            switch (_type)
            {
                case "G":
                    mGSkillUseable = false;
                    break;
                case "U":
                    mUSkillUseable = false;
                    break;
            }
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
        // �ñر� ��ų ��ư�� ��ġ�е�� ����(��ųŬ���� �ѹ��̰ų� �����ΰ�츸)
        if (_skill.Spec.SkillCount == 1 || _skill.Spec.SkillCount == -1)
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
        // ���ӽð����� �����Ǵ� �����̸� ���⼭ off
        if (_skill.SkillBuffType != Skill.ESkillBuffType.None)
        {
            _skill.BuffOff();
        }
        // ��� �ڵ�...
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