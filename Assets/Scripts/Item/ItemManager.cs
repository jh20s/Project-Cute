using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleToneMaker<ItemManager>
{
    public GameObject[] ItemList;
    public struct ItemData
    {
        public string itemInName;
        public int damage;
        public int hp;
        public int gold;
        public int dropRate;
        public int scale;
        public int mustDrop;
    }
    [SerializeField]
    private List<ItemData> dataSet;

    

    private void Awake()
    {
        InitItemData();
    }

    // Start is called before the first frame update
    void Start()
    {
        createObjectPool();
    }

    public ItemData GetItemData(int num)
    {
        /*
         * TO-DO 없는 key값에 대한 예외처리 필요
         * */
        return dataSet[num];
    }

    private void InitItemData()
    {
        dataSet = new List<ItemData>();
        List<Dictionary<string, object>> itemDataCsv = CSVReader.Read("CSVFile\\ItemData");
        for (int idx = 0; idx < itemDataCsv.Count; idx++)
        {
            int key;
            ItemData data = new ItemData();
            key = int.Parse(itemDataCsv[idx]["ID"].ToString());
            data.itemInName = itemDataCsv[idx]["ItemInName"].ToString();
            data.damage = int.Parse(itemDataCsv[idx]["Damage"].ToString());
            data.hp = int.Parse(itemDataCsv[idx]["Hp"].ToString());
            // md.damage = int.Parse(itemDataCsv[idx]["Damage"].ToString());
            data.gold = int.Parse(itemDataCsv[idx]["Gold"].ToString());
            data.dropRate = int.Parse(itemDataCsv[idx]["Droprate"].ToString());
            data.scale = int.Parse(itemDataCsv[idx]["Scale"].ToString());
            data.mustDrop = int.Parse(itemDataCsv[idx]["MustDrop"].ToString());
            dataSet.Add(data);
        }
    }

    private void createObjectPool()
    {
        foreach (GameObject item in ItemList)
        {
            ObjectPoolManager.Instance.CreateDictTable(item, 3, 3);
        }

        //폭탄에 사용될 objectpool생성
        GameObject obj = Resources.Load<GameObject>("Prefabs\\Projectiles\\s99");
        ObjectPoolManager.Instance.CreateDictTable(obj, 10, 10);
    }
}
