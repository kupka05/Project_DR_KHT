using Js.Crafting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace BNG {
    public class SnapZone : MonoBehaviour {


        [Header("Holder Item")]
        // 지환 : 이 스냅존이 고정 아이템 전용인지
        public bool isHolder;
        public float resetTime;
        Coroutine resetCoroutine;

        [Header("Crafting")]
        // 강화 슬롯 스냅존 전용
        [SerializeField] private bool isEnhance = false;

        [Header("Starting / Held Item")]
        [Tooltip("The currently held item. Set this in the editor to equip on Start().")]
        public Grabbable HeldItem;

        [Tooltip("TSet this in the editor to equip on Start().")]
        public Grabbable StartingItem;
        public Grabbable[] StartingItems;

        [Header("Options")]
        /// <summary>
        /// If false, Item will Move back to inventory space if player drops it.
        /// </summary>
        [Tooltip("If false, Item will Move back to inventory space if player drops it.")]
        public bool CanDropItem = true;

        /// <summary>
        /// If false the snap zone cannot have it's content replaced.
        /// </summary>
        [Tooltip("If false the snap zone cannot have it's content replaced.")]
        public bool CanSwapItem = true;

        /// <summary>
        /// If false the item inside the snap zone may not be removed
        /// </summary>
        [Tooltip("If false the snap zone cannot have it's content replaced.")]
        public bool CanRemoveItem = true;

        /// <summary>
        /// Multiply Item Scale times this when in snap zone.
        /// </summary>
        [Tooltip("Multiply Item Scale times this when in snap zone.")]
        public float ScaleItem = 1f;
        private float _scaleTo;

        public bool DisableColliders = true;
        List<Collider> disabledColliders = new List<Collider>();

        [Tooltip("If true the item inside the SnapZone will be duplicated, instead of removed, from the SnapZone.")]
        public bool DuplicateItemOnGrab = false;

        /// <summary>
        /// Only snap if Grabbable was dropped maximum of X seconds ago
        /// </summary>
        [Tooltip("Only snap if Grabbable was dropped maximum of X seconds ago")]
        public float MaxDropTime = 0.1f;

        /// <summary>
        /// Last Time.time this item was snapped into
        /// </summary>
        [HideInInspector]
        public float LastSnapTime;

        [Header("Filtering")]
        /// <summary>
        /// If not empty, can only snap objects if transform name contains one of these strings
        /// </summary>
        [Tooltip("If not empty, can only snap objects if transform name contains one of these strings")]
        public List<string> OnlyAllowNames;

        /// <summary>
        /// Do not allow snapping if transform contains one of these names
        /// </summary>
        [Tooltip("Do not allow snapping if transform contains one of these names")]
        public List<string> ExcludeTransformNames;

        [Header("Audio")]
        public AudioClip SoundOnSnap;
        public AudioClip SoundOnUnsnap;


        [Header("Events")]
        /// <summary>
        /// Optional Unity Event  to be called when something is snapped to this SnapZone. Passes in the Grabbable that was attached.
        /// </summary>
        public GrabbableEvent OnSnapEvent;

        /// <summary>
        /// Optional Unity Event to be called when something has been detached from this SnapZone. Passes in the Grabbable is being detattached.
        /// </summary>
        public GrabbableEvent OnDetachEvent;

        GrabbablesInTrigger gZone;

        Rigidbody heldItemRigid;
        bool heldItemWasKinematic;
        Grabbable trackedItem; // If we can't drop the item, track it separately

        // Closest Grabbable in our trigger
        [HideInInspector]
        public Grabbable ClosestGrabbable;

        SnapZoneOffset offset;


        void Start() {

            GetData();
            gZone = GetComponent<GrabbablesInTrigger>();
            _scaleTo = ScaleItem;

            // 강화 슬롯일 경우 Player의 drill을 찾아서 할당
            if (isEnhance)
            {
                StartingItems = new Grabbable[2];
                GameObject drill_L = GameObject.Find("Drill L");
                GameObject drill_R = GameObject.Find("Drill R");
                StartingItems[0] = drill_L.GetComponent<Grabbable>();
                StartingItems[1] = drill_R.GetComponent<Grabbable>();
            }

            // Auto Equip item by moving it into place and grabbing it
            if (StartingItem != null) {
                StartingItem.transform.position = transform.position;
                GrabGrabbable(StartingItem);
            }
            // Can also use HeldItem (retains backwards compatibility)
            else if (HeldItem != null) {
                HeldItem.transform.position = transform.position;
                GrabGrabbable(HeldItem);
            }
        }

        void Update() {

            ClosestGrabbable = getClosestGrabbable();

            // 고정 아이템이고 지금 들고있는 아이템이 없는 경우
            if (isHolder && HeldItem == null && StartingItem != null)
            {
                // 부모오브젝트가 있을 때
                if (StartingItem.transform.parent != null)
                {
                    // 이 스냅존이 없으면 : 아이템이 보관되어있지 않으면
                    if (StartingItem.transform.parent.GetComponent<SnapZone>() == null)
                    {
                        // 현재 코루틴이 없을때만 실행
                        if (resetCoroutine == null)
                        {
                            resetCoroutine = StartCoroutine(ResetCount());
                        }
                    }
                }
            }

            // Can we grab something
            if (HeldItem == null && ClosestGrabbable != null) {
                float secondsSinceDrop = Time.time - ClosestGrabbable.LastDropTime;
                if (secondsSinceDrop < MaxDropTime) {
                    GrabGrabbable(ClosestGrabbable);
                }
            }

            // Keep snapped to us or drop
            if (HeldItem != null) {

                // Something picked this up or changed transform parent
                if (HeldItem.BeingHeld || HeldItem.transform.parent != transform) {
                    ReleaseAll();
                }
                else
                {
                    // 강화 슬롯이 아닐 경우 기존 코드 실행
                    // 사유: 사이즈가 커지는 현상 발생
                    if (!isEnhance)
                    {
                        // Scale Item while inside zone.
                        HeldItem.transform.localScale = Vector3.Lerp(HeldItem.transform.localScale, HeldItem.OriginalScale * _scaleTo, Time.deltaTime * 30f);
                    }

                    // Make sure this can't be grabbed from the snap zone
                    if (HeldItem.enabled || (disabledColliders != null && disabledColliders.Count > 0 && disabledColliders[0] != null && disabledColliders[0].enabled)) {
                        disableGrabbable(HeldItem);
                    }
                }
            }

            // Can't drop item. Lerp to position if not being held
            //if (!CanDropItem && trackedItem != null && HeldItem == null) {
            //    if (!trackedItem.BeingHeld) {
            //        GrabGrabbable(trackedItem);
            //    }
            //}
        }

        Grabbable getClosestGrabbable() {

            Grabbable closest = null;
            float lastDistance = 9999f;

            if (gZone == null || gZone.NearbyGrabbables == null) {
                return null;
            }

            foreach (var g in gZone.NearbyGrabbables) {

                // Collider may have been disabled
                if (g.Key == null) {
                    continue;
                }

                float dist = Vector3.Distance(transform.position, g.Value.transform.position);
                if (dist < lastDistance) {

                    //  Not allowing secondary grabbables such as slides
                    if (g.Value.OtherGrabbableMustBeGrabbed != null) {
                        continue;
                    }

                    // Don't allow SnapZones in SnapZones
                    if (g.Value.GetComponent<SnapZone>() != null) {
                        continue;
                    }

                    // Don't allow InvalidSnapObjects to snap
                    if (g.Value.CanBeSnappedToSnapZone == false) {
                        continue;
                    }

                    // Must contain transform name
                    if (OnlyAllowNames != null && OnlyAllowNames.Count > 0) {
                        string transformName = g.Value.transform.name;
                        bool matchFound = false;
                        for (int x = 0; x < OnlyAllowNames.Count; x++) {
                            string name = OnlyAllowNames[x];
                            if (transformName.Contains(name)) {
                                matchFound = true;
                            }
                        }

                        // Not a valid match
                        if (!matchFound) {
                            continue;
                        }
                    }

                    // Check for name exclusion
                    if (ExcludeTransformNames != null) {
                        string transformName = g.Value.transform.name;
                        bool matchFound = false;
                        for (int x = 0; x < ExcludeTransformNames.Count; x++) {
                            // Not a valid match
                            if (transformName.Contains(ExcludeTransformNames[x])) {
                                matchFound = true;
                            }
                        }
                        // Exclude this
                        if (matchFound) {
                            continue;
                        }
                    }

                    // Only valid to snap if being held or recently dropped
                    if (g.Value.BeingHeld || (Time.time - g.Value.LastDropTime < MaxDropTime)) {
                        closest = g.Value;
                        lastDistance = dist;
                    }
                }
            }

            // 고정아이템일 경우
            if (isHolder)
            {
                // 스타트 아이템이 아니면 보관할 수 없도록 하기
                if (closest != null && closest != StartingItem)
                    return null;
            }

            // 강화슬롯일 경우
            if (isEnhance)
            {
                bool isDrill = false;
                // 스타트 아이템[](드릴L,R)이 아니면 보관할 수 없도록 하기
                for (int i = 0; i < StartingItems.Length; i++)
                {
                    // 넣을 아이템(closest)가 드릴일 경우
                    if (closest != null && closest.Equals(StartingItems[i]))
                    {
                        isDrill = true;
                    }
                }

                // 넣을 아이템(closest)이 드릴이 아닐 경우
                if (isDrill.Equals(false))
                {
                    return null;
                }
            }

            return closest;
        }

        public virtual void GrabGrabbable(Grabbable grab) {

            // Grab is already in Snap Zone
            if (grab.transform.parent != null && grab.transform.parent.GetComponent<SnapZone>() != null) {
                return;
            }

            if (HeldItem != null) {
                ReleaseAll();
            }

            // 강화 전용 슬롯일 경우 grab 인스턴스로 변경
            if (isEnhance)
            {
                Grabbable tempGrab = Instantiate(grab);
                grab.gameObject.SetActive(false);
                grab = tempGrab;
                // 캔버스 끄기
                grab.transform.Find("Canvas").gameObject.SetActive(false);
                // 강화 캔버스 켜기
                gameObject.GetComponent<EnhanceSlot>().InSlot();
            }

            // 아닐 경우 기존 grab 사용
            HeldItem = grab;
            heldItemRigid = HeldItem.GetComponent<Rigidbody>();

            // Mark as kinematic so it doesn't fall down
            if (heldItemRigid) {
                heldItemWasKinematic = heldItemRigid.isKinematic;
                heldItemRigid.isKinematic = true;
            }
            else {
                heldItemWasKinematic = false;
            }

            // Set the parent of the object 
            grab.transform.parent = transform;

            // Set scale factor            
            // Use SnapZoneScale if specified

            if (grab.GetComponent<SnapZoneScale>())
            {
                _scaleTo = grab.GetComponent<SnapZoneScale>().Scale;
            }
            else
            {
                _scaleTo = ScaleItem;
            }


            // Is there an offset to apply?
            SnapZoneOffset off = grab.GetComponent<SnapZoneOffset>();
            if (off) {
                offset = off;
            }
            else {
                offset = grab.gameObject.AddComponent<SnapZoneOffset>();
                offset.LocalPositionOffset = Vector3.zero;
                offset.LocalRotationOffset = Vector3.zero;
            }

            // Lock into place
            if (offset) {
                HeldItem.transform.localPosition = offset.LocalPositionOffset;
                HeldItem.transform.localEulerAngles = offset.LocalRotationOffset;
            }
            else {
                HeldItem.transform.localPosition = Vector3.zero;
                HeldItem.transform.localEulerAngles = Vector3.zero;
            }

            // Disable the grabbable. This is picked up through a Grab Action
            disableGrabbable(grab);

            // Call Grabbable Event from SnapZone
            if (OnSnapEvent != null) {
                OnSnapEvent.Invoke(grab);
            }

            // Fire Off Events on Grabbable
            GrabbableEvents[] ge = grab.GetComponents<GrabbableEvents>();
            if (ge != null) {
                for (int x = 0; x < ge.Length; x++) {
                    ge[x].OnSnapZoneEnter();
                }
            }

            if (SoundOnSnap) {
                // Only play the sound if not just starting the scene
                if (Time.timeSinceLevelLoad > 0.1f) {
                    VRUtils.Instance.PlaySpatialClipAt(SoundOnSnap, transform.position, 0.75f);
                }
            }

            LastSnapTime = Time.time;
        }

        void disableGrabbable(Grabbable grab) {

            if (DisableColliders) {
                disabledColliders = grab.GetComponentsInChildren<Collider>(false).ToList();
                for (int x = 0; x < disabledColliders.Count; x++) {
                    disabledColliders[x].enabled = false;
                }
            }

            // Disable the grabbable. This is picked up through a Grab Action
            grab.enabled = false;
        }

        /// <summary>
        /// This is typically called by the GrabAction on the SnapZone
        /// </summary>
        /// <param name="grabber"></param>
        public virtual void GrabEquipped(Grabber grabber)
        {
            if (grabber != null) {
                if (HeldItem)
                {
                    //// 강화 슬롯일 경우 grabber 체인지
                    //if (isEnhance)
                    //{
                    //    Grabber tempGrabber = grabber;
                    //    grabber.gameObject.SetActive(false);
                    //    grabber = tempGrabber;
                    //}

                    // Not allowed to be removed
                    if (!CanBeRemoved()) {
                        return;
                    }

                    var g = HeldItem;
                    if (DuplicateItemOnGrab) {

                        ReleaseAll();

                        // Position next to grabber if somewhat far away
                        if (Vector3.Distance(g.transform.position, grabber.transform.position) > 0.2f) {
                            g.transform.position = grabber.transform.position;
                        }

                        // Instantiate the object before it is grabbed
                        GameObject go = Instantiate(g.gameObject, transform.position, Quaternion.identity) as GameObject;
                        Grabbable grab = go.GetComponent<Grabbable>();

                        // Ok to attach it to snap zone now
                        this.GrabGrabbable(grab);

                        // Finish Grabbing the desired object
                        grabber.GrabGrabbable(g);
                    }
                    else {
                        ReleaseAll();

                        // Position next to grabber if somewhat far away
                        if (Vector3.Distance(g.transform.position, grabber.transform.position) > 0.2f) {
                            g.transform.position = grabber.transform.position;
                        }

                        // Do grab
                        grabber.GrabGrabbable(g);
                    }
                }
            }
        }

        public virtual bool CanBeRemoved() {
            // Not allowed to be removed
            if (!CanRemoveItem) {
                return false;
            }

            // Not a valid grab if we just snapped this item in an it's a toggle type
            if (HeldItem.Grabtype == HoldType.Toggle && (Time.time - LastSnapTime < 0.1f)) {
                return false;
            }

            return true;
        }


        // 슬롯에서 나왔을 경우 호출
        /// <summary>
        /// Release  everything snapped to us
        /// </summary>
        public virtual void ReleaseAll() {

            // No need to keep checking
            if (HeldItem == null) {
                return;
            }

            // Still need to keep track of item if we can't fully drop it
            if (!CanDropItem && HeldItem != null) {
                trackedItem = HeldItem;
            }

            // 강화 슬롯이 아닐 경우 기존 코드 실행
            // 사유: 사이즈가 커지는 현상 발생
            if (!isEnhance)
            {
                HeldItem.ResetScale();
            }

            if (DisableColliders && disabledColliders != null) {
                foreach (var c in disabledColliders) {
                    if (c) {
                        c.enabled = true;
                    }
                }
            }
            disabledColliders = null;

            // Reset Kinematic status
            if (heldItemRigid) {
                heldItemRigid.isKinematic = heldItemWasKinematic;
            }

            HeldItem.enabled = true;
            HeldItem.transform.parent = null;

            // Play Unsnap sound
            if (HeldItem != null) {
                if (SoundOnUnsnap) {
                    if (Time.timeSinceLevelLoad > 0.1f) {
                        VRUtils.Instance.PlaySpatialClipAt(SoundOnUnsnap, transform.position, 0.75f);
                    }
                }

                // Call event
                if (OnDetachEvent != null) {
                    OnDetachEvent.Invoke(HeldItem);
                }

                // Fire Off Grabbable Events
                GrabbableEvents[] ge = HeldItem.GetComponents<GrabbableEvents>();
                if (ge != null) {
                    for (int x = 0; x < ge.Length; x++) {
                        ge[x].OnSnapZoneExit();
                    }
                }
            }

            // 강화 슬롯일 경우
            if (isEnhance)
            {
                // 삭제 && 강화 UI 캔버스 끄기
                Destroy(HeldItem.gameObject);

                HeldItem = null;

                // 강화 UI 캔버스 끄기
                gameObject.GetComponent<EnhanceSlot>().OutSlot();
            }

            HeldItem = null;
        }

        // 강화 슬롯 초기화
        public void ResetEnhanceSlot()
        {
            ReleaseAll();
        }

        // 지환 : 아이템을 다시 홀스터로 되돌리는 스크립트
        public void ResetItem()
        {
            // 코루틴들을 꺼주고 null로 바꿔준다.
            StopAllCoroutines();
            resetCoroutine = null;

            // 만약 들고있는 아이템이 없는 경우 : 있으면 되돌릴 필요가 없기 때문에
            if (HeldItem == null)
            {
                StartingItem.gameObject.SetActive(false);
                StartingItem.gameObject.SetActive(true);
                StartingItem.transform.position = transform.position;
                GrabGrabbable(StartingItem);
            }
        }

        // 아이템 리셋 코루틴 
        IEnumerator ResetCount()
        {
            yield return new WaitForSeconds(resetTime);
            ResetItem();
        }

        private void GetData()
        {
            resetTime = (float)DataManager.Instance.GetData(1100, "ResetTimer", typeof(float));
        }
    }
}