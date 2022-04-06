using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : IAttack
{
    public GameObject mAutoAttack;
    public Button GBtn;
    public Button UBtn;
    // Start is called before the first frame update
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
    // Update is called once per frame
    void Update()
    {
        if (mAutoAttackCheckTime > mAutoAttackSpeed)
        {

            //StartCoroutine(AutoAttackCorutines());
            /*
             * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
             * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
             * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
             * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
             */
            // �ڵ����� �ż���
            {
                int launchCount = TileDict[SkillManager.Instance.CurrentBaseSkill][0].Spec.Count + Projectile.AddProjectilesCount;
                int angle = TileDict[SkillManager.Instance.CurrentBaseSkill][0].Spec.Angle;
                for (int i = 0; i < launchCount; i++)
                {
                    StartCoroutine(
                        AutoAttackCorutines((launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i), TileDict[SkillManager.Instance.CurrentBaseSkill][0].gameObject.name));
                }
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
        foreach(Skill key in TileDict.Keys)
        {
            for(int i = 0; i <  TileDict[key].Count; i++)
            {
                ObjectPoolManager.Instance.CreateDictTable(TileDict[key][i].gameObject, 10, 10);
            }
        }
    }

    //IEnumerator AutoAttackCorutines()
    //{
    //    Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
    //    GameObject obj = ObjectPoolManager.Instance.EnableGameObject(mAutoAttack.name);
    //    //Debug.Log(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position));
    //    obj.GetComponent<Projectile>().setEnable(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position)
    //            , firePosition.transform.position, 0);
    //    yield return new WaitForSeconds(mAutoAttackSpeed);
    //    obj.GetComponent<Projectile>().setDisable();
    //    ObjectPoolManager.Instance.DisableGameObject(obj);
    //}
    /*
    * _angle : �߰� ���� �����Դϴ�.
    */
    IEnumerator AutoAttackCorutines(float _angle, string _name)
    {
        Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(_name);
        Debug.Log(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position));
        obj.GetComponent<Projectile>().setEnable(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position)
                , firePosition.transform.position, _angle);
        yield return new WaitForSeconds(mAutoAttackSpeed);
        obj.GetComponent<Projectile>().setDisable();
        ObjectPoolManager.Instance.DisableGameObject(obj);
    }
}
