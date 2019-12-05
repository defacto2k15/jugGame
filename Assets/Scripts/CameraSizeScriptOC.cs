﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraSizeScriptOC : MonoBehaviour
    {
        public Camera CameraComponent;
        public Rigidbody PlayerRigidbody;
        public float ByPlayerVelocitySizeMultiplier;
        public float ByDistanceToCameraSizeMultiplier;
        public Vector2 SizeLimits;

        private Queue<float> _samplesCount;

        void Start()
        {
            _samplesCount = new Queue<float>(Enumerable.Range(0,30).Select(c=>SizeLimits.x));
        }

        void Update()
        {
            var delta = new Vector2(transform.position.x, transform.position.z) -
                        new Vector2(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.z);
            delta.y *= ((float)Screen.width) / Screen.height;

            var flatDistance = delta.magnitude;

            var sizeByDistance = flatDistance * ByDistanceToCameraSizeMultiplier;

            var sizeByVelocity = PlayerRigidbody.velocity.magnitude * ByPlayerVelocitySizeMultiplier;

            var newSizeSample = Mathf.Min(SizeLimits.y, Mathf.Max(new[] {sizeByDistance, sizeByVelocity, SizeLimits.x}));

            _samplesCount.Dequeue();
            _samplesCount.Enqueue(newSizeSample);
            var finalSize = _samplesCount.Average();

            CameraComponent.orthographicSize = finalSize;
        }

    }
}