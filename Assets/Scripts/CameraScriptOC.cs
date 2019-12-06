using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraScriptOC : ReactingOnPlayerDeath
    {
        public Camera CameraComponent;
        public Rigidbody PlayerRigidbody;
        public float ByPlayerVelocitySizeMultiplier;
        public float ByDistanceToCameraSizeMultiplier;
        public Vector2 SizeLimits;
        public float FollowingSpeed;
        public GameObject VisibleAreaDownLeftPointMarker;
        public GameObject VisibleAreaUpperRightPointMarker;

        private Queue<float> _sizeSamplesCount;


        void Start()
        {
            ResetSamplesCount();
        }

        void Update()
        {
            //CameraComponent.orthographicSize = CalculateSize();
            //transform.position = CalculatePosition();
            //AffirmCameraInVisibleSpace();
        }

        private float CalculateSize()
        {
            var delta = new Vector2(transform.position.x, transform.position.y) -
                        new Vector2(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.y);
            delta.y *= ((float) Screen.width) / Screen.height;

            var flatDistance = delta.magnitude;

            var sizeByDistance = flatDistance * ByDistanceToCameraSizeMultiplier;

            var sizeByVelocity = PlayerRigidbody.velocity.magnitude * ByPlayerVelocitySizeMultiplier;

            var newSizeSample = Mathf.Min(SizeLimits.y, Mathf.Max(new[] {sizeByDistance, sizeByVelocity, SizeLimits.x}));

            _sizeSamplesCount.Dequeue();
            _sizeSamplesCount.Enqueue(newSizeSample);
            var finalSize = _sizeSamplesCount.Average();
            return finalSize;
        }

        private Vector3 CalculatePosition()
        {
            return Vector3.Lerp(
                new Vector3(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.y, transform.position.z),
                transform.position,
                1 - Time.deltaTime*FollowingSpeed
            );
        }

        public override void PlayerIsDead()
        {
            ResetSamplesCount();
        }

        private void ResetSamplesCount()
        {
            _sizeSamplesCount = new Queue<float>(Enumerable.Range(0,30).Select(c=>SizeLimits.x));
        }

        private Rect VisibleSpace
        {
            get
            {
                var downLeftFlatPos = VisibleAreaDownLeftPointMarker.transform.position.XYComponent();
                var upperRightFlatPos = VisibleAreaUpperRightPointMarker.transform.position.XYComponent();

                return new Rect(downLeftFlatPos.x, downLeftFlatPos.y, upperRightFlatPos.x - downLeftFlatPos.x, upperRightFlatPos.y-downLeftFlatPos.y);
            }
        }

        private Rect WorldSpaceCameraArea
        {
            get
            {
                var orthographicSize = CameraComponent.orthographicSize;
                var worldSpaceScreenSize = Vector2.zero;
                float widthToHeightRatio = ((float) Screen.width / Screen.height);
                if (Screen.width > Screen.height)
                {
                    worldSpaceScreenSize = new Vector2(orthographicSize * widthToHeightRatio, orthographicSize)*2;
                }
                else
                {
                    worldSpaceScreenSize = new Vector2(orthographicSize, orthographicSize / widthToHeightRatio)*2;
                }

                var cameraFlatPos = transform.position.XYComponent();
                return new Rect(cameraFlatPos.x - worldSpaceScreenSize.x/2f, cameraFlatPos.y - worldSpaceScreenSize.y/2f, worldSpaceScreenSize.x, worldSpaceScreenSize.y);
            }
        }

        private void AffirmCameraInVisibleSpace()
        {
            var cameraArea = WorldSpaceCameraArea;
            var possibleMaxSpaceRatio = new Vector2(VisibleSpace.width / cameraArea.width, VisibleSpace.height / cameraArea.height);
            var correctionFactor = Mathf.Max(0, Mathf.Min(1, Mathf.Max(possibleMaxSpaceRatio.x, possibleMaxSpaceRatio.y)));

            CameraComponent.orthographicSize *= correctionFactor;

            var deltaToMin = VisibleSpace.min - WorldSpaceCameraArea.min;
            var absDeltaToMin = new Vector2(Mathf.Max(0, deltaToMin.x), Mathf.Max(0, deltaToMin.y));
            transform.position += new Vector3(absDeltaToMin.x, absDeltaToMin.y, 0);


            var deltaToMax = VisibleSpace.max - WorldSpaceCameraArea.max;
            var absDeltaToMax = new Vector2(Mathf.Min(0,deltaToMax.x), Mathf.Min(0,deltaToMax.y));
            transform.position +=new Vector3( absDeltaToMax.x,  absDeltaToMax.y, 0);
        }
    }
}
