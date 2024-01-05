using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.GuestBook
{
    public class GuestManager : MonoBehaviour
    {
        /*************************************************
         *               Public Properties
         *************************************************/
        #region 싱글톤 패턴
        private static GuestManager m_Instance = null; // 싱글톤이 할당될 static 변수    

        public static GuestManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<GuestManager>();
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject("GuestManager");
                    m_Instance = obj.AddComponent<GuestManager>();
                    DontDestroyOnLoad(obj);
                }
                return m_Instance;
            }
        }
        #endregion
        public List<GuestBook> GuestBookList => _guestBookList;                     // 방명록 리스트
        public List<GuestBookData> GuestBookDataList => _guestBookDataList;         // 방명록 데이터 리스트


        /*************************************************
         *                 Private Fields
         *************************************************/
        private List<GuestBook> _guestBookList = new List<GuestBook>(); 
        private List<GuestBookData> _guestBookDataList = new List<GuestBookData>();


        /*************************************************
         *                 Unity Events
         *************************************************/
        void Start()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *               Initialize Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            // TODO: DB에서 방명록 정보 가져오기
            // 가져온 정보를 guestbookdata로 변경 후 guestbook 생성하기

            for (int i = 0; i < _guestBookDataList.Count; i++)
            {
                // 방명록 생성 & 리스트에 추가
                _guestBookList.Add(CreateGuestBook(_guestBookDataList[i]));
            }

            // 방명록 리스트 안에 있는 모든 방명록을 업데이트
            UpdateAllGuestBookFromList();
        }


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 방명록을 생성한다.
        public GuestBook CreateGuestBook(GuestBookData guestBookData)
        {
            // 프리팹 생성
            string prefabName = Data.GetString(guestBookData.ItemID, "PrefabName");
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            // 방명록 오브젝트 생성 & 초기화
            GameObject guestBookObject = Instantiate(prefab);
            guestBookObject.name = GFunc.SumString("[방명록] ", prefabName);
            GuestBook guestBook = guestBookObject.AddComponent<GuestBook>();
            guestBook.Initialize(guestBookData);

            return guestBook;
        }

        // 모든 방명록을 업데이트 한다.
        public void UpdateAllGuestBookFromList()
        {
            for (int i = 0; i < _guestBookList.Count; i++)
            {
                // 방명록 업데이트
                _guestBookList[i].UpdateGuestBook();
            }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 방명록 데이터를 생성한다.
        public GuestBookData CreateGuestBookData(int itemID, string text, string time,
            Vector3 posistion, Quaternion rotation)
        {
            // 방명록 데이터 생성
            GuestBookData guestBookData = new GuestBookData(
                itemID, text, time, posistion, rotation);

            return guestBookData;
        }

        // 방명록 데이터를 JSON으로 직렬화한다.
        public string SerializeGuestBookData(GuestBookData guestBookData)
        {
            // 데이터 직렬화
            string json = JsonUtility.ToJson(guestBookData);

            return json;
        }

        // 직렬화된 데이터를 방명록 데이터로 구조화한다.
        public GuestBookData JSONParseGuestBookData(string json)
        {
            // json으로 변환된 string은 .NET Framework 디코딩이 필요
            json = System.Web.HttpUtility.UrlDecode(json);

            GFunc.Log($"디코딩된 데이터: {json}");

            // 데이터가 비어있을 경우 예외처리
            if (json.Equals("")) { return default; }

            // 방명록 데이터로 JSON 구조화
            GuestBookData guestBookData = JsonUtility.FromJson<GuestBookData>(json);

            GFunc.Log($"구조화된 데이터: {guestBookData}");

            return guestBookData;
        }

        // 방명록 데이터를 DB에 저장한다.
        public void SaveGuestBookDataToDB(string json)
        {
            // TODO: DB에 저장하는 방식 새로 추가
            // DB 람다 함수 추가하기
            //PlayerDataManager.Save();
        }
    }
}
