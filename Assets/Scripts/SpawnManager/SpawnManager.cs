using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnManager : SingleToneMaker<SpawnManager>
{
    private bool DEBUG = true;
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
        public float speedUp;
    }

    [SerializeField]
    private List<WaveData> dataSetWave;
    [SerializeField]
    private List<SpawnData> dataSetNormal;
    [SerializeField]
    private List<SpawnData> dataSetBoss;
    
    private float currentTime = 0f;

    public Transform[] spawnPoints;

    [SerializeField]
    private int mBossNum = 0;


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

    [SerializeField]
    private MapManager.MapType mMapType;

    [SerializeField]
    private Collider2D[] mCheckMonsterCount;
    [SerializeField]
    private Transform[] mMonsterArea;
    [SerializeField]
    private Vector2 mMonsterAreadSize = new Vector2(9f, 5f);
    [SerializeField]
    private Collider2D[] mHit;


    public void init()
    {
        allMonsterCount = 0;
        currentKillMosterCount = 0;
    }
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InitWaveData();       
        mBossNum = 0;
        mMonsterArea = GameObject.Find("MonsterCountCheckObject").transform.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        List<int> num = new List<int>();
        
        string s="";

        for (int i = 1; i < mMonsterArea.Length; i++) {
            mHit = Physics2D.OverlapBoxAll(mMonsterArea[i].transform.position, mMonsterAreadSize, 0, LayerMask.NameToLayer("Monster"));
            num.Add(Physics2D.OverlapBoxAll(mMonsterArea[i].transform.position, mMonsterAreadSize, 0).Length);
            if(DEBUG)
                s = s+" "+i+"�� ���ͼ�: " + num[i].ToString();
        }
        if(DEBUG)
            Debug.Log(s);
        */

        if (PlayerManager.Instance.IsGameStart) {
            SpawnMonster();
            currentTime += Time.deltaTime;
            allMonsterText.text = "��ü : " + allMonsterCount.ToString() + " ����";
            currentKillMonsterText.text = "ų : " + currentKillMosterCount.ToString() + " ����";
            currentAllMonsterText.text = "�ʵ� : " + (allMonsterCount - currentKillMosterCount).ToString() +" ����";
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < mMonsterArea.Length; i++)
        {
            Gizmos.DrawWireCube(mMonsterArea[i].position, mMonsterAreadSize);
        }
    }
    */
    private void SpawnMonster()
    {
        SpawnNormalMonster();
        SpawnBossMonster();
    }

    private void SpawnNormalMonster()
    {
        //��ָ� ����
        for (int i = 0; i < dataSetNormal.Count; i++)
        {
            SpawnData temp = dataSetNormal[i];
            if (dataSetNormal[i].realStartSpawnTime < currentTime && dataSetNormal[i].currentTime > dataSetNormal[i].spawnTime)
            {
                List<int> spawnZone = RandSpawnList(dataSetNormal[i].spawnNumber + mBossNum);//����� �䱸�������� ���̺긶�� +1������ �� ���� 
                for (int j = 0; j < spawnZone.Count; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetNormal[i].spawnMonster);
                    if (monster != null && spawnPoints[spawnZone[j]].GetComponent<SpawnZone>().Spawnable)
                    {
                        SpawnMonsterSet(ref monster, spawnPoints[spawnZone[j]].position);
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
    }

    private void SpawnBossMonster()
    {
        if (mBossNum < dataSetBoss.Count && dataSetBoss[mBossNum].realStartSpawnTime < currentTime)
        {

            int childCnt = GameObject.Find("ObjectPoolSet").transform.childCount;
            for(int i=0;i< childCnt; i++)
            {
                GameObject obj = GameObject.Find("ObjectPoolSet").transform.GetChild(i).gameObject;
                for(int j=0;j< dataSetNormal.Count; j++)
                {
                    if (dataSetNormal[j].spawnMonster.Equals(obj.name))
                    {
                        ObjectPoolManager.Instance.DisableGameObject(obj);
                    }
                }
            }


            //���� ��ȯ time �ʱ�ȭ
            for (int i = 0; i < dataSetNormal.Count; i++)
            {
                SpawnData temp = dataSetNormal[i];
                temp.currentTime = 0f;
                dataSetNormal[i] = temp;
            }

            GameObject monster = ObjectPoolManager.Instance.EnableGameObject(dataSetBoss[mBossNum].spawnMonster);
            monster.GetComponent<MonsterEventHandler>().registerHpObserver(registerBossHp);

            List<int> spawnZone = RandSpawnList(1);
            SpawnMonsterSet(ref monster, spawnPoints[spawnZone[0]].position);

            mBossNum++;

            if (bossMessageCoroutine != null)
                StopCoroutine(bossMessageCoroutine);
            bossMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "���� ����", 6, 0);
            StartCoroutine(bossMessageCoroutine);

            //TO-DO ������ ������ �ް� ���� �ʿ�
            currentTime = -60f;//���� ���̺� ���۽ð� 

            if (waveMessageCoroutine != null)
                StopCoroutine(waveMessageCoroutine);
            waveMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "wave " + mBossNum, 6, 60);
            StartCoroutine(waveMessageCoroutine);
        }
    }


    private void SpawnMonsterSet(ref GameObject monster, Vector3 _vec)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(monster.name);
        if(md.monsterGrade == MonsterManager.MonsterGrade.Boss)
        {
            monster.GetComponent<MonsterStatus>().Hp = md.monsterHp;
            monster.GetComponent<MonsterStatus>().MaxHP = md.monsterHp;
            
        }
        else {
            monster.GetComponent<MonsterStatus>().Hp = (int)((float)md.monsterHp * dataSetWave[mBossNum].hpScale);
            monster.GetComponent<MonsterStatus>().MaxHP = (int)((float)md.monsterHp * dataSetWave[mBossNum].hpScale);
        }

        monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        monster.GetComponent<MonsterStatus>().MonsterInName = md.monsterInName;
        monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;

        if (md.monsterGrade != MonsterManager.MonsterGrade.Boss)
            monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed + dataSetWave[mBossNum].speedUp;

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
            ws.hpScale = float.Parse(waveData[idx]["HpScale"].ToString());
            ws.speedUp = float.Parse(waveData[idx]["SpeedUp"].ToString());
            dataSetWave.Add(ws);
        }
    }



    public void InitAllSpawnData()
    {
        mMapType = MapManager.Instance.CurrentMapType;

        dataSetNormal = new List<SpawnData>();
        dataSetBoss = new List<SpawnData>();
        List<Dictionary<string, object>> spawnData = CSVReader.Read("CSVFile\\SpawnData");
        if (DEBUG)
            Debug.Log("���� ��Ÿ��" + mMapType);
        
        for (int idx = 0; idx < spawnData.Count; idx++)
        {
            if ((MapManager.MapType)Enum.Parse(typeof(MapManager.MapType), spawnData[idx]["SpawnMap"].ToString()) == mMapType) { 
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
                GameObject obj = Resources.Load<GameObject>("Prefabs\\Monster\\" + mMapType.ToString() + "\\" + spawnData[idx]["SpawnMonster"].ToString());

                if (MonsterManager.Instance.GetMonsterData(ds.spawnMonster).monsterGrade ==  MonsterManager.MonsterGrade.Boss)
                {
                    dataSetBoss.Add(ds);
                    ObjectPoolManager.Instance.CreateDictTable(obj, 1, 1);
                }
                else 
                {
                    ObjectPoolManager.Instance.CreateDictTable(obj, 100, 10);
                    dataSetNormal.Add(ds);
                }
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
                waveMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "wave " + mBossNum, 6, 0);
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
