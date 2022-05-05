using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : SingleToneMaker<MapManager>
{
    #region variables
    // ���Ŀ� Ÿ�ϸ� Ŭ���� ���� ����(�����ͼ°� �Բ�)
    [SerializeField]
    private StringGameObject maps;
    [SerializeField]
    private Transform[,] tileMapList = new Transform[3, 3];
    [SerializeField]
    private MapType mCurrentMapType;

    public MapType CurrentMapType
    {
        get { return mCurrentMapType; }
        set { mCurrentMapType = value; }
    }

    [SerializeField]
    private Vector3 nowPos;
    [SerializeField]
    private int baseY;
    [SerializeField]
    private int baseX;
    [SerializeField]
    private bool mIsChange = false;
    [SerializeField]
    private bool mIsUpdate = false;

    public enum MapType
    {
        Field,
        Volcano,
        Ice,
        Dungeon,
        BossRelay,
        Exit
    }
    #endregion
    
    #region method
    void Start()
    {
        // ���� �÷��̾� ��ġ ����
        nowPos = Vector3.zero;
        // ���õ� �� ����Ʈ �ʱ�ȭ
        // �� ������ �Ľ��� �ֱ�
        MapDataParse();
    }
    void Update()
    {
        if(mIsUpdate)
            moveMap();
    }
    private void MapDataParse()
    {
        // �������� �ҷ��ͼ�
        GameObject[] mapList = Resources.LoadAll<GameObject>("Prefabs/Map");
        // Dic�� ����
        for (int i = 0; i < mapList.Length; i++)
            maps.Add(mapList[i].name, mapList[i]);

        // csv���� �б�
        List<Dictionary<string, object>> mapData = CSVReader.Read("CSVFile/Map");

        for(int i = 0; i < mapList.Length; i++)
        {
            Map item = maps[mapData[i]["MapInName"].ToString()].GetComponent<Map>();

            item.ID = int.Parse(mapData[i]["ID"].ToString());
            item.Width = int.Parse(mapData[i]["Width"].ToString());
            item.Height = int.Parse(mapData[i]["Height"].ToString());
        }
    }
    /*
     *  �ʿ� ���� ������ ���Ͱ� �ٸ���
     *  return ������ �ش� ���� �̸��� ��ȯ���ش�
     *  ���Ŀ� ���� �������� api�̿��Ͻø� �˴ϴ�.
     */
    public void RandMapSelect()
    {
        // �ʹ�ȣ�� �������� �̰�
        MapType  ranNum = (MapType)Random.Range((int)MapType.Field, (int)MapType.Exit);
        //CurrentMapType = MapType.Volcano;

        // �ش� ���� Dic���� �ҷ�����
        GameObject newMap = maps[mCurrentMapType.ToString()];
        // Grid�ȿ� �������ش�
        Instantiate(newMap, Vector3.zero, Quaternion.identity, GameObject.Find("Grid").transform);
        // ���� �ʱ�ȭ ���ش�.
        baseX = newMap.GetComponent<Map>().Width;
        baseY = newMap.GetComponent<Map>().Height;
        if(baseX > 0 && baseY > 0)
        {
            initMapList();
            moveTileMap(0, 0, 1, -1);
            moveTileMap(0, 1, 1, 0);
            moveTileMap(0, 2, 1, 1);
            moveTileMap(1, 0, 0, -1);
            moveTileMap(1, 1, 0, 0);
            moveTileMap(1, 2, 0, 1);
            moveTileMap(2, 0, -1, -1);
            moveTileMap(2, 1, -1, 0);
            moveTileMap(2, 2, -1, 1);
            mIsUpdate = true;
        }
    }

    //DEBUG�� �ʼ��� API
    public void MapSelect()
    {
        // �ʹ�ȣ�� �������� �̰�
        //MapType  ranNum = (MapType)Random.Range((int)MapType.Field, (int)MapType.Exit);
        //CurrentMapType = MapType.Volcano;
        
        // �ش� ���� Dic���� �ҷ�����
        GameObject newMap = maps[mCurrentMapType.ToString()];
        // Grid�ȿ� �������ش�
        Instantiate(newMap, Vector3.zero, Quaternion.identity, GameObject.Find("Grid").transform);
        // ���� �ʱ�ȭ ���ش�.
        baseX = newMap.GetComponent<Map>().Width;
        baseY = newMap.GetComponent<Map>().Height;
        if (baseX > 0 && baseY > 0)
        {
            initMapList();
            moveTileMap(0, 0, 1, -1);
            moveTileMap(0, 1, 1, 0);
            moveTileMap(0, 2, 1, 1);
            moveTileMap(1, 0, 0, -1);
            moveTileMap(1, 1, 0, 0);
            moveTileMap(1, 2, 0, 1);
            moveTileMap(2, 0, -1, -1);
            moveTileMap(2, 1, -1, 0);
            moveTileMap(2, 2, -1, 1);
            mIsUpdate = true;
        }
    }
    /*
     * �ش� position�� objectŸ�ϸ��� Ÿ���� �ִ��� �Ǵ����ִ� api�Դϴ�.
     * _pos : ������ ��ġ
     * return : true�� ���� false�� �Ұ���
     */
    private void initMapList()
    {
        // ���� �θ�׷� Grid�� �ҷ��´�
        Transform grid = GameObject.Find("Grid").transform;
        // Gird�� �ڽ� Ÿ�ϸʼ��� �ҷ��´�.
        Transform[] mapList = grid.GetComponentsInChildren<Transform>();
        foreach(Transform map in mapList)
        {
            // ���� �θ�� �����ڽ��� ����
            if (map.name == grid.name || map.CompareTag("Tilemap"))
                continue;
            // �� �ڽĵ��� ��ġ�� �Ľ��Ͽ� �ش� ��ġ�� �־��ش�.
            string[] pos = map.name.Split(',');
            int y;
            int x;
            int.TryParse(pos[0], out y);
            int.TryParse(pos[1], out x);
            tileMapList[y, x] = map;
        }
        mIsChange = false;
    }

    // �÷��̾��� ��ġ�� ���� ���� �̵������ݴϴ�.
    private void moveMap()
    {
        // �� �����Ӹ��� �÷��̾��� ��ġ�� �޾ƿ´�
        // ���Ŀ� �÷��̾� �Ŵ����� ���Ͽ� �޾ƿ����� ����
        Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
        // Todo : �÷��̾��� ��ġ������ ���� ���� �̵������ִ� ���� ����
        float changeY = playerPos.y - nowPos.y;
        float changeX = playerPos.x - nowPos.x;
        // ���̵��� �־����� �Ǻ�
        if (!mIsChange)
        {
            // ��
            if (changeY >= baseY)
            {
                mIsChange = true;
                moveTileMap(2, 0, 3, 0);
                moveTileMap(2, 1, 3, 0);
                moveTileMap(2, 2, 3, 0);
                nowPos.y += baseY;
                tileMapList[2, 0].name = 0 + "," + 0;
                tileMapList[2, 1].name = 0 + "," + 1;
                tileMapList[2, 2].name = 0 + "," + 2;
                tileMapList[1, 0].name = 2 + "," + 0;
                tileMapList[1, 1].name = 2 + "," + 1;
                tileMapList[1, 2].name = 2 + "," + 2;
                tileMapList[0, 0].name = 1 + "," + 0;
                tileMapList[0, 1].name = 1 + "," + 1;
                tileMapList[0, 2].name = 1 + "," + 2;
            }
            // ��
            else if (changeY <= -baseY)
            {
                mIsChange = true;
                moveTileMap(0, 0, -3, 0);
                moveTileMap(0, 1, -3, 0);
                moveTileMap(0, 2, -3, 0);
                nowPos.y -= baseY;
                tileMapList[0, 0].name = 2 + "," + 0;
                tileMapList[0, 1].name = 2 + "," + 1;
                tileMapList[0, 2].name = 2 + "," + 2;
                tileMapList[1, 0].name = 0 + "," + 0;
                tileMapList[1, 1].name = 0 + "," + 1;
                tileMapList[1, 2].name = 0 + "," + 2;
                tileMapList[2, 0].name = 1 + "," + 0;
                tileMapList[2, 1].name = 1 + "," + 1;
                tileMapList[2, 2].name = 1 + "," + 2;
            }
            // ��
            else if (changeX <= -baseX)
            {
                mIsChange = true;
                moveTileMap(0, 2, 0, -3);
                moveTileMap(1, 2, 0, -3);
                moveTileMap(2, 2, 0, -3);
                nowPos.x -= baseX;
                tileMapList[0, 2].name = 0 + "," + 0;
                tileMapList[1, 2].name = 1 + "," + 0;
                tileMapList[2, 2].name = 2 + "," + 0;
                tileMapList[0, 1].name = 0 + "," + 2;
                tileMapList[1, 1].name = 1 + "," + 2;
                tileMapList[2, 1].name = 2 + "," + 2;
                tileMapList[0, 0].name = 0 + "," + 1;
                tileMapList[1, 0].name = 1 + "," + 1;
                tileMapList[2, 0].name = 2 + "," + 1;
            }
            // ��
            else if (changeX >= baseX)
            {
                mIsChange = true;
                moveTileMap(0, 0, 0, 3);
                moveTileMap(1, 0, 0, 3);
                moveTileMap(2, 0, 0, 3);
                nowPos.x += baseX;
                tileMapList[0, 0].name = 0 + "," + 2;
                tileMapList[1, 0].name = 1 + "," + 2;
                tileMapList[2, 0].name = 2 + "," + 2;
                tileMapList[0, 1].name = 0 + "," + 0;
                tileMapList[1, 1].name = 1 + "," + 0;
                tileMapList[2, 1].name = 2 + "," + 0;
                tileMapList[0, 2].name = 0 + "," + 1;
                tileMapList[1, 2].name = 1 + "," + 1;
                tileMapList[2, 2].name = 2 + "," + 1;
            }
        }
        // ���̵��� �־��ٸ� Ÿ�� �迭�� �ʱ�ȭ ���ش�.
        if (mIsChange)
        {
            initMapList();
        }
    }
    /*
     * Ÿ�ϸ��� ���� ��ǥ�迡�� �Ű��ִ� �Լ�
     * _y : Ÿ�ϸʸ���Ʈ�� y��ǥ
     * _x : Ÿ�ϸʸ���Ʈ�� x��ǥ
     * cy : ��ǥ�迡�� �ű� y��ǥ�� ���
     * cx : ��ǥ�迡�� �ű� x��ǥ�� ���
     */
    private void moveTileMap(int _y, int _x, int cy, int cx)
    {
        tileMapList[_y, _x].position = tileMapList[_y, _x].position + new Vector3(baseX * cx, baseY * cy, 0);
    }
    #endregion
}
