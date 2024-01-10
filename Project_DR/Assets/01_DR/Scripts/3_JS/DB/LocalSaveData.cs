using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.LocalData
{
    [System.Serializable]
    public class LocalSaveData
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        // "PC Data"
        public int PlayerID => _playerID;       // 플레이어 아이디
        public int Gold => _gold;               // 보유 골드
        public int EXP => _exp;                 // 경험치

        // "PC Upgrade Data"
        private int HPLV => _hpLV;                      // HP 레벨
        private int GainGoldLV => _gainGoldLV;                // 골드 획득 레벨
        private int GainEXPLV => _gainEXPLV;                 // 경험치 획득 레벨

        // "Weapon Data"
        private float WeaponAtkLV => _weaponAtkLV;                  // 무기 공격 레벨
        private float WeaponAtkRate => _weaponAtkRate;              // 무기 공격 속도
        private float WeaponCritRate => _weaponCritRate;            // 크리 공격 확률
        private float WeaponCritDamage => _weaponCritDamage;        // 크리 공격 데미지

        // "Skill 1 Data"
        private int Skill_1_LV_1 => _skill_1_LV_1;                  // 테라드릴 레벨
        private int Skill_1_LV_2 => _skill_1_LV_2;                  // 테라드릴 레벨

        // "Skill 2 Data"
        private int Skill_2_LV_1 => _skill_2_LV_1;                  // 드릴연마 레벨
        private int Skill_2_LV_2 => _skill_2_LV_2;                  // 드릴연마 레벨
        private int Skill_2_LV_3 => _skill_2_LV_3;                  // 드릴연마 레벨

        // "Skill 3 Data"
        private int Skill_3_LV => _skill_3_LV;                      // 드릴분쇄 레벨

        // "Skill 4 Data"
        private int Skill_4_LV_1 => _skill_4_LV_1;                  // 드릴랜딩 레벨
        private int Skill_4_LV_2 => _skill_4_LV_2;                  // 드릴랜딩 레벨
        private int Skill_4_LV_3 => _skill_4_LV_3;                  // 드릴랜딩 레벨

        // "Clear Data"
        private int ClearCount => _clearCount;                      // 클리어 횟수
        private string JsonData => _jsonData;                       // Json을 담을 직렬화된 클리어 데이터
        private string QuestData => _questData;                     // 퀘스트 데이터


        /*************************************************
         *                Private Fields
         *************************************************/
        [Header("PC Data")]
        private int _playerID;                  // 플레이어 아이디
        private int _gold;                      // 보유 골드
        private int _exp;                       // 경험치

        [Header("PC Upgrade Data")]
        private int _hpLV;                      // HP 레벨
        private int _gainGoldLV;                // 골드 획득 레벨
        private int _gainEXPLV;                 // 경험치 획득 레벨

        [Header("Weapon Data")]
        private float _weaponAtkLV;             // 무기 공격 레벨
        private float _weaponAtkRate;           // 무기 공격 속도
        private float _weaponCritRate;          // 크리 공격 확률
        private float _weaponCritDamage;        // 크리 공격 데미지

        [Header("Skill 1 Data")]
        private int _skill_1_LV_1;                 // 테라드릴 레벨
        private int _skill_1_LV_2;                 // 테라드릴 레벨

        [Header("Skill 2 Data")]
        private int _skill_2_LV_1;                 // 드릴연마 레벨
        private int _skill_2_LV_2;                 // 드릴연마 레벨
        private int _skill_2_LV_3;                 // 드릴연마 레벨

        [Header("Skill 3 Data")]
        private int _skill_3_LV;                   // 드릴분쇄 레벨

        [Header("Skill 4 Data")]
        private int _skill_4_LV_1;                 // 드릴랜딩 레벨
        private int _skill_4_LV_2;                 // 드릴랜딩 레벨
        private int _skill_4_LV_3;                 // 드릴랜딩 레벨

        [Header("Clear Data")]
        private int _clearCount;                 // 클리어 횟수
        private string _jsonData;                // Json을 담을 직렬화된 클리어 데이터
        private string _questData;               // 퀘스트 데이터


        /*************************************************
         *                 Public Methods
         *************************************************/
        public LocalSaveData(int gold, int exp, int hpLV,
            int gainGoldLV, int gainEXPLV, float weaponAtkLV, float weaponAtkRate,
            float weaponCritRate, float weaponCritDamage, int skill_1_LV_1, int skill_1_LV_2,
            int skill_2_LV_1, int skill_2_LV_2, int skill_2_LV_3, int skill_3_LV, int skill_4_LV_1,
            int skill_4_LV_2, int skill_4_LV_3, int clearCount, string jsonData, string questData, 
            int playerID = default)
        {
            // Init
            _playerID = playerID;
            _gold = gold;
            _exp = exp;
            _hpLV = hpLV;
            _gainGoldLV = gainGoldLV;
            _gainEXPLV = gainEXPLV;
            _weaponAtkLV = weaponAtkLV;
            _weaponAtkRate = weaponAtkRate;
            _weaponCritRate = weaponCritRate;
            _weaponCritDamage = weaponCritDamage;
            _skill_1_LV_1 = skill_1_LV_1;
            _skill_1_LV_2 = skill_1_LV_2;
            _skill_2_LV_1 = skill_2_LV_1;
            _skill_2_LV_2 = skill_2_LV_2;
            _skill_2_LV_3 = skill_2_LV_3;
            _skill_3_LV = skill_3_LV;
            _skill_4_LV_1 = skill_4_LV_1;
            _skill_4_LV_2 = skill_4_LV_2;
            _skill_4_LV_3 = skill_4_LV_3;
            _clearCount = clearCount;
            _questData = questData;
        }
    }
}
