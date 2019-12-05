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
        private List<JumpableWallWithEntryTime> _currentJumpableWalls;

        void Start()
        {
            _currentJumpableWalls = new List<JumpableWallWithEntryTime>();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger enter");
            if (other.gameObject.tag.Equals(Constants.WallTriggerTag))
            {
                Debug.Log("Enter collision");
                var wallFace = other.gameObject.GetComponent<JumpableWallFaceOC>();
                Assert.IsNotNull(wallFace);
                _currentJumpableWalls.Add(new JumpableWallWithEntryTime()
                {
                    FirstContactTime = Time.time,
                    Wall = wallFace,
                    IsFresh = true
                });
            };
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log("Trigger exit");
            if (other.gameObject.tag.Equals(Constants.WallTriggerTag))
            {
                Debug.Log("Exit collision");
                var wallFace = other.gameObject.GetComponent<JumpableWallFaceOC>();
                Assert.IsNotNull(wallFace);
                _currentJumpableWalls = _currentJumpableWalls.Where(c => !c.Wall.Equals(wallFace)).ToList();
            };
        }

        public bool HasActiveContact => _currentJumpableWalls.Any(WallCanBeJumpedFrom);

        private bool WallCanBeJumpedFrom(JumpableWallWithEntryTime c)
        {
            return (Time.time - c.FirstContactTime) < ContanctActiveDuration && c.IsFresh;
        }

        public Vector3 RetriveAndClearContactNormal()
        {
            var element = _currentJumpableWalls.Last(WallCanBeJumpedFrom);
            element.IsFresh = false;
            return element.Wall.WallFaceNormal;
        }


        class JumpableWallWithEntryTime
        {
            public JumpableWallFaceOC Wall;
            public float FirstContactTime;
            public bool IsFresh;
        }
    }
}
