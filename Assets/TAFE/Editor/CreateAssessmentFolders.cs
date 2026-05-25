using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Hoey.Scripts;
using UnityEditor;
using System.IO;

namespace Hoey.Scripts
{
    /// <summary>
    /// Author: Mark Hoey
    /// Description: This script is used to create the expected folders for the assessment in Unity
    /// </summary>
    public class CreateAssessmentFolders : EditorWindow
    {
        private string firstName = "MyFirstName";
        private string lastName = "MyLastName";
        private int numberOfQuestionsToComplete = 20;

        [MenuItem("TAFE/Make Expected Assessment Folder Structure", false, 100)]
        public static void ShowWindow()
        {
            GetWindow<CreateAssessmentFolders>("Assessment Folder Structure Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Assessment Folder Structure Creator", EditorStyles.boldLabel);

            firstName = EditorGUILayout.TextField("First Name", firstName);
            lastName = EditorGUILayout.TextField("Last Name", lastName);
            numberOfQuestionsToComplete = EditorGUILayout.IntField("Number of Questions", numberOfQuestionsToComplete);

            if (GUILayout.Button("Create Assessment Folder Structure"))
            {
                CreateFolderStructure();
            }
        }

        private void CreateFolderStructure()
        {
            string effectiveFirstName = string.IsNullOrEmpty(firstName) ? "MyFirstName" : firstName;
            string effectiveLastName = string.IsNullOrEmpty(lastName) ? "MyLastName" : lastName;
            int effectiveNumberOfQuestionsToComplete = (numberOfQuestionsToComplete <=0) ? 1 : numberOfQuestionsToComplete;

            string subFolderToCreate = $"{effectiveFirstName} {effectiveLastName}";
            CommonUtilitiesTAFE.CreateFolderWithName(subFolderToCreate);

            
            string rootFolder = $"Assets/{subFolderToCreate}";

            for (int i = 1; i <= effectiveNumberOfQuestionsToComplete; i++)
            {
                subFolderToCreate = $"Q{i}";
                CommonUtilitiesTAFE.CreateFolderWithName(rootFolder, subFolderToCreate);
            }
        }
    }
}