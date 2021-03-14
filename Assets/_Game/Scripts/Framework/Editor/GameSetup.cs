using UnityEditor;

namespace Framework.Editor
{
    public static class GameSetup
    {
        public static void AddScene(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var buildManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/EditorBuildSettings.asset")[0]);
            var scenes = buildManager.FindProperty("m_Scenes");

            for (int i = 0; i < scenes.arraySize; i++)
            {
                var scene = scenes.GetArrayElementAtIndex(i);
                var pathProp = scene.FindPropertyRelative("path");
                if (pathProp.stringValue == path)
                {
                    scene.FindPropertyRelative("enabled").boolValue = true;
                    buildManager.ApplyModifiedProperties();
                    return;
                }
            }

            scenes.arraySize++;
            var newScene = scenes.GetArrayElementAtIndex(scenes.arraySize - 1);

            newScene.FindPropertyRelative("enabled").boolValue = true;
            newScene.FindPropertyRelative("path").stringValue = path;

            buildManager.ApplyModifiedProperties();
        }

        public static void AddTag(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tags = tagManager.FindProperty("tags");

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < tags.arraySize; i++)
            {
                var tagProp = tags.GetArrayElementAtIndex(i);
                var stringValue = tagProp.stringValue;
                if (stringValue == name)
                {
                    return;
                }

                if (stringValue != string.Empty)
                {
                    continue;
                }

                if (firstEmptyProp == null)
                {
                    firstEmptyProp = tagProp;
                }
            }

            if (firstEmptyProp == null)
            {
                tags.arraySize++;
                firstEmptyProp = tags.GetArrayElementAtIndex(tags.arraySize - 1);
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }

        public static void AddLayer(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layers = tagManager.FindProperty("layers");

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < layers.arraySize; i++)
            {
                var layerProp = layers.GetArrayElementAtIndex(i);
                var stringValue = layerProp.stringValue;
                if (stringValue == name)
                {
                    return;
                }

                if (i < 8 || stringValue != string.Empty)
                {
                    continue;
                }

                if (firstEmptyProp == null)
                {
                    firstEmptyProp = layerProp;
                }
            }

            if (firstEmptyProp == null)
            {
                UnityEngine.Debug.LogError("Maximum limit of " + layers.arraySize + " layers exceeded. Layer \"" + name + "\" not created.");
                return;
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }

        public static void AddNavArea(string name, float cost)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var areasManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/NavMeshAreas.asset")[0]);
            var areas = areasManager.FindProperty("areas");

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < areas.arraySize; i++)
            {
                var areaProp = areas.GetArrayElementAtIndex(i);
                var areaName = areaProp.FindPropertyRelative("name");

                var stringValue = areaName.stringValue;
                if (stringValue == name)
                {
                    return;
                }

                if (i < 3 || stringValue != string.Empty)
                {
                    continue;
                }

                if (firstEmptyProp == null)
                {
                    firstEmptyProp = areaProp;
                }
            }

            if (firstEmptyProp == null)
            {
                UnityEngine.Debug.LogError("Maximum limit of " + areas.arraySize + " areas exceeded. Area \"" + name + "\" not created.");
                return;
            }

            var nameProp = firstEmptyProp.FindPropertyRelative("name");
            var costProp = firstEmptyProp.FindPropertyRelative("cost");
            nameProp.stringValue = name;
            costProp.floatValue = cost;

            areasManager.ApplyModifiedProperties();
        }
    }
}