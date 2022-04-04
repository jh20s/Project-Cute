using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnManager : SingleToneMaker<SpawnManager>
{
    public struct SpawnData
    {
        public int Id;
        public string spawnMonster;
        public string spawnMap;
        public int spawnOrder;
        public int spawnNumber;
        public float spawnStartTime_1;
        public float spawnStartTime_2;
        public float realStartSpawnTime; //TO-DO : �� ������ ���۵Ǹ� �̺κ��� reset�Ҽ� �ֵ��� ó�� �ʿ�
        public float spawnTime;
        public float currentTime;
    }
    /*
     * TO-DO Spawn_Map�� Enum���� �ؼ� ���� ���������� �𸣴� �ϴ� skip
     */

    public List<SpawnData> dataSet;
    private float currentTime = 0f;

    public Transform[] spawnPoints;

    public GameObject[] TempMonster;

    void Awake()
    {
        initAllSpawnData();
        /*
         * TO-DO: 
         * 1. spawn point�� Awake���� ������Ʈ�� ã�� �о���� ���
         * 2. MonsterManager�κ��� ���� ���� �޾ƿ��� API ������ �ӽ÷� MonsterA�� ������Ʈ �־��
         * 3. ���ͺ� ������ ��� �����ϴ°� ������ ����� �ʿ�. ��Ģ�� -> ��ȹ�е鿡�� ���ǰ� �ʿ��Ѻκ� -> �ϴ��� �׳� ����
         * 
         * */
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        for (int i = 0; i < TempMonster.Length; i++)
        {
            ObjectPoolManager.Instance.CreateDictTable(TempMonster[i], 5, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < dataSet.Count; i++)
        {
            SpawnData temp = dataSet[i];
            if (dataSet[i].realStartSpawnTime < currentTime && dataSet[i].currentTime > dataSet[i].spawnTime)
            {
                for (int j = 0; j < dataSet[i].spawnNumber; j++)
                {
                    GameObject enemy = ObjectPoolManager.Instance.EnableGameObject(dataSet[i].spawnMonster);
                    if (enemy != null)
                    {
                        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
                        enemy.transform.position = spawnPoints[index].position;
                        enemy.SetActive(true);
                    }
                }
                temp.currentTime = 0;
            }
            /*
             * TO-DO : C#�� Ư���� ����ü�� ����ü�� ������ �����Ҽ� ��� �������·� ¥��. ���߿� curretTime���� �迭���� ����ϴ°� ����
             */
            currentTime += Time.deltaTime;
            temp.currentTime += Time.deltaTime;
            dataSet[i] = temp;
        }
    }

    private void initAllSpawnData()
    {
        dataSet = new List<SpawnData>();
        List<Dictionary<string, object>> spawnData = CSVReader.Read("CSVFile\\SpawnData");
        for (int idx = 0; idx < spawnData.Count; idx++)
        {

            SpawnData ds = new SpawnData();
            ds.Id = (int)spawnData[idx]["ID"];
            ds.spawnMonster = spawnData[idx]["Spawn_Monster"].ToString();
            ds.spawnMap = spawnData[idx]["Spawn_Map"].ToString();
            ds.spawnOrder = (int)spawnData[idx]["Spawn_Order"];
            ds.spawnNumber = (int)spawnData[idx]["Spawn_Number"];
            ds.spawnStartTime_1 = (int)spawnData[idx]["Spawn_StartTime_1"];
            ds.spawnStartTime_2 = (int)spawnData[idx]["Spawn_StartTime_2"];
            ds.realStartSpawnTime = UnityEngine.Random.Range(ds.spawnStartTime_1, ds.spawnStartTime_2);
            ds.spawnTime = (int)spawnData[idx]["Spawn_Time"];
            ds.currentTime = 0f;
            dataSet.Add(ds);
        }
    }


}
