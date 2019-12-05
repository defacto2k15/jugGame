using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class JumpableWallFaceOC : MonoBehaviour
    {
        public Vector3 WallFaceNormal => gameObject.transform.TransformDirection(1, 0, 0);
    }
}
