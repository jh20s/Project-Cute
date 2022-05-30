using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : SingleToneMaker<ProjectileManager>
{
    private bool DEBUG = false;

    #region variable
    // key : s, c, l, r(타입 첫글자) value :  <key : 0~(타입 다음글자), value = 발사체 오브젝트>
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
        // Projectiles 프리펩을 불러온다
        GameObject[] projectilesList = Resources.LoadAll<GameObject>("Prefabs\\Projectiles");
        // Dic에 저장해둔다.
        foreach (GameObject projectile in projectilesList)
        {
            allProjectiles.Add(projectile.name, projectile);
        }
        // 모든 발사체 오브젝트 초기화
        List<Dictionary<string, object>> projectilesData = CSVReader.Read("CSVFile\\Projectile");
        Projectile item;
        for(int i = 0; i < projectilesList.Length; i++)
        {
            if (DEBUG)
                Debug.Log("projectile 명 : "+projectilesData[i]["ProjectileType"]);
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
            // 스폰형인 경우 스폰 타입 지정
            if (item.Spec.Type[0] == 's')
            {
                item.GetComponent<Spawn>().MSpawnType = projectilesData[i]["ProjectileSpawnType"].ToString();
            }
        }
    }
}

