
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using GltfCharacterLoader;

namespace GltfCharacterLoader
{
    /// <summary>
	/// Description: This script 
	/// </summary>
	public class RenameByTMPText : EditorWindow
    {
        private string prefix = "Text (TMP) - ";
        private string suffix = "";

        [MenuItem("TAFE/TMP Renamers/Rename by TMP Text with Prefix and Suffix")]
        public static void ShowWindow()
        {
            GetWindow<RenameByTMPText>("Rename by TMP Text");
        }

        private void OnGUI()
        {
            GUILayout.Label("Rename Selected GameObjects", EditorStyles.boldLabel);
            prefix = EditorGUILayout.TextField("Prefix", prefix);
            suffix = EditorGUILayout.TextField("Suffix", suffix);

            if(GUILayout.Button("Rename Selected"))
            {
                RenameSelectedByTMPText();
            }
        }

        void RenameSelectedByTMPText()
        {
            foreach(GameObject gameObjectToUpdate in Selection.gameObjects)
            {
                UpdateTextOfSelectedObject<TextMeshProUGUI>(gameObjectToUpdate);
            }

            foreach(GameObject gameObjectToUpdate in Selection.gameObjects)
            {
                UpdateTextOfSelectedObject<TextMeshPro>(gameObjectToUpdate);
            }
        }

        private void UpdateTextOfSelectedObject<T>(GameObject gameObjectToUpdate) where T : TMP_Text
        {
            //TextMeshProUGUI tmp = gameObjectToUpdate.GetComponentInChildren<TextMeshProUGUI>();
            T textComponent = gameObjectToUpdate.GetComponentInChildren<T>();

            if(textComponent != null)
            {
                string newName = textComponent.text;

                (string oldValue, string newValue)[] replacements = {
                        (" ", "_"),
                        ("-", "_"),
                        ("/", "_"),
                        ("\n", "")
                    };

                foreach(var (oldValue, newValue) in replacements)
                {
                    newName = newName.Replace(oldValue, newValue);
                }

                newName = prefix + newName + suffix;

                // Optional: limit or sanitize name
                if(!string.IsNullOrEmpty(newName))
                {
                    Undo.RecordObject(gameObjectToUpdate, "Rename GameObject");
                    gameObjectToUpdate.name = newName;
                }
                else
                {
                    Debug.LogWarning($"Text is empty for GameObject: {gameObjectToUpdate.name}");
                }
            }
            else
            {
                Debug.LogWarning($"No {typeof(T).Name} component found in children of {gameObjectToUpdate.name}");
            }
        }
    }
}