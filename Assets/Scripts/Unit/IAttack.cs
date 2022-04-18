using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{
    // ���� ��� �� ��ų�� �߻�ü 2�� �̻��ε�
    // ù�߻�ü�� disable�� position���� enable
    protected GameObject firstProjectile;


    //TO-Do value�� List<Projectile>�� ���� 
    protected SkillDic TileDict;
    public GameObject firePosition;

    [SerializeField]
    protected float mAutoAttackSpeed;
    protected float mAutoAttackCheckTime;

    [SerializeField]
    private Skill currentBaseSkill;
    public Skill CurrentBaseSkill
    {
        get { return currentBaseSkill; }
        set
        {
            currentBaseSkill = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // key : ��ų ���ӿ�����Ʈ value : �� ��ų�� �߻�ü ������Ʈ
    }

    // Update is called once per frame
    void Update()
    {

    }

      
    public void setTileDict(Skill _skill, List<Projectile> _projectiles)
    {
        TileDict.Add(_skill, _projectiles);
    }

    // �޾ƿ� ��ų�� �߻�ü ����Ʈ�� �߻�ü�Ŵ����� ���� �޾ƿ´�
    //  <��ų, �߻�ü����Ʈ> Ÿ���� Dic�� �߰�
    protected void pushProjectile(Skill _skill)
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



    // ���������� �߻� �ڷ�ƾ���� ȣ���ϴ� �Լ�
    /*
    * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
    * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
     * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
    * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
    */
    protected void launchProjectile(Skill mSkill, int mProjectileIndex, Vector3 mTargetPos, Vector3 mFirePos, bool mNotSingle)
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

    /*
    * _angle : �߰� ���� �����Դϴ�.
    * _name : �ش� �߻�ü������Ʈ�� name�Դϴ�.
    */
    protected IEnumerator LaunchCorutines(float _angle, string _name, Vector3 _targetPos, Vector3 _firePos, bool _notSingle)
    {
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


    protected IEnumerator multiLuanch(Skill _skill, int _count, Vector3 _target, Vector3 _fire)
    {
        for (int i = 0; i < _count; i++)
        {
            launchProjectile(_skill, 0, _target, _fire, false);
            yield return new WaitForSeconds(0.4f); // ���Ŀ� ������ �߻��� ��� �ش� �����Ͱ� �Է�
        }
    }

    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        obj.GetComponent<Projectile>().Damage = 
            (int)((gameObject.GetComponent<IStatus>().BaseDamage + gameObject.GetComponent<IStatus>().getCurrentWeponeDamage()) * obj.GetComponent<Projectile>().Spec.ProjectileDamage);
    }

    protected void createObjectPool()
    {
        foreach (Skill key in TileDict.Keys)
        {
            for (int i = 0; i < TileDict[key].Count; i++)
            {
                ObjectPoolManager.Instance.CreateDictTable(TileDict[key][i].gameObject, 10, 10);
            }
        }
    }

}
