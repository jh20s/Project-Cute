using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentManager : SingleToneMaker<EquipmentManager>
{
    // Start is called before the first frame update
    #region variable
    
    // key : ��� �з�(0: ����, 1: �ڽ�Ƭ...) , value : <key : typename, value : �ش���� ������Ʈ 
    [SerializeField]
    private StringGameObject weapons;
    [SerializeField]
    private StringGameObject costumes;
    [SerializeField]
    private Weapon playerCurrentWeapon;
    [SerializeField]
    private Costume playerCurrentCostume;
    // key : ���� �з� , value : �ش� ���� ��� ������Ʈ
    public StringGameObject monsterCurrentEquip;
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
                }
            }
        }

        // Costume
        //{
        //    
        //    // Costumes �������� �ҷ��´�
        //    GameObject[] costumesList = Resources.LoadAll<GameObject>("Prefabs\\Costumes");
        //    // Dic�� �����صд�.
        //    foreach (GameObject costume in costumesList)
        //    {
        //        costumes.Add(costume.name, costume);
        //    }
        //    List<Dictionary<string, object>> costumeData = CSVReader.Read("CSVFile\\Costume");
        //    Costume item;
        //for (int i = 0; i < costumesList.Length; i++)
        //    {
        //        // �ڽ�Ƭ ���� ���� �� �߰�
        //        {
        //            item = costumes[costumeData[i]["CostumeType"].ToString()].GetComponent<Costume>();
        //            item.Spec.TypeName = costumeData[i]["CostumeTypeName "].ToString();
        //            item.Spec.EquipName = costumeData[i]["CostumeName "].ToString();
        //            item.Spec.EquipDesc = costumeData[i]["CostumeDesc "].ToString();
        //            item.Spec.EquipRank = int.Parse(costumeData[i]["CostumeRank "].ToString());
        //            item.IsLocked = false;
        //        }
        //    }
        //}
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
            playerCurrentWeapon = cWeapon;
            SpriteRenderer playerSprite = PlayerManager.Instance.getPlayerWeaponSprite();
            playerSprite.sprite = cWeapon.GetComponent<SpriteRenderer>().sprite;
        }
        // �ر��� �ȵǾ��ִٸ�
        else
        {
            // ����� ���
        }
    }
    private void loadUserEquip()
    {
        // ������ ������ �ε� �� ����
    }
    public int getCurrentDamage()
    {
        return playerCurrentWeapon.Spec.WeaponDamage;
    }
    #endregion
}