using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnManager : SingleToneMaker<SpawnManager>
{
    private bool DEBUG = false;

    //CSVFile/SpawnData.csv ������ ����� ����ü 
    //���� spawnRule ����
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

    //CSVFile/Wave.csv ������ ����� ����ü 
    //Wave�� ���� ���� stat����
    public struct WaveData
    {
        public float hpScale;
        public float speedUp;
    }

    //CSVFile/BossBerserkerData.csv ������ ����� ����ü 
    //�������� ����Ŀ��� ���� ����
    public struct BerserkerData
    {
        public int bossLimitTime;
        //�������� �ӵ� ���
        public float speedUp;
        //�������� ���ݷ� ���
        public float powerUp;
    }


    [SerializeField]
    private List<WaveData> mDataSetWave;
    [SerializeField]
    private List<SpawnData> mDataSetNormal;
    [SerializeField]
    private List<SpawnData> mDataSetBoss;
    [SerializeField]
    private Dictionary<string, BerserkerData> mDataSetBerserker;

    [SerializeField]
    private float currentTime = 0f;

    //������ ���� ������ȣ üũ & WaveNumber
    [SerializeField]
    private int mBossNum = 0;

    //TO-DO UI�ʿ��� �����ϵ��� �����ʿ�
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

    //spawn�꿡 ���õ� ��� ����
    [SerializeField]
    private Transform[] mMonsterArea;
    [SerializeField]
    private Vector2 mMonsterAreadSize = new Vector2(9f, 5f);
    [SerializeField]
    private List<int> mMonsterAreaNum;
    [SerializeField]
    private Transform[,] mSpawnPoints;

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
        //swapZone �ʱ�ȭ
        InitSwapZone();

        //������ �о����
        InitBerserkerData();
        InitWaveData();

        //boss üũ �ʱ�ȭ
        mBossNum = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerManager.Instance.IsGameStart)
        {

            string s ="";

            for (int i = 1; i < mMonsterArea.Length; i++) {
                mMonsterAreaNum[i-1] = Physics2D.OverlapBoxAll(mMonsterArea[i].transform.position, mMonsterAreadSize, 0, LayerMask.GetMask("Monster")).Length;
                if(DEBUG)
                    s = s+" "+i+"�� ���ͼ�: " + mMonsterAreaNum[i-1].ToString();
            }
            if(DEBUG)
                Debug.Log(s);

            SpawnMonster();
            currentTime += Time.deltaTime;
            allMonsterText.text = "��ü : " + allMonsterCount.ToString() + " ����";
            currentKillMonsterText.text = "ų : " + currentKillMosterCount.ToString() + " ����";
            currentAllMonsterText.text = "�ʵ� : " + (allMonsterCount - currentKillMosterCount).ToString() +" ����";
        }
    }

    /*
    spawnzone üũ�� ����� gizmos
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

    //NormalMonster ����
    private void SpawnNormalMonster()
    {
        //��ָ� ����
        for (int i = 0; i < mDataSetNormal.Count; i++)
        {
            SpawnData monsterData = mDataSetNormal[i];
            if (mDataSetNormal[i].realStartSpawnTime < currentTime && mDataSetNormal[i].currentTime > mDataSetNormal[i].spawnTime)
            {
                List<Transform> spawnZone = GetRandomZoneList(mDataSetNormal[i].spawnNumber + mBossNum);//����� �䱸�������� ���̺긶�� +1������ �� ���� 
                for (int j = 0; j < spawnZone.Count; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetNormal[i].spawnMonster);
                    if (monster != null && spawnZone[j].GetComponent<SpawnZone>().Spawnable)
                    {
                        SpawnMonsterSet(ref monster, spawnZone[j].position);
                    }
                    else
                    {
                        //�����ѻ���. �̺κ� �αװ� ������ ����
                        Debug.Log(spawnZone[j] + "��° ��ֹ� ���� ��ȯ �ȵ�");
                    }
                }
                monsterData.currentTime = 0;
            }
            monsterData.currentTime += Time.deltaTime;
            mDataSetNormal[i] = monsterData;
        }
    }


    //�������� ����
    private void SpawnBossMonster()
    {
        if (mBossNum < mDataSetBoss.Count && mDataSetBoss[mBossNum].realStartSpawnTime < currentTime)
        {

            int childCnt = GameObject.Find("ObjectPoolSet").transform.childCount;
            for(int i=0;i< childCnt; i++)
            {
                GameObject obj = GameObject.Find("ObjectPoolSet").transform.GetChild(i).gameObject;
                for(int j=0;j< mDataSetNormal.Count; j++)
                {
                    if (obj.activeInHierarchy && mDataSetNormal[j].spawnMonster.Equals(obj.name))
                    {
                        ObjectPoolManager.Instance.DisableGameObject(obj);
                    }
                }
            }


            //���� ��ȯ time �ʱ�ȭ
            for (int i = 0; i < mDataSetNormal.Count; i++)
            {
                SpawnData temp = mDataSetNormal[i];
                temp.currentTime = 0f;
                mDataSetNormal[i] = temp;
            }

            GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetBoss[mBossNum].spawnMonster);
            monster.GetComponent<MonsterEventHandler>().registerHpObserver(registerBossHp);

            List<Transform> spawnZone = GetRandomZoneList(1);
            SpawnMonsterSet(ref monster, spawnZone[0].position);

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

            //����Ŀ��� �ڷ�ƾ
            StartCoroutine(BossWaitBerserkerMode(monster));
        }
    }

    //������ ���� status ����
    private void SpawnMonsterSet(ref GameObject _monster, Vector3 _pos)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(_monster.name);
        if(md.monsterGrade == MonsterManager.MonsterGrade.Boss)
        {
            _monster.GetComponent<MonsterStatus>().Hp = md.monsterHp;
            _monster.GetComponent<MonsterStatus>().MaxHP = md.monsterHp;
            _monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed;

        }
        else {
            _monster.GetComponent<MonsterStatus>().Hp = (int)((float)md.monsterHp * mDataSetWave[mBossNum].hpScale);
            _monster.GetComponent<MonsterStatus>().MaxHP = (int)((float)md.monsterHp * mDataSetWave[mBossNum].hpScale);
            _monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed + mDataSetWave[mBossNum].speedUp;
        }

        _monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        _monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        _monster.GetComponent<MonsterStatus>().MonsterInName = md.monsterInName;
        _monster.GetComponent<MonsterStatus>().MoveSpeedRate = 1f;
        _monster.GetComponent<MonsterStatus>().mIsDieToKillCount = false;
        _monster.GetComponent<MonsterStatus>().mIsDieToGetExp = false;
        _monster.transform.position = _pos;


        _monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;

        _monster.GetComponent<IMove>().Moveable = true;
        _monster.GetComponent<BoxCollider2D>().isTrigger = false;
        Color monsterColor = _monster.GetComponent<SpriteRenderer>().color;
        monsterColor.a = 1f;
        _monster.GetComponent<SpriteRenderer>().color = monsterColor;

        //TO-DO : monster�� ����� event�� ������ �����Ͽ� hp register�� Player���� �����ϵ��� ������ �ʿ�.
        _monster.GetComponent<MonsterEventHandler>().registerHpObserver(PlayerManager.Instance.Player.GetComponent<PlayerStatus>().registerMonsterHp);

        _monster.SetActive(true);
        allMonsterCount++;
    }

    //swap�� �ʱ�ȭ
    private void InitSwapZone()
    {
        mMonsterAreaNum = new List<int>() { 0, 0, 0, 0 };
        mSpawnPoints = new Transform[GameObject.Find("SwapZone").transform.childCount, GameObject.Find("Zone0").transform.childCount];
        mMonsterArea = GameObject.Find("MonsterCountCheckObject").transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < GameObject.Find("SwapZone").transform.childCount; i++)
        {
            for (int j = 0; j < GameObject.Find("Zone" + i.ToString()).transform.childCount; j++)
            {
                mSpawnPoints[i, j] = GameObject.Find("Zone" + i.ToString()).transform.GetChild(j);
            }
        }
    }

    //Wave������ ����
    private void InitWaveData()
    {
        mDataSetWave = new List<WaveData>();
        List<Dictionary<string, object>> waveData = CSVReader.Read("CSVFile\\Wave");
        for (int idx = 0; idx < waveData.Count; idx++)
        {
            WaveData ws = new WaveData();
            ws.hpScale = float.Parse(waveData[idx]["HpScale"].ToString());
            ws.speedUp = float.Parse(waveData[idx]["SpeedUp"].ToString());
            mDataSetWave.Add(ws);
        }
    }

    //BerserkerData ����

    private void InitBerserkerData()
    {
        mDataSetBerserker = new Dictionary<string, BerserkerData>();
        List<Dictionary<string, object>> waveData = CSVReader.Read("CSVFile\\BossBerserkerData");
        for (int idx = 0; idx < waveData.Count; idx++)
        {
            BerserkerData ws = new BerserkerData();
            string name = waveData[idx]["BossMonster"].ToString();

            ws.bossLimitTime = int.Parse(waveData[idx]["BossLimitTime"].ToString());
            ws.powerUp = float.Parse(waveData[idx]["SpeedUp"].ToString());
            ws.speedUp = float.Parse(waveData[idx]["PowerUp"].ToString());
            mDataSetBerserker[name] = ws;
        }
    }

    //���� ������ ����
    public void InitAllSpawnData()
    {
        mMapType = MapManager.Instance.CurrentMapType;

        mDataSetNormal = new List<SpawnData>();
        mDataSetBoss = new List<SpawnData>();
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
                    mDataSetBoss.Add(ds);
                    ObjectPoolManager.Instance.CreateDictTable(obj, 1, 1);
                }
                else 
                {
                    ObjectPoolManager.Instance.CreateDictTable(obj, 100, 20);
                    mDataSetNormal.Add(ds);
                }
            }
        }
    }

    //return : cnt������ŭ ���ͼ��� ���� Zone���� spawn
    private List<Transform> GetRandomZoneList(int _cnt)
    {
        //mMonsterAreaNum���� ���Ͱ� ���� ���� ��ġ üũ
        //�ش� ��ġ���� �������� ����
        int nextSpawnIdx = 0;
        int checkMonsterNum = int.MaxValue;
        int zoneLength;
        List<Transform> randomList = new List<Transform>();

        for (int i = 0; i < mMonsterAreaNum.Count; i++)
        {
            if (mMonsterAreaNum[i] < checkMonsterNum)
            {
                nextSpawnIdx = i;
                checkMonsterNum = mMonsterAreaNum[i];
            }
        }
        if (DEBUG)
            Debug.Log("Zone" + nextSpawnIdx.ToString() + "���� ����");

        zoneLength = GameObject.Find("Zone" + nextSpawnIdx.ToString()).transform.childCount;

        
        //��� ���������� ������ �ȵɼ��� �ֱ⶧���� infiniteCheck
        int infiniteCheck = 0;

        while (randomList.Count < _cnt && infiniteCheck < 100)
        {
            int idx = UnityEngine.Random.Range(0, zoneLength);
            if (mSpawnPoints[nextSpawnIdx,idx].GetComponent<SpawnZone>().Spawnable)
            {
                randomList.Add(mSpawnPoints[nextSpawnIdx, idx]);
            }
            infiniteCheck++;
        }
        return randomList;
    }

    //���� ����Ŀ��� ��� �ڷ�ƾ
    IEnumerator BossWaitBerserkerMode (GameObject _obj)
    {
        BerserkerData ws;
        ws = mDataSetBerserker[_obj.name];
        if (DEBUG)
            Debug.Log("����Ŀ��� ���� ���ð�"+ ws.bossLimitTime);

        yield return new WaitForSeconds(ws.bossLimitTime);

        if (_obj.activeInHierarchy)
        {
            
            if(DEBUG)
                Debug.Log("����Ŀ��� ���� " + gameObject.name + " speedUp:" + ws.speedUp + " , powerUp" + ws.powerUp);
            _obj.GetComponent<IStatus>().MoveSpeed *= ws.speedUp;
            _obj.GetComponent<MonsterAttack>().BerserkerModeScale = ws.powerUp;
        }

    }
    
    //TO-DO �������� ü���� �����ִ� UI, UIManager��� �����ϵ��� ������ �ʿ����� ����ʿ�
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
