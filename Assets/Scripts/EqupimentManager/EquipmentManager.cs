using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    
    // key : ���� �з� , value : �ش� ���� ��� ������Ʈ
    public StringGameObject monsterCurrentEquip;

    public struct CostumeSprite
    {
        public CostumeSprite(Sprite _sprite, float _r, float _g, float _b)
        {
            sprite = _sprite;
            r = _r;
            g = _g;
            b = _b;
        }
        public Sprite sprite;
        public float r;
        public float g;
        public float b;
    }

    public enum SpriteType
    {
        CostumeHelmet,
        CostumeCloth,
        CostumeArmor,
        CostumePant,
        CostumeBack,
        Exit,
    }

    private const string NULL = "null";
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
                    item.Spec.Name = weaponData[i]["WeaponName"].ToString();
                    item.Spec.Desc = weaponData[i]["WeaponDesc"].ToString();
                    //item.Spec.EquipRank = int.Parse(weaponData[i]["WeaponRank"].ToString());
                    item.Spec.WeaponDamage = int.Parse(weaponData[i]["WeaponATK"].ToString());
                    //item.Spec.WeaponAttackSpeed = float.Parse(weaponData[i]["WeaponAttackSpeed"].ToString());
                    //item.Spec.WeaponAttackRange = int.Parse(weaponData[i]["WeaponAttackRange"].ToString());
                    item.Spec.WeaponAddSpeed = float.Parse(weaponData[i]["WeaponSPD"].ToString());

                    item.IsLocked = true;
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
                    item.Spec.Name = costumeData[i]["CostumeName"].ToString();
                    item.Spec.Desc = costumeData[i]["CostumeDesc"].ToString();
                    //item.Spec.Rank = int.Parse(costumeData[i]["CostumeRank "].ToString());
                    item.Spec.CoustumeHP = int.Parse(costumeData[i]["CostumeHP"].ToString());
                    item.Spec.CoustumeSpeed = int.Parse(costumeData[i]["CostumeSPD"].ToString());
                    item.IsLocked = false;
                }
            }
            for (int i = 0; i < costumesList.Length; i++)
            {
                // �ڽ�Ƭ ��������Ʈ ������ ����
                {
                    item = costumes[costumeLoadData[i]["CostumeLoadName"].ToString()].GetComponent<Costume>();
                    string path = costumeLoadData[i]["CostumeLoadPath"].ToString();
                    Sprite newSprite = null;
                    string[] rgb = new string[3];
                    // �� ��������Ʈ�� rgb������ ����
                    for(SpriteType j = SpriteType.CostumeHelmet; j < SpriteType.Exit; j++)
                    {
                        if (costumeLoadData[i][j.ToString()].ToString() != NULL)
                            newSprite = Resources.Load<Sprite>(path + costumeLoadData[i][j.ToString()].ToString());
                        if (costumeLoadData[i][j.ToString() +"RGB"].ToString() != NULL)
                            rgb = costumeLoadData[i][j.ToString() + "RGB"].ToString().Split('/');
                        if(newSprite != null)
                        {
                            CostumeSprite CSprite = new CostumeSprite(
                                newSprite, float.Parse(rgb[0]), float.Parse(rgb[1]), float.Parse(rgb[2]));
                            item.AddSpriteList(j, CSprite);
                        }
                    }
                }
            }
            
        }
    }
    // �������� ��� ��ü���ִ� �Լ�
    // name : �ٲ� ����� typeName
    // type : 0 ����, 1 �ڽ�Ƭ
    public void changeEquip(string name)
    {
        // Todo : playerCurrentWeapon�� ���ӿ�����Ʈ �ش� ���ӿ�����Ʈ�� ����
        Weapon cWeapon = weapons[name].GetComponent<Weapon>();
        // �ر��� �Ǿ��ִٸ�
        if (cWeapon.IsLocked)
        {
            GameObject.Find("PlayerObject").GetComponent<PlayerStatus>().PlayerCurrentWeapon = cWeapon;
            SpriteRenderer playerSprite = PlayerManager.Instance.getPlayerWeaponSprite();
            playerSprite.sprite = cWeapon.GetComponent<SpriteRenderer>().sprite;
        }
        // �ر��� �ȵǾ��ִٸ�
        else
        {
            // ����� ���
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
    private void loadUserEquip()
    {
        // ������ ������ �ε� �� ����
    }
    #endregion
}