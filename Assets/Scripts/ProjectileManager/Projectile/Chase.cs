using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class Chase : Projectile
{
        #region variable
        [SerializeField]
        private ProjectileSpec spec = new ProjectileSpec();
        public override ProjectileSpec Spec
        {
            get { return spec; }
            set { spec = value; }
        }

        [SerializeField]
        private Vector3 target;
        public override Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }
        #endregion
        #region method
        protected override void destroySelf()
        {
            /*������ �ı��Ǵ� �ż���*/
            if(Target == null)
                Destroy(gameObject, Spec.SpawnTime);
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

