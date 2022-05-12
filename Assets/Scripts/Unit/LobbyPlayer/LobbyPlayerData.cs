using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayerData : MonoBehaviour
{
    [SerializeField]
    private LobbyPlayerInfo mInfo = null;
    public LobbyPlayerInfo Info
    {
        get { return mInfo; }
        set 
        { 
            mInfo = value;
            GetComponent<LobbyPlayerEventHendler>().ChangeMoveSpeed(mInfo.BaseSPD * mInfo.MoveSpeedRate);
            GetComponent<LobbyPlayerEventHendler>().ChangeGoods(mInfo.Gold, mInfo.Diamond);
        }
    }

    [SerializeField]
    private LobbyPlayerAchievementData mAchieveData = null;
    public LobbyPlayerAchievementData AchieveData 
    {
        get => mAchieveData;
        set
        {
            mAchieveData = value;
        }
    }

    [SerializeField]
    private bool mIsChangedDate;
    private void Start()
    {
        Info = SaveLoadManager.Instance.LoadBaseData();
        AchieveData = SaveLoadManager.Instance.LoadAchieveData();
        AchievementManager.Instance.SaveDataLoadFromAchievement();
        DailyAdCountCheck();
        initEquip();
    }
    // ����� ��¥�� Ȯ���Ͽ� ����̽� �ð��� ���ؼ� ���� Ƚ�� ����
    public void DailyAdCountCheck()
    {
        StartCoroutine(WebChk());
    }
    // ������ �ð��� �������� �ڷ�ƾ �Լ�
    IEnumerator WebChk()
    {
        string url = "www.naver.com";
        UnityWebRequest request = new UnityWebRequest();
        using(request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // ���ͳ� ���� ����
                LobbyUIManager.Instance.OpenAlertEnterPannel("���ͳ� ������ �ʿ��մϴ�.\n������ ���� �� �������� ���� �� �ֽ��ϴ�.");
            }
            else
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date).ToUniversalTime();
                mIsChangedDate = dateTime.Day != Info.Date;
            }
        }
        if (mIsChangedDate)
        {
            // ��¥�� �ٲ���ٸ� �ִ� ��ûȽ���� ����
            Info.DailyAddCount = 3;
        }
    }
    public void  initEquip()
    {
        if(mInfo.CurrentWeaponName != "")
        {
            EquipmentManager.Instance.ChangeWeaponLobbyPlayer(mInfo.CurrentWeaponName);
        }
        if(mInfo.CurrentCostumeName != "")
        {
            EquipmentManager.Instance.ChangeCostumeLobbyPlayer(mInfo.CurrentCostumeName);
        }
        if(mInfo.CurrentCostumeShapeName != "")
        {
            EquipmentManager.Instance.ChangeCostumeShapeLobbyPlayer(mInfo.CurrentCostumeShapeName);
        }

        // ���� �ر� ���� ����
        foreach(string key in mInfo.Weaponlock.Keys)
        {
            EquipmentManager.Instance.FindWeapon(key).GetComponent<Weapon>().IsLocked = mInfo.Weaponlock[key];
        }
        // �ڽ�Ƭ �ر� ���� ����
        foreach (string key in mInfo.Costumelock.Keys)
        {
            EquipmentManager.Instance.FindCostume(key).GetComponent<Costume>().IsLocked = mInfo.Costumelock[key];
        }
        // ��ų �ر� ���� ����
        foreach (string key in mInfo.Skilllock.Keys)
        {
            SkillManager.Instance.FindSkill(key).GetComponent<Skill>().Spec.IsLocked = mInfo.Skilllock[key];
        }
    }
}
