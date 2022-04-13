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

            // �������� ��� ���� Ÿ�� ����
            if(item.Spec.Type[0] == 's')
            {
                item.GetComponent<Spawn>().MSpawnType = (Spawn.SpawnType)int.Parse(projectilesData[i]["ProjectileSpawnType"].ToString());
            }
        }
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
    /*
    * �������� ���� �߻�ü ���� ������ �߰� api�Դϴ�
    * ��ü������ ��� �߻�ü�� ���� �������� �߰� �����ݴϴ�.
    * _passCount : ���� ������ �߰� ��ġ�Դϴ�.
    */
    public void AddPassCount(int _passCount)
    {
        if (_passCount < 0)
            if (Projectile.AddPassCount <= 0)
                _passCount = 0;
        Projectile.AddPassCount += _passCount;
    }
}

