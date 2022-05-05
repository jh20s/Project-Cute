using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : SingleToneMaker<GachaManager>
{
    [SerializeField]
    private List<string> mGachaList = new List<string>();
    [SerializeField]
    private string mCutePotion = "CutePotion";
    public string CutePotion
    {
        get { return mCutePotion; }
    }
    // Start is called before the first frame update
    void Start()
    {
        initGachaList();
    }
    private void initGachaList()
    {
        List<Dictionary<string, object>> chanceData = CSVReader.Read("CSVFile/GachaData");
        for(int i = 0; i< chanceData.Count; i++)
        {
            for(int j = 0; j < int.Parse(chanceData[i]["Chance"].ToString()); j++)
            {
                mGachaList.Add(chanceData[i]["Name"].ToString());
            }
        }
    }
    // �ѹ� �̱�
    public string OneItemDraw()
    {
        LobbyPlayerInfo info = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        int ran = UnityEngine.Random.Range(0, mGachaList.Count);
        string item = mGachaList[ran];
        Debug.Log(ran);
        if (!info.Costumelock[item])
        {
            item = mCutePotion;
            info.CutePotionCount += 1;
        }
        else
        {
            // �ش� �ڽ�Ƭ �ر�(���̺��)
            info.Costumelock[item] = false;
            // UI����
            WareHouseManager.Instance.ChangeCostumeUnlock(item, false);
        }
        SaveLoadManager.Instance.SavePlayerInfoFile(info);
        return item;
    }
    // 10�� �̱�
    public List<string> TenItemDraw()
    {
        List<string> items = new List<string>();
        for(int i = 0; i < 10; i++)
        {
            items.Add(OneItemDraw());
        }
        return items;
    }
}
