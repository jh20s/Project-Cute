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
        ObjectPoolManager.Instance.CreateDictTable(mAutoAttack, 10, 10); //TO-DO ������� Create�� ���⼭�ϴ°� ��������. PlayerManager���� ���������� Ȯ���� �������� �ű⼭ Create�ϴ°ɷ� ����
        mAutoAttackSpeed = mAutoAttack.GetComponent<Projectile>().Spec.SpawnTime; //TO-DO �ӽ÷� �־����. ���� ������ ����?
        mAutoAttackCheckTime = mAutoAttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (mAutoAttackCheckTime > mAutoAttackSpeed) {

            StartCoroutine(AutoAttackCorutines());
            /*
             * �߻�ü�� ���� ������ ���� ������ �߻� ���� �Դϴ�.(���� ������ ���� ���� �ٸ� Ÿ������ ���ߵ�)
             * luanchCount = �߻�ü�� �߻� ���� + (static ����)��ü���� �߻�ü�� �߻� ����(���������� ����)
             * angle = �߻�� �߻�ü�� �����Դϴ�.(�� �߻�ü������ ����)
             * luanchCount��ŭ �߻簡 �ǰ�, �߻�ɶ����� ������ŭ �����ݴϴ�.(������)
             */
            //{
            //    int launchCount = mAutoAttack.GetComponent<Projectile>().Spec.Count + Projectile.AddProjectilesCount;
            //    int angle = mAutoAttack.GetComponent<Projectile>().Spec.Angle;
            //    for (int i = 0; i < launchCount; i++)
            //    {
            //        StartCoroutine(AutoAttackCorutines(launchCount == 1 ? 0 : -((launchCount - 1) * angle / 2) + angle * i));
            //    }
            //}
            mAutoAttackCheckTime = 0f;
        }
        mAutoAttackCheckTime += Time.deltaTime;

    }

    IEnumerator AutoAttackCorutines()
    {
        Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject(mAutoAttack.name);
        //Debug.Log(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position));
        obj.GetComponent<Projectile>().setEnable(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position)
                , firePosition.transform.position, 0);
        yield return new WaitForSeconds(mAutoAttackSpeed);
        obj.GetComponent<Projectile>().setDisable();
        ObjectPoolManager.Instance.DisableGameObject(obj);
    }
    /*
    * _angle : �߰� ���� �����Դϴ�.
    */
    //IEnumerator AutoAttackCorutines(float _angle)
    //{
    //    Vector3 temp = new Vector3(1, 1, 0);//TO-DO �÷��̾� ���⿡ ���� �������ֵ��� value �������ִ� api�����
    //    GameObject obj = ObjectPoolManager.Instance.EnableGameObject(mAutoAttack.name);
    //    Debug.Log(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position));
    //    obj.GetComponent<Projectile>().setEnable(MonsterManager.Instance.GetNearestMonsterPos(firePosition.transform.position)
    //            , firePosition.transform.position, _angle);
    //    yield return new WaitForSeconds(mAutoAttackSpeed);
    //    obj.GetComponent<Projectile>().setDisable();
    //    ObjectPoolManager.Instance.DisableGameObject(obj);
    //}
}
