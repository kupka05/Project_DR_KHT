
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Events;
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
using Invector;
#endif

namespace BNG {
    /// <summary>
    /// A basic damage implementation. Call a function on death. Allow for respawning.
    /// </summary>
    public class Damageable : MonoBehaviour {

        // 보스 할당
        private Js.Boss.Boss _boss;
        
        public float Health = 100;
        private float _startingHealth;

        [Tooltip("If specified, this GameObject will be instantiated at this transform's position on death.")]
        public GameObject SpawnOnDeath;

        [Tooltip("Activate these GameObjects on Death")]
        public List<GameObject> ActivateGameObjectsOnDeath;

        [Tooltip("Deactivate these GameObjects on Death")]
        public List<GameObject> DeactivateGameObjectsOnDeath;

        [Tooltip("Deactivate these Colliders on Death")]
        public List<Collider> DeactivateCollidersOnDeath;

        /// <summary>
        /// Destroy this object on Death? False if need to respawn.
        /// </summary>
        [Tooltip("Destroy this object on Death? False if need to respawn.")]
        public bool DestroyOnDeath = true;

        [Tooltip("If this object is a Grabbable it can be dropped on Death")]
        public bool DropOnDeath = true;

        /// <summary>
        /// How long to wait before destroying this objects
        /// </summary>
        [Tooltip("How long to wait before destroying this objects")]
        public float DestroyDelay = 0f;

        /// <summary>
        /// If true the object will be reactivated according to RespawnTime
        /// </summary>
        [Tooltip("If true the object will be reactivated according to RespawnTime")]
        public bool Respawn = false;

        /// <summary>
        /// If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.
        /// </summary>
        [Tooltip("If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.")]
        public float RespawnTime = 10f;

        /// <summary>
        /// Remove any decals that were parented to this object on death. Useful for clearing unused decals.
        /// </summary>
        [Tooltip("Remove any decals that were parented to this object on death. Useful for clearing unused decals.")]
        public bool RemoveBulletHolesOnDeath = true;

        [Header("Events")]
        [Tooltip("Optional Event to be called when receiving damage. Takes damage amount as a float parameter.")]
        public FloatEvent onDamaged;

        public Vector3Event onKnockback;

        [Tooltip("Optional Event to be called once health is <= 0")]
        public UnityEvent onDestroyed;

        [Tooltip("Optional Event to be called once the object has been respawned, if Respawn is true and after RespawnTime")]
        public UnityEvent onRespawn;

#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
        // Invector damage integration
        [Header("Invector Integration")]
        [Tooltip("If true, damage data will be sent to Invector object using 'ApplyDamage'")]
        public bool SendDamageToInvector = true;
#endif

        bool destroyed = false;
        public bool stun = false;

        Rigidbody rigid;
        bool initialWasKinematic;

        private void Start() {
            _startingHealth = Health;
            rigid = GetComponent<Rigidbody>();
            if (rigid) {
                initialWasKinematic = rigid.isKinematic;
            }
        }

        // Init
        public void Initialize()
        {
            //LEGACY:
            //_boss = boss;
            //_startingHealth = boss.BossData.MaxHP;
            // 데미지를 두번 연산해서 Boss와 Damageable간의
            // 오차가 생겨 _startingHealth를 변경함
            // 왜냐면 Damageable의 체력이 0이 되면 데미지가 안들어가기 떄문
            _startingHealth = 99999999f;
            Health = _startingHealth;
        }

        public virtual void DealDamage(float damageAmount, bool _critical = default) {
            DealDamage(damageAmount, transform.position, critical : _critical);

        }

        //public virtual void DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null) {
        public virtual void DealDamage(float damageAmount, Vector3 hitPosition, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null, bool critical = default, bool left = default)
        {
            if (destroyed || stun) {
                return;
            }

            if(damageAmount < 0)
            {
                critical = true;
                damageAmount = Mathf.Abs(damageAmount);
            }

            damageAmount = Mathf.RoundToInt(damageAmount);  // 반올림
            Health -= damageAmount;

            onDamaged?.Invoke(damageAmount);
            CreateDamageUI(damageAmount, hitPosition, _left : left, _crit : critical);

            // Invector Integration
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
            if(SendDamageToInvector) {
                var d = new Invector.vDamage();
                d.hitReaction = reactToHit;
                d.hitPosition = (Vector3)hitPosition;
                d.receiver = receiver == null ? this.gameObject.transform : null;
                d.damageValue = (int)damageAmount;

                this.gameObject.ApplyDamage(new Invector.vDamage(d));
            }
#endif

            if (Health <= 0) {
                DestroyThis();
            }
        }
        public void OnKnockBack(Vector3 hitPosition)
        {
            onKnockback?.Invoke(hitPosition);
        }

        public virtual void DestroyThis() {
            Health = 0;
            destroyed = true;

            // Activate
            foreach (var go in ActivateGameObjectsOnDeath) {
                go.SetActive(true);
            }

            // Deactivate
            foreach (var go in DeactivateGameObjectsOnDeath) {
                go.SetActive(false);
            }

            // Colliders
            foreach (var col in DeactivateCollidersOnDeath) {
                col.enabled = false;
            }

            // Spawn object
            if (SpawnOnDeath != null) {
                var go = GameObject.Instantiate(SpawnOnDeath);
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
            }

            // Force to kinematic if rigid present
            if (rigid) {
                rigid.isKinematic = true;
            }

            // Invoke Callback Event
            if (onDestroyed != null) {
                onDestroyed.Invoke();
            }

            if (DestroyOnDeath) {
                Destroy(this.gameObject, DestroyDelay);
            }
            else if (Respawn) {
                StartCoroutine(RespawnRoutine(RespawnTime));
            }

            // Drop this if the player is holding it
            Grabbable grab = GetComponent<Grabbable>();
            if (DropOnDeath && grab != null && grab.BeingHeld) {
                grab.DropItem(false, true);
            }

            // Remove an decals that may have been parented to this object
            if (RemoveBulletHolesOnDeath) {
                BulletHole[] holes = GetComponentsInChildren<BulletHole>();
                foreach (var hole in holes) {
                    GameObject.Destroy(hole.gameObject);
                }

                Transform decal = transform.Find("Decal");
                if (decal) {
                    GameObject.Destroy(decal.gameObject);
                }
            }
        }

        IEnumerator RespawnRoutine(float seconds) {

            yield return new WaitForSeconds(seconds);

            Health = _startingHealth;
            destroyed = false;

            // Deactivate
            foreach (var go in ActivateGameObjectsOnDeath) {
                go.SetActive(false);
            }

            // Re-Activate
            foreach (var go in DeactivateGameObjectsOnDeath) {
                go.SetActive(true);
            }
            foreach (var col in DeactivateCollidersOnDeath) {
                col.enabled = true;
            }

            // Reset kinematic property if applicable
            if (rigid) {
                rigid.isKinematic = initialWasKinematic;
            }

            // Call events
            if (onRespawn != null) {
                onRespawn.Invoke();
            }
        }

        // 아이템에 네임택을 넣어주는 메서드
        public void CreateDamageUI(float _damage, Vector3 _position = default, bool _left = false,  bool _crit = false)
        {
            // 플레이어의 경우 예외
            if(gameObject.CompareTag("Player"))
            { return; }

            GameObject damageObj = Resources.Load<GameObject>("Prefabs/DamageUI");
            GameObject damageUI = Instantiate(damageObj, _position, Quaternion.identity);
            damageUI.GetComponent<DamageUI>().OnDeal(_damage, position: _position, left : _left, critical: _crit); ;         
        }
    }
}