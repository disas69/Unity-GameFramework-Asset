using Framework.Tools.Data;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class StorageEditor<TItem, TStorage> : CustomEditorBase<TStorage> where TItem : StorageItem, new() where TStorage : Storage<TItem>
    {
        private string _itemTypeName;
        private SearchBar _searchBar;

        protected override void OnEnable()
        {
            base.OnEnable();

            _itemTypeName = typeof(TItem).Name;
            _searchBar = new SearchBar();
        }

        protected virtual void DrawItem(SerializedProperty itemName, SerializedProperty item, int index)
        {
            EditorGUILayout.PropertyField(itemName, new GUIContent($"{_itemTypeName} {index + 1}"));
        }

        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField($"[{_itemTypeName}] Storage", HeaderStyle);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    _searchBar.Draw();

                    if (GUILayout.Button("Add Item"))
                    {
                        RecordObject("Storage Change");
                        Target.Items.Add(new TItem());
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            var items = serializedObject.FindProperty("Items");
            var count = items.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    var searchResultsCount = 0;
                    for (var i = 0; i < count; i++)
                    {
                        var item = items.GetArrayElementAtIndex(i);
                        var itemName = item.FindPropertyRelative("Name");

                        if (!_searchBar.IsEmpty && !_searchBar.IsMatchingTheFilter(itemName.stringValue))
                        {
                            continue;
                        }

                        searchResultsCount++;
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                DrawItem(itemName, item, i);
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Storage Change");
                                Target.Items.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (searchResultsCount == 0)
                    {
                        EditorGUILayout.LabelField("No matches found...", LabelStyle);
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}