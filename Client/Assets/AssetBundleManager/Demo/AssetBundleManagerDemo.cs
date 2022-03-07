using UnityEngine;
using System.Collections;

using AssetBundleManager;
using System.Linq;

namespace AssetBundleManager.Demo
{
    public class AssetBundleManagerDemo : MonoBehaviour
    {
        // prevent duplicate call
        bool isWorking = false;

        string _noticeText = "";
        
        private GameObject _gameObjectToBeLoad = null;
        private GameObject _instantiate;

        // Use this for initialization
        void Start()
        {
            ABManager.OnConfDownloadFail += ABManager_OnConfDownloadFail;
            ABManager.OnABDownloadFail += ABManager_OnABDownloadFail;
        }

        // Called when config file fail to download
        private void ABManager_OnConfDownloadFail()
        {
            _noticeText += "Config file download fail!\n";
            // In production, we should notice player network is not stable, and wait for player response then retry
            // by calling AssetBundleManager.ABManager.GetInstance().LoadConf();
            // Now let just ignore and proceed to next asset bundle
            ABManager.GetInstance().Proceed();
        }

        // Called when asset bundle fail to download
        private void ABManager_OnABDownloadFail(IABLoadResult result)
        {
            _noticeText += "Asset bundle " + result.objName + " download fail!\n";
            // In production, we should notice player network is not stable, and wait for player response then retry
            // by calling AAssetBundleManager.ABManager.GetInstance().Retry(result);
            // Now let just ignore and proceed to next asset bundle
            ABManager.GetInstance().Proceed();
        }

        void OnGUI()
        {
            GUI.Label(_GetSize(1f, 0.05f), "AssetBundleManager - Simple asset bundle handler");

            if (GUI.Button(_GetSize(0.5f, 0.025f, 0.15f, 0.25f), isWorking ? "Working..." : "Load"))
            {
                if (!isWorking)
                {
                    // start loading!
                    // as download takes time, use a coroutine

                    //StartCoroutine(_DemoLoad());
                    StartCoroutine(_DemoTest());
                }
            }

            GUI.Label(_GetSize(1f, 0.2f, 0.4f), _noticeText);
        }

        private Rect _GetSize(float w, float h, float top = 0f, float left = 0f)
        {
            return new Rect(Screen.width * left, Screen.height * top, Screen.width * w, Screen.height * h);
        }

        private IEnumerator _DemoTest()
        {
            yield return StartCoroutine(ABManager.instance.DownloadFilesGameObj());
        }

        private IEnumerator _DemoLoad()
        {
            _noticeText = "";
            isWorking = true;

            // if previously loaded the asset on scene, destroy it first
            if (_instantiate != null) { Destroy(_instantiate); _instantiate = null; }

            // start the loading
            ABLoadResult<GameObject> result = ABManager.GetInstance().Load<GameObject>("Demo/Cube");

            // wait until the asset is loaded
            while(!result.isDone)
            {
                yield return 0;
            }

            // here we got the asset!
            _gameObjectToBeLoad = result.data;

            if (!result.isSuccess)
            {
                _noticeText += "Download Fail!\n";
            }
            else
            {
                // lets instantiate the asset
                _instantiate = Instantiate<GameObject>(_gameObjectToBeLoad);
            }

            isWorking = false;
        }
    }
}