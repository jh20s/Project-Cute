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
    public struct WaveData
    {
        public float hpScale;
    }

    /*
     * TO-DO Spawn_Map�� Enum���� �ؼ� ���� ���������� �𸣴� �ϴ� skip
     */

    public List<WaveData> dataSetWave;
    public List<SpawnData> dataSetNormal;
    public List<SpawnData> dataSetBoss;
    
    private float currentTime = 0f;

    public Transform[] spawnPoints;

    public GameObject[] TempMonster;

    [SerializeField]
    private int bossNum = 0;


    IEnumerator bossMessageCoroutine;
    IEnumerator waveMessageCoroutine;

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

    public void init()
    {
        allMonsterCount = 0;
        currentKillMosterCount = 0;
    }
    void Awake()
    {
        InitAllSpawnData();
        InitWaveData();
    }

    // Start is called before the first frame update
    void Start()
    {
        bossNum = 0;
        //TO-DO ��� create ���� ���ؾ���.
        for (int i = 0; i < TempMonster.Length; i++)
        {
            ObjectPoolManager.Instance.CreateDictTable(TempMonster[i], 5, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnMonster();
        currentTime += Time.deltaTime;

        allMonsterText.text = "��ü : " + allMonsterCount.ToString() + " ����";
        currentKillMonsterText.text = "ų : " + currentKillMosterCount.ToString() + " ����";
        currentAllMonsterText.text = "�ʵ� : " + (allMonsterCount - currentKillMosterCount).ToString() +" ����";
    }

    private void SpawnMonster()
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
                    if (monster != null && spawnPoints[spawnZone[j]].GetComponent<SpawnZone>().Spawnable)
                    {
                        SpawnMonster(ref monster, spawnPoints[spawnZone[j]].position);
                    }
                    else
                    {
                        //�����ѻ���. �̺κ� �αװ� ������ ����
                        Debug.Log(spawnZone[j] + "��° ��ֹ� ���� ��ȯ �ȵ�");
                    }
                }
                temp.currentTime = 0;
            }
            temp.currentTime += Time.deltaTime;
            dataSetNormal[i] = temp;
        }

        //���� ����
        if (bossNum  <dataSetBoss.Count && dataSetBoss[bossNum].realStartSpawnTime < currentTime)
        {
            for (int i = 0; i < dataSetNormal.Count; i++)
            {
                SpawnData temp = dataSetNormal[i];
                temp.currentTime = 0f;
                dataSetNormal[i] = temp;
            }


            GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetBoss[bossNum].spawnMonster);
            monster.GetComponent<MonsterEventHandler>().registerHpObserver(registerBossHp);
            bossNum++;

            List<int> spawnZone = RandSpawnList(1);
            SpawnMonster(ref monster, spawnPoints[spawnZone[0]].position);

            if(bossMessageCoroutine!=null)
                StopCoroutine(bossMessageCoroutine);
            bossMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "���� ����", 6, 0);
            StartCoroutine(bossMessageCoroutine);

            //TO-DO ������ ������ �ް� ���� �ʿ�
            currentTime = -60f;//���� ���̺� ���۽ð� 

            if(waveMessageCoroutine!=null)
                StopCoroutine(waveMessageCoroutine);
            waveMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "wave " + bossNum, 6, 60);
            StartCoroutine(waveMessageCoroutine);

            
        }
    }
    
    private void SpawnMonster(ref GameObject monster, Vector3 _vec)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(monster.name);
        monster.GetComponent<MonsterStatus>().Hp = (int)((float)md.monsterHp * dataSetWave[bossNum].hpScale);
        monster.GetComponent<MonsterStatus>().MaxHP = (int)((float)md.monsterHp * dataSetWave[bossNum].hpScale);
        monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        monster.GetComponent<MonsterStatus>().MonsterInName = md.monsterInName;
        monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;
        monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed;
        monster.GetComponent<MonsterStatus>().MoveSpeedRate = 1f;
        monster.GetComponent<MonsterStatus>().mIsDieToKillCount = false;
        monster.GetComponent<MonsterStatus>().mIsDieToGetExp = false;
        monster.transform.position = _vec;
        //TO-DO : monster�� ����� event�� ������ �����Ͽ� hp register�� Player���� �����ϵ��� ������ �ʿ�.
        monster.GetComponent<MonsterEventHandler>().registerHpObserver(GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().registerMonsterHp);

        monster.SetActive(true);
        allMonsterCount++;
    }


    private void InitWaveData()
    {
        dataSetWave = new List<WaveData>();
        List<Dictionary<string, object>> waveData = CSVReader.Read("CSVFile\\Wave");
        for (int idx = 0; idx < waveData.Count; idx++)
        {
            WaveData ws = new WaveData();
            Debug.Log("ws.hpScale :" + waveData[idx]["HpScale"]);
            ws.hpScale = float.Parse(waveData[idx]["HpScale"].ToString());
            dataSetWave.Add(ws);
        }
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
            ds.realStartSpawnTime = UnityEngine.Random.Range(ds.spawnStartTime_1, ds.spawnStartTime_2);
            ds.spawnTime = (int)spawnData[idx]["SpawnTime"];
            ds.currentTime = 0f;
            if (MonsterManager.Instance.GetMonsterData(ds.spawnMonster).monsterGrade ==  MonsterManager.MonsterGrade.Boss)
            {
                dataSetBoss.Add(ds);
            }
            else 
            {
                dataSetNormal.Add(ds);
            }
        }
    }
    private List<int> RandSpawnList(int cnt)
    {
        List<int> list = new List<int>();
        int temp = 0;
        //���ѷ����� ���ɼ��� ������? ��� spanw�� Spawnable���?
        while (list.Count < cnt && temp<100)
        {
            int idx = UnityEngine.Random.Range(0, spawnPoints.Length);
            if (spawnPoints[idx].GetComponent<SpawnZone>().Spawnable)
            {
                list.Add(idx);
            }
            temp++;
        }
        return list;
    }

    /*
     * TO-DO ����ȭ ��� ���� ���� �ʿ�
    //����ȭ ������ MonsterAttack���� �̵���Ű�°� ����
    IEnumerator (int i, GameObject _obj)
    {
        yield return new WaitForSeconds(60);
            if (monster.activeInHierarchy)
            {
                monster.GetComponent<SpriteRenderer>().color = Color.red;
                monster.GetComponent<MonsterAttack>().CloseAttackPower = 30;
            }
        }
    }
    */

    public void registerBossHp(int _hp, GameObject _obj)
    {
        if (_hp <= 0)
        {
            if (currentTime < 0f)
            {
                if (waveMessageCoroutine != null)
                    StopCoroutine(waveMessageCoroutine);
                waveMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "wave " + bossNum, 6, 0);
                StartCoroutine(waveMessageCoroutine);
            }
            currentTime = Mathf.Max(0f, currentTime);
        }
    }


    //TO-DO �������� �޽����� ���⼭�ϴ°� ������?
    IEnumerator SpawnMessage(GameObject _obj, string _txt, int _cnt, float _time)
    {
        yield return new WaitForSeconds(_time);

        Debug.Log(_obj.name + _txt + _cnt);
        _obj.GetComponent<Text>().text = _txt;
        for (int i = 0; i < _cnt; i++)
        {
            yield return new WaitForSeconds(0.5f);
            _obj.SetActive(!_obj.activeInHierarchy);
        }
        _obj.SetActive(false);
    }
}
