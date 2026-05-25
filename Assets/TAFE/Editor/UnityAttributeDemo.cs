using System;
using UnityEngine;
//https://docs.unity3d.com/ScriptReference/AddComponentMenu.html
[AddComponentMenu("Demo/Script To Demo Unity Attributes")]
[RequireComponent(typeof(Rigidbody))]
[HelpURL("https://www.google.com/search?q=unity+attributes")]
[SelectionBase]
public class UnityAttrbuteDemo : MonoBehaviour
{
    [Header("Serialization")]
    public int serializedPublicField;
    private int nonserializedPrivateField;

    [SerializeField] private int serializedPrivateField;

    [NonSerialized] public int nonserializedPublicField;
    [HideInInspector] public int serializedButHiddenPublicField;

    public NonserializableClass nonSerializableObject;
    public SerializableClass serializableObject;
    public SerializableStruct serializableStruct;

    [Header("Numbers")]
    [Min(0)]
    public int intWithMin;

    [Range(0, 100)]
    public int intWithRange;

    [Range(0, 1)]
    public float floatWithRange;

    [Header("Strings")]
    [Multiline(4)]
    public string multilineString;

    [TextArea(4, 8)]
    public string stringWithTextArea;

    [Header("Colors")]
    [ColorUsage(true, true)]
    public Color hdrColor;

    [ColorUsage(false, false)]
    public Color noAlphaColor;

    [Header("Context Menus")]
    [ContextMenuItem("SetToThree", nameof(SetPropertyToThree))]
    public int fieldWithContextMenu;
    void SetPropertyToThree()
    {
        fieldWithContextMenu = 3;
    }

    [ContextMenu("Do Something")]
    private void DoSomething()
    {
        Debug.Log("Did something");
    }

    [Header("Other")]
    [Tooltip("I'm a tooltip")]
    public string fieldWithTooltip;

    [Space(25)]

    public string fieldAfterSpace;
}

public class NonserializableClass
{
    public int value1;
    public string value2;
}

[Serializable]
public class SerializableClass
{
    public int value1;
    public string value2;
}

[Serializable]
public struct SerializableStruct
{
    public int value1;
    public string value2;
}
