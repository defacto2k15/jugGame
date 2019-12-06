using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class CameraFollowingPositionScriptOC : ReactingOnPlayerDeath
    {
        public float FollowingSpeed = 1;
        public GameObject Target;

        void Start()
        {
            ResetPosition();
        }
        public override void PlayerIsDead()
        {
            ResetPosition();
        }

        private void ResetPosition()
        {
            transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
        }

        public void Update()
        {
            transform.position = Vector3.Lerp(
                new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z),
                transform.position,
                1 - Time.deltaTime*FollowingSpeed
            );
        }
    }
}
