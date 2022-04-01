using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 
    �� ���� gameObject�� ���Ͽ� List�� ����Ͽ� GameObject���� �����س��´�.  

 */
public class ObjectPool
{
    private List<GameObject> objectPool;
    private GameObject objectFactory;
    private int overAllocateCount; //�ش� ������ 0�̾ƴϸ�, Ǯ�� �����ϸ� overAllocateCount��ŭ objectPool����

    public ObjectPool(GameObject objectFactory, int initCount, int overAllocateCount)
    {
        this.objectFactory = objectFactory;
        this.overAllocateCount = overAllocateCount;
        objectPool = new List<GameObject>();
        Allocate(initCount);
    }

    /*
     * ������ƮǮ�� ������ cnt������ŭ�÷��ش�.
     */
    public void Allocate(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = GameObject.Instantiate(objectFactory, GameObject.Find("ObjectPoolSet").transform);
            obj.name = objectFactory.name;
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }

    }

    /*
     *  ������ƮǮ�� ����ִ� GameObject�� ������.
     */
    public GameObject EnableObject()
    {

        if (objectPool.Count <= 0 && overAllocateCount <= 0)
        {
            Debug.Log(objectFactory.name+"�� Enable�Ҽ� �����ϴ�.");
            return null;
        }
        else if (objectPool.Count <= 0 && overAllocateCount > 0)
        {
            Debug.Log("������Ʈ ���� ��� " + overAllocateCount + "���� �߰��մϴ�");
            Allocate(overAllocateCount);
        }

        GameObject retObj = objectPool[0];
        objectPool.Remove(retObj);

        return retObj;
    }
    public void DisableObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Add(obj);
    }
}
