using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingleToneMaker<MonsterManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Pos�� ���ڷ� �־��ָ� Ingame�� �ִ� ���� ����� Enemy Tag�� ���� ���� Position�� Return �����ش�.
     */
    public Vector3 GetNearestMonsterPos(Vector3 Pos)
    {
        Vector3 dir = new Vector3(0, 0, 0);
        GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        float diff = 99999999;
        foreach (GameObject enemy in AllEnemy)
        {
            float tempDiff = Mathf.Abs(enemy.transform.position.x - Pos.x) + Mathf.Abs(enemy.transform.position.y - Pos.y);
            if (tempDiff < diff)
            {
                diff = tempDiff;
                dir = enemy.transform.position;
                //dir.x = enemy.transform.position.x - Pos.x;
                //dir.y = enemy.transform.position.y - Pos.y;
            }
        }
        return dir;
    }

    /*
     * ���� �ʿ� ���� ���� List�� return ���ش�. 
     */
    public List<GameObject> GetMonsterList()
    {
        List<GameObject> temp = new List<GameObject>();

        return temp;
    }
}
