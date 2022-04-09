using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public List<SpawnData> dataSetNormal;
    public List<SpawnData> dataSetBoss;
    private float currentTime = 0f;

    public Transform[] spawnPoints;

    public GameObject[] TempMonster;

    /*
     *  �ӽ÷� �����Ŵ����� ���� ���� Todo : ���Ŀ� ���� ������ ���� �÷��̾� �Ŵ�����
     *  ���߿� ���ӸŴ������� ����
     */
    // ��ü ������ ��
    public Text allMonsterText;
    public static int allMonsterCount;
    // �ʵ忡 ��ȯ�� ������ ��
    public Text currentAllMonsterText;
    // ���� ���� ������ ��
    public Text currentKillMonsterText;
    public static int currentKillMosterCount;

    void Awake()
    {
        InitAllSpawnData();
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
        spawnBossMonster();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnNormalMonster();
        allMonsterText.text = "��ü : " + allMonsterCount.ToString() + " ����";
        currentKillMonsterText.text = "ų : " + currentKillMosterCount.ToString() + " ����";
        currentAllMonsterText.text = "�ʵ� : " + (allMonsterCount - currentKillMosterCount).ToString() +" ����";
    }

    private void SpawnNormalMonster()
    {
        for (int i = 0; i < dataSetNormal.Count; i++)
        {
            SpawnData temp = dataSetNormal[i];
            if (dataSetNormal[i].realStartSpawnTime < currentTime && dataSetNormal[i].currentTime > dataSetNormal[i].spawnTime)
            {
                for (int j = 0; j < dataSetNormal[i].spawnNumber; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetNormal[i].spawnMonster);
                    setMonsterData(ref monster);
                    if (monster != null)
                    {
                        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
                        monster.transform.position = spawnPoints[index].position;
                        monster.SetActive(true);
                        // ������ ������ �� ����
                        allMonsterCount++;
                    }
                }
                temp.currentTime = 0;
            }
            /*
             * TO-DO : C#�� Ư���� ����ü�� ����ü�� ������ �����Ҽ� ��� �������·� ¥��. ���߿� curretTime���� �迭���� ����ϴ°� ����
             */
            currentTime += Time.deltaTime;
            temp.currentTime += Time.deltaTime;
            dataSetNormal[i] = temp;
        }
    }

    private void spawnBossMonster()
    {
        for (int i = 0; i < dataSetBoss.Count; i++)
        {
            StartCoroutine(spawnBoss(i));
        }
    }

    /*
     * TO-DO : ����� MonsterManager���� data�� ������ �����ϴ� �����θ� �س���. ���� ���̵��� ���� ����ȭ�� �ʿ�
     */
    private void setMonsterData(ref GameObject monster)
    {
        /*
         *  TO-Do : �� �������� ���´� �Ʒ��� ���� ������� ������ ����. ������ ���Ҽ� �ֱ⶧���� �ϴ� ���� �ʿ��� hp,attack�� ¥����.
         */
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(monster.name);
        monster.GetComponent<MonsterStatus>().Hp = md.monsterHp;
        monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;
        monster.GetComponent<MonsterMove>().Speed = md.monsterSpeed;
    }

    private void InitAllSpawnData()
    {
        dataSetNormal = new List<SpawnData>();
        dataSetBoss = new List<SpawnData>();
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
            if (ds.spawnTime == 0)
            {
                dataSetBoss.Add(ds);
            }
            else
            {
                dataSetNormal.Add(ds);
            }
        }
    }


    IEnumerator spawnBoss(int i)
    {
        yield return new WaitForSeconds(dataSetBoss[i].realStartSpawnTime);
        GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetBoss[i].spawnMonster);
        setMonsterData(ref monster);
        if (monster != null)
        {
            int index = UnityEngine.Random.Range(0, spawnPoints.Length);
            monster.transform.position = spawnPoints[index].position;
            monster.SetActive(true);
        }
    }
}
