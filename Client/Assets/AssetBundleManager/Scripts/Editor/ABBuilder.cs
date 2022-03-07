#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using AssetBundleManager;

namespace AssetBundleManager.Editor
{
    public class ABBuilder : EditorWindow
    {
        private static ABSetting _settings
        {
            get { if (__settings == null) { __settings = _GetSettingFile(); } return __settings; }
            set { __settings = value; }
        }
        #region Static Funcs
        private static ABSetting __settings = null;

        [MenuItem("AssetBundleManager/Settings")]
        public static void SelectSettingFile()
        {
            Selection.activeObject = _settings;
        }

        [MenuItem("AssetBundleManager/ClearCache")]
        public static void ClearCache()
        {
            Caching.ClearCache();
        }

        [MenuItem("AssetBundleManager/Builder")]
        public static void ShowBuilder()
        {
            _GetSettingFile();
            EditorWindow.GetWindow<ABBuilder>().OnWindowOpen();
        }

        private static ABSetting _GetSettingFile()
        {
            // create setting file if not found
            string _settingPath = "Assets/AssetBundleManager/Resources/" + ABManager.settingPath + ".asset";
            UnityEngine.Object settingObj = AssetDatabase.LoadAssetAtPath(_settingPath, typeof(ABSetting));
            if (settingObj == null)
            {
                ABSetting setting = ScriptableObject.CreateInstance<ABSetting>();
                AssetDatabase.CreateAsset(setting, _settingPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                settingObj = AssetDatabase.LoadAssetAtPath(_settingPath, typeof(ABSetting));
            }
            return settingObj as ABSetting;
        }
        #endregion

        private List<FolderInfo> _folderList = new List<FolderInfo>();
        private Vector2 _folderScrollPos;
        private List<PrebuildBundleInfo> _infoList = new List<PrebuildBundleInfo>();
        private Vector2 _scrollPos;

        private static string _inputFolderPath = "Assets/AssetBundleManager/Assets"; public static string inputFolderPath { get { return _inputFolderPath; } }
        private static string _outputFolderPath = "Assets/AssetBundleManager/Bundles"; public static string outputFolderPath { get { return _outputFolderPath; } }

        public BuildTarget currPlatform { get { return EditorUserBuildSettings.activeBuildTarget; } }
        public string currPlatformStr
        {
            get {
                if (currPlatform.ToString().StartsWith("Standalone")) { return "Standalone"; }
                return currPlatform.ToString();
            }
        }

        public void OnWindowOpen()
        {
            _ScanFolder();
        }

        private void _ScanFolder()
        {
            _folderList.Clear();
            DirectoryInfo inputFolderInfo = new DirectoryInfo(_inputFolderPath);
            if(!inputFolderInfo.Exists)
            {
                inputFolderInfo = Directory.CreateDirectory(_inputFolderPath);
                AssetDatabase.Refresh();
            }
            DirectoryInfo[] fileInfo = inputFolderInfo.GetDirectories();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                string assetPath = FolderInfo.GetAssetPath(fileInfo[i].FullName);
                FolderInfo info = _settings.FindFolderInfo(assetPath);
                if (info == null)
                {
                    info = new FolderInfo(fileInfo[i].FullName);
                }
                _folderList.Add(info);
            }
            _FolderInfosUpdate();
        }

        void OnGUI()
        {
#region folder list
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);
            GUILayout.BeginVertical();

            _folderScrollPos = EditorGUILayout.BeginScrollView(_folderScrollPos, GUILayout.Width(position.width), GUILayout.Height(EditorGUIUtility.singleLineHeight * 10f));

            GUILayout.Space(-1f);
            GUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(20f));
            GUI.backgroundColor = Color.white;

            GUILayout.Label("", GUILayout.Width(20f));

            GUILayout.Label("Folder Name", GUILayout.Width(200f));
            GUILayout.Label("Folder Path", GUILayout.Width(500f));
            GUILayout.Label("Asset Type", GUILayout.Width(200f));

            GUILayout.EndHorizontal();

            for (int i = 0; i < _folderList.Count; ++i)
            {
                GUILayout.Space(-1f);
                GUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(20f));
                GUI.backgroundColor = Color.white;

                ///////////////////////////
                GUILayout.Label((i + 1).ToString(), GUILayout.Width(20f));

