using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class GroundedCheckerOC : MonoBehaviour
    {
        private int _floorCollidersCount = 0;
        private int _floorTriggersCount = 0;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals(Constants.FloorTag))
            {
                _floorCollidersCount++;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag.Equals(Constants.FloorTag))
            {
                _floorCollidersCount--;
            };
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(Constants.FloorTriggerTag))
            {
                _floorTriggersCount++;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals(Constants.FloorTriggerTag))
            {
                _floorTriggersCount--;
            }
        }

        public bool IsTouchingGround => _floorCollidersCount > 0;
        public bool IsNearGround => _floorTriggersCount > 0;
    }
}
