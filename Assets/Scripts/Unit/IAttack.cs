using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{
    //TO-Do value�� List<Projectile>�� ���� 
    protected SkillDic TileDict;
    public GameObject firePosition;

    protected float mAutoAttackSpeed;
    protected float mAutoAttackCheckTime;

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
}
