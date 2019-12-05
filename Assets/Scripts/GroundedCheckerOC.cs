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

        public bool IsGrounded => _floorCollidersCount > 0;
    }
}
