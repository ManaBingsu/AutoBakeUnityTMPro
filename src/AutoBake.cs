using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bobbin;
using TMPro;
using System.IO;
using System.Text;
using System;
using UnityEditorInternal;

namespace AutoBake
{
    [CreateAssetMenu]
    public class AutoBake : ScriptableObject
    {
        [Header("Path")]
        [Tooltip("Enter the path of the csv file to be parsed")]
        [SerializeField]
        string stringPath = "Assets/strings.csv";
        [Header("Location setting")]
        [Tooltip("Enter the location info")]
        [SerializeField]
        List<Location> locations = new List<Location>(
            new Location[]
            {
                new Location("KOREAN", 1),
                new Location("ENGLISH", 2),
                new Location("JAPANESE", 3),
                new Location("SIMPLIFIED_CHINESE", 4),
                new Location("GERMAN", 5),
            }
            );

#if UNITY_EDITOR
        [CustomEditor(typeof(AutoBake))]
        class Editor : UnityEditor.Editor
        {
            AutoBake _target;
            new AutoBake target => _target ?? (_target = (AutoBake)base.target);

            private SerializedProperty m_shortcutData;
            private ReorderableList m_ReorderableList;

            private void OnEnable()
            {
                //Find the list in our ScriptableObject script.
                m_shortcutData = serializedObject.FindProperty("locations");

                //Create an instance of our reorderable list.
                m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_shortcutData, draggable: true, displayHeader: true,
                    displayAddButton: true, displayRemoveButton: true);

                //Set up the method callback to draw our list header
                m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

                //Set up the method callback to draw each element in our reorderable list
                m_ReorderableList.drawElementCallback = DrawElementCallback;

                //Set the height of each element.
                m_ReorderableList.elementHeightCallback += ElementHeightCallback;

                //Set up the method callback to define what should happen when we add a new object to our list.
                m_ReorderableList.onAddCallback += OnAddCallback;
            }

            /// <summary>
            /// Draws the header for the reorderable list
            /// </summary>
            /// <param name="rect"></param>
            private void DrawHeaderCallback(Rect rect)
            {
                EditorGUI.LabelField(rect, "Location setting shortcut");
            }

            /// <summary>
            /// This methods decides how to draw each element in the list
            /// </summary>
            /// <param name="rect"></param>
            /// <param name="index"></param>
            /// <param name="isactive"></param>
            /// <param name="isfocused"></param>
            private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
            {
                //Get the element we want to draw from the list.
                SerializedProperty element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                //We get the name property of our element so we can display this in our list.
                SerializedProperty elementName = element.FindPropertyRelative("name");
                string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
                    ? "New Shortcut"
                    : $"{elementName.stringValue}";

                //Draw the list item as a property field, just like Unity does internally.
                EditorGUI.PropertyField(position:
                    new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
                    element, label: new GUIContent(elementTitle), includeChildren: true);
            }

            /// <summary>
            /// Calculates the height of a single element in the list.
            /// This is extremely useful when displaying list-items with nested data.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            private float ElementHeightCallback(int index)
            {
                //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
                float propertyHeight =
                    EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

                float spacing = EditorGUIUtility.singleLineHeight / 2;

                return propertyHeight + spacing;
            }

            /// <summary>
            /// Defines how a new list element should be created and added to our list.
            /// </summary>
            /// <param name="list"></param>
            private void OnAddCallback(ReorderableList list)
            {
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
            }


            public override void OnInspectorGUI()
            {
                if (GUILayout.Button("Auto bake"))
                    target.Load();

                serializedObject.Update();

                m_ReorderableList.DoLayoutList();

                serializedObject.ApplyModifiedProperties();

                base.OnInspectorGUI();
            }
        }
#endif
        public void Load()
        {
            // Bobbin refresh
            BobbinCore.DoRefresh();
            Debug.Log("Complete Bobbin Refresh");

            // Check if list is null

            // Bake text
            foreach (Location location in locations)
            {
                int locationIndex = location.index;
                foreach (TMP_FontAsset fontAsset in location.fontAssets)
                {
                    if (fontAsset == null)
                    {
                        Debug.LogError("<color=blue>[Error] </color> Font asset doesn't set in list");
                        continue;
                    }

                    Debug.Log("Bake [" + location.name + "](" + fontAsset.name + ")");
                    // Step 1 : Parsing with CsvReader
                    StringBuilder strBuilder = new StringBuilder();
                    using (var streamRdr = new StreamReader(stringPath))
                    {
                        var csvReader = new CsvReader(streamRdr, ",");
                        while (csvReader.Read())
                        {
                            if (!string.IsNullOrWhiteSpace(csvReader[locationIndex]))
                                strBuilder.Append(csvReader[locationIndex]);
                        }
                    }
                    // Step 2 : Delete duplicates
                    string characterSequence = strBuilder.ToString();
                    uint[] characterSet = null;
                    List<uint> characters = new List<uint>();
                    for (int i = 0; i < characterSequence.Length; i++)
                    {
                        uint unicode = characterSequence[i];
                        // Handle surrogate pairs
                        if (i < characterSequence.Length - 1 && char.IsHighSurrogate((char)unicode) && char.IsLowSurrogate(characterSequence[i + 1]))
                        {
                            unicode = (uint)char.ConvertToUtf32(characterSequence[i], characterSequence[i + 1]);
                            i += 1;
                        }
                        // Check to make sure we don't include duplicates
                        if (characters.FindIndex(item => item == unicode) == -1)
                            characters.Add(unicode);
                    }
                    characterSet = characters.ToArray();
                    Debug.Log("Total count : " + characterSet.Length);
                    // Step 4 : Generate atlas
                    uint[] missingString = null;
                    fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
                    fontAsset.ClearFontAssetData();
                    fontAsset.TryAddCharacters(characterSet, out missingString);

                    StringBuilder missingStrBuilder = new StringBuilder();
                    if (missingString != null)
                    {
                        foreach (uint unicode in missingString)
                        {
                            missingStrBuilder.Append(Convert.ToChar(unicode));
                            missingStrBuilder.Append(" ");
                        }
                        Debug.LogError($"<color=red>[Fatal error] </color>Missing string : {missingStrBuilder} ");
                    }
                    fontAsset.atlasPopulationMode = AtlasPopulationMode.Static;
                    TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(true, fontAsset);
                    EditorUtility.SetDirty(fontAsset);
                }
            }
        }

        [Serializable]
        struct Location
        {
            public string name;
            public int index;
            public List<TMP_FontAsset> fontAssets;
            public Location(string name, int index)
            {
                fontAssets = new List<TMP_FontAsset>();
                this.name = name;
                this.index = index;
            }
        }
    }
}
