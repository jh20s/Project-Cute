using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WareHouseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mEquipButtonPrefab;

    [Header("����")]
    [SerializeField]
    private GameObject mWeaponPannel;
    [SerializeField]
    private GameObject mWeaponButtonImage;
    [SerializeField]
    private StringGameObject mWeaponButtonList;
    [SerializeField]
    private List<GameObject> mWContainer = new List<GameObject>();
    [SerializeField]
    private GameObject mWeaponInfoPannel;
    [SerializeField]
    private string mClickedWeaponType;
    [SerializeField]
    private GameObject mWeaponChangePannel;

    [Header("�ڽ�Ƭ")]
    [SerializeField]
    private GameObject mCostumePannel;
    [SerializeField]
    private GameObject mCostumeButtonImage;
    [SerializeField]
    private StringGameObject mCostumeButtonList;
    [SerializeField]
    private List<GameObject> mCContainer = new List<GameObject>();
    [SerializeField]
    private GameObject mCostumeInfoPannel;
    [SerializeField]
    private string mClickedCostumeType;
    [SerializeField]
    private GameObject mCostumeChangePannel;
    [SerializeField]
    private GameObject mCostumeShapeChangePannel;
    // Start is called before the first frame update
    void Start()
    {
        initWeaponButtonList();
        initCostumeButtonList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenWeaponPannel()
    {
        mWeaponPannel.SetActive(true);
        mWeaponButtonImage.SetActive(true);
        mCostumePannel.SetActive(false);
        mCostumeButtonImage.SetActive(false);


    }
    public void OpenCostumePannel()
    {
        mWeaponPannel.SetActive(false);
        mWeaponButtonImage.SetActive(false);
        mCostumePannel.SetActive(true);
        mCostumeButtonImage.SetActive(true);
    }

    // ���� ������ �ε�
    private void initWeaponButtonList()
    {
        mWeaponButtonList.Clear();
        // ���Ŵ������� �ش� Ÿ���� ��� �޾ƿ� ��ư�� ����� �־��ش�.
        for (EquipmentManager.WeaponType type = EquipmentManager.WeaponType.sw; 
            type < EquipmentManager.WeaponType.Exit; type++)
        {
            List<GameObject> newList = EquipmentManager.Instance.FindWepaonList(type.ToString());
            for (int i = 0; i < newList.Count; i++)
            {
                string weaponType = newList[i].GetComponent<Weapon>().Spec.Type;
                Sprite newSprite = newList[i].GetComponent<SpriteRenderer>().sprite;
                string equipName = newList[i].GetComponent<Weapon>().Spec.EquipName;
                int damage = newList[i].GetComponent<Weapon>().Spec.WeaponDamage;
                float addSpeed = newList[i].GetComponent<Weapon>().Spec.WeaponAddSpeed;
                string desc = newList[i].GetComponent<Weapon>().Spec.EquipDesc;
                GameObject newButton = Instantiate(
                    mEquipButtonPrefab, Vector3.zero, Quaternion.identity, mWContainer[(int)type].transform);
                newButton.GetComponent<RectTransform>().localScale = new Vector3(0.83f, 0.83f, 0.83f);
                newButton.GetComponent<EquipButton>().UnlockImage.SetActive(newList[i].GetComponent<Weapon>().IsLocked);
                newButton.GetComponent<EquipButton>().Image.GetComponent<Image>().sprite = newSprite;

                mWeaponButtonList.Add(weaponType, newButton);
                // �� ��ư�� ������ �޾��ֱ�
                newButton.GetComponent<Button>().onClick.AddListener(() => {ClickWeaponButton(weaponType, newSprite, equipName, damage, addSpeed, desc); } );
            }
        }

        // �ε� �� �����ǰ� �ִ� ��� ǥ��
        string currentWeapon = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName;
        if(currentWeapon != "")
        {
            mWeaponButtonList[currentWeapon].GetComponent<EquipButton>().EquipCheckImage.SetActive(true);
        }
    }
    private void ClickWeaponButton(string _type, Sprite _image, string _name, int _ATK, float _addSpeed, string _desc)
    {
        mClickedWeaponType = _type;
        mWeaponInfoPannel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _name;
        mWeaponInfoPannel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _image;
        mWeaponInfoPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "ATK : +" + _ATK.ToString();
        if (_addSpeed > 0)
            mWeaponInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "SPD : +" + (_addSpeed * 100).ToString() + "%";
        else
            mWeaponInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "";
        mWeaponInfoPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = _desc;
        mWeaponInfoPannel.SetActive(true);
    }
    public void CloseWeaponInfoPannel()
    {
        mWeaponInfoPannel.SetActive(false);
    }
    // ���� ����
    public void ClickChangeWeapon()
    {
        CloseWeaponInfoPannel();
        string costumeType = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName;
        if (EquipmentManager.Instance.CheckUnlockWeapon(mClickedWeaponType))
        {
            // ���� ������ �ڽ�Ƭ ���� Ȯ��
            if(costumeType == "" || costumeType.Substring(3, costumeType.Length - 5).Contains(mClickedWeaponType.Substring(0, 2)))
            {
                // ���� �ǳ� ����
                string currentWeapon = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName;
                if (currentWeapon != "")
                {
                    GameObject curWeapon = EquipmentManager.Instance.FindWeapon(currentWeapon);
                    mWeaponChangePannel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text
                        = curWeapon.GetComponent<Weapon>().Spec.EquipName;
                    mWeaponChangePannel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                        = curWeapon.GetComponent<SpriteRenderer>().sprite;
                    mWeaponChangePannel.transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<Text>().text
                        = "ATK : +" + curWeapon.GetComponent<Weapon>().Spec.WeaponDamage;
                    if (curWeapon.GetComponent<Weapon>().Spec.WeaponAddSpeed > 0)
                        mWeaponChangePannel.transform.GetChild(3).GetChild(4).GetChild(0).GetComponent<Text>().text
                            = "SPD : +" + (curWeapon.GetComponent<Weapon>().Spec.WeaponAddSpeed * 100).ToString() + "%";
                    else
                        mWeaponChangePannel.transform.GetChild(3).GetChild(4).GetChild(0).GetComponent<Text>().text = "";
                }

                GameObject newWeapon = EquipmentManager.Instance.FindWeapon(mClickedWeaponType);

                mWeaponChangePannel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text
                    = newWeapon.GetComponent<Weapon>().Spec.EquipName;
                mWeaponChangePannel.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = newWeapon.GetComponent<SpriteRenderer>().sprite;
                mWeaponChangePannel.transform.GetChild(4).GetChild(3).GetChild(0).GetComponent<Text>().text
                    = "ATK : +" + newWeapon.GetComponent<Weapon>().Spec.WeaponDamage;
                if (newWeapon.GetComponent<Weapon>().Spec.WeaponAddSpeed > 0)
                    mWeaponChangePannel.transform.GetChild(4).GetChild(4).GetChild(0).GetComponent<Text>().text
                        = "SPD : +" + (newWeapon.GetComponent<Weapon>().Spec.WeaponAddSpeed * 100).ToString() + "%";
                else
                    mWeaponChangePannel.transform.GetChild(4).GetChild(4).GetChild(0).GetComponent<Text>().text = "";

                mWeaponChangePannel.SetActive(true);
            }
            else
            {
                // ��� ����
                LobbyUIManager.Instance.OpenAlertEnterPannel("���� ������ �ڽ�Ƭ���δ� ���� �Ұ����� �����Դϴ�.");
            }
        }
        else
        {
            // ��� ����
            LobbyUIManager.Instance.OpenAlertEnterPannel("�ش� ����� �رݵǾ� ���� �ʽ��ϴ�.");
        }
    }
    public void CloseWeaponChangePannel()
    {
        mWeaponChangePannel.SetActive(false);
    }
    public void ChangeWeapon()
    {
        string currentWeapon = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName;
        if (currentWeapon != "")
            mWeaponButtonList[currentWeapon].GetComponent<EquipButton>().EquipCheckImage.SetActive(false);
        EquipmentManager.Instance.ChangeWeaponLobbyPlayer(mClickedWeaponType);
        mWeaponButtonList[mClickedWeaponType].GetComponent<EquipButton>().EquipCheckImage.SetActive(true);
        CloseWeaponChangePannel();
    }
    // ���� �ر� ���� ���� ��
    public void ChangeWeaponUnlock(string _name, bool _locked)
    {
        GameObject btn = mWeaponButtonList[_name];
        btn.GetComponent<EquipButton>().UnlockImage.SetActive(_locked);
    }

    // �ڽ�Ƭ ������ �ε�
    private void initCostumeButtonList()
    {
        mCostumeButtonList.Clear();
        for (EquipmentManager.CostumeTpye type = EquipmentManager.CostumeTpye.swsp;
            type < EquipmentManager.CostumeTpye.Exit; type++)
        {
            List<GameObject> newList = EquipmentManager.Instance.FindCostumeList(type.ToString());
            for (int i = 0; i < newList.Count; i++)
            {
                string costumeName = newList[i].name;
                EquipmentManager.CostumeTpye thisType = type;
                Sprite newSprite = newList[i].GetComponent<SpriteRenderer>().sprite;
                string equipName = newList[i].GetComponent<Costume>().Spec.EquipName;
                string desc = newList[i].GetComponent<Costume>().Spec.EquipDesc;
                string rank = newList[i].GetComponent<Costume>().Spec.EquipRankDesc;
                string source = newList[i].GetComponent<Costume>().Spec.EquipSource;
                int addHp = newList[i].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
                int addSPD = newList[i].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD);
                GameObject newButton = Instantiate(
                mEquipButtonPrefab, Vector3.zero, Quaternion.identity, mCContainer[(int)type].transform);
                newButton.GetComponent<EquipButton>().UnlockImage.SetActive(newList[i].GetComponent<Costume>().IsLocked);
                newButton.GetComponent<EquipButton>().Image.GetComponent<Image>().sprite = newSprite;
                switch (newList[i].GetComponent<Costume>().Spec.Rank)
                {
                    case 1:
                        newButton.GetComponent<EquipButton>().CommonRank.SetActive(true);
                        break;
                    case 2:
                        newButton.GetComponent<EquipButton>().AdvancedRank.SetActive(true);
                        break;
                    case 3:
                        newButton.GetComponent<EquipButton>().SuperiorRank.SetActive(true);
                        break;
                }

                mCostumeButtonList.Add(costumeName, newButton);
                newButton.GetComponent<Button>().onClick.AddListener(() => {
                    ClickCostumeButton(costumeName, thisType, newSprite, equipName, addHp, addSPD, desc, rank, source); });
            }
        }

        // �ε� �� �������ǰ� �ִ� ��� ǥ��
        string currentCostume = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName;
        if (currentCostume != "")
        {
            mCostumeButtonList[currentCostume].GetComponent<EquipButton>().EquipCheckImage.SetActive(true);
        }
        // �ε� �� ���� ���� ��� ǥ��
        currentCostume = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeShapeName;
        if(currentCostume != "")
        {
            mCostumeButtonList[currentCostume].GetComponent<EquipButton>().ShapeCheckImage.SetActive(true);
        }
    }
    private void ClickCostumeButton(string _typeName, EquipmentManager.CostumeTpye _type, Sprite _image, string _name, int _hp, int _addSpeed, string _desc, string _rank, string _source)
    {
        mClickedCostumeType = _typeName;
        mCostumeInfoPannel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _name;
        mCostumeInfoPannel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _image;

        if (_hp > 0)
            mCostumeInfoPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "HP : +" + _hp.ToString();
        else
            mCostumeInfoPannel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "";

        if(_addSpeed > 0)
            mCostumeInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "SPD : +" + _addSpeed.ToString() + "%";
        else
            mCostumeInfoPannel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "";

        mCostumeInfoPannel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = _source;
        mCostumeInfoPannel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = _desc;

        // TODO : ��ũ ������ֱ�
        mCostumeInfoPannel.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = _rank;

        switch (_type)
        {
            case EquipmentManager.CostumeTpye.swsp:
                mCostumeInfoPannel.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "�ҵ� �Ǵ� ���Ǿ� ���⿡ ���� �����մϴ�.";
                break;
            case EquipmentManager.CostumeTpye.bgst:
                mCostumeInfoPannel.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "������ �Ǵ� ���� ���⿡ ���� �����մϴ�.";
                break;
            case EquipmentManager.CostumeTpye.bgstswsp:
                mCostumeInfoPannel.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = "��� ���⿡ ���� �����մϴ�.";
                break;
        }
        
        mCostumeInfoPannel.SetActive(true);
    }
    public void CloseCostumeInfoPannel()
    {
        mCostumeInfoPannel.SetActive(false);
    }

    // �ڽ�Ƭ ����
    public void ClickChangeCostume()
    {
        CloseCostumeInfoPannel();
        string weaponType = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName;
        if(weaponType == "")
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("���Ⱑ �����Ǿ����� �ʽ��ϴ�.");
            return;
        }
        weaponType = weaponType.Substring(0, 2);
        if (EquipmentManager.Instance.CheckUnlockCostume(mClickedCostumeType))
        {
            // �ش� ����� �´� �ڽ�Ƭ
            if (mClickedCostumeType.Substring(3).Contains(weaponType))
            {
                string currentCostume = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName;
                if (currentCostume != "")
                {
                    GameObject curCostume = EquipmentManager.Instance.FindCostume(currentCostume);
                    mCostumeChangePannel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text
                        = curCostume.GetComponent<Costume>().Spec.EquipName;
                    mCostumeChangePannel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                        = curCostume.GetComponent<SpriteRenderer>().sprite;
                    if (curCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP) > 0)
                        mCostumeChangePannel.transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<Text>().text
                            = "HP : +" + curCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
                    else
                        mCostumeChangePannel.transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<Text>().text = "";
                    if (curCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) > 0)
                        mCostumeChangePannel.transform.GetChild(3).GetChild(4).GetChild(0).GetComponent<Text>().text
                            = "SPD : +" + curCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) + "%";
                    else
                        mCostumeChangePannel.transform.GetChild(3).GetChild(4).GetChild(0).GetComponent<Text>().text = "";
                }

                GameObject newCostume = EquipmentManager.Instance.FindCostume(mClickedCostumeType);

                mCostumeChangePannel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text
                         = newCostume.GetComponent<Costume>().Spec.EquipName;
                mCostumeChangePannel.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = newCostume.GetComponent<SpriteRenderer>().sprite;
                if (newCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP) > 0)
                    mCostumeChangePannel.transform.GetChild(4).GetChild(3).GetChild(0).GetComponent<Text>().text
                        = "HP : +" + newCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
                else
                    mCostumeChangePannel.transform.GetChild(4).GetChild(3).GetChild(0).GetComponent<Text>().text = "";
                if (newCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) > 0)
                    mCostumeChangePannel.transform.GetChild(4).GetChild(4).GetChild(0).GetComponent<Text>().text
                        = "SPD : +" + newCostume.GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) + "%";
                else
                    mCostumeChangePannel.transform.GetChild(4).GetChild(4).GetChild(0).GetComponent<Text>().text = "";

                mCostumeChangePannel.SetActive(true);
            }
            // ����� ���� �ʴ� �ڽ�Ƭ
            else
            {
                LobbyUIManager.Instance.OpenAlertEnterPannel("���� ������ ����δ� ���� �Ұ����� �ڽ�Ƭ�Դϴ�.");
            }
        }
        // �رݵǾ����� ����
        else
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("�ش� �ڽ�Ƭ�� �رݵǾ� ���� �ʽ��ϴ�.");
        }
    }
    public void CloseCostumeChangePannel()
    {
        mCostumeChangePannel.SetActive(false);
    }
    public void ChangeCostume()
    {
        // ���ο� �ڽ�Ƭ ���� �� ���� �ڽ�Ƭ�� ���� �������ش�.
        string currentCostume = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName;
        string currentShape = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeShapeName;
        if (currentCostume != "")
            mCostumeButtonList[currentCostume].GetComponent<EquipButton>().EquipCheckImage.SetActive(false);
        EquipmentManager.Instance.ChangeCostumeLobbyPlayer(mClickedCostumeType);
        mCostumeButtonList[mClickedCostumeType].GetComponent<EquipButton>().EquipCheckImage.SetActive(true);

        if(currentShape != "")
            mCostumeButtonList[currentShape].GetComponent<EquipButton>().ShapeCheckImage.SetActive(false);
        EquipmentManager.Instance.ChangeCostumeShapeLobbyPlayer(mClickedCostumeType);
        mCostumeButtonList[mClickedCostumeType].GetComponent<EquipButton>().ShapeCheckImage.SetActive(true);

        CloseCostumeChangePannel();
    }
    // �ڽ�Ƭ ���� ����
    public void CloseCostumeShpaeChangePannel()
    {
        mCostumeShapeChangePannel.SetActive(false);
    }
    public void ClickChangeShapeCostume()
    {
        CloseCostumeInfoPannel();
        if (EquipmentManager.Instance.CheckUnlockCostume(mClickedCostumeType))
        {
            string currentCostume = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName;
            if (currentCostume != "")
            {
                GameObject curCostume = EquipmentManager.Instance.FindCostume(currentCostume);
                mCostumeShapeChangePannel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text
                    = curCostume.GetComponent<Costume>().Spec.EquipName;
                mCostumeShapeChangePannel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                    = curCostume.GetComponent<SpriteRenderer>().sprite;
            }

            GameObject newCostume = EquipmentManager.Instance.FindCostume(mClickedCostumeType);

            mCostumeShapeChangePannel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text
                     = newCostume.GetComponent<Costume>().Spec.EquipName;
            mCostumeShapeChangePannel.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Image>().sprite
                = newCostume.GetComponent<SpriteRenderer>().sprite;

            mCostumeShapeChangePannel.SetActive(true);
        }
        // �ر� �Ǿ����� ����
        else
        {
            LobbyUIManager.Instance.OpenAlertEnterPannel("�ش� �ڽ�Ƭ�� �رݵǾ� ���� �ʽ��ϴ�.");
        }
    }
    public void ChangeShapeCostume()
    {
        // ���ο� �ڽ�Ƭ ���� �� ���� �ڽ�Ƭ�� ���� �������ش�.
        string currentShape = GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeShapeName;

        if (currentShape != "")
            mCostumeButtonList[currentShape].GetComponent<EquipButton>().ShapeCheckImage.SetActive(false);
        EquipmentManager.Instance.ChangeCostumeShapeLobbyPlayer(mClickedCostumeType);
        mCostumeButtonList[mClickedCostumeType].GetComponent<EquipButton>().ShapeCheckImage.SetActive(true);

        CloseCostumeShpaeChangePannel();
    }
    // �ڽ�Ƭ �ر� ���� ���� ��
    public void ChangeCostumeUnlock(string _name, bool _locked)
    {
        GameObject btn = mCostumeButtonList[_name];
        btn.GetComponent<EquipButton>().UnlockImage.SetActive(_locked);
    }
}
