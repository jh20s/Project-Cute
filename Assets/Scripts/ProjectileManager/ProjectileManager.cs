using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YJY
{
    public class ProjectileManager : MonoBehaviour
    {
        #region variable
        // key : s, c, l, r(Ÿ�� ù����) value :  <key : 0~(Ÿ�� ��������), value = �߻�ü ������Ʈ>
        public PBL.StringIntGameObject allProjectiles;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            initAllProjectiles();
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void initAllProjectiles()
        {
            // ��� �߻�ü ������Ʈ �ʱ�ȭ
        }
    }
}

