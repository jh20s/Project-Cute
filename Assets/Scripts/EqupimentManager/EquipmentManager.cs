using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YJY
{
    public class EquipmentManager : MonoBehaviour
    {
        // Start is called before the first frame update
        #region variable
        // key : ��� �з� , value : �ش� ��� ������Ʈ(0: ����, 1: �ڽ�Ƭ...)
        [SerializeField] private Dictionary<int, List<GameObject>> equipments = new Dictionary<int, List<GameObject>>();
        [SerializeField] private Dictionary<int, List<GameObject>> userCurrentEquip = new Dictionary<int, List<GameObject>>();
        // key : ���� �з� , value : �ش� ���� ��� ������Ʈ
        [SerializeField] private Dictionary<int, GameObject> monsterCurrentEquip = new Dictionary<int, GameObject>();
        #endregion
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {

        }
        // ���� ��ü�� �����Ͽ� ���� ��ü�� autoAttack()�� ȣ���ϴ� �Լ�
        public void autoAttack(float _dmg)
        {

        }
        // ���� ��ü�� �����Ͽ� ���� ��ü�� activeGeneralSkill()�� ȣ���ϴ� �Լ�
        public void generalSkillClicked()
        {

        }
        // ���� ��ü�� �����Ͽ� ���� ��ü�� activeUltimateSkill()�� ȣ���ϴ� �Լ�
        public void ultimateSkillClicked()
        {

        }
    }
}

