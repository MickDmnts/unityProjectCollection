namespace htm.editor
{
    using UnityEngine;
    using UnityEditor;
    using htm.core;
    using System.Collections.Generic;

    [CustomEditor(typeof(HoudiniExportSorter))]
    public class HoudiniSorterEditor : Editor
    {
        HoudiniExportSorter sorter;
        SerializedObject serSorter;
        HTMDatabase database;

        #region SCRIPT_FIELDS
        SerializedProperty parentTile;

        SerializedProperty lowDetailBasePath;
        SerializedProperty highDetailBasePath;

        SerializedProperty lat;
        SerializedProperty lon;

        SerializedProperty tileType;
        SerializedProperty tileRes;

        SerializedProperty scriptsToAdd;

        SerializedProperty customLayerPrefix;
        SerializedProperty customDetailLevel;
        SerializedProperty customLayerType;
        #endregion

        #region BOOLEAN_FLAGS
        bool showCustom = false;
        bool showPaths = false;
        bool isPrefixValid = false;
        #endregion

        private void OnEnable()
        {
            sorter = (HoudiniExportSorter)target;
            serSorter = serializedObject;

            parentTile = serSorter.FindProperty("parentTile");

            lowDetailBasePath = serSorter.FindProperty("lowDetailBasePath");
            highDetailBasePath = serSorter.FindProperty("highDetailBasePath");

            lat = serSorter.FindProperty("lat");
            lon = serSorter.FindProperty("lon");

            tileType = serSorter.FindProperty("tileType");
            tileRes = serSorter.FindProperty("tileRes");

            scriptsToAdd = serSorter.FindProperty("scriptsToAdd");

            customLayerPrefix = serSorter.FindProperty("customLayerPrefix");
            customDetailLevel = serSorter.FindProperty("customDetailLevel");
            customLayerType = serSorter.FindProperty("customLayerType");
        }

        public override void OnInspectorGUI()
        {
            serSorter.Update();

            DrawSaveLoadingButtons();
            serSorter.ApplyModifiedProperties();

            #region PARENT_TILE
            EditorGUILayout.LabelField("Input Parent Tile", EditorStyles.boldLabel);

            if (parentTile.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(parentTile, new GUIContent("Parent Tile", "The initial tile that contains the sub tiles to be named and saved"));
                serSorter.ApplyModifiedProperties();
                return;
            }
            else if (!sorter.IsParentTileAcceptable())
            {
                EditorGUILayout.PropertyField(parentTile, new GUIContent("Parent Tile", "The initial tile that contains the sub tiles to be named and saved"));
                serSorter.ApplyModifiedProperties();
                EditorGUILayout.HelpBox("The parent tile must not be empty and contain at least two child tiles.", MessageType.Error);
                return;
            }

            EditorGUILayout.PropertyField(parentTile);
            #endregion

            EditorGUILayout.Space();
            DrawDetailPaths();

            EditorGUILayout.Space();
            DrawCordsFields();

            EditorGUILayout.Space();
            DrawTileInfo();

            EditorGUILayout.Space();
            DrawMonoBehaviourArray();

            EditorGUILayout.Space();
            if (sorter.IsCustomLayerType())
            {
                DrawCustomFields();

                if (!sorter.IsPrefixValid(customLayerPrefix.stringValue))
                {
                    EditorGUILayout.HelpBox("Prefix field can not be empty", MessageType.Warning);
                    isPrefixValid = false;
                }
                else { isPrefixValid = true; }
            }
            else
            {
                customLayerPrefix.stringValue = string.Empty;
                isPrefixValid = true;
            }

            if (sorter.IsPassedDetailSelected(CustomDetailLevel.LowDetail))
            {
                if (sorter.IsPathValid(lowDetailBasePath.stringValue) && sorter.IsInAssetFolder(lowDetailBasePath.stringValue)
                                && isPrefixValid)
                {
                    if (GUILayout.Button("Create Prefab Tiles"))
                    {
                        sorter.StartSorterSequence();
                    }

                    DrawNameHelpBox();
                }
                else
                {
                    EditorGUILayout.HelpBox("Low Detail path must be a physical address, not empty and not end with a symbol.", MessageType.Warning);
                }
            }
            else if (sorter.IsPassedDetailSelected(CustomDetailLevel.HighDetail))
            {
                if (sorter.IsPathValid(highDetailBasePath.stringValue) && isPrefixValid)
                {
                    if (GUILayout.Button("Create Prefab Tiles"))
                    {
                        sorter.StartSorterSequence();
                    }

                    DrawNameHelpBox();
                }
                else
                {
                    EditorGUILayout.HelpBox("High Detail path must be a physical address, not empty and not end with a symbol.", MessageType.Warning);
                }
            }

            serSorter.ApplyModifiedProperties();
        }

        void DrawSaveLoadingButtons()
        {
            #region SAVE_LOADING
            if (database == null)
            { database = new HTMDatabase(); }

            EditorGUILayout.BeginHorizontal("box");
            GUI.enabled = database.GetHTMHasEntry(0);
            EditorGUILayout.HelpBox("Double click the Load Button to load the scripts.", MessageType.None);
            if (GUILayout.Button("Load Preset"))
            {
                LoadMechanism();
            }
            GUI.enabled = true;

            if (GUILayout.Button("Save Preset"))
            {
                SaveMechanism();
            }
            EditorGUILayout.EndHorizontal();
        }

        void SaveMechanism()
        {
            if (parentTile.objectReferenceValue == null)
            {
                Debug.Log("Nothing to save");
                return;
            }

            EntryInfoPacket packet = new EntryInfoPacket();
            packet.parentTile = parentTile.objectReferenceValue.name.ToString();

            packet.ldPath = lowDetailBasePath.stringValue.ToString();
            packet.hdPath = highDetailBasePath.stringValue.ToString();

            packet.lat = lat.doubleValue;
            packet.lon = lon.doubleValue;

            packet.tileType = tileType.enumValueIndex;
            packet.tileRes = tileRes.doubleValue;

            packet.scriptToAdd = sorter.GetScriptsToAdd();

            packet.customLayerPrefix = customLayerPrefix.stringValue.ToString();
            packet.customDetailLevel = customDetailLevel.enumValueIndex;
            packet.customLayerType = customLayerType.enumValueIndex;

            string jsonStr = HTMSerializer.SerializeEntryInfoPacket(packet);

            if (database != null)
            { database.UpdateHTMSaveString(jsonStr, 0); }
            else
            { Debug.Log("Database not created"); }
        }

        void LoadMechanism()
        {
            string jsonStr = database.GetHTMSaveString(0);
            EntryInfoPacket packet = HTMSerializer.DeserializeEntryInfoPacket(jsonStr);

            parentTile.objectReferenceValue = Resources.Load<GameObject>(packet.parentTile);

            if (parentTile.objectReferenceValue == null)
            {
                Debug.Log("Asset not found in Resources folder.");
                return;
            }

            lowDetailBasePath.stringValue = packet.ldPath;
            highDetailBasePath.stringValue = packet.hdPath;

            lat.doubleValue = packet.lat;
            lon.doubleValue = packet.lon;

            tileType.enumValueIndex = packet.tileType;
            tileRes.doubleValue = packet.tileRes;

            sorter.PopulateScriptsToAddArray(packet.scriptToAdd);

            customLayerPrefix.stringValue = packet.customLayerPrefix;
            customDetailLevel.enumValueIndex = packet.customDetailLevel;
            customLayerType.enumValueIndex = packet.customLayerType;
        }
        #endregion

        void DrawDetailPaths()
        {
            EditorGUILayout.LabelField("Input Save Paths", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            showPaths = EditorGUILayout.Foldout(showPaths, "Save Paths");
            EditorGUILayout.HelpBox("Paths passed must be physical addresses and not end with a symbol.", MessageType.None);
            if (showPaths)
            {
                if (Selection.activeTransform)
                {
                    EditorGUILayout.BeginVertical();
                    lowDetailBasePath.stringValue = EditorGUI.TextField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("Low Detail Path", "The low detail Assets path."), lowDetailBasePath.stringValue);
                    EditorGUILayout.HelpBox("Low Detail path must reside inside the Assets folder", MessageType.None);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical();
                    highDetailBasePath.stringValue = EditorGUI.TextField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("High Detail Path", "The high detail Assets path."), highDetailBasePath.stringValue);
                    EditorGUILayout.EndVertical();
                }

                if (!showPaths)
                {
                    showPaths = false;
                }
            }
            EditorGUILayout.EndVertical();
        }

        void DrawCordsFields()
        {
            EditorGUILayout.LabelField("Input Bottom Left Coordinates", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            lat.doubleValue = EditorGUI.DoubleField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("Bottom Left LAT", "The bottom left LAT coords of the parent tile."), lat.doubleValue);
            lon.doubleValue = EditorGUI.DoubleField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("Bottom Left LON", "The bottom left LON coords of the parent tile."), lon.doubleValue);
            EditorGUILayout.EndVertical();
        }

        void DrawTileInfo()
        {
            EditorGUILayout.LabelField("Select export layer and resolution", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            tileType.enumValueIndex = EditorGUILayout.Popup("Export Layer", tileType.enumValueIndex, tileType.enumNames);
            tileRes.doubleValue = EditorGUI.DoubleField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("Tile Resolution", "The sub tile export resolution"), tileRes.doubleValue);
            EditorGUILayout.EndVertical();
        }

        void DrawMonoBehaviourArray()
        {
            EditorGUILayout.LabelField("Import any scripts you want the tiles to have.", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(scriptsToAdd, new GUIContent("Scripts to Add", "MonoBehaviour Scripts you want the exported tiles to have on them."));

            EditorGUILayout.EndVertical();
        }

        void DrawCustomFields()
        {
            EditorGUILayout.LabelField("Input custom prefix and Detail Level", EditorStyles.boldLabel);

            GUILayout.BeginVertical("box");

            showCustom = EditorGUILayout.Foldout(showCustom, "Custom Options");

            if (showCustom)
            {
                if (Selection.activeTransform)
                {
                    EditorGUILayout.BeginVertical();
                    customLayerPrefix.stringValue = EditorGUI.TextField(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), new GUIContent("Custom Prefix", "The custom prefix the user inputs if he selects a custom layer"), customLayerPrefix.stringValue);
                    customDetailLevel.enumValueIndex = EditorGUILayout.Popup("Custom Detail", customDetailLevel.enumValueIndex, customDetailLevel.enumNames);
                    customLayerType.enumValueIndex = EditorGUILayout.Popup("Custom Layer Filtering", customLayerType.enumValueIndex, customLayerType.enumNames);
                    EditorGUILayout.EndVertical();
                }

                if (!showCustom)
                {
                    showCustom = false;
                }
            }

            GUILayout.EndVertical();
        }

        void DrawNameHelpBox()
        {
            string tempName = string.Empty;

            if (tileRes.doubleValue.ToString().Length <= 3) { return; }

            if (customLayerPrefix.stringValue != string.Empty)
                tempName = sorter.GenerateSampleName(customLayerPrefix.stringValue);
            else { tempName = sorter.GenerateSampleName("layerPrefix"); }

            EditorGUILayout.HelpBox($"Tile Output Info \n   Tile Group: {sorter.AddressableGroup} \n   Tile Name: {tempName}", MessageType.Info);
        }
    }
}