using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace BNG
{

    /// <summary>
    /// This collider will Damage a Damageable object on impact
    /// </summary>
    public class DamageCollider : MonoBehaviour
    {

        /// <summary>
        /// 데미지 양, 크리티컬 여부
        /// </summary>
        public (float, bool) damage;    

        /// <summary>
        /// 이 충돌체의 속도를 결정하는 데 사용됩니다.
        /// </summary>
        public Rigidbody ColliderRigidbody;

        /// <summary>
        /// 손상을 입히는 데 필요한 최소한의 힘. relativeVelocity.magnitude로 표현됩니다.
        /// </summary>
        public float MinForce = 0.1f;

        /// <summary>
        /// 이전 프레임의 마지막 상대 속도 값
        /// </summary>
        public float LastRelativeVelocity = 0;

        // 충돌 입력 시 마지막으로 적용된 충격력의 양
        public float LastDamageForce = 0;

        /// <summary>
        /// 이 콜라이더가 무언가와 충돌하면 데미지를 입어야하는지 여부, ex.책상에서 떨어지면 박스가 부서짐
        /// </summary>
        public bool TakeCollisionDamage = false;

        /// <summary>
        /// 빠른 속도로 물체와 충돌할 경우 적용할 피해량
        /// </summary>
        public float CollisionDamage = 5;

        [Header("Damage Option")]
        // 플레이어 여부, 플레이어는 자기 자신을 공격 할 수 없다.
        public bool isPlayer;
        public bool canSelfHarm = false; // 자해 여부
        public bool isKnockback = true;
        public bool isBossProjectile;  //보스 예외 처리
        public bool isEliteProjectile; //엘리트몬스터 예외 처리
        public bool isProjectile;
        public bool isLeft;
        public bool isDrill;
        private float drillDamage;  // 드릴을 위한 데미지

        Damageable thisDamageable;
        public bool isMonster;

        private void Start()
        {
            if (ColliderRigidbody == null)
            {
                ColliderRigidbody = GetComponent<Rigidbody>();
            }

            thisDamageable = GetComponent<Damageable>();
           
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(isMonster)
            {
                return;
            }

            if (!this.isActiveAndEnabled)
            {
                return;
            }
            if (!canSelfHarm)
            {
                if (this.transform.root.gameObject == collision.transform.root.gameObject)
                {
                    return;
                }
            }
            if (isPlayer && collision.gameObject.CompareTag("Player"))
                return;

            if (isBossProjectile && collision.gameObject.GetComponent<Old_Boss>())
                return;

            if (isEliteProjectile && collision.gameObject.GetComponent<EliteMonster>())
                return;

            DrillDamage();   // 드릴일 경우 

            OnCollisionEvent(collision);
        }

        public virtual void OnCollisionEvent(Collision collision)
        {
            LastDamageForce = collision.impulse.magnitude;
            LastRelativeVelocity = collision.relativeVelocity.magnitude;


            if (LastDamageForce >= MinForce)
            {

                // Can we damage what we hit?
                Damageable d = collision.gameObject.GetComponent<Damageable>();
                DamageablePart damagePart = collision.gameObject.GetComponent<DamageablePart>();
                if (d)
                {
                    d.DealDamage(damage.Item1, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject, left : isLeft, critical: damage.Item2);

                    if (isKnockback)
                    {
                        d.OnKnockBack(collision.GetContact(0).point);
                    }

                }
                else if (damagePart)
                {

                    damagePart.parent.DealDamage(damage.Item1, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject, left: isLeft, critical: damage.Item2);
                    if (isKnockback)
                    {
                        damagePart.parent.OnKnockBack(collision.GetContact(0).point);
                    }

                }
            }
            // Otherwise, can we take damage ourselves from this collision?
            else if (TakeCollisionDamage && thisDamageable != null)
            {

                thisDamageable.DealDamage(CollisionDamage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject, left : isLeft);
            }
        }

        public void SetDamage(float _damage)
        {
            damage.Item1 = _damage;
        }
        public void DrillDamage()
        {
            if (isDrill)
            {
                damage = Damage.instance.DamageCalculate(drillDamage);
            }
        }
        public void SetDrillDamage(float _damage)
        {
            drillDamage = _damage;
        }
    }
}
