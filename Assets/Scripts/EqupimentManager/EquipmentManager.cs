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