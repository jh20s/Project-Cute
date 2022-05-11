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
        public int monsterRank;
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
    private List<SpawnData> mDataSetRelayBoss;
    [SerializeField]
    private Dictionary<Tuple<string, int>, BerserkerData> mDataSetBerserker;

    [SerializeField]
    private float currentTime = 0f;

    //������ ���� ������ȣ üũ & WaveNumber
    
    //���� ������ ���� ����
    [SerializeField]
    private bool mIsSpawnRelayBoss = false;

    [SerializeField]
    private int mWaveCount = 0;

    public int WaveCount
    {
        get { return mWaveCount; }
    }

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
    // ���� ���� ���������� ��
    public static int currentKillBossMonsterCount = 0;

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
        currentKillBossMonsterCount = 0;
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
        mWaveCount = 0;
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
            //allMonsterText.text = "��ü : " + allMonsterCount.ToString() + " ����";
            currentKillMonsterText.text = "óġ : " + currentKillMosterCount.ToString() + " ����";
            //currentAllMonsterText.text = "�ʵ� : " + (allMonsterCount - currentKillMosterCount).ToString() +" ����";
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
        SpawnRelayBossMonster();
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
                //TO-DO : �̹��� wave�� ���� ���� ���� �����ϸ� �ȵǼ� �ϵ��ڵ����� �ӽ�����
                //���� type�̳� plus�� �� ���θ� csv�� �޾� �����Ͱ����� �����ϵ��� ���� �ʿ�
                int plusSwapnCount = mDataSetNormal[i].spawnMonster.Equals("Mimic") ? 0 : mWaveCount;

                List <Transform> spawnZone = GetRandomZoneList(mDataSetNormal[i].spawnNumber + plusSwapnCount);//����� �䱸�������� ���̺긶�� +1������ �� ���� 
                for (int j = 0; j < spawnZone.Count; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetNormal[i].spawnMonster);
                    if (monster != null && spawnZone[j].GetComponent<SpawnZone>().Spawnable)
                    {
                        SpawnMonsterSet(ref monster, mDataSetNormal[i].monsterRank, spawnZone[j].position);
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
        if (mWaveCount < mDataSetBoss.Count && mDataSetBoss[mWaveCount].realStartSpawnTime < currentTime)
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

            GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetBoss[mWaveCount].spawnMonster);
            monster.GetComponent<MonsterEventHandler>().registerIsDieObserver(registerBossDieCheck);

            
            List<Transform> spawnZone = GetRandomZoneList(1);
            SpawnMonsterSet(ref monster, mDataSetBoss[mWaveCount].monsterRank, spawnZone[0].position);

            mWaveCount++;

            //�������� �˸�
            if (bossMessageCoroutine != null)
                StopCoroutine(bossMessageCoroutine);
            bossMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "���� ����", 6, 0);
            StartCoroutine(bossMessageCoroutine);

            //�������̺� ���۾˸�
            currentTime = -60f;//���� ���̺� ���۽ð� 
            //60�� ���� ���� ���̺� ���� UI�� ����ش�.
            MakeWaveAlarm(60f);
            //����Ŀ��� �ڷ�ƾ
            StartCoroutine(BossWaitBerserkerMode(monster));
        }
    }


    private void SpawnRelayBossMonster()
    {
        if (!mIsSpawnRelayBoss && mMapType == MapManager.MapType.BossRelay)
        {
            mIsSpawnRelayBoss = true;
           
            GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetRelayBoss[mWaveCount].spawnMonster);
            monster.GetComponent<MonsterEventHandler>().registerIsDieObserver(registerBossRelayDieCheck);

            SpawnMonsterSet(ref monster, mDataSetRelayBoss[mWaveCount].monsterRank, GameObject.Find("BossRelaySpawnZone").transform.position);
            mWaveCount++;

            if (bossMessageCoroutine != null)
                StopCoroutine(bossMessageCoroutine);
            bossMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, mWaveCount + "��° ���� ����", 6, 0);
            StartCoroutine(bossMessageCoroutine);
        }        
    }

    //������ ���� status ����
    private void SpawnMonsterSet(ref GameObject _monster, int _rank, Vector3 _pos)
    {
        MonsterManager.MonsterData md = MonsterManager.Instance.GetMonsterData(_monster.name, _rank);
        if(md.monsterGrade == MonsterManager.MonsterGrade.Boss)
        {
            _monster.GetComponent<MonsterStatus>().Hp = md.monsterHp;
            _monster.GetComponent<MonsterStatus>().MaxHP = md.monsterHp;
            _monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed;
            _monster.GetComponent<CircleCollider2D>().enabled = true;
        }
        else {
            _monster.GetComponent<MonsterStatus>().Hp = (int)((float)md.monsterHp * mDataSetWave[mWaveCount].hpScale);
            _monster.GetComponent<MonsterStatus>().MaxHP = (int)((float)md.monsterHp * mDataSetWave[mWaveCount].hpScale);
            _monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed + mDataSetWave[mWaveCount].speedUp;
        }

        _monster.GetComponent<MonsterStatus>().IsDie = false;
        _monster.GetComponent<MonsterMove>().IsDie = false;
        _monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        _monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        _monster.GetComponent<MonsterStatus>().MonsterRank = md.monsterRank;
        _monster.GetComponent<MonsterStatus>().IsBerserker = false;
        _monster.GetComponent<MonsterStatus>().MoveSpeedRate = 1f;
        _monster.transform.position = _pos;


        _monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;

        //TO-DO MonsterEventHandler���� MonsterDie�� ���� ��ġ�ؾ� ����ġ �ʴ� ���װ� �����ȴ�. �ش� �κ��� apiȭ�� �س��°� ���� ������ ���ƺ��δ�.
        _monster.GetComponent<IMove>().Moveable = true;
        Color monsterColor = _monster.GetComponent<SpriteRenderer>().color;
        ColorUtility.TryParseHtmlString(md.monsterColor.ToString(), out monsterColor);


        _monster.GetComponent<SpriteRenderer>().color = monsterColor;
        _monster.GetComponent<MonsterAttack>().enabled = true;
        _monster.GetComponent<CapsuleCollider2D>().enabled = true;
        

        //TO-DO : monster�� ����� event�� ������ �����Ͽ� hp register�� Player���� �����ϵ��� ������ �ʿ�.
        _monster.GetComponent<MonsterEventHandler>().registerIsDieObserver(PlayerManager.Instance.Player.GetComponent<PlayerStatus>().registerMonsterDie);

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
        mDataSetBerserker = new Dictionary<Tuple<string,int>, BerserkerData>();
        List<Dictionary<string, object>> waveData = CSVReader.Read("CSVFile\\BossBerserkerData");
        for (int idx = 0; idx < waveData.Count; idx++)
        {
            BerserkerData ws = new BerserkerData();
            Tuple<string, int> key = new Tuple<string, int>(waveData[idx]["BossMonster"].ToString(), (int)waveData[idx]["MonsterRank"]);
            
            ws.bossLimitTime = int.Parse(waveData[idx]["BossLimitTime"].ToString());
            ws.speedUp = float.Parse(waveData[idx]["SpeedUp"].ToString());
            ws.powerUp = float.Parse(waveData[idx]["PowerUp"].ToString());
            mDataSetBerserker[key] = ws;
        }
    }

    //���� ������ ����
    public void InitAllSpawnData()
    {
        mMapType = MapManager.Instance.CurrentMapType;

        mDataSetNormal = new List<SpawnData>();
        mDataSetBoss = new List<SpawnData>();
        mDataSetRelayBoss = new List<SpawnData>();
        List<Dictionary<string, object>> spawnData = CSVReader.Read("CSVFile\\SpawnData");
        if (DEBUG)
            Debug.Log("���� ��Ÿ��" + mMapType);
        
        for (int idx = 0; idx < spawnData.Count; idx++)
        {
            if ((MapManager.MapType)Enum.Parse(typeof(MapManager.MapType), spawnData[idx]["SpawnMap"].ToString()) == mMapType) {
             
                SpawnData ds = new SpawnData();
                ds.Id = (int)spawnData[idx]["ID"];
                ds.spawnMonster = spawnData[idx]["SpawnMonster"].ToString();
                ds.monsterRank = (int)spawnData[idx]["MonsterRank"];
                ds.spawnMap = spawnData[idx]["SpawnMap"].ToString();
                ds.spawnOrder = (int)spawnData[idx]["SpawnOrder"];
                ds.spawnNumber = (int)spawnData[idx]["SpawnNumber"];
                ds.spawnStartTime_1 = (int)spawnData[idx]["SpawnStartTime1"];
                ds.spawnStartTime_2 = (int)spawnData[idx]["SpawnStartTime2"];
                ds.realStartSpawnTime = UnityEngine.Random.Range(ds.spawnStartTime_1, ds.spawnStartTime_2);
                ds.spawnTime = (int)spawnData[idx]["SpawnTime"];
                ds.currentTime = 0f;
                GameObject obj = Resources.Load<GameObject>("Prefabs\\Monster\\" + mMapType.ToString() + "\\" + spawnData[idx]["SpawnMonster"].ToString());

                MonsterManager.MonsterData temp = MonsterManager.Instance.GetMonsterData(ds.spawnMonster, ds.monsterRank);

                if (temp.monsterGrade ==  MonsterManager.MonsterGrade.Boss)
                {
                    if (MonsterManager.Instance.GetMonsterData(ds.spawnMonster, ds.monsterRank).monsterSpawnMap == MapManager.MapType.BossRelay)
                    {
                        mDataSetRelayBoss.Add(ds);
                    }
                    else {
                        mDataSetBoss.Add(ds);
                    }
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
        List<Transform> randomList = new List<Transform>();

        //mMonsterAreaNum���� ���Ͱ� ���� ���� ��ġ üũ
        //�ش� ��ġ���� �������� ����
        int nextSpawnIdx = 0;
        int checkMonsterNum = int.MaxValue;
        int zoneLength;
        

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

        Tuple<string, int> key = new Tuple<string, int>(_obj.name, _obj.GetComponent<MonsterStatus>().MonsterRank);
        if (mDataSetBerserker.ContainsKey(key)) {   
            BerserkerData ws;
            ws = mDataSetBerserker[key];
            if (DEBUG)
                Debug.Log("����Ŀ��� ���� ���ð�"+ ws.bossLimitTime);

            yield return new WaitForSeconds(ws.bossLimitTime);

            if (_obj.activeInHierarchy)
            {
            
                if(DEBUG)
                    Debug.Log("����Ŀ��� ���� " + gameObject.name + " speedUp:" + ws.speedUp + " , powerUp" + ws.powerUp);
                _obj.GetComponent<IStatus>().MoveSpeed *= ws.speedUp;
                _obj.GetComponent<MonsterAttack>().BerserkerModeScale = ws.powerUp;
                _obj.GetComponent<MonsterStatus>().IsBerserker = true;
                _obj.GetComponent<IStatus>().Hp = _obj.GetComponent<IStatus>().Hp;

            }
        }

    }
    
    //TO-DO �������� ü���� �����ִ� UI, UIManager��� �����ϵ��� ������ �ʿ����� ����ʿ�
    public void registerBossDieCheck(bool _hp, GameObject _obj)
    {
        if (currentTime < 0f)
        {
            //���� ����� �ٷ� UI�� ����ش�.
            MakeWaveAlarm(0f);
        }
        currentTime = Mathf.Max(0f, currentTime);
    }
    public void registerBossRelayDieCheck(bool isDie, GameObject _obj)
    {
        mIsSpawnRelayBoss = !isDie;
    }


    void MakeWaveAlarm(float _time)
    {
        if (waveMessageCoroutine != null)
            StopCoroutine(waveMessageCoroutine);
        waveMessageCoroutine = SpawnMessage(GameObject.Find("MonsterStatusObject").transform.Find("Alarm").gameObject, "wave " + mWaveCount, 6, _time);
        StartCoroutine(waveMessageCoroutine);
        
    }

    //TO-DO �������� �޽����� ���⼭�ϴ°� ������?
    IEnumerator SpawnMessage(GameObject _obj, string _txt, int _cnt, float _time)
    {
        yield return new WaitForSeconds(_time);

        _obj.GetComponent<Text>().text = _txt;
        for (int i = 0; i < _cnt; i++)
        {
            yield return new WaitForSeconds(0.5f);
            _obj.SetActive(!_obj.activeInHierarchy);
        }
        _obj.SetActive(false);
    }
}
