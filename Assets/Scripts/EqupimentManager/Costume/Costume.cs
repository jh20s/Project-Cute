using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YJY
{
    public class Costume : MonoBehaviour
    {
        // Start is called before the first frame update
        #region varialbe
        public CostumeSpec spec = new CostumeSpec();
        private bool isLocked;
        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }
        #endregion
        #region method
        public void setInit()
        {
            // �ڽ�Ƭ ������ ����
        }
        #endregion
    }
}

