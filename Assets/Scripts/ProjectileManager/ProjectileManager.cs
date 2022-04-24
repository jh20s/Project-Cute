using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : SingleToneMaker<ProjectileManager>
{
    #region variable
    // key : s, c, l, r(Ÿ�� ù����) value :  <key : 0~(Ÿ�� ��������), value = �߻�ü ������Ʈ>
    public StringGameObject allProjectiles;

    #endregion
    private void Start()
    {
        initAllProjectiles();
    }
    private void Update()
    {
        
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
            item = allProjectiles[projectilesData[i]["ProjectileType"].ToString()].GetComponent<Projectile>();
            item.Spec.Type = projectilesData[i]["ProjectileType"].ToString();
            item.Spec.Speed = float.Parse(projectilesData[i]["ProjectileSpeed"].ToString());
            item.Spec.ProjectileDamage = float.Parse(projectilesData[i]["ProjectileDamage"].ToString());
            item.Spec.Count = int.Parse(projectilesData[i]["ProjectileCount"].ToString());
            item.Spec.Angle = int.Parse(projectilesData[i]["ProjectileAngle"].ToString());
            item.Spec.SpawnTime = float.Parse(projectilesData[i]["ProjectileSpawnTime"].ToString());
            item.Spec.MaxPassCount = int.Parse(projectilesData[i]["ProjectileMaxPassCount"].ToString());
            item.Spec.StiffTime = float.Parse(projectilesData[i]["ProjectileConferStiff"].ToString());
            item.Spec.Knockback = float.Parse(projectilesData[i]["ProjectileConferKnockback"].ToString());
            // �������� ��� ���� Ÿ�� ����
            if (item.Spec.Type[0] == 's')
            {
                item.GetComponent<Spawn>().MSpawnType = projectilesData[i]["ProjectileSpawnType"].ToString();
            }
        }
    }
}

