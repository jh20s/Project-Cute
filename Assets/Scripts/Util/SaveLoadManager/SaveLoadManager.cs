using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : SingleToneMaker<SaveLoadManager>
{
    private string mLoadFileName;
    private string mWarFileName;
    private bool mIsCreate = false;
    // Start is called before the first frame update
    void Awake()
    {
        mLoadFileName = "PlayerData.json";
        mWarFileName = "WarData.json";
    }

    public LobbyPlayerInfo LoadBaseData()
    {
        string path = Path.Combine(Application.dataPath, mLoadFileName);
        string fileStream = null;
        try
        {
            fileStream = File.ReadAllText(path);
        }
        catch
        {
            return CreateSaveFile();
        }
        
        fileStream = AES.Decrypt(fileStream, AES.key);

        LobbyPlayerInfo loadInfo = JsonUtility.FromJson<LobbyPlayerInfo>(fileStream);
        return loadInfo;
    }
    private LobbyPlayerInfo CreateSaveFile()
    {
        mIsCreate = true;
        string path = Path.Combine(Application.dataPath, mLoadFileName);
        LobbyPlayerInfo newInfo = new LobbyPlayerInfo();
        // �ʱⰪ ���� : TODO -> ���Ŀ� �����ͷ� �ʱⰪ �ޱ�
        List<Dictionary<string, object>> playerBaseStatData = CSVReader.Read("CSVFile/PlayerBaseStat");
        for (int i = 0; i < playerBaseStatData.Count; i++)
        {
            newInfo.BaseHp = int.Parse(playerBaseStatData[i]["PlayerBaseHP"].ToString());
            newInfo.BaseATK = int.Parse(playerBaseStatData[i]["PlayerBaseATK"].ToString());
            newInfo.BaseSPD = float.Parse(playerBaseStatData[i]["PlayerBaseSPD"].ToString());
            newInfo.BaseCriticalChance = float.Parse(playerBaseStatData[i]["PlayerBaseCritChance"].ToString());
            newInfo.BaseCriticalDamage = float.Parse(playerBaseStatData[i]["PlayerBaseCritDamage"].ToString());
            newInfo.BaseATKSPD = float.Parse(playerBaseStatData[i]["PlayerBaseATKSPD"].ToString());
        }
        newInfo.TrainingHp = 0;
        newInfo.TrainingATK = 0;
        newInfo.TrainingAddDamage = 1f;
        newInfo.MoveSpeedRate = 1f;
        newInfo.CurrentWeaponName = "";
        newInfo.CurrentCostumeName = "";
        newInfo.CurrentCostumeShapeName = "";
        newInfo.Gold = 0;
        newInfo.Diamond = 1000;
        newInfo.Stemina = 999;

        string fileStream = JsonUtility.ToJson(newInfo, true);
        fileStream = AES.Encrypt(fileStream, AES.key);

        File.WriteAllText(path, fileStream);
        return newInfo;
    }
    
    public void SavePlayerInfoFile(LobbyPlayerInfo _info)
    {
        if (!mIsCreate)
        {
            string path = Path.Combine(Application.dataPath, mLoadFileName);
            string fileStream = JsonUtility.ToJson(_info, true);
            fileStream = AES.Encrypt(fileStream, AES.key);
            File.WriteAllText(path, fileStream);
        }
    }

    // �������� ���� ������ ����
    public void SavePlayerWarData(WarInfo _info)
    {
        string path = Path.Combine(Application.dataPath, mWarFileName);
        string fileStream = JsonUtility.ToJson(_info, true);
        fileStream = AES.Encrypt(fileStream, AES.key);
        File.WriteAllText(path, fileStream);
    }

    // ���������� �ε��� ������
    public WarInfo LoadPlayerWarData()
    {
        string path = Path.Combine(Application.dataPath, mWarFileName);
        string fileStream = File.ReadAllText(path);

        fileStream = AES.Decrypt(fileStream, AES.key);

        WarInfo loadInfo = JsonUtility.FromJson<WarInfo>(fileStream);
        return loadInfo;
    }

    // ������ ���� �� �����ϴ� ������
    public void SaveWarEndData()
    {
        LobbyPlayerInfo info = LoadBaseData();

        // ������ ���� �� ������� ����
        // ���� ���, ���̾�...
        // TODO : ���������� �ʿ��� ������ �߰�
        info.Diamond = PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond;
        info.Gold += PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold;

        SavePlayerInfoFile(info);
    }
}
