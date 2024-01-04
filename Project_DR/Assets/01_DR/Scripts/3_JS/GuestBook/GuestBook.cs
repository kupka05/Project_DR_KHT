using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.GuestBook
{
    public class GuestBook : MonoBehaviour
    {
        /*************************************************
         *               Public Properties
         *************************************************/
        public int ItemID => _guestBookData.ItemID;
        public string Text => _guestBookData.Text;
        public string Time => _guestBookData.Time;
        public float Position_X => _guestBookData.Position_X;
        public float Position_Y => _guestBookData.Position_Y;
        public float Position_Z => _guestBookData.Position_Z;
        public float Rotation_X => _guestBookData.Rotation_X;
        public float Rotation_Y => _guestBookData.Rotation_Y;
        public float Rotation_Z => _guestBookData.Rotation_Z;


        /*************************************************
         *                 Private Fields
         *************************************************/
        private GuestBookData _guestBookData;
        

        /*************************************************
         *                 Unity Events
         *************************************************/
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        /*************************************************
         *               Initialize Methods
         *************************************************/
        public void Initialize(GuestBookData guestBookData)
        {
            // Init
            _guestBookData = guestBookData;
        }


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 방명록을 업데이트 한다.
        public void UpdateGuestBook()
        {
            // TODO: Update 로직 추가
        }
    }
}
