using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingleToneMaker<MapManager>
{
    #region variables
    // ���Ŀ� Ÿ�ϸ� Ŭ���� ���� ����(�����ͼ°� �Բ�)
    private Transform[,] tileMapList = new Transform[3, 3];
    private Vector3 nowPos;
    private int baseY = 19;
    private int baseX = 25;
    private bool isChange = false;
    #endregion

    #region method
    void Start()
    {
        // ���� �÷��̾� ��ġ ����
        nowPos = Vector3.zero;
        // �� ����Ʈ �ʱ�ȭ
        initMapList();
    }
    void Update()
    {
        moveMap();
    }

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
            string tmp = map.name.Split('/')[1];
            string[] pos = tmp.Split(',');
            int y = int.Parse(pos[0]);
            int x = int.Parse(pos[1]);
            tileMapList[y, x] = map;
        }
        isChange = false;
    }

    // �÷��̾��� ��ġ�� ���� ���� �̵������ݴϴ�.
    private void moveMap()
    {
        // �� �����Ӹ��� �÷��̾��� ��ġ�� �޾ƿ´�
        // ���Ŀ� �÷��̾� �Ŵ����� ���Ͽ� �޾ƿ����� ����
        Vector3 playerPos = GameObject.Find("PlayerObject").transform.position;
        // Todo : �÷��̾��� ��ġ������ ���� ���� �̵������ִ� ���� ����
        float changeY = playerPos.y - nowPos.y;
        float changeX = playerPos.x - nowPos.x;
        // ���̵��� �־����� �Ǻ�
        if (!isChange)
        {
            // ��
            if (changeY >= baseY)
            {
                isChange = true;
                moveTileMap(2, 0, 3, 0);
                moveTileMap(2, 1, 3, 0);
                moveTileMap(2, 2, 3, 0);
                nowPos.y += baseY;
                tileMapList[2, 0].name = "TilemapSet/" + 0 + "," + 0;
                tileMapList[2, 1].name = "TilemapSet/" + 0 + "," + 1;
                tileMapList[2, 2].name = "TilemapSet/" + 0 + "," + 2;
                tileMapList[1, 0].name = "TilemapSet/" + 2 + "," + 0;
                tileMapList[1, 1].name = "TilemapSet/" + 2 + "," + 1;
                tileMapList[1, 2].name = "TilemapSet/" + 2 + "," + 2;
                tileMapList[0, 0].name = "TilemapSet/" + 1 + "," + 0;
                tileMapList[0, 1].name = "TilemapSet/" + 1 + "," + 1;
                tileMapList[0, 2].name = "TilemapSet/" + 1 + "," + 2;
            }
            // ��
            else if (changeY <= -baseY)
            {
                isChange = true;
                moveTileMap(0, 0, -3, 0);
                moveTileMap(0, 1, -3, 0);
                moveTileMap(0, 2, -3, 0);
                nowPos.y -= baseY;
                tileMapList[0, 0].name = "TilemapSet/" + 2 + "," + 0;
                tileMapList[0, 1].name = "TilemapSet/" + 2 + "," + 1;
                tileMapList[0, 2].name = "TilemapSet/" + 2 + "," + 2;
                tileMapList[1, 0].name = "TilemapSet/" + 0 + "," + 0;
                tileMapList[1, 1].name = "TilemapSet/" + 0 + "," + 1;
                tileMapList[1, 2].name = "TilemapSet/" + 0 + "," + 2;
                tileMapList[2, 0].name = "TilemapSet/" + 1 + "," + 0;
                tileMapList[2, 1].name = "TilemapSet/" + 1 + "," + 1;
                tileMapList[2, 2].name = "TilemapSet/" + 1 + "," + 2;
            }
            // ��
            else if (changeX <= -baseX)
            {
                isChange = true;
                moveTileMap(0, 2, 0, -3);
                moveTileMap(1, 2, 0, -3);
                moveTileMap(2, 2, 0, -3);
                nowPos.x -= baseX;
                tileMapList[0, 2].name = "TilemapSet/" + 0 + "," + 0;
                tileMapList[1, 2].name = "TilemapSet/" + 1 + "," + 0;
                tileMapList[2, 2].name = "TilemapSet/" + 2 + "," + 0;
                tileMapList[0, 1].name = "TilemapSet/" + 0 + "," + 2;
                tileMapList[1, 1].name = "TilemapSet/" + 1 + "," + 2;
                tileMapList[2, 1].name = "TilemapSet/" + 2 + "," + 2;
                tileMapList[0, 0].name = "TilemapSet/" + 0 + "," + 1;
                tileMapList[1, 0].name = "TilemapSet/" + 1 + "," + 1;
                tileMapList[2, 0].name = "TilemapSet/" + 2 + "," + 1;
            }
            // ��
            else if (changeX >= baseX)
            {
                isChange = true;
                moveTileMap(0, 0, 0, 3);
                moveTileMap(1, 0, 0, 3);
                moveTileMap(2, 0, 0, 3);
                nowPos.x += baseX;
                tileMapList[0, 0].name = "TilemapSet/" + 0 + "," + 2;
                tileMapList[1, 0].name = "TilemapSet/" + 1 + "," + 2;
                tileMapList[2, 0].name = "TilemapSet/" + 2 + "," + 2;
                tileMapList[0, 1].name = "TilemapSet/" + 0 + "," + 0;
                tileMapList[1, 1].name = "TilemapSet/" + 1 + "," + 0;
                tileMapList[2, 1].name = "TilemapSet/" + 2 + "," + 0;
                tileMapList[0, 2].name = "TilemapSet/" + 0 + "," + 1;
                tileMapList[1, 2].name = "TilemapSet/" + 1 + "," + 1;
                tileMapList[2, 2].name = "TilemapSet/" + 2 + "," + 1;
            }
        }
        // ���̵��� �־��ٸ� Ÿ�� �迭�� �ʱ�ȭ ���ش�.
        if (isChange)
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
