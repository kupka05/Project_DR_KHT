using BNG;
using TMPro;
using UnityEngine;

/// <summary>
/// 확장 메서드들을 정의하는 클래스
/// </summary>
public static class Extensions
{
    // Rigidbody 컴포넌트를 가져오거나 없으면 추가해서 반환하는 확장 메서드
    public static Rigidbody GetOrAddRigidbody(this GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        // Rigidbody 컴포넌트가 없으면 추가
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 50f;
            rb.drag = 1f;
            rb.angularDrag = 0.05f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        }

        return rb;
    }

    // 게으른 초기화
    // TMP_Text 객체에 값이 없을 경우 GetComponent함
    // 있을 경우 if 연산 없이 바로 객체 값 반환
    public static TMP_Text GetTMPText(this GameObject gameObject, ref TMP_Text text)
    {
        return text ?? (text = gameObject.GetComponent<TMP_Text>());
    }

    // Grabbable 설정 프리셋
    public static void GrabbablePreset(this Grabbable grabbable)
    {
        // 프리셋 (1)
        grabbable.GrabButton = GrabButton.Grip;
        grabbable.GrabPhysics = GrabPhysics.None;
        grabbable.ParentHandModel = false;
        grabbable.CanBeSnappedToSnapZone = false;
        grabbable.handPoseType = HandPoseType.AnimatorID;
    }

}