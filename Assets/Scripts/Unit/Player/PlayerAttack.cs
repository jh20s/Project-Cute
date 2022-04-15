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

    private GameObject firstProjectile;
    private GameObject mChargingBar;


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
                    SkillManager.Instance.CurrentBaseSkill,
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
        pushProjectile(SkillManager.Instance.CurrentBaseSkill);
        pushProjectile(SkillManager.Instance.CurrentGeneralSkill);
        pushProjectile(SkillManager.Instance.CurrentUltimateSkill);
        createObjectPool();
        mIsGameStart = true;
    }

    // �޾ƿ� ��ų�� �߻�ü ����Ʈ�� �߻�ü�Ŵ����� ���� �޾ƿ´�
    //  <��ų, �߻�ü����Ʈ> Ÿ���� Dic�� �߰�
    private void pushProjectile(Skill _skill)
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

    //TO-DO ������� Create�� ���⼭�ϴ°� ��������.
    //PlayerManager���� ���������� Ȯ���� �������� �ű⼭ Create�ϴ°ɷ� ����
    private void createObjectPool()
    {
        foreach (Skill key in TileDict.Keys)
        {
            for (int i = 0; i < TileDict[key].Count; i++)
            {
                Debug.Log(TileDict[key][i].gameObject.name);
                ObjectPoolManager.Instance.CreateDictTable(TileDict[key][i].gameObject, 10, 10);
            }
        }
    }

    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        // �ϵ��ڵ��� �κ��� �÷��̾� ������ ���ݷ��� �޾ƿ´�.
        int randomDamage = Random.Range(30, 50);
        obj.GetComponent<Projectile>().Damage = 
            (int)((randomDamage + EquipmentManager.Instance.getCurrentDamage()) * obj.GetComponent<Projectile>().Spec.ProjectileDamage);
    }


    /*
     * ��ų Ŭ�� �� ȣ��˴ϴ�.
     */
    public void clickGeneralSkillBtn()
    {
        if (mGSkillUseable)
        {
            mGSkillUseable = false;
            useSkill(GBtn, SkillManager.Instance.CurrentGeneralSkill);
            StartCoroutine(activeAnimation(SkillManager.Instance.CurrentUltimateSkill));
        }
    }
    public void clickUltimateSkillBtn()
    {
        if (mUSkillUseable)
        {
            mUSkillUseable = false;
            useSkill(UBtn, SkillManager.Instance.CurrentUltimateSkill);
            StartCoroutine(activeAnimation(SkillManager.Instance.CurrentUltimateSkill));
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
            // ��ġ�е尡 �ʿ��� ���
            if (launchCount == -1)
            {
                // Todo : ��Ÿ�ӵ��ȿ��� ���ѹ߻簡 ������ ��ų�� ��� 
                // �ñر� ��ų ��ư�� ��ġ�е�� ����
                UBtn.SetActive(false);
                Ujoystick.gameObject.SetActive(true);
                // ��ġ�е��� �������� �ش� �ð����� �߻�
                StartCoroutine(InfiniteLaunch(_skill));
            }
            // �ѹ� Ŭ���� ���ӽð����� �߻�Ǵ� ���
            else if (count == 1)
            {
                launchSkill(_skill);
                StartCoroutine(WaitForChargingTime(mChargingBar, _skill.Spec.MSkillRunTime, _btn, _skill));
            }
            // �ƴ� ���(�ͼ�Ƽ�� �ҵ�)
            else
            {

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
        Ujoystick.gameObject.SetActive(false);
        UBtn.SetActive(true);
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
    // ���������� �߻� �ڷ�ƾ���� ȣ���ϴ� �Լ�
    /*
    * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
    * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
     * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
    * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
    */
    private void launchProjectile(Skill mSkill, int mProjectileIndex, Vector3 mTargetPos, Vector3 mFirePos, bool mNotSingle)
    {
        int launchCount = TileDict[mSkill][mProjectileIndex].Spec.Count + Projectile.AddProjectilesCount;
        int angle = TileDict[mSkill][mProjectileIndex].Spec.Angle;
        // ������ ���� ��� �ѹ߸� �߻�
        for (int i = 0; i < launchCount; i++)
        {
            StartCoroutine(
                LaunchCorutines(
                    (launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i),
                    TileDict[mSkill][mProjectileIndex].gameObject.name,
                    mTargetPos,
                    mFirePos, mNotSingle));
        }
    }
    IEnumerator multiLuanch(Skill _skill, int _count, Vector3 _target, Vector3 _fire)
    {
        for (int i = 0; i < _count; i++)
        {
            launchProjectile(_skill, 0, _target, _fire, false);
            yield return new WaitForSeconds(0.4f); // ���Ŀ� ������ �߻��� ��� �ش� �����Ͱ� �Է�
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
    /*
* _angle : �߰� ���� �����Դϴ�.
* _name : �ش� �߻�ü������Ʈ�� name�Դϴ�.
*/
    IEnumerator LaunchCorutines(float _angle, string _name, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
        Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(_name);
        if (_notSingle) firstProjectile = obj;
        float keepTime = obj.GetComponent<Projectile>().Spec.SpawnTime;
        setProjectileData(ref obj);
        obj.GetComponent<Projectile>().CurrentPassCount = 0;
        obj.GetComponent<Projectile>().setEnable(_targetPos, _firePos, _angle);
        yield return new WaitForSeconds(keepTime);
        // ������ disable���������� �ش� ������Ʈ�� Active�� true�� �ø� disable�ǰ� �߽��ϴ�
        // �����ð��� �ٵǱ� �� �߻�ü�� ���Ϳ� �ε��� �� disable�� �ѹ� ȣ��Ǵµ� Ȥ�ó� �ٸ� ��ü��
        // �����ؼ� disable�ؼ� ������ ������ �߻��� �� �����Ŷ� �����ؼ� ���ǹ� �ɾ����ϴ�.
        // ���Ŀ� �ٸ��������� ������ �߰ߵǸ� ������ ����
        if (obj.activeInHierarchy)
        {
            obj.GetComponent<Projectile>().setDisable();
            ObjectPoolManager.Instance.DisableGameObject(obj);
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
        _bar.SetActive(true);
        float lefttime = _spawnTime;
        while(lefttime > 0f)
        {
            lefttime -= Time.deltaTime;
            _bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            _bar.transform.GetChild(0).GetComponent<Image>().fillAmount = (lefttime / _spawnTime);
            yield return new WaitForFixedUpdate();
        }
        _bar.SetActive(false);
        StartCoroutine(
                   WaitForCoolTime(_btn, _skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex], _skill.Spec.Type));

    }
}