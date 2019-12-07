using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CheckpointScriptOC : MonoBehaviour
    {
        public float CheckpointIndex;
        public SceneRootOC SceneRoot;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(Constants.PlayerTag))
            {
                SceneRoot.PlayerEnteredCheckpoint(this);
            }
        }
    }
}
