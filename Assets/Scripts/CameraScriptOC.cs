using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraScriptOC : ReactingOnPlayerReset
    {
        public Camera CameraComponent;
        public Rigidbody PlayerRigidbody;
        public GameObject VisibleAreaDownLeftPointMarker;
        public GameObject VisibleAreaUpperRightPointMarker;
        public float ByPlayerVelocitySizeMultiplier;
        public float ByDistanceToCameraSizeMultiplier;
        public float PositionPredictionTransformMultiplier;
        public Vector2 CameraOrtographicSizeLimits;
        public float PositionFollowingSpeed;
        public float SizeFollowingSpeed;

        private Vector3 _velocityOffsetComponent;

        void Update()
        {
            transform.position = CalculatePosition();
            CameraComponent.orthographicSize = CalculateSize();
            AffirmCameraInVisibleSpace();
            _velocityOffsetComponent = Vector3.zero;
        }

        public override void PlayerIsReset()
        {
            transform.position = new Vector3(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.y, transform.position.z);
        }

        private float CalculateSize()
        {
            var delta = new Vector2(transform.position.x, transform.position.y) -
                        new Vector2(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.y);
            delta.y *= ((float) Screen.width) / Screen.height;

            var flatDistance = delta.magnitude;

            var sizeByDistance = flatDistance * ByDistanceToCameraSizeMultiplier;

            var sizeByVelocity = PlayerRigidbody.velocity.magnitude * ByPlayerVelocitySizeMultiplier;

            var newSizeSample = Mathf.Min(CameraOrtographicSizeLimits.y, Mathf.Max(new[] {sizeByDistance, sizeByVelocity, CameraOrtographicSizeLimits.x}));

            return Mathf.Lerp(newSizeSample, CameraComponent.orthographicSize, 1-Time.deltaTime*SizeFollowingSpeed);
        }

        private Vector3 CalculatePosition()
        {
            var targetPosition = new Vector3(PlayerRigidbody.transform.position.x, PlayerRigidbody.transform.position.y, transform.position.z);
            var thisFrameVelocityComponent = new Vector3(PlayerRigidbody.velocity.x, PlayerRigidbody.velocity.y, 0);
            _velocityOffsetComponent = Vector3.Lerp(_velocityOffsetComponent, thisFrameVelocityComponent*PositionPredictionTransformMultiplier, Time.time );
            targetPosition += _velocityOffsetComponent;

            return Vector3.Lerp(
                targetPosition,
                transform.position,
                1 - Time.deltaTime*PositionFollowingSpeed
            );
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
                Vector2 worldSpaceScreenSize;
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
