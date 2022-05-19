using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SaveLoadManager : SingleToneMaker<SaveLoadManager>
{
    private string mLoadFileName;
    private string mWarFileName;
    private string mAchieveFileName;
    private bool mIsCreate = false;
    private int mCurrentDay;
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
        DateTime nowTime = DateTime.Now;
        if (nowTime.Day != loadInfo.Date)
            loadInfo.DailyAddCount = 3;
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
        LobbyPlayerInfo newInfo = new LobbyPlayerInfo();
        // 초기값 세팅 : TODO -> 추후에 데이터로 초기값 받기
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
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingLevelList.Add(0);
        newInfo.TrainingHp = 0;
        newInfo.TrainingATK = 0;
        newInfo.TrainingAddDamage = 1f;
        newInfo.TrainingMagnetPower = 0;
        newInfo.TrainingGoldPower = 1f;
        newInfo.TrainingRevive = 0;
        newInfo.TrainingShieldTime = 0;
        newInfo.TrainingDodgeTime = 0f;
        newInfo.MoveSpeedRate = 1f;
        newInfo.CurrentWeaponName = "";
        newInfo.CurrentCostumeName = "";
        newInfo.CurrentCostumeShapeName = "";
        newInfo.Gold = 0;
        newInfo.Diamond = 10000;
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
        newInfo.DailyAddCount = 3;
        newInfo.IsAdsPass = false;
        mIsCreate = false;
        SavePlayerInfoFile(newInfo);
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
            // 전체적인 진행도
            newData.Progress.Add(key, AchievementManager.Instance.Achievements[key].GetComponent<Achievement>().State);
        }

        // 모드 클리어
        newData.ModeClear = new List<int>();
        newData.ModeClear.Add(0);
        newData.ModeClear.Add(0);

        // 처지(무기)
        newData.KillToWeapon = new List<int>();
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);
        newData.KillToWeapon.Add(0);

        //처치(스킬)
        newData.KillToSkill = new StringInt();
        newData.KillToSkill.Add("Sword Release", 0);
        newData.KillToSkill.Add("Storm", 0);
        newData.KillToSkill.Add("Cross Fire", 0);
        newData.KillToSkill.Add("Incinerate", 0);

        //생존시간(무기)
        newData.TimeToWeapon = new List<int>();
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);
        newData.TimeToWeapon.Add(0);

        //생존시간(코스튬)
        newData.TimeToCostume = new List<int>();
        newData.TimeToCostume.Add(0);
        newData.TimeToCostume.Add(0);

        // 보스처지(무기)
        newData.BossKillToWeapon = new List<IntInt>();
        for(EquipmentManager.WeaponType i = EquipmentManager.WeaponType.sw; i < EquipmentManager.WeaponType.Exit; i++)
        {
            newData.BossKillToWeapon.Add(new IntInt());
            newData.BossKillToWeapon[(int)i].Add(0, 0);
            newData.BossKillToWeapon[(int)i].Add(1, 0);
            newData.BossKillToWeapon[(int)i].Add(2, 0);
            newData.BossKillToWeapon[(int)i].Add(3, 0);
        }

        // 보스처치(코스튬)
        newData.BossKillToCostume = new List<IntInt>();
        for (EquipmentManager.CostumeTpye i = EquipmentManager.CostumeTpye.swsp; i < EquipmentManager.CostumeTpye.bgstswsp; i++)
        {
            newData.BossKillToCostume.Add(new IntInt());
            newData.BossKillToCostume[(int)i].Add(0, 0);
            newData.BossKillToCostume[(int)i].Add(1, 0);
            newData.BossKillToCostume[(int)i].Add(2, 0);
            newData.BossKillToCostume[(int)i].Add(3, 0);
        }

        // 웨이브 클리어(무기)
        newData.WaveClearToWeapon = new List<IntInt>();
        for (EquipmentManager.WeaponType i = EquipmentManager.WeaponType.sw; i < EquipmentManager.WeaponType.Exit; i++)
        {
            newData.WaveClearToWeapon.Add(new IntInt());
            newData.WaveClearToWeapon[(int)i].Add(0, 0);
            newData.WaveClearToWeapon[(int)i].Add(1, 0);
            newData.WaveClearToWeapon[(int)i].Add(2, 0);
            newData.WaveClearToWeapon[(int)i].Add(3, 0);
        }

        // 보스 모드 클리어
        newData.BossModeWaveClear = new List<int>();
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        newData.BossModeWaveClear.Add(0);
        // 파일 저장
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
            _info.Date = DateTime.Now.Day;
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
    }

    // 전투씬에 보낼 데이터 저장
    public void SavePlayerWarData(WarInfo _info)
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mWarFileName);
        string fileStream = JsonUtility.ToJson(_info, true);
        //fileStream = AES.Encrypt(fileStream, AES.key);
        File.WriteAllText(path, fileStream);
    }

    // 전투씬에서 로드할 데이터
    public WarInfo LoadPlayerWarData()
    {
        string path = "";
        path = Path.Combine(Application.persistentDataPath, mWarFileName);
        string fileStream = File.ReadAllText(path);

        //fileStream = AES.Decrypt(fileStream, AES.key);

        WarInfo loadInfo = JsonUtility.FromJson<WarInfo>(fileStream);
        return loadInfo;
    }

    // 전투가 끝난 후 저장하는 데이터
    public void SaveWarEndData()
    {
        LobbyPlayerInfo info = LoadBaseData();

        // 전투가 끝난 후 변경사항 저장
        // 얻은 골드, 다이아...
        // TODO : 업적관리에 필요한 데이터 추가
        info.Diamond = PlayerManager.Instance.Player.GetComponent<PlayerStatus>().Diamond;
        // 획득 골드 수치
        info.Gold += (int)(PlayerManager.Instance.Player.GetComponent<PlayerStatus>().GainGold * PlayerManager.Instance.EndGoldRate);

        SavePlayerInfoFile(info);

        LobbyPlayerAchievementData achieveInfo = LoadAchieveData();

        string weaponType = PlayerManager.Instance.Player.GetComponent<PlayerStatus>()
            .PlayerCurrentWeapon.Spec.Type.Substring(0, 2);
        EquipmentManager.WeaponType type = (EquipmentManager.WeaponType)Enum.Parse
            (typeof(EquipmentManager.WeaponType), weaponType);

        // 무기별 킬 체크
        achieveInfo.KillToWeapon[(int)type] += SpawnManager.currentKillMosterCount;
        // 무기별 생존 시간 체크
        achieveInfo.TimeToWeapon[(int)type] += (int)UIManager.Instance.GamePlayerTime;
        // 코스튬 생존 시간 체크(누적x)
        if ((EquipmentManager.CostumeTpye.swsp).ToString().Contains(weaponType))
            achieveInfo.TimeToCostume[0] = Mathf.Max(achieveInfo.TimeToCostume[0], (int)UIManager.Instance.GamePlayerTime);
        else
            achieveInfo.TimeToCostume[1] = Mathf.Max(achieveInfo.TimeToCostume[1], (int)UIManager.Instance.GamePlayerTime);
        // 웨이브 및 보스 킬체크는 생존모드에서만
        if (MapManager.Instance.CurrentMapType != MapManager.MapType.BossRelay)
        {
            // 무기별 보스 킬 체크
            for (int i = 0; i < SpawnManager.currentKillBossMonsterCount; i++)
            {
                achieveInfo.BossKillToWeapon[(int)type][i]++;
            }

            // 무기별 웨이브 클리어 체크
            for (int i = 0; i < SpawnManager.Instance.WaveCount; i++)
            {
                achieveInfo.WaveClearToWeapon[(int)type][i]++;
            }
            // 스킬별 킬 체크
            if (achieveInfo.KillToSkill.ContainsKey(PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentGeneralSkill.name))
            {
                achieveInfo.KillToSkill[PlayerManager.Instance.Player.GetComponent<PlayerAttack>().CurrentGeneralSkill.name]
                    += SpawnManager.currentKillMosterCount;
            }
            // 코스튬 보스 킬 체크
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
        }

        // 모드 도전 횟수
        if (MapManager.Instance.CurrentMapType == MapManager.MapType.BossRelay)
            achieveInfo.ModeClear[1]++;
        else
            achieveInfo.ModeClear[0]++;

        // 보스 릴레이 모드 클리어 단계수
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
