using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAttack : MonoBehaviour
{
    //TO-Do value�� Projectile�� ���� 
    protected Dictionary<string, GameObject> TileDict;
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

      
    public void setTileDict(string str, GameObject obj)
    {
        TileDict.Add(str, obj);
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
