using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{

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
        
    }

    // Update is called once per frame
    void Update()
    {

    }

      
    public void setTileDict(Skill _skill, List<Projectile> _projectiles)
    {
        TileDict.Add(_skill, _projectiles);
    }

    //TO-DO property ���� set�� value�� Dictionanry�ϰ�� ��� �ִ��� Ȯ���ϰ� �����ϸ� set�Լ� ��ü
    /*
    public Dictionary<string, GameObject> propTileDict
    {
        set
        {
            Debug.Log("what value"+value);
            //TO-Do C#
        }
        get {
            return TileDict;
        }

    }
    */

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


    // �߻�ü �������� �����մϴ�.
    private void setProjectileData(ref GameObject obj)
    {
        obj.GetComponent<Projectile>().Damage = 
            (int)((gameObject.GetComponent<IStatus>().BaseDamage + gameObject.GetComponent<IStatus>().getCurrentWeponeDamage()) * obj.GetComponent<Projectile>().Spec.ProjectileDamage);
    }



}
