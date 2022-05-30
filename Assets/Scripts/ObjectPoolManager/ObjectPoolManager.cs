using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingleToneMaker<ObjectPoolManager>
{
    private Dictionary<string, ObjectPool> dictTable;
    void Awake()
    {
        if (dictTable == null)
        {
            dictTable = new Dictionary<string, ObjectPool>();
        }
    }

    public void CreateDictTable(GameObject objectPrfab)
    {
        CreateDictTable(objectPrfab, 5, 0);
    }
    public void CreateDictTable(GameObject objectPrfab, int initCount, int overAllocateCount)
    {
        if (dictTable.ContainsKey(objectPrfab.name).Equals(false))
        {
            dictTable.Add(objectPrfab.name, new ObjectPool(objectPrfab, initCount, overAllocateCount));
        }
    }

    public bool ObjectPoolEmptyCheck(string name)
    {
        if (dictTable.ContainsKey(name).Equals(false))
        {
            Debug.Log("잘못된" + name + "이 들어왔습니다");
            return false;
        }
        else
        {
            return dictTable[name].IsNull();
        }
    }

    public GameObject EnableGameObject(string name)
    {
        GameObject obj = null;
        if (dictTable.ContainsKey(name).Equals(false))
        {
            Debug.Log("잘못된" + name + "이 들어왔습니다");
            return obj;
        }
        obj = dictTable[name].EnableObject();
        return obj;
    }

    //setactive dsiable된 GameObject에 대해서만 objectPool에 반환
    //object의 activeInHierarchy 상태를 체크하고 true일 경우만 오브젝트풀에 다시 반환하기때문에 Disable시 반드시 active true인 상태에서 호출할것
    public void DisableGameObject(GameObject obj)
    {
        if (dictTable.ContainsKey(obj.name).Equals(true))
        {
            dictTable[obj.name].DisableObject(obj);
        }
        else
        {
            Debug.Log("잘못된 obj " + obj.name + "이 들어왔습니다");
            Destroy(obj);
        }
    }
}
