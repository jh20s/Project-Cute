using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrainingManager : SingleToneMaker<TrainingManager>
{
    [Serializable]
    public struct Training
    {
        public TrainingType mType;
        public string mName;
        public string mDesc;
        public int mLevel;
        public float mIncrease;
        public float mMax;
        public int mCostMin;
        public int mCostIncrease;
        public TrainingType mPriorType;
        public Sprite mIcon;

        public int mCurrentCost;
        public float mCurrentValue;
        public string mUnit;
    }
    public enum TrainingType
    {
        None,
        PlayerHP,
        PlayerATK,
        PlayerDamage,
        PlayerDamageZ,
        PlayerMagnet,
        PlayerGold,
        PlayerRevive,
        PlayerShield,
        PlayerDodge,
        Exit,
    }

    [SerializeField]
    private TrainingSet mTrainingSet;
    public TrainingSet TrainingSet
    {
        get { return mTrainingSet; }
        set { mTrainingSet = value; }
    }

    [SerializeField]
    private TrainingButtonSet mTrainingButtonSet;
    public TrainingButtonSet TrainingButtonSet
    {
        get => mTrainingButtonSet;
        set
        {
            mTrainingButtonSet = value;
        }
    }

    [SerializeField]
    private GameObject mTrainingPannel;
    [SerializeField]
    private GameObject mContainer;
    [SerializeField]
    private GameObject mPrefab;

    [SerializeField]
    private TrainingType mCurrentSelectType;
    public TrainingType CurrentSelectType => mCurrentSelectType;

    // Start is called before the first frame update
    void Start()
    {
        initTrainingSet();
    }

    // csv�����Ϳ�, �÷��̾��� �����͸� ���Ͽ� �ʱ�ȭ
    private void initTrainingSet()
    {
        List<Dictionary<string, object>> trainingData = CSVReader.Read("CSVFile/TrainingData");
        List<int> playerSaveData = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.TrainingLevelList;

        for(TrainingType type = TrainingType.PlayerHP; type < TrainingType.Exit; type++)
        {
            Training newTraining = new Training();
            newTraining.mType = (TrainingType)Enum.Parse(typeof(TrainingType), trainingData[(int)type - 1]["StatusType"].ToString());
            newTraining.mName = trainingData[(int)type - 1]["StatusName"].ToString();
            newTraining.mDesc = trainingData[(int)type - 1]["StatusDesc"].ToString();
            newTraining.mIncrease = float.Parse(trainingData[(int)type - 1]["StatusIncrease"].ToString());
            newTraining.mMax = float.Parse(trainingData[(int)type - 1]["StatusMax"].ToString());
            newTraining.mCostMin = int.Parse(trainingData[(int)type - 1]["StatusCostMin"].ToString());
            newTraining.mCostIncrease = int.Parse(trainingData[(int)type - 1]["StatusCostIncrease"].ToString());
            newTraining.mUnit = trainingData[(int)type - 1]["StatusUnit"].ToString();
            newTraining.mPriorType = (TrainingType)Enum.Parse(typeof(TrainingType), trainingData[(int)type - 1]["StatusPriorType"].ToString());
            newTraining.mIcon = Resources.Load<Sprite>(trainingData[(int)type - 1]["StatusIcon"].ToString()); 

            newTraining.mLevel = playerSaveData[(int)type - 1];
            newTraining.mCurrentValue = newTraining.mLevel * newTraining.mIncrease;
            newTraining.mCurrentCost = newTraining.mCostMin + newTraining.mLevel * newTraining.mCostIncrease;

            mTrainingSet.Add(type, newTraining);

            // �Ʒ� �гο� Item�߰�
            GameObject newButton = Instantiate(mPrefab, mContainer.transform);
            // Item�� Ŭ�������� �߰�
            TrainingType newType = type;
            newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                ClickStatus(newType);
            });
            // ��ư �¿� �߰�
            mTrainingButtonSet.Add(type, newButton);
        }
    }
    // �� ���� ��ư ������
    private void ClickStatus(TrainingType _type)
    {
        // ���� �������� Ÿ�� ����
        mCurrentSelectType = _type;
        // ���� ǥ��
        for(TrainingType type = TrainingType.PlayerHP; type < TrainingType.Exit; type++)
        {
            if(type == _type)
                mTrainingButtonSet[type].transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            else
                mTrainingButtonSet[type].transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        }
        // ����â�� ���� ǥ��
        mTrainingPannel.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite =
            mTrainingSet[_type].mIcon;
        mTrainingPannel.transform.GetChild(3).GetChild(1).GetComponent<Text>().text =
            mTrainingSet[_type].mName;
        mTrainingPannel.transform.GetChild(3).GetChild(2).GetComponent<Text>().text =
            mTrainingSet[_type].mDesc;
        mTrainingPannel.transform.GetChild(3).gameObject.SetActive(true);
    }
    // ���� ����Ʈ UI ������Ʈ
    public void UpdateStstus()
    {
        for (TrainingType type = TrainingType.PlayerHP; type < TrainingType.Exit; type++)
        {
            Training training = mTrainingSet[type];
            GameObject button = mTrainingButtonSet[type];
            // �Ʒ� �������� �Ǵ�
            if (PossibleForLevelUp(type))
            {
                // �ش� �Ʒ� ������ ǥ��
                button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = training.mIcon;
                // �ش� �Ʒ��� Max�̸� ������ġ Max�� ǥ��
                if (training.mMax == training.mCurrentValue)
                {
                    button.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "MAX";
                    button.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else
                {
                    button.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "Lv. " + training.mLevel.ToString();
                    button.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.yellow;
                }

                // ���� �� ��ġ ǥ��
                button.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = training.mName;
                button.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "+ " + training.mCurrentValue + training.mUnit;
            }
            else
            {
                // ��� ǥ��
                button.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = mTrainingSet[training.mPriorType].mName;
                button.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    // �Ʒ� ������ �� ������ ����
    public void LevelUpTraining(TrainingType _type)
    {
        Training newTraining = mTrainingSet[_type];
        mTrainingSet.Remove(_type);

        newTraining.mLevel++;
        newTraining.mCurrentValue = newTraining.mLevel * newTraining.mIncrease;
        newTraining.mCurrentCost = newTraining.mCostMin + newTraining.mLevel * newTraining.mCostIncrease;

        LobbyPlayerInfo playerInfo = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info;
        playerInfo.TrainingLevelList[(int)_type - 1] += 1;
        switch (_type)
        {
            case TrainingType.PlayerHP:
                playerInfo.TrainingHp += (int)newTraining.mIncrease;
                break;
            case TrainingType.PlayerATK:
                playerInfo.TrainingATK += (int)newTraining.mIncrease;
                break;
            case TrainingType.PlayerDamage:
                playerInfo.TrainingAddDamage += newTraining.mIncrease * 0.01f;
                break;
            case TrainingType.PlayerDamageZ:
                playerInfo.TrainingAddDamage += newTraining.mIncrease * 0.01f;
                break;
            case TrainingType.PlayerMagnet:
                playerInfo.TrainingMagnetPower += (int)newTraining.mIncrease;
                break;
            case TrainingType.PlayerRevive:
                playerInfo.TrainingRevive += (int)newTraining.mIncrease;
                break;
            case TrainingType.PlayerGold:
                playerInfo.TrainingGoldPower += newTraining.mIncrease * 0.01f;
                break;
            case TrainingType.PlayerShield:
                playerInfo.TrainingShieldTime += newTraining.mIncrease;
                break;
            case TrainingType.PlayerDodge:
                playerInfo.TrainingDodgeTime += (int)newTraining.mIncrease;
                break;
        }
        mTrainingSet.Add(_type, newTraining);
    }

    // �������� ������ �����ϴ� API
    // ���� ���� value����
    public float NextLevelValue(TrainingType _type)
    {
        Training newTraining = mTrainingSet[_type];

        return (newTraining.mLevel + 1) * newTraining.mIncrease;
    }
    // ���� ���� cost����
    public int NextLevelCost(TrainingType _type)
    {
        Training newTraining = mTrainingSet[_type];

        return newTraining.mCostMin + (newTraining.mLevel + 1) * newTraining.mCostIncrease;
    }
    // ���� �Ʒ��� ������ ���ִ��� �Ǵ�
    public bool PossibleForLevelUp(TrainingType _type)
    {
        Training newTraining = mTrainingSet[_type];

        if (newTraining.mPriorType == TrainingType.None)
            return true;
        else
        {
            if (mTrainingSet[newTraining.mPriorType].mCurrentValue == mTrainingSet[newTraining.mPriorType].mMax)
                return true;
            else
                return false;
        }
    }
}
