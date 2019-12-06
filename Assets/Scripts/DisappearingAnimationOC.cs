using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class DisappearingAnimationOC: MonoBehaviour
    {
        public float AnimationDuration=1;
        private float _startTime;
        private Vector3 _orignalScale;
        private bool _animationStarted;

        void Start()
        {
            _animationStarted = false;
        }

        public void StartAnimation()
        {
            _startTime = Time.time;
            _animationStarted = true;
            _orignalScale = transform.localScale;
        }

        void Update()
        {
            if (_animationStarted)
            {
                var animationFactor = (Time.time - _startTime) / AnimationDuration;
                    Debug.Log("AF "+animationFactor);
                if (animationFactor > 1)
                {
                    Debug.Log("DESTROYING");
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    transform.localScale = Vector3.Lerp( _orignalScale, new Vector3(0,_orignalScale.y,  _orignalScale.z), animationFactor);
                }
            }
        }
    }
}
