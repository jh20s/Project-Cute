using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class EquipmentManager : MonoBehaviour
    {
        // Start is called before the first frame update
        #region variable
        // key : ��� �з�(0: ����, 1: �ڽ�Ƭ...) , value : <key : typename, value : �ش���� ������Ʈ 
        public PBL.IntStringGameObject equipments;
        public PBL.IntGameObject userCurrentEquip;
        // key : ���� �з� , value : �ش� ���� ��� ������Ʈ
        public PBL.IntGameObject monsterCurrentEquip;
        #endregion
        void Start()
        {
            initAllEquips();
        }
        #region method
        // ��� ���� ������Ʈ ������ �Ľ� �� �� �����ϴ� �Լ�
        public void initAllEquips()
        {
            // Weapon
            List<Dictionary<string, object>> weaponData = PBL.CSVReader.Read("CSVFile\\Weapon");
            for(int i = 0; i < weaponData.Count; i++)
            {
                Dictionary<string, GameObject> weapons = equipments[0];
                foreach(GameObject item in weapons.Values)
                {
                    Weapon newWeapon = item.GetComponent<Weapon>();
                    newWeapon.spec.type = weaponData[i]["WeaponType"].ToString();
                    newWeapon.spec.typeName = weaponData[i]["WeaponTypeName"].ToString();
                    newWeapon.spec.equipName = weaponData[i]["WeaponName"].ToString();
                    newWeapon.spec.equipDesc = weaponData[i]["WeaponDesc"].ToString();
                    newWeapon.spec.equipRank = int.Parse(weaponData[i]["WeaponRank"].ToString());
                    newWeapon.spec.WeaponDamage = float.Parse(weaponData[i]["WeaponDamage"].ToString());
                    newWeapon.spec.WeaponAttackSpeed = float.Parse(weaponData[i]["WeaponAttackSpeed"].ToString());
                    newWeapon.spec.WeaponAttackRange = int.Parse(weaponData[i]["WeaponAttackRange"].ToString());
                    newWeapon.spec.AttackProjectile = weaponData[i]["AttackProjectile"].ToString();

                    string[] projectiles = weaponData[i]["GeneralSkillProjectile"].ToString().Split('/');
                    string[] coolTimes = weaponData[i]["GeneralSkillCoolTime"].ToString().Split('/');
                    string[] coefficients = weaponData[i]["GeneralSkillCoefficient"].ToString().Split('/');

                    for(int j = 0; j < projectiles.Length; j++)
                    {
                        newWeapon.spec.addGeneralSkill(
                            projectiles[i], 
                            int.Parse(coolTimes[i]), 
                            int.Parse(coefficients[i]), 
                            j == 0 ? true : false);
                    }

                    projectiles = weaponData[i]["UltimateSkillProjectile"].ToString().Split('/');
                    coolTimes = weaponData[i]["UltimateSkillCoolTime"].ToString().Split('/');
                    coefficients = weaponData[i]["UltimateSkillCoefficient"].ToString().Split('/');

                    for (int j = 0; j < projectiles.Length; j++)
                    {
                        newWeapon.spec.addUltimateSkill(
                            projectiles[i],
                            int.Parse(coolTimes[i]),
                            int.Parse(coefficients[i]),
                            j == 0 ? true : false);
                    }
                }
            }
            // Costume
            List<Dictionary<string, object>> costumeData = PBL.CSVReader.Read("CSVFile\\Costume");
            for (int i = 0; i < costumeData.Count; i++)
            {
                Dictionary<string, GameObject> costumes = equipments[1];
                foreach(GameObject item in costumes.Values)
                {
                    Costume newCostume = item.GetComponent<Costume>();
                    newCostume.spec.typeName = costumeData[i]["CostumeTypeName "].ToString();
                    newCostume.spec.equipName = costumeData[i]["CostumeName "].ToString();
                    newCostume.spec.equipDesc = costumeData[i]["CostumeDesc "].ToString();
                    newCostume.spec.equipRank = int.Parse(costumeData[i]["CostumeRank "].ToString());
                    newCostume.IsLocked = false;
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
        #endregion
    }
}