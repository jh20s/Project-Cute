using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingleToneMaker<SpawnManager>
{

    private float currentTime = 0f;
    private float minTime = 1f;
    private float maxTime = 3f;

    public Transform[] spawnPoints;

    /*
     * TO-DO : �Ϲݸ����� ������ 4����, �׿����� ���� Ÿ��,���۽ð�, �������ݵ��� ����������.
     *         �� ������ ��� �������� ���.
     */
    public GameObject MonsterA;
    public string Spawn_Map;
    public int SpawnOrder;
    public int SpawnNumber = 3;
    public float SpawnTime = 1f;

    void Awake()
    {


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
        ObjectPoolManager.Instance.CreateDictTable(MonsterA, 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * TO-DO������ Ÿ���� �������� ���������� ����Ʈ�� �����ؼ� �Ź� for�� ��ȸ�ϸ鼭 object�� ��ȯ��Ű��
         *
         */
        currentTime += Time.deltaTime;
        if (currentTime > SpawnTime)
        {
            for(int i = 0; i < SpawnNumber; i++) { 
                GameObject enemy = ObjectPoolManager.Instance.EnableGameObject(MonsterA.name);
                if (enemy != null)
                {
                    int index = UnityEngine.Random.Range(0, spawnPoints.Length);
                    enemy.transform.position = spawnPoints[index].position;
                    enemy.SetActive(true);
                }
                currentTime = 0;
            }
        }
    }
}
