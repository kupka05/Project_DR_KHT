using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {

    /// <summary>
    /// A set of events that allow you to apply haptics to a controller
    /// </summary>
    public class GrabbableHaptics : GrabbableEvents {

        public bool HapticsOnValidPickup = true;
        public bool HapticsOnValidRemotePickup = true;
        public bool HapticsOnCollision = true;
        public bool HapticsOnGrab = true;

        public float VibrateFrequency = 0.3f;
        public float VibrateAmplitude = 0.1f;
        public float VibrateDuration = 0.1f;

        // itemColliderHandler의 상태를 기본으로 변경하는 대기 시간
        private float itemColliderHandlerStateDelay = 3f;

        Grabber currentGrabber;
        public Grabber CurrentGrabber => currentGrabber;

        // HandColliderHandler 클래스에서 그립 여부를 파악하기
        // 위해 아래의 함수를 수정하여 사용함.
        public override void OnGrab(Grabber grabber) {
            // Store grabber so we can use it if we need to vibrate the controller
            currentGrabber = grabber;
            if(HapticsOnGrab) {
                doHaptics(grabber.HandSide);
            }
            // TODO: 차후 수정 / 낭비가 심함 / 폴리싱 때 수정하기
            // 아이템을 그립할 경우
            ItemColliderHandler itemColliderHandler = GetComponent<ItemColliderHandler>();
            if (itemColliderHandler != null)
            {
                itemColliderHandler.ChangeKinematic(false);                         // 물리 효과 동작 상태로 변경
                //itemColliderHandler.Coroutine(
                    //itemColliderHandler.ResetState, itemColliderHandlerStateDelay); // 3초 후에 기본 상태로 변경(인벤토리 보관 가능)
            }
        }

        public override void OnRelease() {
            currentGrabber = null;
        }

        // Fires if this is the closest grabbable but wasn't in the previous frame
        public override void OnBecomesClosestGrabbable(ControllerHand touchingHand) {
            
            if (HapticsOnValidPickup) {
                doHaptics(touchingHand);
            }
        }

        public override void OnBecomesClosestRemoteGrabbable(ControllerHand touchingHand) {
            if (HapticsOnValidRemotePickup) {
                doHaptics(touchingHand);
            }
        }

        void doHaptics(ControllerHand touchingHand) {
            if(input) {
                input.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, touchingHand);
            }
        }

        private void OnCollisionEnter(Collision collision) {
            // Play Haptic on collision
            if (HapticsOnCollision && currentGrabber != null && input != null) {
                // Only play collision haptics if being held
                if(grab != null && grab.BeingHeld) {
                    input.VibrateController(0.1f, 0.1f, 0.1f, currentGrabber.HandSide);
                }
            }
        }
    }
}

