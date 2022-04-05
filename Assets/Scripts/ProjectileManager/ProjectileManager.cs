using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    #region variable
    // key : s, c, l, r(Ÿ�� ù����) value :  <key : 0~(Ÿ�� ��������), value = �߻�ü ������Ʈ>
    public StringIntGameObject allProjectiles;

    #endregion
    private void Start()
    {
        initAllProjectiles();
    }
    private void Update()
    {
#if DEBUG
#endif
    }
    public void initAllProjectiles()
    {
        // ��� �߻�ü ������Ʈ �ʱ�ȭ
        List<Dictionary<string, object>> projectilesData = CSVReader.Read("CSVFile\\Projectile");
        int i = 0;
        Projectile item;
        foreach (string key0 in allProjectiles.Keys)
        {
            for (int j = 0; j < allProjectiles[key0].Count; j++)
            {
                item = allProjectiles[key0][j].GetComponent<Projectile>();
                dataParser(item, ref projectilesData, i + j);
            }
            i += allProjectiles[key0].Count;
        }
    }
    private void dataParser(Projectile item, ref List<Dictionary<string, object>> projectilesData, int index)
    {
        item.Spec.Type = projectilesData[index]["ProjectileType"].ToString();
        item.Spec.TypeName = projectilesData[index]["ProjectileTypeName"].ToString();
        item.Spec.EquipName = projectilesData[index]["ProjectileName"].ToString();
        item.Spec.EquipDesc = projectilesData[index]["ProjectileDesc"].ToString();
        // item.Spec.EquipRank = int.Parse(projectilesData[index]["ProjectileRank"].ToString());
        item.Spec.Speed = float.Parse(projectilesData[index]["ProjectileSpeed"].ToString());
        item.Spec.Count = int.Parse(projectilesData[index]["ProjectileCount"].ToString());
        item.Spec.Angle = int.Parse(projectilesData[index]["ProjectileAngle"].ToString());
        item.Spec.SpawnTime = float.Parse(projectilesData[index]["ProjectileSpawnTime"].ToString());
        item.Spec.MaxPassCount = int.Parse(projectilesData[index]["ProjectileMaxPassCount"].ToString());
    }

    /*
     * �������� ���� �߻�ü �߰��� �ʿ��� api�Դϴ�.
     * ��ü������ ��� �߻�ü�� �߰���ų�� �ʿ�
     * _num : �߰���ų �߻�ü �����Դϴ�. 
     */
    public void AddProjectilesCount(int _num)
    {
        if (_num < 0)
            if (Projectile.AddProjectilesCount <= 0)
                _num = 0;
        Projectile.AddProjectilesCount += _num;
    }

    /*
 * �������� ���� �߻�ü ���� ������ �ʿ��� api�Դϴ�.
 * ��ü������ ��� �߻�ü�� ������ ���� ��ų�� �ʿ�
 * _coefficient : ���� ���� ��ġ �Դϴ�.
 */
    public void AddProjectilesScale(float _coefficient)
    {
        if (_coefficient < 0)
            if (Projectile.AddScaleCoefficient <= 1)
                _coefficient = 0;
        Projectile.AddScaleCoefficient += _coefficient;
    }


    // �ӽ� �׽�Ʈ�� �Դϴ�. ���� ����
    public void createUserDamageBox()
    {
        int damage = UnityEngine.Random.Range(10, 100);
        MessageBoxManager.Instance.createMessageBox(
            MessageBoxManager.BoxType.UserDamage, damage.ToString(), new Vector3(0, 0, 0));
    }
    // �ӽ� �׽�Ʈ�� �Դϴ�. ���� ����
    public void createMonsterDamageBox()
    {
        int damage = UnityEngine.Random.Range(10, 100);
        MessageBoxManager.Instance.createMessageBox(
            MessageBoxManager.BoxType.MonsterDamage, damage.ToString(), new Vector3(0, 0, 0));
    }
}

