using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class Spawn : Projectile
    {
        #region variable
        public override ProjectileSpec spec
        {
            get { return spec; }
            set { spec = value; }
        }
        public override Vector3 target
        {
            get { return target; }
            set { target = value; }
        }
        #endregion
        #region method
        protected override void setInit()
        {
            // �߻�ü ������ �Ľ� �� �� ����
            ProjectileSpec newSpec = new ProjectileSpec();
            // spec ������ ����
            spec = newSpec;

        }
        protected override void destroySelf()
        {
            /*������ �ı��Ǵ� �ż���*/
        }
        protected override void launchProjectile()
        {
            /*���� �ż���*/
        }
        // Start is called before the first frame update
        void Start()
        {
            destroySelf();
        }
        // Update is called once per frame
        void Update()
        {
            launchProjectile();
        }
        #endregion
    }
}
