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





    private bool mGSkillUseable  = true;
    private bool mUSkillUseable = true;

    public bool mIsGameStart = false;
    void Start()
    {
        // key : ��ų ���ӿ�����Ʈ value : �� ��ų�� �߻�ü ������Ʈ
        TileDict = new SkillDic();
        mChargingBar = GameObject.Find("ChargingBar").gameObject;
        mChargingBar.SetActive(false);
        mAutoAttackSpeed = 1f; //TO-DO �ӽ÷� �־����. ���� ������ ����?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }
    void Update()
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
        mAutoAttackCheckTime += Time.deltaTime;
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
        GetComponent<PlayerMove>().MMoveable = false;
        GetComponent<PlayerMove>().MAnim.SetFloat("AttackState", 0f);
        GetComponent<PlayerMove>().MAnim.SetFloat("NormalState", num);
        GetComponent<PlayerMove>().MAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f); // Todo : ���Ŀ� ��ų �����Ϳ��� ���ð��� �޾� �Է� 
        GetComponent<PlayerMove>().MMoveable = true;
    }

    private void useSkill(GameObject _btn, Skill _skill)
    {
        int count = _skill.Spec.SkillClickCount;
        // ��ų�� ���ӽð��� �ִ� ���
        if (_skill.Spec.MSkillRunTime > 0)
        {
            int launchCount = _skill.Spec.SkillCount;
            // (�ͼ�Ƽ�� �ҵ�)
            if (launchCount > 1)
            {
                Vector3 targetPos = new Vector3(Mjoystick.GetHorizontalValue() * 5f, Mjoystick.GetVerticalValue() * 5f, 0);
                if (targetPos == Vector3.zero)
                    targetPos = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
                Vector3 firePos = firePosition.transform.position;
                // ������ Ŭ���� ���
                if (count - 1 == _skill.CurrentUseCount)
                {
                    launchProjectile(_skill, 0, targetPos, firePos, false);
                    launchProjectile(_skill, 2, targetPos, firePos, false);
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount = 0;
                }
                // ùŬ�� �� ���
                else if(_skill.CurrentUseCount == 0)
                {
                    // �ι�°���� ���ӽð����� �Ϲ� ��Ÿ���ڷ�ƾ ����
                    launchProjectile(_skill, 0, targetPos, firePos, false);
                    launchProjectile(_skill, 1, targetPos, firePos, false);
                    StartCoroutine(WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
                    // ù �˱� �߻� �� ���ӽð� ��Ÿ�� �ڷ�ƾ ����
                    _skill.CurrentCoolTimeIndex++;
                    _skill.CurrentUseCount++;
                    StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, _btn, _skill));
                }
                // �߰��� ���
                else
                {
                    _skill.CurrentUseCount++;
                    launchProjectile(_skill, 0, targetPos, firePos, false);
                    launchProjectile(_skill, 2, targetPos, firePos, false);
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
                StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, _btn, _skill));
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
        int count = (int)(_skill.Spec.MSkillRunTime / 0.2f); //  ���� �ð� / �ѹߴ� �߻�ð�
        StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, UBtn, _skill));
        Vector3 targetPos;
        Vector3 firePos;
        while (count != 0)
        {
            count--;
            targetPos = new Vector3(Ujoystick.GetHorizontalValue(), Ujoystick.GetVerticalValue(), 0);
            if(targetPos == Vector3.zero)
                targetPos = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
            firePos = firePosition.transform.position;
            // ��ġ�е� ���� ������ �� �̹� ����ȭ�� �Ǿ��ִ� ���¶�
            // �������� ������� ����ε� �������� ����
            if(TileDict[_skill].Count > 1)
            {
                int ranNum = Random.Range(0, TileDict[_skill].Count);
                launchProjectile(_skill, ranNum, targetPos * 5f, firePos, false);
            }
            else
            {
                launchProjectile(_skill, 0, targetPos * 5f, firePos, false);
            }

            yield return new WaitForSeconds(0.2f); // �ѹߴ� �߻�ð�
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
        Vector3 targetPos = new Vector3(Mjoystick.GetHorizontalValue() * 5f, Mjoystick.GetVerticalValue() * 5f, 0);
        if(targetPos == Vector3.zero)
            targetPos = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
        Vector3 firePos = firePosition.transform.position;
        // �߻�ü�� �Ѱ��� ���
        if(projectileCount <= 1)
        {
            // �ѹ� �߻��� ���
            if (count == 1)
            {
                launchProjectile(_skill, 0, targetPos, firePos, false);
            }
            // �ߺ� �߻��� ���
            else
            {
                StartCoroutine(multiLuanch(_skill, count, targetPos, firePos));
            }
        }
        // �߻�ü�� 2�� �̻��ΰ��
        // 1. ù��° �߻�ü�� ������ ���ڸ����� ����
        // 2. ������ �߻�ü�� 2�� �̻��� ���
        else
        {
            // 1. ù��° �߻�ü�� ������ ���ڸ����� ����
            StartCoroutine(notSingleProjectileLaunch(_skill, targetPos, firePos));
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
            lefttime -= Time.deltaTime;
            _btn.transform.GetChild(1).GetComponent<Image>().fillAmount =( lefttime  / _cooltime);
            yield return new WaitForFixedUpdate();
        }
        _btn.transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
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
        if(_skill.Spec.SkillCount == 1 || _skill.Spec.SkillCount == -1)
        {
            UBtn.SetActive(false);
            Ujoystick.gameObject.SetActive(true);
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
        mUSkillUseable = false;
        _bar.SetActive(false);
        Ujoystick.gameObject.SetActive(false);
        UBtn.SetActive(true);
        if (_skill.CurrentCoolTimeIndex == 1)
            _skill.CurrentCoolTimeIndex++;
        StartCoroutine(
                   WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));
        _skill.CurrentUseCount = 0;
        _skill.CurrentCoolTimeIndex = 0;
    }
}