using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class KeyRotatorOC : MonoBehaviour
    {
        public Rigidbody KeyRigidbody;
        public float Force;

        public void Update()
        {
            KeyRigidbody.AddTorque(Force*new Vector3(1,0,1), ForceMode.Force);
        }
    }
}
