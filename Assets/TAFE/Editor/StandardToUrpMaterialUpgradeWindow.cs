using UnityEngine;
using UnityEditor;

namespace Hoey.Scripts
{
    /// <summary>
    /// Author: Mark Hoey
    /// Description: This script is used to convert 'Standard' materials to 'URP/Lit' versions in Unity
    /// </summary>
    public class StandardToUrpMaterialUpgradeWindow : EditorWindow
    {
        private Material material;
        private Material[] materials = new Material[0];

        [MenuItem("TAFE/Upgrade Materials to URP", false, 301)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(StandardToUrpMaterialUpgradeWindow));
        }

        private void AddMaterial(Material material)
        {
            ArrayUtility.Add(ref materials, material);
        }

        private void ClearMaterials()
        {
            materials = new Material[0];
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Select materials to upgrade to Universal Render Pipeline (URP) Lit shader.", MessageType.Info);
            EditorGUILayout.HelpBox("Not full-featured: Currently changes the shader type, and some basic links to textures and colors. \nCreated as currently when you upgrade using Unity tools the base texutre and base color is often stripped.", MessageType.Warning);
            
            GUILayout.Space(5);

            GUILayout.Label("Drop Materials Here:");

            Event currentEventDetected = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drop materials here");

            switch (currentEventDetected.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(currentEventDetected.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (currentEventDetected.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            Material material = draggedObject as Material;
                            //EditorGUILayout.HelpBox("One or more selected objects are not materials or already have URP shader.", MessageType.Error);
                            if (material != null && !material.shader.name.Contains("Universal Render Pipeline/Lit"))
                            {
                                AddMaterial(material);
                            }
                        }
                    }
                    break;
            }

            CommonUtilitiesTAFE.HorizontalBarWithPadding();

            GUILayout.Label("Materials to Upgrade:");

            foreach (Material material in materials)
            {
                EditorGUILayout.ObjectField(material, typeof(Material), false);
            }

            if (GUILayout.Button("Clear Materials"))
            {
                ClearMaterials();
            }

            CommonUtilitiesTAFE.HorizontalBarWithPadding();

            if (GUILayout.Button("Upgrade"))
            {
                if (material != null || (materials != null && materials.Length > 0))
                {
                    if (material != null)
                    {
                        UpgradeMaterial(material);
                    }
                    if (materials != null && materials.Length > 0)
                    {
                        foreach (var material in materials)
                        {
                            Material selectedMaterial = material as Material;
                            UpgradeMaterial(selectedMaterial);
                        }
                    }
                    Debug.Log("Materials upgraded successfully!");
                }
                else
                {
                    Debug.LogWarning("Please select materials to upgrade.");
                }
            }
        }


        Texture GetTexture(Material materialToUse, string shaderPropertyName)
        {
            Texture textureFound = null;
            if (materialToUse.HasProperty(shaderPropertyName))
            {
                textureFound = materialToUse.GetTexture(shaderPropertyName);
            }
            else
            {
                Debug.LogWarning($"Material does not have {shaderPropertyName} property.");
            }

            return textureFound;
        }
        /// <summary>
        /// Not full-featured: Currently changes the shader type, and retains some basic links to textures and colors.
        /// Created as currently when you upgrade using Unity tools the base texutre and base color is often stripped.
        /// </summary>
        /// <param name="material"></param>
        private void UpgradeMaterial(Material material)
        {
            Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");

            if (urpShader != null)
            {

                Color baseColor = material.GetColor("_Color");
                Color emissionColor = material.GetColor("_EmissionColor");
                Color specularColor = material.GetColor("_SpecColor");

                Texture baseMap = GetTexture(material, "_MainTex");
                Texture metallicGlossMap = GetTexture(material, "_MetallicGlossMap");
                Texture normalMap = GetTexture(material, "_BumpMap");
                Texture heightMap = GetTexture(material, "_ParallaxMap");
                Texture occlusionMap = GetTexture(material, "_OcclusionMap");
                Texture emissionMap = GetTexture(material, "_EmissionMap");

                Texture detailMaskMap = GetTexture(material, "_DetailMask");
                Texture detailAlbedoMap = GetTexture(material, "_DetailAlbedoMap");
                Texture detailNormalMap = GetTexture(material, "_DetailNormalMap");

                float smoothnessValue = material.GetFloat("_Glossiness");
                float normalValue = material.GetFloat("_BumpScale");
                float heightMapValue = material.GetFloat("_Parallax");
                float occlusionMapValue = material.GetFloat("_OcclusionStrength");

                material.shader = urpShader;
                material.SetTexture("_BaseMap", baseMap);
                material.SetColor("_BaseColor", baseColor);

                material.SetTexture("_MetallicGlossMap", metallicGlossMap);
                material.SetFloat("_Smoothness", smoothnessValue);

                material.SetTexture("_BumpMap", normalMap);
                material.SetFloat("_BumpScale", normalValue);

                material.SetTexture("_ParallaxMap", heightMap);
                material.SetFloat("_Parallax", heightMapValue);

                material.SetTexture("_OcclusionMap", occlusionMap);
                material.SetFloat("_OcclusionStrength", occlusionMapValue);

                material.SetTexture("_EmissionMap", emissionMap);
                material.SetColor("_EmissionColor", emissionColor);

                material.SetTexture("_DetailMask", detailMaskMap);
                material.SetTexture("_DetailAlbedoMap", detailAlbedoMap);
                material.SetTexture("_DetailNormalMap", detailNormalMap);

                EditorUtility.SetDirty(material);
            }
            else
            {
                Debug.LogError("Universal Render Pipeline/Lit shader not found. Make sure URP is installed.");
            }
        }
    }
}