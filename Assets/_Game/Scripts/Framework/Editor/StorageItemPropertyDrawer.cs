using System.IO;
using Framework.Tools.Data;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class StorageItemPropertyDrawer<TItem, TStorage> : PropertyDrawer where TItem : StorageItem where TStorage : Storage<TItem>
    {
        private const string StorageFolderPath = "Assets/_Game/Resources/Storage/";

        private int _selectedIndex = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var rect = new Rect(position.x, position.y, position.width, position.height);
            var storagePath = $"{StorageFolderPath}{typeof(TStorage).Name}.asset";
            var storage = AssetDatabase.LoadAssetAtPath(storagePath, typeof(TStorage)) as TStorage;

            if (storage != null)
            {
                if (storage.Items.Count > 0)
                {
                    var name = property.FindPropertyRelative("Name");

                    EditorGUI.BeginChangeCheck();
                    {
                        var index = Mathf.Max(0, storage.Items.FindIndex(n => n.Name == name.stringValue));
                        if (index != _selectedIndex)
                        {
                            _selectedIndex = index;
                            GUI.changed = true;
                        }

                        _selectedIndex = EditorGUI.Popup(rect, _selectedIndex, storage.GetNames());
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        name.stringValue = storage.Items[_selectedIndex].Name;
                    }
                }
                else
                {
                    if (GUI.Button(rect, "Add Item"))
                    {
                        Selection.activeObject = storage;
                    }
                }
            }
            else
            {
                if (GUI.Button(rect, "Create Storage"))
                {
                    if (!Directory.Exists(StorageFolderPath))
                    {
                        Directory.CreateDirectory(StorageFolderPath);
                    }

                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(typeof(TStorage)), storagePath);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}