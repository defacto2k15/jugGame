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
        public GameObject Target;
        private Queue<Vector2> _samplesCount;

        void Start()
        {
            ResetSamples();
        }
        public override void PlayerIsDead()
        {
            ResetSamples();
        }

        private void ResetSamples()
        {
            _samplesCount = new Queue<Vector2>(Enumerable.Range(0, 30).Select(c => Target.transform.position.XYComponent()).ToList());
        }

        public void Update()
        {
            _samplesCount.Dequeue();
            _samplesCount.Enqueue( Target.transform.position.XYComponent());

            var avgPosition = new Vector2(_samplesCount.Select(c => c.x).Average(), _samplesCount.Select(c => c.y).Average());
            transform.position = new Vector3(avgPosition.x, avgPosition.y, transform.position.z);
        }
    }
}
