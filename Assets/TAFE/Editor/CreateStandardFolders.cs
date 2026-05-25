using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hoey.Scripts;
using System.Linq;


namespace Hoey.Scripts
{
    public struct FolderToCreateDetails
    {
        public string folderName;
        public bool create;

        public FolderToCreateDetails(string _folderName)
        {
            folderName = _folderName;
            create = false;
        }
    }
    /// <summary>
    /// Author: Mark Hoey
    /// Description: This script demonstrates how to create the standard folders for a clean folder strucuture in Unity
    /// https://answers.unity.com/questions/472808/how-to-get-the-current-selected-folder-of-project.html
    /// </summary>
    public class CreateStandardFolders : UnityEditor.EditorWindow
    {
        bool showCreationButton = true;
        static List<FolderToCreateDetails> createFolders = new List<FolderToCreateDetails>();
        string extraName = "";

        private bool showExtraFoldersSection = false;

        [MenuItem("TAFE/Make Common Sub-Folders", false, 300)]
        static void InitialiseAndShowWindow()
        {

            createFolders = new List<FolderToCreateDetails>()
            {
                new FolderToCreateDetails("Scenes"),
                new FolderToCreateDetails("Prefabs"),
                new FolderToCreateDetails("Textures"),
                new FolderToCreateDetails("Materials"),
                new FolderToCreateDetails("Scripts"),
                new FolderToCreateDetails("FreeAssetStore"),
                new FolderToCreateDetails("Fonts"),
                new FolderToCreateDetails("Models"),

                new FolderToCreateDetails("Sprites"),
                new FolderToCreateDetails("Animation"),
                new FolderToCreateDetails("Shaders"),
                new FolderToCreateDetails("Audio"),
                new FolderToCreateDetails("Video"),
                new FolderToCreateDetails("Plugins")

            };




            CreateStandardFolders window = (CreateStandardFolders)EditorWindow.GetWindow(typeof(CreateStandardFolders), true, "Create Standard Sub-Folders");

            window.minSize = new Vector2(400, 550);
            window.maxSize = new Vector2(400, 550);

            window.position = new Rect(100, 100, 400, 550); // Set the window position (x, y) and size (width, height)
            window.Show();
        }

        [MenuItem("Assets/Script Template", false, 100)]
        private static void ShowWindowFromContext()
        {
            InitialiseAndShowWindow();
        }

        [MenuItem("Assets/Script Template", true)]
        private static bool ShowWindowFromContextValidation()
        {
            //return Selection.activeObject != null;

            foreach (Object obj in Selection.objects)
            {
                if (!(obj is DefaultAsset))
                {
                    return false; // Not all selected items are folders
                }
            }
            return Selection.objects.Length > 0; // At least one folder is selected
        }

     

        void OnGUI()
        {
            showCreationButton = false;

            GUILayout.Space(5);

            GUILayout.Label("Tick the box next to the folder(s) you want.");
            GUILayout.Label("Then press the 'Create Sub-Folders' button that appears below.");
            GUILayout.Space(5);


            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 250;
            for (int i = 0; i < createFolders.Count; i++)
            {
                //createFolders[i].create = EditorGUILayout.Toggle("Create folder named " + createFolders[i].folderName, createFolders[i].create);

                var folderDetails = createFolders[i];
                folderDetails.create = EditorGUILayout.Toggle($"Create sub-folder named '{folderDetails.folderName}'", folderDetails.create);
                createFolders[i] = folderDetails;
            }
            EditorGUIUtility.labelWidth = originalValue;

           CommonUtilitiesTAFE.HorizontalBarWithPadding();

             showExtraFoldersSection = EditorGUILayout.BeginFoldoutHeaderGroup(showExtraFoldersSection, "Need more custom folders?");

            if (showExtraFoldersSection)
            {
                GUILayout.Label("Add the folder name and press the button to add to the list");

                extraName = EditorGUILayout.TextField("Extra sub-folder NAME", extraName);

                if (GUILayout.Button("Add Sub-Folder Name To Create"))
                {
                    if (!string.IsNullOrEmpty(extraName))
                    {
                        createFolders.Add(new FolderToCreateDetails(extraName));
                        extraName = ""; 
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            CommonUtilitiesTAFE.HorizontalBarWithPadding();

            foreach (var item in createFolders)
            {
                if (item.create == true)
                {
                    showCreationButton = true;
                    break;
                }
            }


            Color originalColor = GUI.backgroundColor;
            if (showCreationButton)
            {
                
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Create Sub-Folders (in folder/s selected) and Close", GUILayout.Height(40)))
                {
                    List<string> foldersSelected = CommonUtilitiesTAFE.GetAllSelectedFolders();

                    foreach (var path in foldersSelected)
                    {
                        foreach (var newFolder in createFolders)
                        {
                            if (newFolder.create == true)
                            {
                                CommonUtilitiesTAFE.CreateFolderWithName(path, newFolder.folderName);
                            }
                        }
                    }

                    this.Close();
                }
            }

           
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Close", GUILayout.Height(40)))
            {
                this.Close();
            }

            GUI.backgroundColor = originalColor;
        }



        #region Custom Public Methods 
        // Add a new menu item that is accessed by Top Menu items in the project view
        //[MenuItem("GameObject/Create Generic Sub Folders")]
        // Add a new menu item that is accessed by right-clicking on an asset in the project view

        //[MenuItem("Assets/Create Generic Sub Folders", false, 2000)]
        //static void CreateGenericSubFolders()
        //{
        //
        //    CreateFolderWithName("Scenes");
        //    CreateFolderWithName("Prefabs");
        //    CreateFolderWithName("Textures");
        //    CreateFolderWithName("Materials");
        //    CreateFolderWithName("Scripts");
        //    CreateFolderWithName("FreeAssetStore");
        //
        //FileUtil.DeleteFileOrDirectory("Assets/TAFE/Tutorial Info/Scripts/EditorMakeCommonFolders.cs");
        //FileUtil.DeleteFileOrDirectory("Assets/TAFE/Tutorial Info/Scripts/EditorMakeCommonFolders.cs.meta");
        //AssetDatabase.Refresh();
        //}


        //[MenuItem("Assets/Create More Generic Sub Folders", false, 2000)]
        //static void CreateMoreGenericSubFolders()
        //{
        //    CreateFolderWithName("Fonts");
        //    CreateFolderWithName("Models");
        //}   
        #endregion
    }

}