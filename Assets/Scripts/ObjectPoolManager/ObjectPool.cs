using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 
    �� ���� gameObject�� ���Ͽ� List�� ����Ͽ� GameObject���� �����س��´�.  

 */
public class ObjectPool
{
    private Queue<GameObject> objectPool;
    private GameObject objectFactory;
    private int overAllocateCount; //�ش� ������ 0�̾ƴϸ�, Ǯ�� �����ϸ� overAllocateCount��ŭ objectPool����

    public ObjectPool(GameObject objectFactory, int initCount, int overAllocateCount)
    {
        this.objectFactory = objectFactory;
        this.overAllocateCount = overAllocateCount;
        objectPool = new Queue<GameObject>();
        Allocate(initCount);
    }

    //TO-DO ���� ������ƮǮ�� �����Ǹ� ObjectPoolSet�̶�� �� ������Ʈ�� �ְ� �ִµ� �̰��� ��� ���������� ���� ����� �ʿ�

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
            objectPool.Enqueue(obj);
        }

    }
    
    public bool IsNull()
    {
        return objectPool.Count == 0 ? true : false;       
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
        /*
         * TO-DO: 
         * �츮���ӿ����� �ڵ����� �þ���� ��������������? ������ ��� �ڵ����� �þ���ؾ��ϳ�? �߰� �޼��尡 �ʿ��ұ�
         * 
         */
        else if (objectPool.Count <= 0 && overAllocateCount > 0)
        {
            //Debug.Log("������Ʈ ���� ��� " + overAllocateCount + "���� �߰��մϴ�");
            Allocate(overAllocateCount);
        }

        GameObject retObj = objectPool.Dequeue();
        //objectPool.Remove(retObj);

        return retObj;
    }
    public void DisableObject(GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            objectPool.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("�ߺ� ȣ�� : " + obj.name);
        }

    }
}
