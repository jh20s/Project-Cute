using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : SingleToneMaker<SaveLoadManager>
{
    private string mLoadFileName;
    private string mWarFileName;
    private string mAchieveFileName;
    private bool mIsCreate = false;
    // Start is called before the first frame update
    void Awake()
    {
        mLoadFileName = "PlayerData.json";
        mWarFileName = "WarData.json";
        mAchieveFileName = "AchieveData.json";
    }

    public LobbyPlayerInfo LoadBaseData()
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mLoadFileName);
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
    public LobbyPlayerAchievementData LoadAchieveData()
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mAchieveFileName);
        string fileStream = null;
        try
        {
            fileStream = File.ReadAllText(path);
        }
        catch
        {
            return CerateAchieveDataFile();
        }

        fileStream = AES.Decrypt(fileStream, AES.key);

        LobbyPlayerAchievementData loadInfo = JsonUtility.FromJson<LobbyPlayerAchievementData>(fileStream);
        return loadInfo;
    }
    private LobbyPlayerInfo CreateSaveFile()
    {
        mIsCreate = true;
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mLoadFileName);
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
        newInfo.TrainingLevelList = new List<int>();
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingHp = 0;
        newInfo.TrainingATK = 0;
        newInfo.TrainingAddDamage = 1f;
        newInfo.MoveSpeedRate = 1f;
        newInfo.CurrentWeaponName = "";
        newInfo.CurrentCostumeName = "";
        newInfo.CurrentCostumeShapeName = "";
        newInfo.Gold = 0;
        newInfo.Diamond = 190;
        List<Dictionary<string, object>> weponLockData = CSVReader.Read("CSVFile\\Weapon");
        newInfo.Weaponlock = new StringBoolean();
        for(int i = 0; i < weponLockData.Count; i++)
        {
            string weaponName = weponLockData[i]["WeaponType"].ToString();
            bool locked = Convert.ToBoolean(weponLockData[i]["Weaponlock"].ToString());
            newInfo.Weaponlock.Add(weaponName, locked);
        }
        List<Dictionary<string, object>> costumeLockData = CSVReader.Read("CSVFile\\Costume");
        newInfo.Costumelock = new StringBoolean();
        for (int i = 0; i < costumeLockData.Count; i++)
        {
            string costumeName = costumeLockData[i]["CostumeLoad"].ToString();
            bool locked = Convert.ToBoolean(costumeLockData[i]["CostumeLock"].ToString());
            newInfo.Costumelock.Add(costumeName, locked);
        }
        List<Dictionary<string, object>> skillLockData = CSVReader.Read("CSVFile\\Skill");
        newInfo.Skilllock = new StringBoolean();
        for (int i = 0; i < skillLockData.Count; i++)
        {
            string skillName = skillLockData[i]["SkillNameENG"].ToString();
            bool locked = Convert.ToBoolean(skillLockData[i]["SkillLock"].ToString());
            newInfo.Skilllock.Add(skillName, locked);
        }
        string fileStream = JsonUtility.ToJson(newInfo, true);
        fileStream = AES.Encrypt(fileStream, AES.key);

        File.WriteAllText(path, fileStream);
        mIsCreate = false;
        return newInfo;
    }
    private LobbyPlayerAchievementData CerateAchieveDataFile()
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mAchieveFileName);

        LobbyPlayerAchievementData newData = new LobbyPlayerAchievementData();

        newData.Progress = new StringState();
        foreach (string key in AchievementManager.Instance.Achievements.Keys)
        {
            // ��ü���� ���൵
            newData.Progress.Add(key, AchievementManager.Instance.Achievements[key].GetComponent<Achievement>().State);
        }

        // ��� Ŭ����
        newData.ModeClear = new List<int>();
        newData.ModeClear.Add(0);
        newData.ModeClear.Add(0);

        // ó��(����)
        newData.KillToWeapon = new List<int>();
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);

        //óġ(��ų)
        newData.KillToSkill = new StringInt();
        newData.KillToSkill.Add("Sword Release", 0);
        newData.KillToSkill.Add("Storm", 0);
        newData.KillToSkill.Add("Cross Fire", 0);
        newData.KillToSkill.Add("Incinerate", 0);

        //�����ð�(����)
        newData.TimeToWeapon = new List<int>();
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);

        //�����ð�(�ڽ�Ƭ)
        newData.TimeToCostume = new List<int>();
        newData.TimeToCostume.Add(0);
        newData.TimeToCostume.Add(0);

        // ����ó��(����)
        newData.BossKillToWeapon = new List<IntInt>();
        for(EquipmentManager.WeaponType i = EquipmentManager.WeaponType.sw; i < EquipmentManager.WeaponType.Exit; i++)
        {
            newData.BossKillToWeapon.Add(new IntInt());
            newData.BossKillToWeapon[(int)i].Add(0, 0);
            newData.BossKillToWeapon[(int)i].Add(1, 0);
            newData.BossKillToWeapon[(int)i].Add(2, 0);
            newData.BossKillToWeapon[(int)i].Add(3, 0);
        }

        // ����óġ(�ڽ�Ƭ)
        newData.BossKillToCostume = new List<IntInt>();
        for (EquipmentManager.CostumeTpye i = EquipmentManager.CostumeTpye.swsp; i < EquipmentManager.CostumeTpye.bgstswsp; i++)
        {
            newData.BossKillToCostume.Add(new IntInt());
            newData.BossKillToCostume[(int)i].Add(0, 0);
            newData.BossKillToCostume[(int)i].Add(1, 0);
            newData.BossKillToCostume[(int)i].Add(2, 0);
            newData.BossKillToCostume[(int)i].Add(3, 0);
        }

        // ���̺� Ŭ����(����)
        newData.WaveClearToWeapon = new List<IntInt>();
        for (EquipmentManager.WeaponType i = EquipmentManager.WeaponType.sw; i < EquipmentManager.WeaponType.Exit; i++)
        {
            newData.WaveClearToWeapon.Add(new IntInt());
            newData.WaveClearToWeapon[(int)i].Add(0, 0);
            newData.WaveClearToWeapon[(int)i].Add(1, 0);
            newData.WaveClearToWeapon[(int)i].Add(2, 0);
            newData.WaveClearToWeapon[(int)i].Add(3, 0);
        }

        // ���� ��� Ŭ����
        newData.BossModeWaveClear = new List<int>();
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);

        // ���� ����
        string fileStream = JsonUtility.ToJson(newData, true);
        fileStream = AES.Encrypt(fileStream, AES.key);

        File.WriteAllText(path, fileStream);

        return newData;
    }
    
    public void SavePlayerInfoFile(LobbyPlayerInfo _info)
    {
        if (!mIsCreate)
        {
            string path = "";
            path = Path.Combine(Application.persistentDataPath, mLoadFileName);
            string fileStream = JsonUtility.ToJson(_info, true);
            fileStream = AES.Encrypt(fileStream, AES.key);
            File.WriteAllText(path, fileStream);
        }
    }
    public void SaveAchievementData(LobbyPlayerAchievementData _info)
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mAchieveFileName);
        string fileStream = JsonUtility.ToJson(_info, true);
        fileStream = AES.Encrypt(fileStream, AES.key);
        File.WriteAllText(path, fileStream);
        Debug.Log("���� ������ ����");
    }

    // �������� ���� ������ ����
    public void SavePlayerWarData(WarInfo _info)
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mWarFileName);
        string fileStream = JsonUtility.ToJson(_info, true);
        fileStream = AES.Encrypt(fileStream, AES.key);
        File.WriteAllText(path, fileStream);
    }

    // ���������� �ε��� ������
    public WarInfo LoadPlayerWarData()
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mWarFileName);
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

        LobbyPlayerAchievementData achieveInfo = LoadAchieveData();

        string weaponType = PlayerManager.Instance.Player.GetComponent<PlayerStatus>()
            .PlayerCurrentWeapon.Spec.Type.Substring(0, 2);
        EquipmentManager.WeaponType type = (EquipmentManager.WeaponType)Enum.Parse
            (typeof(EquipmentManager.WeaponType), weaponType);

        // ���⺰ ų üũ
        achieveInfo.KillToWeapon[(int)type] += SpawnManager.currentKillMosterCount;
        // ���⺰ ���� �ð� üũ
        achieveInfo.TimeToWeapon[(int)type] += (int)UIManager.Instance.GamePlayerTime;
        // ���⺰ ���� ų üũ
        for(int i = 0; i < SpawnManager.currentKillBossMonsterCount; i++)
        {
            achieveInfo.BossKillToWeapon[(int)type][i]++;
        }
        // ���⺰ ���̺� Ŭ���� üũ
        for(int i = 0; i < SpawnManager.Instance.WaveCount; i++)
        {
            achieveInfo.WaveClearToWeapon[(int)type][i]++;
        }
        // ��ų�� ų üũ
        if (achieveInfo.KillToSkill.ContainsKey(PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentGeneralSkill.name))
        {
            achieveInfo.KillToSkill[PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentGeneralSkill.name] 
                += SpawnManager.currentKillMosterCount;
        }
        // �ڽ�Ƭ ���� �ð� üũ(����x)
        if((EquipmentManager.CostumeTpye.swsp).ToString().Contains(weaponType))
            achieveInfo.TimeToCostume[0] = (int)UIManager.Instance.GamePlayerTime;
        else
            achieveInfo.TimeToCostume[1] = (int)UIManager.Instance.GamePlayerTime;
        // �ڽ�Ƭ ���� ų üũ
        if ((EquipmentManager.CostumeTpye.swsp).ToString().Contains(weaponType))
        {
            for (int i = 0; i < SpawnManager.currentKillBossMonsterCount; i++)
            {
                achieveInfo.BossKillToCostume[0][i]++;
            }
        }
        else
        {
            for (int i = 0; i < SpawnManager.currentKillBossMonsterCount; i++)
            {
                achieveInfo.BossKillToCostume[1][i]++;
            }
        }

        // ��� ���� Ƚ��
        if (MapManager.Instance.CurrentMapType == MapManager.MapType.BossRelay)
            achieveInfo.ModeClear[1]++;
        else
            achieveInfo.ModeClear[0]++;

        // ���� ������ ��� Ŭ���� �ܰ��
        if(MapManager.Instance.CurrentMapType == MapManager.MapType.BossRelay)
        {
            for (int i = 0; i < SpawnManager.Instance.WaveCount; i++)
            {
                achieveInfo.BossModeWaveClear[i]++;
            }
        }

        SaveAchievementData(achieveInfo);
    }
}
