using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace AssetBundleManager
{
    public class ABManager : MonoBehaviour
    {
        private static string _settingPath = "AssetBundleSetting";
        public static string settingPath { get { return _settingPath; } }

        public ConcurrentDictionary<string, GameObject> gameObjectList = new ConcurrentDictionary<string, GameObject>();

        public ConcurrentDictionary<string, GameObject> gameMapsList = new ConcurrentDictionary<string, GameObject>();

        private static ABManager _instance;
        public static ABManager GetInstance()
        {
            if(_instance == null)
            {
                // put an gameobject out
                GameObject o = new GameObject();
                o.name = "[AssetBundleManager]";
                _instance = o.AddComponent<ABManager>();
                DontDestroyOnLoad(o);
            }
            return _instance;
        }
        public static ABManager instance { get { return GetInstance(); } }

        private ABSetting _setting;
        private bool isSettingReady = false;
        private ConfData _confData = null;
        public bool isConfReady { get { return _confData != null; } }
        
        private bool _isWorking = false;
        public bool isWorking { get { return _isWorking; } }

        public delegate void OnConfDownloadFailDelegate();
        public delegate void OnABDownloadFailDelegate(IABLoadResult result);
        public static event OnConfDownloadFailDelegate OnConfDownloadFail;
        public static event OnABDownloadFailDelegate OnABDownloadFail;
        
        private Queue<IABLoadResult> _downloadQueue = new Queue<IABLoadResult>();
        public IABLoadResult[] downloadQueue { get { return _downloadQueue.ToArray(); } }

        public string currentPlatform
        {
            get
            {
#if UNITY_ANDROID
                return "Android/";
#elif UNITY_IOS
                return "iOS/";
#elif UNITY_STANDALONE
                return "Standalone/";
#else
                return "";
#endif
            }
        }

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            StartCoroutine(_GetSetting());
        }

        public GameObject LoadResourcesOBJ(string objName)
        {
            return Resources.Load<GameObject>(objName);
        }

        private IEnumerator _GetSetting()
        {
            while (_setting == null)
            {
                ResourceRequest r = Resources.LoadAsync<ABSetting>(_settingPath);
                while (!r.isDone)
                {
                    yield return 0;
                }
                _setting = r.asset as ABSetting;
            }
            isSettingReady = true;
        }

        public void LoadConf()
        {
            StartCoroutine(_LoadConfList());
        }

        public string[] GetAllObjNames()
        {
            if (isConfReady)
            {
                string[] allNames = new string[_confData.abEntries.Count];
                int i = 0;
                foreach (string key in _confData.abEntries.Keys)
                {
                    allNames[i] = key;
                    i++;
                }
                return allNames;
            }

            return null;
        }

        public ABLoadResult<T> Load<T>(string objName) where T : UnityEngine.Object
        {
            ABLoadResult<T> loadResult = new ABLoadResult<T>(objName);
            _downloadQueue.Enqueue(loadResult);
            return loadResult;
        }

        public IEnumerator LoadCoroutine<T>(string objName, Action<T> callback) where T : UnityEngine.Object
        {
            yield return _LoadCoroutine(objName, callback);
        }

        private IEnumerator _LoadCoroutine<T>(string objName, Action<T> callback) where T : UnityEngine.Object
        {
            while (!isSettingReady) { yield return null; }
            while (!Caching.ready) { yield return null; }
            while (_isWorking) { yield return null; }
            _isWorking = true;

            if (_confData == null)
            {
                yield return _LoadConfList();
            }
            ConfData.ABEntry abEntry = null;
            try
            {
                abEntry = _confData.abEntries[objName];
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                callback(null);
            }

            if (abEntry != null)
            {
                //WWW www = WWW.LoadFromCacheOrDownload(_setting.AssetBundleUrl + currentPlatform + abEntry.url, abEntry.GetHash128());
                WWW www = new WWW(_setting.AssetBundleUrl + currentPlatform + abEntry.url);
                yield return www;


                if (www.isDone) {
                    byte[] bt = www.bytes;

                    byte[] dc = Crypt.Decrypt(Crypt.MD5Calc(abEntry.url.Substring(0, abEntry.url.IndexOf("."))), bt);


                    AssetBundle ac = AssetBundle.LoadFromMemory(dc);

                    yield return ac;

                    string[] objPathSplit = objName.Split('/');

                    AssetBundleRequest request = ac.LoadAssetAsync<T>(objPathSplit[objPathSplit.Length - 1]);

                    callback(request.asset as T);
                    ac.Unload(false);
                }

                /*if (ab != null)
                {
                    string[] objPathSplit = objName.Split('/');
					AssetBundleRequest request = ab.LoadAssetAsync<T>(objPathSplit[objPathSplit.Length - 1]);
                    yield return request;
                    callback(request.asset as T);
                    ab.Unload(false);
                }
                else
                {
                    callback(null);
                }*/
            }

            _isWorking = false;
        }

        private IEnumerator _LoadConfList()
        {
            while (!isSettingReady) { yield return null; }
            WWW www = new WWW(_setting.AssetBundleUrl + currentPlatform + "conf.json");
            yield return www;
            try
            {
                _confData = LitJson.JsonMapper.ToObject<ConfData>(www.text);

                Debug.LogWarning(_confData.abEntries.Count);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                if (OnConfDownloadFail != null)
                {
                    OnConfDownloadFail();
                };
            }

            if(_currentWorkingAB != null)
            {
                // resume AB loading
                Retry(_currentWorkingAB);
            }
        }

        public void DownloadFileGameObje()
        {
            StartCoroutine(DownloadFilesGameObj());
        }

        public IEnumerator DownloadFilesGameObj()
        {
            if (gameObjectList.Count <= 0)
            {
                Debug.LogError("START");

                if (_confData == null)
                {
                    yield return _LoadConfList();
                }

                foreach (string ea in _confData.abEntries.Keys)
                {
                    if (!ea.Contains("Maps"))
                    {
                        ConfData.ABEntry conf = _confData.abEntries[ea];
                        WWW www = new WWW(_setting.AssetBundleUrl + currentPlatform + conf.url);

                        yield return www;

                        if (www.isDone)
                        {
                            byte[] bc = www.bytes;
                            byte[] dc = Crypt.Decrypt(Crypt.MD5Calc(conf.url.Substring(0, conf.url.IndexOf("."))), bc);

                            AssetBundle ac = AssetBundle.LoadFromMemory(dc);

                            yield return ac;


                            string[] objPathSplit = ea.Split('/');


                            AssetBundleRequest req = ac.LoadAssetAsync<GameObject>(objPathSplit[objPathSplit.Length - 1]);


                            gameObjectList.TryAdd(ea, req.asset as GameObject);

                            ac.Unload(false);
                        }


                        Debug.LogError("BAIXOU");

                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }

        public IEnumerator DownloadMap(string name)
        {
            if (!gameMapsList.ContainsKey(name))
            {
                if (_confData == null)
                {
                    yield return _LoadConfList();
                }

                ConfData.ABEntry conf = _confData.abEntries["Maps/" + name];

                WWW www = new WWW(_setting.AssetBundleUrl + currentPlatform + conf.url);

                yield return www;

                if (www.isDone)
                {
                    byte[] bc = www.bytes;
                    byte[] bt = Crypt.Decrypt(Crypt.MD5Calc(conf.url.Substring(0, conf.url.IndexOf("."))), bc);

                    AssetBundle ac = AssetBundle.LoadFromMemory(bt);

                    yield return ac;

                    AssetBundleRequest request = ac.LoadAssetAsync<GameObject>(name);

                    yield return request;

                    gameMapsList.TryAdd(name, request.asset as GameObject);

                    ac.Unload(false);
                }
            }
        }

        IABLoadResult _currentWorkingAB = null;
        void Update()
        {
            if ((_currentWorkingAB == null || _currentWorkingAB.isSuccess) && _downloadQueue.Count > 0)
            {
                _currentWorkingAB = _downloadQueue.Dequeue();
                StartCoroutine(_currentWorkingAB.StartDownload());
            }
        }

        public void OnFail(IABLoadResult result)
        {
            if (OnABDownloadFail != null)
            {
                OnABDownloadFail(result);
            }
        }

        public void Retry(IABLoadResult result)
        {
            _downloadQueue.Enqueue(result);
            Proceed();
        }

        public void Proceed()
        {
            _currentWorkingAB = null;
        }

        public static void ClearAllCachedBundles()
        {
            Caching.ClearCache();
        }
    }

    public class ConfData
    {
        public class ABEntry
        {
            public string url;
            public string hash;

            public ABEntry() { }
            public ABEntry(string u, string h) { url = u; hash = h; }

            public Hash128 GetHash128() { return Hash128.Parse(hash); }
        }
        public Dictionary<string, ABEntry> abEntries = new Dictionary<string, ABEntry>();
    }

    public interface IABLoadResult
    {
        Type type { get; }
        string objName { get; }
        bool isDone { get; }
        bool isSuccess { get; }
        void AssignData(UnityEngine.Object d);

        IEnumerator StartDownload();
    }

    public class ABLoadResult<T> : IABLoadResult where T : UnityEngine.Object
    {
        private string _objName;
        private T _data;
        private bool _isDone = false;

        public Type type { get { return typeof(T); } }
        public string objName { get { return _objName; } }
        public T data { get { return _data; } }
        public bool isDone { get { return _isDone; } }
        public bool isSuccess { get { return _isDone && _data != null; } }

        public ABLoadResult(string o)
        {
            _objName = o;
        }

        public void AssignData(UnityEngine.Object d)
        {
            AssignData(d as T);
        }

        public void AssignData(T d)
        {
            _data = d;
            _isDone = true;

            if(_data == null)
            {
                ABManager.GetInstance().OnFail(this);
            }
        }

        public IEnumerator StartDownload()
        {
            yield return ABManager.GetInstance().LoadCoroutine<T>(_objName, AssignData);
        }
    }
}