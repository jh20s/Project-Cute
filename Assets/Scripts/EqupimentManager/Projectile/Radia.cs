using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJY
{
    public class Radia : Projectile
    {
        #region varialbe
        protected override float moveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }
        protected override float damage
        {
            get { return damage; }
            set { damage = value; }
        }
        protected override Vector3 target
        {
            get { return target; }
            set { target = value; }
        }
        protected override int count
        {
            get { return count; }
            set { count = value; }
        }
        private float angle
        {
            get { return angle; }
            set { angle = value; }
        }
        #endregion
        protected override void destroySelf()
        {
            /*������ �ı��Ǵ� �ż���*/
        }

        protected override void launchProjectile()
        {
            /*����� �߻� �ż���*/
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
    }
}