                GUILayout.Label(_folderList[i].folderName, GUILayout.Width(200f));
                GUILayout.Label(_folderList[i].folderAssetPath, GUILayout.Width(500f));
                FolderInfo.AssetsType t = (FolderInfo.AssetsType)EditorGUILayout.Popup((int)_folderList[i].assetsType, FolderInfo.GetAssetsTypeOptions(), GUILayout.Width(200f));
                if(t != _folderList[i].assetsType)
                {
                    _folderList[i].assetsType = t;
                    _FolderInfosUpdate();
                }
                ///////////////////////////
                GUILayout.EndHorizontal();
            }
            //GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
#endregion

            if (GUILayout.Button("Scan Assets"))
            {
                _ScanAssets();
            }

            if (_infoList.Count == 0)
            {
                return;
            }

#region assets list
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);
            GUILayout.BeginVertical();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - (EditorGUIUtility.singleLineHeight * 23f)));

            GUILayout.Space(-1f);
            GUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(20f));
            GUI.backgroundColor = Color.white;

            GUILayout.Label("", GUILayout.Width(20f));

            GUILayout.Label("File Path", GUILayout.Width(500f));
            GUILayout.Label("Folder", GUILayout.Width(100f));
            GUILayout.Label("MD5 Name", GUILayout.Width(250f));

            GUILayout.EndHorizontal();

            for (int i = 0; i < _infoList.Count; ++i)
            {
                GUILayout.Space(-1f);
                GUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(20f));
                GUI.backgroundColor = Color.white;
                
                GUILayout.Label((i + 1).ToString(), GUILayout.Width(20f));

                GUILayout.Label(_infoList[i].fileRelPath, GUILayout.Width(500f));
                GUILayout.Label(_infoList[i].folderName, GUILayout.Width(100f));
                GUILayout.Label(_infoList[i].md5FileName, GUILayout.Width(250f));

                GUILayout.EndHorizontal();
            }
            //GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
