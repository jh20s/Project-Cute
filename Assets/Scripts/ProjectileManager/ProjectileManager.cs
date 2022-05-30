using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : SingleToneMaker<ProjectileManager>
{
    private bool DEBUG = false;

    #region variable
    // key : s, c, l, r(Ÿ�� ù����) value :  <key : 0~(Ÿ�� ��������), value = �߻�ü ������Ʈ>
    public StringGameObject allProjectiles;

    public enum DamageType
    {
        Normal,
        SplitByNumber,
        SplitByTime
    }
    #endregion
    private void Start()
    {
        initAllProjectiles();
    }

    public void initAllProjectiles()
    {
        // Projectiles �������� �ҷ��´�
        GameObject[] projectilesList = Resources.LoadAll<GameObject>("Prefabs\\Projectiles");
        // Dic�� �����صд�.
        foreach (GameObject projectile in projectilesList)
        {
            allProjectiles.Add(projectile.name, projectile);
        }
        // ��� �߻�ü ������Ʈ �ʱ�ȭ
        List<Dictionary<string, object>> projectilesData = CSVReader.Read("CSVFile\\Projectile");
        Projectile item;
        for(int i = 0; i < projectilesList.Length; i++)
        {
            if (DEBUG)
                Debug.Log("projectile �� : "+projectilesData[i]["ProjectileType"]);
            item = allProjectiles[projectilesData[i]["ProjectileType"].ToString()].GetComponent<Projectile>();
            item.Spec.Type = projectilesData[i]["ProjectileType"].ToString();
            item.Spec.ProjectileDamage = float.Parse(projectilesData[i]["ProjectileDamage"].ToString());
            item.Spec.ProjectileDamageType = (DamageType)Enum.Parse(
                typeof(DamageType), projectilesData[i]["ProjectileDamageType"].ToString());
            item.Spec.ProjectileDamageSplit = int.Parse(projectilesData[i]["ProjectileDamageSplit"].ToString());
            item.Spec.ProjectileDamageSplitSec = float.Parse(projectilesData[i]["ProjectileDamageSplitSec"].ToString());
            item.Spec.ProjectileAttackSpeed = float.Parse(projectilesData[i]["ProjectileAttackSpeed"].ToString());
            item.Spec.MoveSpeed = float.Parse(projectilesData[i]["ProjectileSpeed"].ToString());
            item.Spec.Count = int.Parse(projectilesData[i]["ProjectileCount"].ToString());
            item.Spec.Angle = int.Parse(projectilesData[i]["ProjectileAngle"].ToString());
            item.Spec.SpawnTime = float.Parse(projectilesData[i]["ProjectileSpawnTime"].ToString());
            item.Spec.MaxPassCount = int.Parse(projectilesData[i]["ProjectileMaxPassCount"].ToString());
            item.Spec.StiffTime = float.Parse(projectilesData[i]["ProjectileConferStiff"].ToString());
            item.Spec.Knockback = float.Parse(projectilesData[i]["ProjectileConferKnockback"].ToString());
            item.Spec.ProjectileSizeX = float.Parse(projectilesData[i]["ProjectileSizeX"].ToString());
            item.Spec.ProjectileSizeY = float.Parse(projectilesData[i]["ProjectileSizeY"].ToString());
            item.Spec.ProjectileDelayTime = float.Parse(projectilesData[i]["ProjectileDelayTime"].ToString());
            // �������� ��� ���� Ÿ�� ����
            if (item.Spec.Type[0] == 's')
            {
                item.GetComponent<Spawn>().MSpawnType = projectilesData[i]["ProjectileSpawnType"].ToString();
            }
        }
    }
}

