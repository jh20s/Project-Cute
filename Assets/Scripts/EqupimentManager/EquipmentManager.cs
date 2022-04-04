using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentManager : MonoBehaviour
{
    // Start is called before the first frame update
    #region variable
    // key : ��� �з�(0: ����, 1: �ڽ�Ƭ...) , value : <key : typename, value : �ش���� ������Ʈ 
    public IntStringGameObject equipments;
    public IntGameObject userCurrentEquip;
    // key : ���� �з� , value : �ش� ���� ��� ������Ʈ
    public IntGameObject monsterCurrentEquip;
    #endregion
    void Start()
    {
        //initAllEquips();
        //loadUserEquip();
#if DEBUG
            //for(int i = 0; i < equipments.Count; i++)
            //{
            //    foreach(GameObject item in equipments[i].Values)
            //    {
            //        Instantiate(item, transform);
            //    }
            //}
#endif
    }
    #region method
    // ��� ���� ������Ʈ ������ �Ľ� �� �� �����ϴ� �Լ�
    public void initAllEquips()
    {

        // Weapon
        // csv������ �о ����Ʈ�������� ����
        List<Dictionary<string, object>> weaponData = CSVReader.Read("CSVFile\\Weapon");
        for (int i = 0; i < weaponData.Count; i++)
        {
            Dictionary<string, GameObject> weapons = equipments[0];
            // ���پ� �ҷ��ͼ� ������Ʈ ����
            foreach (GameObject item in weapons.Values)
            {
                Weapon newWeapon = item.GetComponent<Weapon>();
                // �⺻ ���� ���� ���� �� �߰�
                {
                    newWeapon.Spec.Type = weaponData[i]["WeaponType"].ToString();
                    newWeapon.Spec.TypeName = weaponData[i]["WeaponTypeName"].ToString();
                    newWeapon.Spec.EquipName = weaponData[i]["WeaponName"].ToString();
                    newWeapon.Spec.EquipDesc = weaponData[i]["WeaponDesc"].ToString();
                    newWeapon.Spec.EquipRank = int.Parse(weaponData[i]["WeaponRank"].ToString());
                    newWeapon.Spec.WeaponDamage = float.Parse(weaponData[i]["WeaponDamage"].ToString());
                    newWeapon.Spec.WeaponAttackSpeed = float.Parse(weaponData[i]["WeaponAttackSpeed"].ToString());
                    newWeapon.Spec.WeaponAttackRange = int.Parse(weaponData[i]["WeaponAttackRange"].ToString());
                    newWeapon.Spec.AttackProjectile = weaponData[i]["AttackProjectile"].ToString();
                }
                // �Ϲ� ��ų ���� ����
                {
                    string[] skillName = weaponData[i]["GeneralSkillName"].ToString().Split('/');
                    string[] skillDesc = weaponData[i]["GeneralSkillDesc"].ToString().Split('/');
                    string[] projectiles = weaponData[i]["GeneralSkillProjectile"].ToString().Split('/');
                    string[] coolTimes = weaponData[i]["GeneralSkillCoolTime"].ToString().Split('/');
                    string[] coefficients = weaponData[i]["GeneralSkillCoefficient"].ToString().Split('/');
                    // �Ϲ� ��ų ���� �߰�
                    for (int j = 0; j < projectiles.Length; j++)
                    {
                        newWeapon.Spec.addSkillList(
                            skillName[i],
                            skillDesc[i],
                            projectiles[i],
                            int.Parse(coolTimes[i]),
                            int.Parse(coefficients[i]),
                            j == 0 ? true : false,
                            Skill.SkillType.GENERAL);
                    }
                }
                // �ñر� ��ų ���� ����
                {
                    string[] skillName = weaponData[i]["UltimateSkillName"].ToString().Split('/');
                    string[] skillDesc = weaponData[i]["UltimateSkillDesc"].ToString().Split('/');
                    string[] projectiles = weaponData[i]["UltimateSkillProjectile"].ToString().Split('/');
                    string[] coolTimes = weaponData[i]["UltimateSkillCoolTime"].ToString().Split('/');
                    string[] coefficients = weaponData[i]["UltimateSkillCoefficient"].ToString().Split('/');

                    // �ñر� ��ų ���� �߰�
                    for (int j = 0; j < projectiles.Length; j++)
                    {
                        newWeapon.Spec.addSkillList(
                            skillName[i],
                            skillDesc[i],
                            projectiles[i],
                            int.Parse(coolTimes[i]),
                            int.Parse(coefficients[i]),
                            j == 0 ? true : false,
                            Skill.SkillType.ULTIMATE);
                    }
                }
                // �� ���⺰ �⺻ ��ų ����
                {
                    newWeapon.changeSkill(
                        newWeapon.Spec.getGeneralSkill()[0].SkillName, Skill.SkillType.GENERAL);
                    newWeapon.changeSkill(
                        newWeapon.Spec.getUltimateSkill()[0].SkillName, Skill.SkillType.ULTIMATE);
                }
            }
        }
        // Costume
        List<Dictionary<string, object>> costumeData = CSVReader.Read("CSVFile\\Costume");
        for (int i = 0; i < costumeData.Count; i++)
        {
            Dictionary<string, GameObject> costumes = equipments[1];
            foreach (GameObject item in costumes.Values)
            {
                Costume newCostume = item.GetComponent<Costume>();
                // �ڽ�Ƭ ���� ���� �� �߰�
                {
                    newCostume.Spec.TypeName = costumeData[i]["CostumeTypeName "].ToString();
                    newCostume.Spec.EquipName = costumeData[i]["CostumeName "].ToString();
                    newCostume.Spec.EquipDesc = costumeData[i]["CostumeDesc "].ToString();
                    newCostume.Spec.EquipRank = int.Parse(costumeData[i]["CostumeRank "].ToString());
                    newCostume.IsLocked = false;
                }
            }
        }
    }
    // �������� ���� ��ü�� �����Ͽ� �������� �Ϲ� ��ų ����ϴ� �Լ�
    public void generalSkillClicked()
    {

    }
    // �������� ���� ��ü�� �����Ͽ� �������� �ñر� ��ų ����ϴ� �Լ�
    public void ultimateSkillClicked()
    {

    }
    // �������� ��� ��ü���ִ� �Լ�
    // name : �ٲ� ����� typeName
    // type : 0 ����, 1 �ڽ�Ƭ
    public void changeEquip(string name, int type)
    {

    }
    private void loadUserEquip()
    {
        // ������ ������ �ε� �� ����
    }
    #endregion
}