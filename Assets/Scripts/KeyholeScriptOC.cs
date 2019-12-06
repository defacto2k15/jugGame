using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public class KeyholeScriptOC : MonoBehaviour
    {
        public DisappearingAnimationOC AnimationToRun;
        public float KeyCloseEnoughDistance = 0.1f;
        private  KeyScriptOC _trackedKey;
        private bool _animationStarted;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(Constants.KeyTag))
            {
                Assert.IsNull(_trackedKey);
                var key = other.gameObject.GetComponent<KeyScriptOC>();
                Assert.IsNotNull(key);
                _trackedKey = key;
            };
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals(Constants.KeyTag))
            {
                Assert.IsNotNull(_trackedKey);
                _trackedKey = null;
            };
        }

        void Update()
        {
            if (_trackedKey != null && !_animationStarted)
            {
                if (Vector2.Distance(transform.position.XYComponent(), _trackedKey.transform.position.XYComponent()) < KeyCloseEnoughDistance)
                {
                    _trackedKey.AchievedKeyhole();
                    _animationStarted = true;
                    AnimationToRun.StartAnimation();
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}
