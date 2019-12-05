using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public class WallContactCheckerOC : MonoBehaviour
    {
        public float ContanctActiveDuration = 0.25f;
        private Vector3? _lastWallNormal;
        private float _lastContactTime;

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger enter");
            if (other.gameObject.tag.Equals(Constants.WallTriggerTag))
            {
                Debug.Log("Enter collision");
                var wallFace = other.gameObject.GetComponent<JumpableWallFaceOC>();
                Assert.IsNotNull(wallFace);
                _lastWallNormal = wallFace.WallFaceNormal;
                _lastContactTime = Time.time;
            };
        }

        public bool HasActiveContact => _lastWallNormal.HasValue && (Time.time - _lastContactTime) < ContanctActiveDuration;

        public Vector3 RetriveAndClearContactNormal()
        {
            Debug.Log("Retr col");
            var toReturn = _lastWallNormal.Value;
            _lastWallNormal = null;
            return toReturn;
        }
    }
}
