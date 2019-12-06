using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public class KeyBoxScriptOC : ReactingOnPlayerDeath
    {
        public Color NotConnectedColor;
        public Color PlayerConnectedColor;
        public Color DoorConnectedColor;

        private KeyMode _mode;

        void Start()
        {
            _mode = KeyMode.NotConnected;
            SetColor();
        }

        private void SetColor()
        {
            Color colorToSet;
            if (_mode == KeyMode.NotConnected)
            {
                colorToSet = NotConnectedColor;
            }else if (_mode == KeyMode.ConnectedToPlayer)
            {
                colorToSet = PlayerConnectedColor;
            }
            else
            {
                colorToSet = DoorConnectedColor;
            }

            GetComponentInChildren<MonocolorMaterialColorSetterOC>().SetColor( colorToSet);
        }

        public override void PlayerIsDead()
        {
            if (_mode == KeyMode.ConnectedToPlayer)
            {
                RemoveOldJoint();
            }
        }

        private void RemoveOldJoint()
        {
            var possibleJoint = GetComponent<SpringJoint>();
            if (possibleJoint != null)
            {
                GameObject.Destroy(possibleJoint);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(Constants.PlayerTag))
            {
                if(_mode == KeyMode.NotConnected)
                {
                    var targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
                    CreateSpringJoint(targetRigidbody);

                    _mode = KeyMode.ConnectedToPlayer;
                    SetColor();
                }
            }
            else if (other.gameObject.tag.Equals(Constants.KeyHoleTag))
            {
                if (_mode == KeyMode.ConnectedToPlayer)
                {
                    RemoveOldJoint();

                    var targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
                    CreateSpringJoint(targetRigidbody);

                    _mode = KeyMode.FlyingToDoor;
                    SetColor();
                }

            }
        }

        private void CreateSpringJoint(Rigidbody targetRigidbody)
        {
            Assert.IsNotNull(targetRigidbody);
            var joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.spring = 1;
            joint.damper = 0.2f;
            joint.connectedBody = targetRigidbody;
            joint.connectedAnchor = Vector3.zero;
            joint.connectedMassScale = 0.0001f;
        }

        enum KeyMode
        {
            NotConnected, ConnectedToPlayer,FlyingToDoor, InPlaceInDoor
        }

        public void AchievedKeyhole()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
