using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        AutoAttackTimesMelee,
        AutoAttackTimesRange,
        AutoAttackRange,
        AutoAttackStiff,
        AutoLaunchSpread,
        AutoLaunchThrough,
        RecoverHP
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

    // ���� ���� �˸� â ����
    // 0 : ����, 1 : ���罺��, 2 : Max����
    [Header("��������UI")]
    // ����
    [SerializeField]
    private Text[] mAutoAttackText = new Text[3];
    [SerializeField]
    private Text[] mCriticalChanceText = new Text[3];
    [SerializeField]
    private Text[] mAutoAttackSPDText = new Text[3];
    [SerializeField]
    private Text[] mMoveSPDText = new Text[3];
    // �ٰŸ�
    [SerializeField]
    private Text[] mAutoAttackTimesMeleeText = new Text[3];
    [SerializeField]
    private Text[] mAutoAttackRangeText = new Text[3];
    [SerializeField]
    private Text[] mAutoAttackStiffText = new Text[3];
    // ���Ÿ�
    [SerializeField]
    private Text[] mAutoAttackTimesRangeText = new Text[3];
    [SerializeField]
    private Text[] mAutoLaunchSpreadText = new Text[3];
    [SerializeField]
    private Text[] mAutoLaunchThroughText = new Text[3];
    // Start is called before the first frame update
    void Start()
    {
        InitStatData();
    }
    private void InitStatData()
    {
        List<Dictionary<string, object>> statusData = CSVReader.Read("CSVFile/LevelUpStatus");

        for(int i = 0; i < statusData.Count; i++)
        {
            Stat newStat = new Stat();
            string[] tmp;
            newStat.Type = (StatType)Enum.Parse(typeof(StatType), statusData[i]["StatType"].ToString());

            tmp = statusData[i]["StatWeaponType"].ToString().Split('/');
            newStat.WeaponTypes = new List<string>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.WeaponTypes.Add(tmp[j]);
            tmp = null;

            newStat.Desc = statusData[i]["StatDesc"].ToString();
            newStat.DescEng = statusData[i]["StatDescENG"].ToString();

            tmp = statusData[i]["StatInSlot"].ToString().Split('/');
            newStat.StatInSlot = new List<int>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.StatInSlot.Add(int.Parse(tmp[j]));
            tmp = null;

            tmp = statusData[i]["StatChance"].ToString().Split('/');
            newStat.StatChance = new List<int>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.StatChance.Add(int.Parse(tmp[j]));
            tmp = null;

            newStat.StatIncrease = float.Parse(statusData[i]["StatIncrease"].ToString());
            if (float.Parse(statusData[i]["StatMax"].ToString()) < 0)
                newStat.StatMax = int.MaxValue;
            else
                newStat.StatMax = float.Parse(statusData[i]["StatMax"].ToString());

            tmp = statusData[i]["StatMaxAssign"].ToString().Split('/');
            newStat.StatMaxAssign = new List<StatType>();
            for (int j = 0; j < tmp.Length; j++)
                newStat.StatMaxAssign.Add((StatType)Enum.Parse(typeof(StatType), tmp[j]));
            tmp = null;

            newStat.StatUnit = statusData[i]["StatUnit"].ToString();
            newStat.SelectCount = -1;
            newStat.SelectMaxCount = (int)Mathf.Ceil(newStat.StatMax/newStat.StatIncrease);
           
            newStat.StatImage = Resources.Load<Sprite>("UI/WarUI/WarIcon/" + newStat.Type.ToString());
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
            for(int j = 0; j < AllStatList[i].WeaponTypes.Count; j++)
            {
                // �ش�Ÿ���� ������ ������ �ִٸ� ���Կ� �°� �־��ش�
                if (AllStatList[i].WeaponTypes[j] == _weaponType && AllStatList[i].Type != StatType.RecoverHP)
                {
                    for(int k = 0; k < AllStatList[i].StatInSlot.Count; k++)
                    {
                        switch (AllStatList[i].StatInSlot[k])
                        {
                            case 1:
                                for(int idx = 0; idx < AllStatList[i].StatChance[k]; idx++)
                                    Slot1List.Add(AllStatList[i].Type);
                                break;
                            case 2:
                                for (int idx = 0; idx < AllStatList[i].StatChance[k]; idx++)
                                    Slot2List.Add(AllStatList[i].Type);
                                break;
                            case 3:
                                for (int idx = 0; idx < AllStatList[i].StatChance[k]; idx++)
                                    Slot3List.Add(AllStatList[i].Type);
                                break;
                        }
                    }
                }
            }
        }
        // ���õ� ���⿡ �°� �̺�Ʈ ��� apiȣ��
        RegisterStatHendler(_weaponType);

    }

    // �� ��ġ���� �̺�Ʈ �ڵ鷯 ���
    private void RegisterStatHendler(string _weaponType)
    {
        for (int i = 0; i < AllStatList.Count; i++)
        {
            for (int j = 0; j < AllStatList[i].WeaponTypes.Count; j++) 
            { 
                // �� ���⿡ �´� Ÿ���� ������ �̺�Ʈ ���
                if(AllStatList[i].WeaponTypes[j] == _weaponType)
                {
                    AllStatList[i].Hendler.registerIntObserver(RegisterIntObserver);
                    AllStatList[i].PlusSelectCount();
                }
            }
        }
    }

    // ����� �ż���
    private void RegisterIntObserver(int _value, StatType _type)
    {
        Stat tmpStat = AllStatList.Find((item) => item.Type == _type);
        switch (_type)
        {
            // �⺻ ���� ���ط� ����
            case StatType.AutoAttack:
                mAutoAttackText[0].text = tmpStat.Desc;
                mAutoAttackText[2].text = (Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100 * 10) / 10).ToString() + tmpStat.StatUnit;
                mAutoAttackText[1].text = Mathf.Floor(tmpStat.StatMax * 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mAutoAttackText);
                break;
            // ġ��Ÿ Ȯ�� ����
            case StatType.CriticalChance:
                mCriticalChanceText[0].text = tmpStat.Desc;
                mCriticalChanceText[2].text = (Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100 * 10) / 10).ToString() + tmpStat.StatUnit;
                mCriticalChanceText[1].text = Mathf.Floor(tmpStat.StatMax * 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mCriticalChanceText);
                break;
            // �⺻ ���� �ӵ� ����
            case StatType.AutoAttackSPD:
                mAutoAttackSPDText[0].text = tmpStat.Desc;
                mAutoAttackSPDText[2].text = Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100).ToString() + tmpStat.StatUnit;
                mAutoAttackSPDText[1].text = Mathf.Floor(tmpStat.StatMax * 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mAutoAttackSPDText);
                break;
            // �̵� �ӵ� ����
            case StatType.MoveSPD:
                mMoveSPDText[0].text = tmpStat.Desc;
                mMoveSPDText[2].text = Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100).ToString() + tmpStat.StatUnit;
                mMoveSPDText[1].text = Mathf.Floor(tmpStat.StatMax * 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mMoveSPDText);
                break;
            // �߻�ü ���� ����(�ٰŸ�)
            case StatType.AutoAttackTimesMelee:
                mAutoAttackTimesMeleeText[0].text = tmpStat.Desc;
                mAutoAttackTimesMeleeText[2].text = (tmpStat.SelectCount * tmpStat.StatIncrease).ToString();
                mAutoAttackTimesMeleeText[1].text = tmpStat.StatMax.ToString();
                ChangeMaxStat(tmpStat, ref mAutoAttackTimesMeleeText);
                break;
            // �߻�ü ���� ����(���Ÿ�)
            case StatType.AutoAttackTimesRange:
                mAutoAttackTimesRangeText[0].text = tmpStat.Desc;
                mAutoAttackTimesRangeText[2].text = (tmpStat.SelectCount * tmpStat.StatIncrease).ToString();
                mAutoAttackTimesRangeText[1].text = tmpStat.StatMax.ToString();
                ChangeMaxStat(tmpStat, ref mAutoAttackTimesRangeText);
                break;
            // �⺻ ���� ���� ����(��)
            case StatType.AutoAttackRange:
                mAutoAttackRangeText[0].text = tmpStat.Desc;
                mAutoAttackRangeText[2].text = Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100).ToString() + tmpStat.StatUnit;
                mAutoAttackRangeText[1].text = Mathf.Floor(tmpStat.StatMax * 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mAutoAttackRangeText);
                break;
            // �⺻ ���� ���� ����(��)
            case StatType.AutoAttackStiff:
                mAutoAttackStiffText[0].text = tmpStat.Desc;
                mAutoAttackStiffText[2].text = (Mathf.Floor(tmpStat.SelectCount * tmpStat.StatIncrease * 100)/100).ToString() + tmpStat.StatUnit;
                mAutoAttackStiffText[1].text = (Mathf.Floor(tmpStat.StatMax * 100) / 100).ToString() + tmpStat.StatUnit;
                ChangeMaxStat(tmpStat, ref mAutoAttackStiffText);
                break;
            // �߻�ü Ƚ�� ����(��)
            case StatType.AutoLaunchSpread:
                mAutoLaunchSpreadText[0].text = tmpStat.Desc;
                mAutoLaunchSpreadText[2].text = (tmpStat.SelectCount * tmpStat.StatIncrease).ToString();
                mAutoLaunchSpreadText[1].text = tmpStat.StatMax.ToString();
                ChangeMaxStat(tmpStat, ref mAutoLaunchSpreadText);
                break;
            // �߻�ü ���� ����(��)
            case StatType.AutoLaunchThrough:
                mAutoLaunchThroughText[0].text = tmpStat.Desc;
                mAutoLaunchThroughText[2].text = (tmpStat.SelectCount * tmpStat.StatIncrease).ToString();
                mAutoLaunchThroughText[1].text = tmpStat.StatMax.ToString();
                ChangeMaxStat(tmpStat, ref mAutoLaunchThroughText);
                break;
        }
    }

    // �ش� ������ MAX�� �� ���������� ����
    private void ChangeMaxStat(Stat _stat, ref Text[] _text)
    {
        if(_stat.SelectCount == _stat.SelectMaxCount)
        {
            for(int i = 0; i < _text.Length; i++)
            {
                _text[i].color = Color.red;
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
            Stat selectStat = null;
            // ���� �ش� ����Ʈ�� ����ٸ� HP ȸ���� �߰� �Ѵ�. 
            if (_list.Count == 0) _list.Add(StatType.RecoverHP);
            int ran = UnityEngine.Random.Range(0, _list.Count);
            selectStat = AllStatList.Find((item) => item.Type == _list[ran]);
            // ���õ� ������ �ƽ��̸�?
            if (selectStat.SelectCount == selectStat.SelectMaxCount)
            {
                // �켱 �ش� ������ ����
                DeleteStatType(selectStat.Type);
                continue;
            }
            else
            {
                switch (_num)
                {
                    case 1:
                        mSlot1Select = selectStat.Type;
                        break;
                    case 2:
                        mSlot2Select = selectStat.Type;
                        break;
                    case 3:
                        mSlot3Select = selectStat.Type;
                        break;
                }
                switch (selectStat.StatUnit)
                {
                    case "%":
                        desc = selectStat.Desc + (selectStat.StatIncrease * 100).ToString() + selectStat.StatUnit;
                        break;
                    default:
                        desc = selectStat.Desc + selectStat.StatIncrease.ToString() + selectStat.StatUnit;
                        break;
                }
                return desc;
            }
        }
    }

    private void DeleteStatType(StatType _type)
    {
        Slot1List.RemoveAll((type) => type == _type);
        Slot2List.RemoveAll((type) => type == _type);
        Slot3List.RemoveAll((type) => type == _type);
    }
    public Sprite SelectSlotImage(int _num)
    {
        StatType selectType = StatType.Null;
        Sprite newSprite = null;
        switch (_num)
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
        for (int i = 0; i < AllStatList.Count; i++)
        {
            if (AllStatList[i].Type == selectType)
            {
                newSprite = AllStatList[i].StatImage;
            }
        }
        return newSprite;
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
            if(AllStatList[i].Type == selectType)
            {
                AllStatList[i].PlusSelectCount();
                // IStatus�� apiȣ��
                GameObject.Find("PlayerObject").GetComponent<IStatus>().LevelUpStatus(AllStatList[i]);
                break;
            }
        }
    }

}
