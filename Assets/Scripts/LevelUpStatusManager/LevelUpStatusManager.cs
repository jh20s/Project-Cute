using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUpStatusManager : SingleToneMaker<LevelUpStatusManager>
{
    [Serializable]
    public enum StatType
    {
        Null,
        AutoAttack,
        CriticalChance,
        AutoAttackSPD,
        MoveSPD,
        AutoAttackTimes,
        AutoAttackRange,
        AutoAttackStiff,
        AutoLaunchSpread,
        AutoLaunchThrough,
        RecoverHP
    }
    [Serializable]
    public struct Stat
    {
        public StatType mType;
        public List<string> mWeaponTypes;
        public string mDesc;
        public string mDescEng;
        public List<int> mStatInSlot;
        public List<int> mStatChance;
        public float mStatIncrease;
        public float mStatMax;
        public List<StatType> mStatMaxAssign;
        public string mStatUnit;
        public int mSelectCount;
        public int mSelectMaxCount;

        public void PlusSelectCount()
        {
            mSelectCount++;
        }
    }
    [SerializeField]
    private List<Stat> AllStatList = new List<Stat>();

    [SerializeField]
    private List<StatType> Slot1List = new List<StatType>();
    [SerializeField]
    private List<StatType> Slot2List = new List<StatType>();
    [SerializeField]
    private List<StatType> Slot3List = new List<StatType>();

    [SerializeField]
    private StatType mSlot1Select;
    [SerializeField]
    private StatType mSlot2Select;
    [SerializeField]
    private StatType mSlot3Select;
    // Start is called before the first frame update
    void Start()
    {
        InitStatData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitStatData()
    {
        List<Dictionary<string, object>> statusData = CSVReader.Read("CSVFile/LevelUpStatus");

        for(int i = 0; i < statusData.Count; i++)
        {
            Stat newStat = new Stat();
            string[] tmp;
            newStat.mType = (StatType)Enum.Parse(typeof(StatType), statusData[i]["StatType"].ToString());

            tmp = statusData[i]["StatWeaponType"].ToString().Split('/');
            newStat.mWeaponTypes = new List<string>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.mWeaponTypes.Add(tmp[j]);
            tmp = null;

            newStat.mDesc = statusData[i]["StatDesc"].ToString();
            newStat.mDescEng = statusData[i]["StatDescENG"].ToString();

            tmp = statusData[i]["StatInSlot"].ToString().Split('/');
            newStat.mStatInSlot = new List<int>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.mStatInSlot.Add(int.Parse(tmp[j]));
            tmp = null;

            tmp = statusData[i]["StatChance"].ToString().Split('/');
            newStat.mStatChance = new List<int>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.mStatChance.Add(int.Parse(tmp[j]));
            tmp = null;

            newStat.mStatIncrease = float.Parse(statusData[i]["StatIncrease"].ToString());
            newStat.mStatMax = float.Parse(statusData[i]["StatMax"].ToString());

            tmp = statusData[i]["StatMaxAssign"].ToString().Split('/');
            newStat.mStatMaxAssign = new List<StatType>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.mStatMaxAssign.Add((StatType)Enum.Parse(typeof(StatType), tmp[j]));
            tmp = null;

            newStat.mStatUnit = statusData[i]["StatUnit"].ToString();
            newStat.mSelectCount = 0;
            newStat.mSelectMaxCount = (int)(newStat.mStatMax / newStat.mStatIncrease);
            AllStatList.Add(newStat);
        }
    }

    // �� ����� ���������� �°� ���Կ� �־��ش�.
    // _weaponType : ������ Ÿ��
    public  void SetSlots(string _weaponType)
    {
        //string wpType = _currentWeapon.Spec.Type.Substring(0,2);
        for(int i = 0; i < AllStatList.Count; i++)
        {
            for(int j = 0; j < AllStatList[i].mWeaponTypes.Count; j++)
            {
                // �ش�Ÿ���� ������ ������ �ִٸ� ���Կ� �°� �־��ش�
                if (AllStatList[i].mWeaponTypes[j] == _weaponType && AllStatList[i].mType != StatType.RecoverHP)
                {
                    for(int k = 0; k < AllStatList[i].mStatInSlot.Count; k++)
                    {
                        switch (AllStatList[i].mStatInSlot[k])
                        {
                            case 1:
                                for(int idx = 0; idx < AllStatList[i].mStatChance[k]; idx++)
                                    Slot1List.Add(AllStatList[i].mType);
                                break;
                            case 2:
                                for (int idx = 0; idx < AllStatList[i].mStatChance[k]; idx++)
                                    Slot2List.Add(AllStatList[i].mType);
                                break;
                            case 3:
                                for (int idx = 0; idx < AllStatList[i].mStatChance[k]; idx++)
                                    Slot3List.Add(AllStatList[i].mType);
                                break;
                        }
                    }
                }
            }
        }

    }

    // �������� �������� ���� �̾��ִ� api
    public string SelectStatus(int _num)
    {
        string desc = "";
        switch (_num)
        {
            case 1:
                desc = SelectSlotList(Slot1List, _num);
                break;
            case 2:
                desc = SelectSlotList(Slot2List, _num);
                break;
            case 3:
                desc = SelectSlotList(Slot3List, _num);
                break;
        }

        return desc;
    }

    private string SelectSlotList(List<StatType> _list, int _num)
    {
        string desc = "";
        while (true)
        {
            int ran = UnityEngine.Random.Range(0, _list.Count);
            Stat selectStat = AllStatList.Find((item) => item.mType == _list[ran]);
            // ���õ� ������ �ƽ��̸�?
            if (selectStat.mSelectCount == selectStat.mSelectMaxCount)
            {
                // �켱 �ش� ������ ����
                Slot1List.RemoveAll((type) => type == selectStat.mType);
                Slot2List.RemoveAll((type) => type == selectStat.mType);
                Slot3List.RemoveAll((type) => type == selectStat.mType);
            }
            else
            {
                switch (_num)
                {
                    case 1:
                        mSlot1Select = selectStat.mType;
                        break;
                    case 2:
                        mSlot2Select = selectStat.mType;
                        break;
                    case 3:
                        mSlot3Select = selectStat.mType;
                        break;
                }
                switch (selectStat.mStatUnit)
                {
                    case "%":
                        desc = selectStat.mDesc + (selectStat.mStatIncrease * 100).ToString() + selectStat.mStatUnit;
                        break;
                    default:
                        desc = selectStat.mDesc + selectStat.mStatIncrease.ToString() + selectStat.mStatUnit;
                        break;
                }
                return desc;
            }
        }
    }

    // �ش� ���� ���� �� IStatus�� api�� ȣ��
    public void SelectToStat(int _type)
    {
        StatType selectType = StatType.Null;
        switch (_type)
        {
            case 1:
                selectType = mSlot1Select;
                break;
            case 2:
                selectType = mSlot2Select;
                break;
            case 3:
                selectType = mSlot3Select;
                break;
        }
        for(int i = 0; i < AllStatList.Count; i++)
        {
            if(AllStatList[i].mType == selectType)
            {
                AllStatList[i].PlusSelectCount();
                // IStatus�� apiȣ��
                GameObject.Find("PlayerObject").GetComponent<IStatus>().LevelUpStatus(AllStatList[i]);
                break;
            }
        }
    }
}
