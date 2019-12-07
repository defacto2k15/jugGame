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
        public GameObject PlayerStartMarker;
        public GameObject CameraObject;

        public void Start()
        {
            Player.gameObject.transform.position = PlayerStartMarker.transform.position;
            CameraObject.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, CameraObject.transform.position.z);
        }
        public void Update()
        {
            if (Player.transform.position.y < DeathZoneY || Input.GetKeyDown(KeyCode.T))
            {
                Player.transform.position = PlayerStartMarker.transform.position;
                CameraObject.transform.position = new Vector3(PlayerStartMarker.transform.position.x, PlayerStartMarker.transform.position.y, CameraObject.transform.position.z);
                FindObjectsOfType<ReactingOnPlayerDeath>().ToList().ForEach(c=>c.PlayerIsDead());
            }
        }
    }
}
