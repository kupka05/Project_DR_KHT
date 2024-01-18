using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossMonsterManager : MonoBehaviour
    {
        /*************************************************
         *                Public Fields
         *************************************************/
        #region 싱글톤 패턴
        private static BossMonsterManager m_Instance = null; // 싱글톤이 할당될 static 변수
        public static BossMonsterManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<BossMonsterManager>();
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject("BossMonsterManager");
                    m_Instance = obj.AddComponent<BossMonsterManager>();
                    DontDestroyOnLoad(obj);
                }
                return m_Instance;
            }
        }
        #endregion


        /*************************************************
         *                Public Methods
         *************************************************/
        // 보스 몬스터를 생성
        public GameObject CreateBossMonster(int id, Vector3 pos)
        {
            string prefabName = Data.GetString(id, "PrefabName");
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab != null)
            {
                GameObject boss = Instantiate(prefab);
                BossMonster bossMonster = boss.AddComponent<BossMonster>();
                bossMonster.Initialize(id);
                boss.transform.position = pos;
                boss.name = prefabName;
                return boss;
            }

            // 프리팹이 없을 경우
            return default;
        }
    }
}
