using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.GuestBook
{
    [System.Serializable]
    public class GuestBookData
    {
        /*************************************************
         *              Public Properties
         *************************************************/
        public int ItemID => _itemID;
        public string Text => _text;
        public string Time => _time;
        public float Position_X => _position_X;
        public float Position_Y => _position_Y;
        public float Position_Z => _position_Z;
        public float Rotation_X => _rotation_X;
        public float Rotation_Y => _rotation_Y;
        public float Rotation_Z => _rotation_Z;


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private string _text;
        private string _time;
        private float _position_X;
        private float _position_Y;
        private float _position_Z;
        private float _rotation_X;
        private float _rotation_Y;
        private float _rotation_Z;


        /*************************************************
         *              Initialize Methods
         *************************************************/
        public GuestBookData(int itemID, string text, string time,
            Vector3 posistion, Quaternion rotation)
        {
            // Init
            _itemID = itemID;
            _text = text;
            _time = time;
            _position_X = posistion.x;
            _position_Y = posistion.y;
            _position_Z = posistion.z;
            _rotation_X = rotation.x;
            _rotation_Y = rotation.y;
            _rotation_Z = rotation.z;
        }
    }
}

