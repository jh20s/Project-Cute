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

    public struct BerserkerData
    {
        public int bossLimitTime;
        public float speedUp;
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
        InitBerserkerData();
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
        for (int i = 0; i < mDataSetNormal.Count; i++)
        {
            SpawnData temp = mDataSetNormal[i];
            if (mDataSetNormal[i].realStartSpawnTime < currentTime && mDataSetNormal[i].currentTime > mDataSetNormal[i].spawnTime)
            {
                List<int> spawnZone = RandSpawnList(mDataSetNormal[i].spawnNumber + mBossNum);//����� �䱸�������� ���̺긶�� +1������ �� ���� 
                for (int j = 0; j < spawnZone.Count; j++)
                {
                    GameObject monster = ObjectPoolManager.Instance.EnableGameObject(mDataSetNormal[i].spawnMonster);
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
            mDataSetNormal[i] = temp;
        }
    }

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
                    if (mDataSetNormal[j].spawnMonster.Equals(obj.name))
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

            //����Ŀ��� �ڷ�ƾ
            StartCoroutine(BossWaitBerserkerMode(monster));
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
            monster.GetComponent<MonsterStatus>().Hp = (int)((float)md.monsterHp * mDataSetWave[mBossNum].hpScale);
            monster.GetComponent<MonsterStatus>().MaxHP = (int)((float)md.monsterHp * mDataSetWave[mBossNum].hpScale);
        }

        monster.GetComponent<MonsterStatus>().Size = md.monsterSize;
        monster.GetComponent<MonsterStatus>().MonsterGrade= md.monsterGrade;
        monster.GetComponent<MonsterStatus>().MonsterInName = md.monsterInName;
        monster.GetComponent<MonsterAttack>().CloseAttackPower = md.closeAttackPower;

        if (md.monsterGrade != MonsterManager.MonsterGrade.Boss)
            monster.GetComponent<MonsterStatus>().MoveSpeed = md.monsterSpeed + mDataSetWave[mBossNum].speedUp;
        else
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
                    ObjectPoolManager.Instance.CreateDictTable(obj, 100, 10);
                    mDataSetNormal.Add(ds);
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
