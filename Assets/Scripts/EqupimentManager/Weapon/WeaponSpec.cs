using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class WeaponSpec : EquipSpec
    {
        #region varialbe
        // ���� �Ӽ� Ÿ���̸�, ����̸�, ��񼳸�, �����, ������....
        protected override string typeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        protected override string equipName
        {
            get { return equipName; }
            set { equipName = value; }
        }
        protected override string equipDesc
        {
            get { return equipDesc; }
            set { equipDesc = value; }
        }
        protected override int equipRank
        {
            get { return equipRank; }
            set { equipRank = value; }
        }
        private float equipDamage
        {
            get { return equipDamage; }
            set { equipDamage = value; }
        }
        #endregion

        #region method
        public void setData()
        {
            /* ���⼭ ���� ���� ������ �Ľ��� �ʱ�ȭ ���ݴϴ�.*/
        }
        #endregion
    }
}

