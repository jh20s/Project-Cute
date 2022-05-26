using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterManager : SingleToneMaker<MonsterManager>
{

    //TO-DO : ������� ���������� ��� class�� ���� �ٸ��ʿ����� ��ü �����ϱ� �����Ұŷ� ���� �ƴ� 
    public struct MonsterData
    {
        public int id;
        public MapManager.MapType monsterSpawnMap;
        public string monsterColor;
        public int monsterSize;
        public string monsterType;
        public string monsterName;
        public MonsterGrade monsterGrade;
        public int monsterHp;
        public int monsterPhysicalDefense;
        public int monsterMagicDefense;
        public float monsterSpeed;
        public int monsterAttackSpeed;
        public int closeAttackPower;
        public int closeAttackRange;
        public int standoffAttackPower;
        public int standoffAttackRange;
        public List<int> skillAttackPower;
        public List<float> skillAttackPowerRange;
        public List<string> skillAttackAnimation;
        public string monsterDrop;
        public int monsterExp;
    }
    public enum MonsterGrade
    {
        Normal,
        Range,
        Boss
    }


    [SerializeField]
    private Dictionary<Tuple<string, int>, MonsterManager.MonsterData> mDataSet;

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

    public MonsterData GetMonsterData(string _str, int _num)
    {
        /*
         * TO-DO ���� key���� ���� ����ó�� �ʿ�
         * */
        Tuple<string, int> tuple = new Tuple<string, int>(_str, _num);
        return mDataSet[tuple];
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
        mDataSet = new Dictionary<Tuple<string, int>, MonsterManager.MonsterData>();
        List<Dictionary<string, object>> monsterDataCsv = CSVReader.Read("CSVFile\\MonsterData");
        for (int idx = 0; idx < monsterDataCsv.Count; idx++)
        {
            MonsterData md = new MonsterData();

            //�̸��� Rank�� ����
            Tuple<string, int> key = new Tuple<string, int>(monsterDataCsv[idx]["MonsterInName"].ToString(), int.Parse(monsterDataCsv[idx]["MonsterRank"].ToString()));

            md.skillAttackPower = new List<int>();
            md.skillAttackPowerRange = new List<float>();
            md.skillAttackAnimation = new List<string>();

            md.id = int.Parse(monsterDataCsv[idx]["ID"].ToString());
            md.monsterSpawnMap = (MapManager.MapType)Enum.Parse(typeof(MapManager.MapType), monsterDataCsv[idx]["MonsterSpawnMap"].ToString());
            md.monsterSize = int.Parse(monsterDataCsv[idx]["MonsterSize"].ToString());
            md.monsterType = monsterDataCsv[idx]["MonsterType"].ToString();
            md.monsterName = monsterDataCsv[idx]["MonsterName"].ToString();
            md.monsterColor = monsterDataCsv[idx]["MonsterColor"].ToString();

            md.monsterGrade = (MonsterGrade)Enum.Parse(typeof(MonsterGrade), monsterDataCsv[idx]["MonsterGrade"].ToString());
            md.monsterHp = int.Parse(monsterDataCsv[idx]["MonsterHp"].ToString());
            md.monsterPhysicalDefense = int.Parse(monsterDataCsv[idx]["MonsterPhysicalDefense"].ToString());
            md.monsterMagicDefense = int.Parse(monsterDataCsv[idx]["MonsterMagicDefense"].ToString());
            md.monsterSpeed = float.Parse(monsterDataCsv[idx]["MonsterSpeed"].ToString());
            md.monsterAttackSpeed = int.Parse(monsterDataCsv[idx]["MonsterAttackSpeed"].ToString());
            md.closeAttackPower = int.Parse(monsterDataCsv[idx]["CloseAttackPower"].ToString());
            md.closeAttackRange = int.Parse(monsterDataCsv[idx]["CloseAttackRange"].ToString());

            //��ų1
            md.skillAttackAnimation.Add(monsterDataCsv[idx]["SkillAttackAnimation1"].ToString());
            md.skillAttackPower.Add(int.Parse(monsterDataCsv[idx]["SkillAttackPower1"].ToString()));
            md.skillAttackPowerRange.Add(float.Parse(monsterDataCsv[idx]["SkillAttackPowerRange1"].ToString()));

            //��ų2
            md.skillAttackAnimation.Add(monsterDataCsv[idx]["SkillAttackAnimation2"].ToString());
            md.skillAttackPower.Add(int.Parse(monsterDataCsv[idx]["SkillAttackPower2"].ToString()));
            md.skillAttackPowerRange.Add(float.Parse(monsterDataCsv[idx]["SkillAttackPowerRange2"].ToString()));

            //��ų3
            md.skillAttackAnimation.Add(monsterDataCsv[idx]["SkillAttackAnimation3"].ToString());
            md.skillAttackPower.Add(int.Parse(monsterDataCsv[idx]["SkillAttackPower3"].ToString()));
            md.skillAttackPowerRange.Add(float.Parse(monsterDataCsv[idx]["SkillAttackPowerRange3"].ToString()));

            //��ų4
            md.skillAttackAnimation.Add(monsterDataCsv[idx]["SkillAttackAnimation4"].ToString());
            md.skillAttackPower.Add(int.Parse(monsterDataCsv[idx]["SkillAttackPower4"].ToString()));
            md.skillAttackPowerRange.Add(float.Parse(monsterDataCsv[idx]["SkillAttackPowerRange4"].ToString()));


            md.monsterDrop = monsterDataCsv[idx]["MonsterDrop"].ToString();
            md.monsterExp = int.Parse(monsterDataCsv[idx]["MonsterExp"].ToString());
            mDataSet[key] = md;
        }
    }


}
