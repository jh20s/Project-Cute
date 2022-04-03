using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IAttack
{
    public GameObject mAutoAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        TileDict = new Dictionary<string, GameObject>();

        //setTileDict(mWeapone.name, mWeapone);
        ObjectPoolManager.Instance.CreateDictTable(mAutoAttack, 5, 0); //TO-DO ������� Create�� ���⼭�ϴ°� ��������. PlayerManager���� ���������� Ȯ���� �������� �ű⼭ Create�ϴ°ɷ� ����
        mAutoAttackSpeed = mAutoAttack.GetComponent<Launch>().Spec.SpawnTime; //TO-DO �ӽ÷� �־����. ���� ������ ����?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (mAutoAttackCheckTime > mAutoAttackSpeed) { 
            StartCoroutine(AutoAttackCorutines());
            mAutoAttackCheckTime = 0f;
        }
        mAutoAttackCheckTime += Time.deltaTime;

    }


    IEnumerator AutoAttackCorutines()
    {
        Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(mAutoAttack.name);
        Debug.Log(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position));
        obj.GetComponent<Launch>().setEnable(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position)
                , firePosition.transform.position);
        yield return new WaitForSeconds(mAutoAttackSpeed);
        obj.GetComponent<Launch>().setDisable();
        ObjectPoolManager.Instance.DisableGameObject(obj);
    }
}
