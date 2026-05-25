using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hoey.Scripts
{
    /// <summary>
    /// Author: Mark Hoey
    /// Description: This script demonstrates how to create a Script Template automatically in the required 'magic' folder in Unity
    /// </summary>
    public class AutoCreateScriptTemplate : EditorWindow
    {
        private string firstName = "MyFirstName";
        private string lastName = "MyLastName";
        private string customNamespace = "";

        private bool showExtraOptionsSection = false;

        private int orderNumber = 61;
        private string parentMenuName = "";
        private string defaultScriptName = "DefaultScriptName";
        private string customMenuName = "";

        string effectiveFirstName = "";
        string effectiveLastName = "";
        string effectiveNamespace = "";


        [MenuItem("TAFE/Make Custom Script Template", false, 200)]
        public static void ShowWindow()
        {
            GetWindow<AutoCreateScriptTemplate>("Script Template Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create Script Template", EditorStyles.boldLabel);

            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 160;


            firstName = EditorGUILayout.TextField("First Name", firstName);
            lastName = EditorGUILayout.TextField("Last Name", lastName);

            if (GUILayout.Button("Create Script Template"))
            {
                CreateScriptTemplate();
            }

            CommonUtilitiesTAFE.HorizontalBarWithPadding();

            showExtraOptionsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showExtraOptionsSection, "Need more custom settings");

            if (showExtraOptionsSection)
            {
                GUILayout.Label("Advanced custimized to override with:");

                orderNumber = EditorGUILayout.IntField("Sort Order Number:", orderNumber);
                parentMenuName = EditorGUILayout.TextField("Inside Parent Menu Named:", parentMenuName);
                customMenuName = EditorGUILayout.TextField("Custom Menu Item Name:", customMenuName);
                defaultScriptName = EditorGUILayout.TextField("Default Script Name:", defaultScriptName);
                customNamespace = EditorGUILayout.TextField("Custom Namespace", customNamespace);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            CommonUtilitiesTAFE.HorizontalBarWithPadding();

            AssignValues();

            string fileName = SetFileName();


            GUILayout.Label("Script Template Name to be made:", EditorStyles.boldLabel);

            // Calculate the required width of the label
            float labelWidth = GUI.skin.label.CalcSize(new GUIContent(fileName)).x;

            // Ensure the window width is at least as wide as the label
            float minWidth = Mathf.Max(450, labelWidth + 20f); // Adding some padding

            // Set the minimum window size
            minSize = new Vector2(minWidth, EditorGUIUtility.singleLineHeight * 19);
            maxSize = new Vector2(minWidth, EditorGUIUtility.singleLineHeight * 19);

            // Display the dynamically sized label
            GUILayout.Label($"{fileName}", GUILayout.Width(labelWidth));
            GUILayout.Label($"{effectiveNamespace}", GUILayout.Width(labelWidth));

           // CommonUtilitiesTAFE.ShowWidthAndHeightOfWindowAtBottom(this);

           
        }

        void AssignValues()
        {
            effectiveFirstName = string.IsNullOrEmpty(firstName) ? "MyFirstName" : firstName;
            effectiveLastName = string.IsNullOrEmpty(lastName) ? "MyLastName" : lastName;
            effectiveNamespace =  (string.IsNullOrEmpty(customNamespace) ? effectiveFirstName + effectiveLastName : customNamespace).Replace(" ","");
        }

  


        private void CreateScriptTemplate()
        {
            AssignValues();

            string scriptTemplate = $@"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using {effectiveNamespace};

namespace {effectiveNamespace}
{{
    /// <summary>
    /// Author: {effectiveFirstName} {effectiveLastName}
    /// Description: This script demonstrates how to ... in Unity
    /// </summary>
    public class #SCRIPTNAME# : MonoBehaviour 
    {{
        //Class Variables

        //Unity Methods

        //Custom Public Methods 
        
        //Custom Private Methods 
        
    }}
}}";

            string folderPath = "Assets/ScriptTemplates";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = SetFileName();

            string filePath = Path.Combine(folderPath, fileName);
            File.WriteAllText(filePath, scriptTemplate);

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Script Template Created", $"Template created at {filePath}", "OK");
        }

        private string SetFileName()
        {
            if (string.IsNullOrEmpty(defaultScriptName))
            {
                defaultScriptName = "DefaultScriptName";
            }


            string menuName = $"C# Script {effectiveFirstName} {effectiveLastName}";

            if (!string.IsNullOrEmpty(customMenuName))
            {
                menuName = customMenuName;
            }



            string fileName = $"{orderNumber}-{menuName}-{defaultScriptName}.cs.txt";

            if (!string.IsNullOrEmpty(parentMenuName))
            {
                fileName = $"{orderNumber}-{parentMenuName}__{menuName}-{defaultScriptName}.cs.txt";
            }

            return fileName;
        }
    }
}