using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoey.Scripts
{
    public static class CommonUtilitiesTAFE
    {
        /// <summary>
        /// Used in checking Class and Namespace validity
        /// </summary>
        /// <param name="c"></param>
        /// <param name="firstChar"></param>
        /// <returns></returns>
        //https://stackoverflow.com/questions/950616/what-characters-are-allowed-in-c-sharp-class-name/
        //https://stackoverflow.com/a/60820647
        public static bool IsValidInIdentifier(this char c, bool firstChar = true)
        {
            switch (char.GetUnicodeCategory(c))
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                    // Always allowed in C# identifiers
                    return true;

                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.SpacingCombiningMark:
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.Format:
                    // Only allowed after first char
                    return !firstChar;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Used to show the Width and Height of an EditorWindow (at the bottom)
        /// Used mostly when creating and testing
        /// </summary>
        /// <param name="windowToShowStatsFor"></param>
        public static void ShowWidthAndHeightOfWindowAtBottom(EditorWindow windowToShowStatsFor)
        {
            float windowWidth;
            float windowHeight;

            windowWidth = windowToShowStatsFor.position.width;
            windowHeight = windowToShowStatsFor.position.height;
            
            // Push the label to the bottom
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField("Width: " + windowWidth.ToString("F2") + ", Height: " + windowHeight.ToString("F2"));
        }

        /// <summary>
        /// Creates a simple horizontal bar to separate items in an EditorWindonw (with padding top and bottom)
        /// </summary>
        /// <param name="lineThickness"></param>
        /// <param name="topPadding"></param>
        /// <param name="bottomPadding"></param>
        public static void HorizontalBarWithPadding(int lineThickness = 3, int topPadding = 10, int bottomPadding = 10)
        {
            GUILayout.Space(topPadding);

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(lineThickness));

            GUILayout.Space(bottomPadding);
        }

        /// <summary>
        /// Create a folder in a Unity project with a particular name, in the root 'Assets' folder
        /// </summary>
        /// <param name="folderNameToCreate"></param>
        public static void CreateFolderWithName(string folderNameToCreate)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }

            if (!AssetDatabase.IsValidFolder(path + "/" + folderNameToCreate))
            {
                string guid = AssetDatabase.CreateFolder(path, folderNameToCreate);
                string newFolderPath = AssetDatabase.GUIDToAssetPath(path);
            }
        }

        /// <summary>
        /// Create a folder in a Unity project with a particular name, with the relative parent folder supplied. e.g. Assets/Level 1
        /// </summary>
        /// <param name="relativeParentFolderPath"></param>
        /// <param name="folderNameToCreate"></param>
        public static void CreateFolderWithName(string relativeParentFolderPath, string folderNameToCreate)
        {
            if (!AssetDatabase.IsValidFolder(relativeParentFolderPath + "/" + folderNameToCreate))
            {
                string guid = AssetDatabase.CreateFolder(relativeParentFolderPath, folderNameToCreate);
                string newFolderPath = AssetDatabase.GUIDToAssetPath(relativeParentFolderPath);
            }
        }

        /// <summary>
        /// Attempting to better select items for making UnityPackages - incomplete
        /// </summary>
        [MenuItem("Assets/Folder AssetInfo", false, 2001)]
        public static void CreateAssetBundles()
        {
            UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            foreach (UnityEngine.Object obj in selectedAsset)
            {
                Debug.Log("Asset name: " + obj.name + "   Type: " + obj.GetType());
            }
        }

        /// <summary>
        /// Get all the folders currently selected in the Project Window
        /// </summary>
        /// <returns>A list of all folder names</returns>
        public static List<string> GetAllSelectedFolders()
        {
            List<string> folders = new List<string>();


            foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                if (System.IO.Directory.Exists(path))
                {
                    folders.Add(path);
                }
            }

            if (folders.Count == 0)
            {
                folders.Add("Assets");
            }
            return folders;
        }

        /// <summary>
        /// Retrieves selected folder in Project Window.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }


        /// <summary>
        /// https://gist.github.com/allanolivei/9260107
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAssetSelectedDirectory()
        {
            foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }


                if (System.IO.Directory.Exists(path))
                {
                    return path;
                }
                else if (System.IO.File.Exists(path))
                {
                    return System.IO.Path.GetDirectoryName(path);
                }
            }

            return "Assets";
        }

        /// <summary>
        /// Breaks a string up based on a supplied character/delimeter
        /// </summary>
        /// <param name="textSupplied"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string[] SplitStringOnDelemiter(string textSupplied, string delimeter = ";")
        {
            return textSupplied.Split(delimeter);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/755166/exclude-certain-file-extensions-when-getting-files-from-a-directory
        /// </summary>
        //uses something like....  var exts = new[] { ".mp3", ".jpg" };
        public static IEnumerable<string> FilterFiles(string folderPath, params string[] extensions)
        {
            return
                Directory
                .GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => !extensions.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFolderPanel.html
        /// </summary>
        //[MenuItem("TAFE/Load Textures To Folder")]
        //static void Apply()
        //{
        //    string path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
        //    string[] files = Directory.GetFiles(path);

        //    foreach (string file in files)
        //    {
        //        if (file.EndsWith(".png"))
        //        {
        //            File.Copy(file, EditorSceneManager.GetActiveScene().name);
        //        }
        //    }

        //}
    }
}