#endregion
            
            if (GUILayout.Button("Build " + currPlatformStr))
            {
                EditorApplication.isPlaying = false;
                string outputPath = _outputFolderPath + "/" + currPlatformStr;

                // create directory if not exists
                if(!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                // clear target location files
                string[] existingFiles = Directory.GetFiles(outputPath, "*.*");
                for (int i = 0; i < existingFiles.Length; i++)
                {
                    FileUtil.DeleteFileOrDirectory(existingFiles[i]);
				}

				AssetDatabase.Refresh();

                ConfData confData = new ConfData();
                BuildBundle(_infoList, outputPath);
                for (int i = 0; i < _infoList.Count; i++)
                {
                    confData.abEntries.Add(_infoList[i].fileRelPathNoExt, 
                        new ConfData.ABEntry(
                            _infoList[i].md5FileName.ToLower() + ".unity3d",
                            _infoList[i].hash
                            ));
                }
                FileStream fs = File.Create(outputPath + "/conf.json");
                byte[] confDataByte = Encoding.UTF8.GetBytes(LitJson.JsonMapper.ToJson(confData));
                fs.Write(confDataByte, 0, confDataByte.Length);
                fs.Flush();
                fs.Close();

                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog(
                    "Done", 
                    "AssetBundle for " + currPlatformStr + " build complete! " + 
                        (string.IsNullOrEmpty(_settings.AssetBundleUrl) ? 
                            "Please go to AssetBundleManager -> Settings to set up url where the bundles will be served." : 
                            "Please upload bundles to " + _settings.AssetBundleUrl), 
                    "OK");

                _OpenExplorer(outputPath);
            }

            if (GUILayout.Button("Unset AssetsBundles"))
            {
                if (EditorUtility.DisplayDialog("Unset all AssetsBundles", "This will delete all built AssetsBundles and unset all entries, sure?", "Yes", "No"))
                {
                    List<string> existingFiles = new List<string>();
                    existingFiles.AddRange(Directory.GetFiles(_outputFolderPath + "/", "*.*"));
                    for (int i = 0; i < existingFiles.Count; i++)
                    {
                        Debug.Log(existingFiles[i]);
                        FileUtil.DeleteFileOrDirectory(existingFiles[i]);
                    }
                    string[] abNames = AssetDatabase.GetAllAssetBundleNames();
                    for (int i = 0; i < abNames.Length; i++)
                    {
                        AssetDatabase.RemoveAssetBundleName(abNames[i], true);
                    }

                    EditorUtility.DisplayDialog("Done", "Unset all AssetsBundles done", "OK");
                }
            }
        }

        private void _FolderInfosUpdate()
        {
            _settings.ImportFolderData(_folderList.ToArray());
        }

        private void _ScanAssets()
        {
            string[] paths = AssetDatabase.GetAllAssetPaths();
            _infoList = new List<PrebuildBundleInfo>();
            for (int i = 0; i < paths.Length; i++)
            {
                for (int j = 0; j < _folderList.Count; j++)
                {
                    if (paths[i].Contains(_folderList[j].folderAssetPath))
                    {
                        bool isValidAsset = false;

                        if(paths[i].LastIndexOf(".") < 0)
                        {
                            // no file extension name, consider as folder, ignore
                            continue;
                        }

                        switch (_folderList[j].assetsType)
                        {
                            case FolderInfo.AssetsType.Prefab:
                                if (paths[i].EndsWith(".prefab")) { isValidAsset = true; }
                                break;
                            case FolderInfo.AssetsType.All:
                                isValidAsset = true;
                                break;
                        }

                        if (isValidAsset)
                        {
                            PrebuildBundleInfo infoItem = new PrebuildBundleInfo(paths[i], _folderList[j].folderName);
                            _infoList.Add(infoItem);
                        }
                        break;
                    }
                }
            }
        }

        public void BuildBundle(List<PrebuildBundleInfo> info, string outputPath)
        {
            BuildTarget target = currPlatform;

			for (int i = 0; i < info.Count; i++) 
			{
				UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(info[i].filePath);
				AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
				importer.assetBundleName = info[i].md5FileName + ".unity3d";

				AssetBundleBuild b = new AssetBundleBuild();
				b.assetBundleName = info[i].md5FileName + ".unity3d";
                List<string> assetNames = new List<string>();
                assetNames.Add(AssetDatabase.GetAssetPath(asset));
                b.assetNames = assetNames.ToArray();

				AssetBundleBuild[] build = new AssetBundleBuild[]{ b };

				AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outputPath, build, BuildAssetBundleOptions.None, target);

				AssetDatabase.Refresh();

				if (AssetDatabase.FindAssets(info[i].md5FileName, new string[] { outputPath }).Length > 0)
				{
                    info[i].hash = manifest.GetAssetBundleHash(manifest.GetAllAssetBundles()[0]).ToString();
					info[i].size = File.ReadAllBytes(outputPath + "/" + info[i].md5FileName + ".unity3d").Length;
				}
			}
        }

        private void _OpenExplorer(string path)
        {
            if (UnityEngine.SystemInfo.operatingSystem.IndexOf("Windows") != 1)
            {
                path = path.Replace("/", "\\");
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    // tried to open win explorer in mac
                    // just silently skip error
                    // we currently have no platform define for the current OS we are in, so we resort to this
                    e.HelpLink = ""; // do anything with this variable to silence warning about not using it
                }
            }
            else if (UnityEngine.SystemInfo.operatingSystem.IndexOf("Mac OS") != 1)
            {
                try
                {
                    string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes
                    if (!macPath.StartsWith("\""))
                    {
                        macPath = "\"" + macPath;
                    }
                    if (!macPath.EndsWith("\""))
                    {
                        macPath = macPath + "\"";
                    }
                    string arguments = "-R " + macPath;
                    System.Diagnostics.Process.Start("open", arguments);
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    // tried to open mac finder in windows
                    // just silently skip error
                    // we currently have no platform define for the current OS we are in, so we resort to this
                    e.HelpLink = ""; // do anything with this variable to silence warning about not using it
                }
            }
        }
    }

    public class PrebuildBundleInfo
    {
        public string filePath;
        public string fileRelPath;
        public string folderName;

        public string md5FileName;
        public string hash;
        public int size;
        
        public string fileRelPathNoExt { get { int fileExtPos = fileRelPath.LastIndexOf("."); if (fileExtPos >= 0) { return fileRelPath.Substring(0, fileExtPos); } return fileRelPath; } }

        public PrebuildBundleInfo(string path, string folder)
        {
            filePath = path;
            fileRelPath = path.Replace(ABBuilder.inputFolderPath + "/", "");
            folderName = folder;
            md5FileName = MD5Calc(fileRelPath);
        }

        public static string MD5Calc(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
#endif