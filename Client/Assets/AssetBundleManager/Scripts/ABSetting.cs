using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssetBundleManager
{
    [Serializable]
    public class ABSetting : ScriptableObject
    {
        public string AssetBundleUrl;

        private Dictionary<string, FolderInfo> _folderInfos = new Dictionary<string, FolderInfo>();

        public void ImportFolderData(FolderInfo[] folderInfo)
        {
            _folderInfos.Clear();
            for (int i = 0; i < folderInfo.Length; i++)
            {
                _folderInfos.Add(folderInfo[i].folderAssetPath, folderInfo[i]);
            }
        }

        public FolderInfo FindFolderInfo(string assetPath)
        {
            FolderInfo info = null;
            if (_folderInfos.TryGetValue(assetPath, out info))
            {
                return info;
            }
            return null;
        }
    }

    public class FolderInfo
    {
        public enum AssetsType
        {
            Prefab,
            All,
            None,
            Count
        }
        public static string[] GetAssetsTypeOptions()
        {
            string[] types = new string[(int)AssetsType.Count];
            for (int i = 0; i < types.Length; i++) { types[i] = ((AssetsType)i).ToString(); }
            return types;
        }
        
        public string folderAssetPath;
        public string folderName;
        public AssetsType assetsType = AssetsType.None;

        public FolderInfo(string path)
        {
            folderAssetPath = GetAssetPath(path);
            path = path.Replace('\\', '/');
            string[] splitedPath = path.Split('/');
            folderName = splitedPath[splitedPath.Length - 1];
        }

        public static string GetAssetPath(string path)
        {
            path = path.Replace('\\', '/');
            return path.Replace(Application.dataPath, "Assets");
        }
    }
}
