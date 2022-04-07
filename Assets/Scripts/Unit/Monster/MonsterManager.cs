using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingleToneMaker<MonsterManager>
{
 
    //TO-DO : ������� ���������� ��� class�� ���� �ٸ��ʿ����� ��ü �����ϱ� �����Ұŷ� ���� �ƴ� 
    public struct MonsterData
    {
        public int id;
        public string monsterSpawnMap;
        public string monsterInName;
        public int monsterSize;
        public string monsterName;
        public string monsterGrade;
        public int monsterHp;
        public int monsterPhysicalDefense;
        public int monsterMagicDefense;
        public int monsterSpeed;
        public int closeAttackPower;
        public int closeAttackRange;
        public int standoffAttackPower;
        public int standoffAttackRange;
        //public int MonsterAI;
        public int monsterExp;

        //public int MonsterDrop1;
        //public int MonsterDrop2;
        //public int MonsterDrop3;
    }

    private Dictionary<string, MonsterData> dataSet;

    private void Awake()
    {
        InitAllSpawnData();
    }

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
        GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag("Monster");
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
     * ���� string�� str�� �־��ָ� ���� data�� return ���ش�.
     */
    
    public MonsterData GetMonsterData(string str)
    {
        /*
         * TO-DO ���� key���� ���� ����ó�� �ʿ�
         * */
        return dataSet[str];
    }

    /*
     * ���� �ʿ� ���� ���� List�� return ���ش�. 
     */
    public List<GameObject> GetMonsterList()
    {
        List<GameObject> temp = new List<GameObject>();

        return temp;
    }

    /*
     * CSVFile\\MonsterData�� data�� �о dataSet�� ����
     */
    private void InitAllSpawnData()
    {
        dataSet = new Dictionary<string, MonsterData>();
        List<Dictionary<string, object>> monsterDataCsv = CSVReader.Read("CSVFile\\MonsterData");
        for (int idx = 0; idx < monsterDataCsv.Count; idx++)
        {
            string key;
            MonsterData md = new MonsterData();
            md.id = int.Parse(monsterDataCsv[idx]["ID"].ToString());
            md.monsterSpawnMap = monsterDataCsv[idx]["MonsterSpawnMap"].ToString();
            key = monsterDataCsv[idx]["MonsterInName"].ToString();
            md.monsterSize = int.Parse(monsterDataCsv[idx]["MonsterSize"].ToString());
            md.monsterName = monsterDataCsv[idx]["MonsterName"].ToString();
            md.monsterGrade = monsterDataCsv[idx]["MonsterGrade"].ToString();
            md.monsterHp = int.Parse(monsterDataCsv[idx]["MonsterHp"].ToString());
            md.monsterPhysicalDefense = int.Parse(monsterDataCsv[idx]["MonsterPhysicalDefense"].ToString());
            md.monsterMagicDefense = int.Parse(monsterDataCsv[idx]["MonsterMagicDefense"].ToString());
            md.monsterSpeed = int.Parse(monsterDataCsv[idx]["MonsterSpeed"].ToString());
            md.closeAttackPower = int.Parse(monsterDataCsv[idx]["CloseAttackPower"].ToString());
            md.closeAttackRange = int.Parse(monsterDataCsv[idx]["CloseAttackRange"].ToString());
            md.standoffAttackPower = int.Parse(monsterDataCsv[idx]["StandoffAttackPower"].ToString());
            md.standoffAttackRange = int.Parse(monsterDataCsv[idx]["StandoffAttackRange"].ToString());
            md.monsterExp = int.Parse(monsterDataCsv[idx]["MonsterExp"].ToString());
            //ds.CloseAttackAnimation = (int)monsterDataCsv[idx]["CloseAttackAnimation"];
            //ds.StandoffAttackAnimation = (int)monsterDataCsv[idx]["StandoffAttackAnimation"];
            //ds.HitAnimation = (int)monsterDataCsv[idx]["HitAnimation"];
            //ds.MonsterAI = (int)monsterDataCsv[idx]["MonsterAI"];
            //ds.spawnStartTime_2 = (int)monsterDataCsv[idx]["ChaseAnimation"];
            //ds.spawnStartTime_2 = (int)monsterDataCsv[idx]["DeathAnimation"];
            //ds.MonsterDrop1 = (int)monsterDataCsv[idx]["MonsterDrop1"];
            //ds.MonsterDrop2 = (int)monsterDataCsv[idx]["MonsterDrop2"];
            //ds.MonsterDrop3 = (int)monsterDataCsv[idx]["MonsterDrop3"];
            dataSet[key] = md;
        }
    }


}
