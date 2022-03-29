using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class Bow : Weapon
    {
        #region variable
        protected override Dictionary<int, GameObject> projectiles
        {
            get { return projectiles; }
            set { projectiles = new Dictionary<int, GameObject>(); }
        }
        protected override Skill generalSkill
        {
            get { return generalSkill; }
            set { generalSkill = value; }
        }
        protected override Skill ultimateSkill
        {
            get { return ultimateSkill; }
            set { ultimateSkill = value; }
        }
        protected override WeaponSpec spec
        {
            get { return spec; }
            set { spec = new WeaponSpec(); }
        }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            setInit();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region method
        protected override void activeGeneralSkill()
        {
            /* SkillManager�� ���Ͽ� �ش� Skill �޼��� ȣ��*/
        }

        protected override void activeUltimateSkill()
        {
            /* SkillManager�� ���Ͽ� �ش� Skill �޼��� ȣ��*/
        }

        protected override void autoAttack(GameObject _projectile, float _dmg, Transform _targetMob)
        {
            /* ���⼭ �ڵ����� �ż��� �ۼ��մϴ�.*/
        }

        protected override void setInit()
        {
            /* ���⼭ ������ �Ľ��� ������ �������ݴϴ�.*/


            // ���� ���� ������ �Ľ�
            spec.setData();
        }
        #endregion
    }
}

