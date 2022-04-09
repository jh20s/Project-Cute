using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : IAttack
{
    public Button GBtn;
    public Button UBtn;

    private GameObject firstProjectile;

    void Start()
    {
        // key : ��ų ���ӿ�����Ʈ value : �� ��ų�� �߻�ü ������Ʈ
        TileDict = new SkillDic();
        // ��ų�Ŵ����� ���Ͽ� �ش� ��ų�� �߻�ü�� �����´�
        getProjectiles();
        // �� �߻�ü�� ������Ʈ Ǯ ����
        createObjectPool();
        mAutoAttackSpeed = 1f; //TO-DO �ӽ÷� �־����. ���� ������ ����?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }

    void Update()
    {
        if (mAutoAttackCheckTime > mAutoAttackSpeed)
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

    private void getProjectiles()
    {
        // ��ų �Ŵ����� ���� ���� �������� ��ų�� �޾ƿ´�
        pushProjectile(SkillManager.Instance.CurrentBaseSkill);
        pushProjectile(SkillManager.Instance.CurrentGeneralSkill);
        pushProjectile(SkillManager.Instance.CurrentUltimateSkill);
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
                ObjectPoolManager.Instance.CreateDictTable(TileDict[key][i].gameObject, 10, 10);
            }
        }
    }

    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        obj.GetComponent<Projectile>().Damage = 
            (int)((50 + EquipmentManager.Instance.getCurrentDamage()) * obj.GetComponent<Projectile>().Spec.ProjectileDamage);
    }


    /*
     * ��ų Ŭ�� �� ȣ��˴ϴ�.
     */
    public void clickGeneralSkillBtn()
    {
        GBtn.gameObject.SetActive(false);
        StartCoroutine(useSkill(GBtn, SkillManager.Instance.CurrentGeneralSkill));
        StartCoroutine(activeAnimation());
    }
    public void clickUltimateSkillBtn()
    {
        UBtn.gameObject.SetActive(false);
        StartCoroutine(useSkill(UBtn, SkillManager.Instance.CurrentUltimateSkill));
        StartCoroutine(activeAnimation());
    }
    /*
     * ��ų�� ���� �� �ִϸ��̼� ���
     * ���� �ִϸ��̼� ����� ������ �Ұ�(������ ����)
     * Todo : PlayerManager������ �ִϸ��̼� �ڵ� �ű��
     */
    IEnumerator activeAnimation()
    {
        GetComponent<PlayerMove>().MMoveable = false;
        GetComponent<PlayerMove>().MAnim.SetFloat("AttackState", 0f);
        GetComponent<PlayerMove>().MAnim.SetFloat("NormalState", 0.5f);
        GetComponent<PlayerMove>().MAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f); // Todo : ���Ŀ� ��ų �����Ϳ��� ���ð��� �޾� �Է� 
        GetComponent<PlayerMove>().MMoveable = true;
    }

    IEnumerator useSkill(Button _btn, Skill _skill)
    {
        int count = _skill.Spec.SkillClickCount;
        // �ѹ� Ŭ��
        if (count == 1)
        {
            launchSkill(_skill);
            yield return new WaitForSeconds(_skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex]);
            _btn.gameObject.SetActive(true);
        }
        // ���� Ŭ��(�ð����� ����)
        else if (count == -1)
        {
            // Todo : ��Ÿ�ӵ��ȿ��� ���ѹ߻簡 ������ ��ų�� ��� 
        }
        // Ŭ��Ƚ���� ������ �ִ� ���
        else
        {
            // ������ Ŭ���� ���
            if (count - 1 == _skill.CurrentUseCount)
            {
                _skill.CurrentCoolTimeIndex++;
                launchSkill(_skill);
                yield return new WaitForSeconds(_skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex]);
                _skill.CurrentCoolTimeIndex = 0;
                _skill.CurrentUseCount = 0;
                _btn.gameObject.SetActive(true);
            }
            else
            {
                _skill.CurrentUseCount++;
                launchSkill(_skill);
                yield return new WaitForSeconds(_skill.Spec.getSkillCoolTime()[_skill.CurrentCoolTimeIndex]);
                _btn.gameObject.SetActive(true);
            }
        }
    }
    // �켱 �߻�ü�� �Ѱ��� ��츸 ����
    private void launchSkill(Skill _skill)
    {
        int count = _skill.Spec.SkillCount;
        int projectileCount = TileDict[_skill].Count;
        Vector3 targetPos = MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position);
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
        launchProjectile(_skill, 0, _target, _fire, true);
        yield return new WaitWhile(() => firstProjectile.activeInHierarchy);
        launchProjectile(_skill, 1, _target, firstProjectile.GetComponent<Projectile>().MyPos, false);
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
}