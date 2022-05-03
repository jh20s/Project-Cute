using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class EquipmentManager : SingleToneMaker<EquipmentManager>
{
    // Start is called before the first frame update
    #region variable
    
    // key : ��� �з�(0: ����, 1: �ڽ�Ƭ...) , value : <key : typename, value : �ش���� ������Ʈ 
    [SerializeField]
    private StringGameObject weapons;
    [SerializeField]
    private StringGameObject costumes;
    public StringGameObject Costumes
    {
        get { return costumes; }
    }
    public enum CostumeTpye
    {
        swsp,
        bgst,
        bgstswsp,
        Exit
    }
    public enum WeaponType
    {
        sw,
        sp,
        st,
        bg,
        Exit
    }

    [Serializable]
    public struct CostumeSprite
    {
        public CostumeSprite(Sprite[] _sprite, float _r, float _g, float _b)
        {
            sprite = _sprite;
            r = _r;
            g = _g;
            b = _b;
        }
        public Sprite[] sprite;
        public float r;
        public float g;
        public float b;
    }

    [Serializable]
    public enum SpriteType
    {
        CostumeHelmet = 0,
        CostumeCloth = 1,
        CostumeArmor = 4,
        CostumePant = 7,
        CostumeBack = 9,
        Exit,
    }

    private const string NULL = "null";

    [SerializeField]
    private SpriteRenderer mLobbyPlayerWeaponSprite;

    [SerializeField]
    private List<SpriteRenderer> mLobbyPlayerCostumeSprites;
    #endregion
    void Start()
    {
        initAllEquips();
        //loadUserEquip();
    }
    #region method
    // ��� ���� ������Ʈ ������ �Ľ� �� �� �����ϴ� �Լ�
    public void initAllEquips()
    {
        // Weapon
        {
            // Weapons �������� �ҷ��´�
            GameObject[] weaponsList = Resources.LoadAll<GameObject>("Prefabs\\Weapons");
            // Dic�� �����صд�.
            foreach (GameObject weapon in weaponsList)
            {
                if (weapons.ContainsKey(weapon.name))
                    weapons.Remove(weapon.name);
                weapons.Add(weapon.name, weapon);
            }
            // csv������ �о ����Ʈ�������� ����
            List<Dictionary<string, object>> weaponData = CSVReader.Read("CSVFile\\Weapon");
            Weapon item;
            for (int i = 0; i < weaponsList.Length; i++)
            {
                // �⺻ ���� ���� ���� �� �߰�
                {
                    item = weapons[weaponData[i]["WeaponType"].ToString()].GetComponent<Weapon>();
                    item.Spec.Type = weaponData[i]["WeaponType"].ToString();
                    item.Spec.TypeName = weaponData[i]["WeaponTypeName"].ToString();
                    item.Spec.EquipName = weaponData[i]["WeaponName"].ToString();
                    item.Spec.EquipDesc = weaponData[i]["WeaponDesc"].ToString();
                    //item.Spec.EquipRank = int.Parse(weaponData[i]["WeaponRank"].ToString());
                    item.Spec.WeaponDamage = int.Parse(weaponData[i]["WeaponATK"].ToString());
                    //item.Spec.WeaponAttackSpeed = float.Parse(weaponData[i]["WeaponAttackSpeed"].ToString());
                    //item.Spec.WeaponAttackRange = int.Parse(weaponData[i]["WeaponAttackRange"].ToString());
                    item.Spec.WeaponAddSpeed = float.Parse(weaponData[i]["WeaponSPD"].ToString());

                    // ���Ŀ� �ʱ� ������ �ʿ�
                    // �⺻ ���� ����鸸 �ر�
                    //if (item.Spec.Type.Substring(2) == "00")
                    //    item.IsLocked = false;
                    //else
                    //    item.IsLocked = true;

                    item.IsLocked = false;
                }
            }
        }

        // Costume
        {

            // Costumes �������� �ҷ��´�
            GameObject[] costumesList = Resources.LoadAll<GameObject>("Prefabs\\Costumes");
            // Dic�� �����صд�.
            foreach (GameObject costume in costumesList)
            {
                costumes.Add(costume.name, costume);
            }
            List<Dictionary<string, object>> costumeData = CSVReader.Read("CSVFile\\Costume");
            List<Dictionary<string, object>> costumeLoadData = CSVReader.Read("CSVFile\\CostumeLoad");
            Costume item;
            for (int i = 0; i < costumesList.Length; i++)
            {
                // �ڽ�Ƭ ���� ���� �� �߰�
                {
                    item = costumes[costumeData[i]["CostumeLoad"].ToString()].GetComponent<Costume>();
                    item.Spec.TypeName = costumeData[i]["CostumeTypeName"].ToString();
                    item.Spec.EquipName = costumeData[i]["CostumeName"].ToString();
                    item.Spec.EquipDesc = costumeData[i]["CostumeDesc"].ToString();
                    item.Spec.Rank = int.Parse(costumeData[i]["EquipmentRank"].ToString());
                    item.Spec.EquipRankDesc = costumeData[i]["EquipmentRankDesc"].ToString();
                    item.Spec.EquipSource = costumeData[i]["EquipmentSource"].ToString();
                    string[] tmp = costumeData[i]["CostumeStatBuff"].ToString().Split('/');
                    for(int idx = 0; idx < tmp.Length; idx++)
                    {
                        // idx�� 0�̳� ¦���ΰ�� key��
                        if(idx == 0 || idx % 2 == 0)
                        {
                            item.SetBuffDic((Costume.CostumeBuffType)Enum.Parse(typeof(Costume.CostumeBuffType),tmp[idx]), int.Parse(tmp[++idx]));
                        }
                    }
                    // �⺻ ���� �ڽ�Ƭ�� �ر�
                    //if (item.gameObject.name == "cstbgstswsp01")
                    //    item.IsLocked = false;
                    //else
                    //    item.IsLocked = true;
                    item.IsLocked = false;
                }
            }
            for (int i = 0; i < costumesList.Length; i++)
            {
                // �ڽ�Ƭ ��������Ʈ ������ ����
                {
                    item = costumes[costumeLoadData[i]["CostumeLoadName"].ToString()].GetComponent<Costume>();
                    string path = costumeLoadData[i]["CostumeLoadPath"].ToString();
                    Sprite[] newSprite = null;
                    string[] rgb = new string[3];
                    // �� ��������Ʈ�� rgb������ ����
                    for(SpriteType j = SpriteType.CostumeHelmet; j < SpriteType.Exit;)
                    {
                        if (costumeLoadData[i][j.ToString()].ToString() != NULL)
                            newSprite = Resources.LoadAll<Sprite>(path + costumeLoadData[i][j.ToString()].ToString());
                        if (costumeLoadData[i][j.ToString() +"RGB"].ToString() != NULL)
                            rgb = costumeLoadData[i][j.ToString() + "RGB"].ToString().Split('/');
                        if(newSprite != null)
                        {
                            CostumeSprite CSprite = new CostumeSprite(
                                newSprite, float.Parse(rgb[0]), float.Parse(rgb[1]), float.Parse(rgb[2]));
                            item.AddSpriteList(j, CSprite);
                        }
                        switch ((int)j)
                        {
                            case 0:
                                j++;
                                break;
                            case 1:
                                j += 3;
                                break;
                            case 4:
                                j += 3;
                                break;
                            case 7:
                                j += 2;
                                break;
                            case 9:
                                j++;
                                break;
                        }
                        newSprite = null;
                        
                    }
                }
            }
            
        }
    }
    // �������� ��� ��ü���ִ� �Լ�
    // name : �ٲ� ����� typeName
    // type : 0 ����, 1 �ڽ�Ƭ
    public void ChangeWeapon(string _name)
    {
        // Todo : playerCurrentWeapon�� ���ӿ�����Ʈ �ش� ���ӿ�����Ʈ�� ����
        Weapon cWeapon = weapons[_name].GetComponent<Weapon>();
        // �ر��� �Ǿ��ִٸ�
        if (!cWeapon.IsLocked)
        {
            GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().PlayerCurrentWeapon = cWeapon;
            SpriteRenderer playerSprite = PlayerManager.Instance.getPlayerWeaponSprite();
            playerSprite.sprite = cWeapon.GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void ChangeWeaponLobbyPlayer(string _name)
    {
        // Todo : playerCurrentWeapon�� ���ӿ�����Ʈ �ش� ���ӿ�����Ʈ�� ����
        Weapon cWeapon = weapons[_name].GetComponent<Weapon>();
        // �ر��� �Ǿ��ִٸ�
        if (!cWeapon.IsLocked)
        {
            if (GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName != "")
            {
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.BaseATK -=
                    weapons[GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName].GetComponent<Weapon>().Spec.WeaponDamage;
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.MoveSpeedRate -=
                weapons[GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName].GetComponent<Weapon>().Spec.WeaponAddSpeed;
            }
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentWeaponName = _name;
            mLobbyPlayerWeaponSprite.sprite = cWeapon.GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.BaseATK += cWeapon.Spec.WeaponDamage;
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.MoveSpeedRate += cWeapon.Spec.WeaponAddSpeed;
        }
    }
    public void ChangeCostume(string _name)
    {
        Costume cCostume = costumes[_name].GetComponent<Costume>();
        if (!cCostume.IsLocked)
        {
            for (SpriteType i = SpriteType.CostumeHelmet; i < SpriteType.Exit;)
            {
                // �ش� �ڽ�Ƭ ������ ������
                if (cCostume.GetSpriteDic().ContainsKey(i))
                {
                    CostumeSprite newCosutume = cCostume.GetSpriteList(i);
                    if (i == SpriteType.CostumeCloth || i == SpriteType.CostumeArmor)
                    {
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Body");
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else if (i == SpriteType.CostumePant)
                    {
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else
                    {
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = newCosutume.sprite[0];
                        PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i++).color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                }
                // �ش� �ڽ�Ƭ ������ ������
                else
                {
                    PlayerManager.Instance.GetPlayerCostumeSpriteRenderer((int)i).sprite = null;
                    switch ((int)i)
                    {
                        case 0:
                            i++;
                            break;
                        case 1:
                            i += 3;
                            break;
                        case 4:
                            i += 3;
                            break;
                        case 7:
                            i += 2;
                            break;
                        case 9:
                            i++;
                            break;
                    }
                }
            }
        }
    }
    public void ChangeCostumeLobbyPlayer(string _name)
    {
        Costume cCostume = costumes[_name].GetComponent<Costume>();
        if (!cCostume.IsLocked)
        {
            // ���� �����ϴ� �ڽ�Ƭ�� ������ �ش� ������ ���ش�.
            if (GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName != "")
            {
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.BaseHp -=
                    costumes[GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
                GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.MoveSpeedRate -=
                    costumes[GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) / 100f;
            }


            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeName = _name;
            for (SpriteType i = SpriteType.CostumeHelmet; i < SpriteType.Exit;)
            {
                // �ش� �ڽ�Ƭ ������ ������
                if (cCostume.GetSpriteDic().ContainsKey(i))
                {
                    CostumeSprite newCosutume = cCostume.GetSpriteList(i);
                    if (i == SpriteType.CostumeCloth || i == SpriteType.CostumeArmor)
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Body");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else if (i == SpriteType.CostumePant)
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = newCosutume.sprite[0];
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                }
                // �ش� �ڽ�Ƭ ������ ������
                else
                {
                    mLobbyPlayerCostumeSprites[(int)i].sprite = null;
                    switch ((int)i)
                    {
                        case 0:
                            i++;
                            break;
                        case 1:
                            i += 3;
                            break;
                        case 4:
                            i += 3;
                            break;
                        case 7:
                            i += 2;
                            break;
                        case 9:
                            i++;
                            break;
                    }
                }
            }
            // ���� ����
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.BaseHp +=
                    costumes[_name].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerHP);
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.MoveSpeedRate +=
                costumes[_name].GetComponent<Costume>().GetBuffValue(Costume.CostumeBuffType.PlayerSPD) / 100f;
        }
    }
    public void ChangeCostumeShapeLobbyPlayer(string _name)
    {
        Costume cCostume = costumes[_name].GetComponent<Costume>();
        if (!cCostume.IsLocked)
        {
            GameObject.Find("LobbyPlayer").GetComponent<LobbyPlayerData>().Info.CurrentCostumeShapeName = _name;
            for (SpriteType i = SpriteType.CostumeHelmet; i < SpriteType.Exit;)
            {
                // �ش� �ڽ�Ƭ ������ ������
                if (cCostume.GetSpriteDic().ContainsKey(i))
                {
                    CostumeSprite newCosutume = cCostume.GetSpriteList(i);
                    if (i == SpriteType.CostumeCloth || i == SpriteType.CostumeArmor)
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Body");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else if (i == SpriteType.CostumePant)
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Left");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        mLobbyPlayerCostumeSprites[(int)i].sprite = Array.Find(newCosutume.sprite, (Sprite s) => s.name == "Right");
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                    else
                    {
                        mLobbyPlayerCostumeSprites[(int)i].sprite = newCosutume.sprite[0];
                        mLobbyPlayerCostumeSprites[(int)i++].color =
                            new Color(newCosutume.r / 255f, newCosutume.g / 255f, newCosutume.b / 255f);
                        continue;
                    }
                }
                // �ش� �ڽ�Ƭ ������ ������
                else
                {
                    mLobbyPlayerCostumeSprites[(int)i].sprite = null;
                    switch ((int)i)
                    {
                        case 0:
                            i++;
                            break;
                        case 1:
                            i += 3;
                            break;
                        case 4:
                            i += 3;
                            break;
                        case 7:
                            i += 2;
                            break;
                        case 9:
                            i++;
                            break;
                    }
                }
            }
        }
    }

    public List<GameObject> FindWepaonList(string _type)
    {
        List<GameObject> newList = new List<GameObject>();
        foreach(string key in weapons.Keys)
        {
            string type = weapons[key].GetComponent<Weapon>().Spec.Type.Substring(0, 2);
            if (type == _type)
            {
                newList.Add(weapons[key]);
            }
        }
        return newList;
    }
    public GameObject FindWeapon(string _name)
    {
        return weapons[_name];
    }


    public bool CheckUnlockWeapon(string _type)
    {
        return !weapons[_type].GetComponent<Weapon>().IsLocked;
    }
    public bool CheckUnlockCostume(string _type)
    {
        return !costumes[_type].GetComponent<Costume>().IsLocked;
    }
    public List<GameObject> FindCostumeList(string _type)
    {
        List<GameObject> newList = new List<GameObject>();
        foreach(string key in costumes.Keys)
        {
            // cst ����
            string tmp = key.Substring(3);
            // ���� index����
            tmp = tmp.Substring(0, tmp.Length - 2);
            if (tmp == _type)
            {
                newList.Add(costumes[key]);
            }
        }
        return newList;
    }
    public GameObject FindCostume(string _name)
    {
        return costumes[_name];
    }
    #endregion
}