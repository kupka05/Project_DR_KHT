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
        }
        
        return rb;
    }
}