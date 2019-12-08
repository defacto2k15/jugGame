using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneRootOC : MonoBehaviour
    {
        public float DeathZoneY;
        public PlayerControllerOC Player;
        public CheckpointScriptOC CurrentActiveCheckpoint;
        private bool _sceneInitialized = false;

        private void ResetPlayerToActiveCheckpoint()
        {
            Player.transform.position = new Vector3(CurrentActiveCheckpoint.transform.position.x, CurrentActiveCheckpoint.transform.position.y,
                Player.transform.position.z);
            FindObjectsOfType<ReactingOnPlayerReset>().ToList().ForEach(c => c.PlayerIsReset());
        }

        public void Update()
        {
            if (!_sceneInitialized)
            {
                ResetPlayerToActiveCheckpoint();
                _sceneInitialized = true;
            }
            if (Player.transform.position.y < DeathZoneY || Input.GetKeyDown(KeyCode.R))
            {
                ResetPlayerToActiveCheckpoint();
            }
        }

        public void PlayerEnteredCheckpoint(CheckpointScriptOC callingCheckpoint )
        {
            if (callingCheckpoint.CheckpointIndex > CurrentActiveCheckpoint.CheckpointIndex)
            {
                CurrentActiveCheckpoint = callingCheckpoint;
            }
        }
    }
}
