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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public GameObject EnableGameObject(string name)
    {
        GameObject obj = null;
        if (dictTable.ContainsKey(name).Equals(false))
        {
            Debug.Log("�߸���" + name + "�� ���Խ��ϴ�");
            return obj;
        }
        obj = dictTable[name].EnableObject();
        return obj;
    }
    public void DisableGameObject(GameObject obj)
    {
        if (dictTable.ContainsKey(obj.name).Equals(true))
        {
            dictTable[obj.name].DisableObject(obj);
        }
        else
        {
            Debug.Log("�߸��� obj " + obj.name + "�� ���Խ��ϴ�");
            Destroy(obj);
        }
    }
}
