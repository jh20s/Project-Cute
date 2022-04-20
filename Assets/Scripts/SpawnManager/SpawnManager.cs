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
    public static int allMonsterCount = 0;
    // �ʵ忡 ��ȯ�� ������ ��
    public Text currentAllMonsterText;
    // ���� ���� ������ ��
    public Text currentKillMonsterText;
    public static int currentKillMosterCount = 0;

    private const int SPAWN_MINUT = 60;

    public void init()
    {
        allMonsterCount = 0;
        currentKillMosterCount = 0;
    }
    void Awake()
    {
        InitAllSpawnData();
    }

    // Start is called before the first frame update
    void Start()
    {
        //TO-DO ��� create ���� ���ؾ���.
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
                List<int> spawnZone = RandSpawnList(dataSetNormal[i].spawnNumber);
                for (int j = 0; j < spawnZone.Count; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetNormal[i].spawnMonster);
                    // Ÿ�ϸʰ� ��ġ��Ű���� ���� �������� intȭ�ؾ� �մϴ�.
                    
                    Vector3Int spawnPos = GameObject.Find("Grid").GetComponent<Grid>().WorldToCell(spawnPoints[spawnZone[j]].position);
                    if (monster != null && MapManager.Instance.SpawnalbePosition(spawnPos))
                    {
                        setMonsterData(ref monster);
                        monster.transform.position = spawnPos;
                        monster.GetComponent<MonsterStatus>().mIsDieToKillCount = false;
                        monster.GetComponent<MonsterStatus>().mIsDieToGetExp = false;
                        monster.SetActive(true);
                        // ������ ������ �� ����
                        allMonsterCount++;
                    }
                    // �켱�� �ȳ����� �س����ϴ� ���ƴ��� ���� �ٸ������� ������ ���� ��Ź�帳�ϴ�.
                    else
                    {
                        //Debug.Log(spawnPos + " �� ��ֹ� �����̶� ��ȯ �ȵ�");
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

    
    private void setMonsterData(ref GameObject monster)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(monster.name);
        monster.GetComponent<MonsterStatus>().Hp = md.monsterHp;
        monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        monster.GetComponent<MonsterStatus>().MonsterInName = md.monsterInName;
        monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;
        monster.GetComponent<MonsterStatus>().Speed = md.monsterSpeed;
        //TO-DO : monster�� ����� event�� ������ �����Ͽ� hp register�� Player���� �����ϵ��� ������ �ʿ�.
        monster.GetComponent<MonsterEventHandler>().registerHpObserver(GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().registerMonsterHp);
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
            ds.spawnMonster = spawnData[idx]["SpawnMonster"].ToString();
            ds.spawnMap = spawnData[idx]["SpawnMap"].ToString();
            ds.spawnOrder = (int)spawnData[idx]["SpawnOrder"];
            ds.spawnNumber = (int)spawnData[idx]["SpawnNumber"];
            ds.spawnStartTime_1 = (int)spawnData[idx]["SpawnStartTime1"];
            ds.spawnStartTime_2 = (int)spawnData[idx]["SpawnStartTime2"];
            ds.realStartSpawnTime = UnityEngine.Random.Range(ds.spawnStartTime_1 * SPAWN_MINUT, ds.spawnStartTime_2 * SPAWN_MINUT);
            ds.spawnTime = (int)spawnData[idx]["SpawnTime"];
            ds.currentTime = 0f;
            if (MonsterManager.Instance.GetMonsterData(ds.spawnMonster).monsterGrade ==  MonsterManager.MonsterGrade.Boss)
            {
                dataSetBoss.Add(ds);
            }
            else if(MonsterManager.Instance.GetMonsterData(ds.spawnMonster).monsterGrade == MonsterManager.MonsterGrade.Normal)
            {
                dataSetNormal.Add(ds);
            }
        }
    }
    private List<int> RandSpawnList(int cnt)
    {
        List<int> list = new List<int>();
        while (list.Count < cnt)
        {
            int idx = UnityEngine.Random.Range(0, spawnPoints.Length);
            if (!list.Contains(idx))
            {
                list.Add(idx);
            }
        }
        return list;
    }

    IEnumerator spawnBoss(int i)
    {
        yield return new WaitForSeconds(dataSetBoss[i].realStartSpawnTime);
        GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetBoss[i].spawnMonster);
        setMonsterData(ref monster);
        if (monster != null)
        {
            monster.GetComponent<MonsterEventHandler>().registerHpObserver(GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().registerMonsterHp);
            int index = UnityEngine.Random.Range(0, spawnPoints.Length);
            monster.transform.position = spawnPoints[index].position;
            monster.SetActive(true);

            //TO-DO : ���� ����ȭ ����. �ϴ��� �ϵ��ڵ��س��� ����.
            //csv���� �о���� �ش� �ð��� ���� �������ϵ��� �����ʿ�
            //����׸�带 ���� �������� �� 3�������� �̺κ� ����ó�� �� ���� �ʿ�
            yield return new WaitForSeconds(30);
            if (monster.activeInHierarchy)
            {
                monster.GetComponent<SpriteRenderer>().color = Color.red;
                monster.GetComponent<MonsterMove>().Speed = 10f;
                monster.GetComponent<MonsterAttack>().CloseAttackPower = 30;
            }
        }
    }
}
