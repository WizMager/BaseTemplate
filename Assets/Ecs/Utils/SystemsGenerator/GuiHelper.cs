using System;
using UnityEditor;
using UnityEngine;

namespace Ecs.Utils.SystemsGenerator
{
    public partial class EcsSystemsGeneratorWindow {
        private static readonly GUILayoutOption[] OrderStyle = {GUILayout.MaxWidth(50), GUILayout.MinWidth(50)};
        private static readonly GUILayoutOption[] ButtonsStyle = {GUILayout.MaxWidth(70), GUILayout.MinWidth(70)};
        private static readonly GUILayoutOption[] HeaderStyle = {GUILayout.MaxWidth(100), GUILayout.MinWidth(100)};
        private static readonly GUILayoutOption[] NameStyle = {GUILayout.MinWidth(100)};
        private static readonly GUILayoutOption[] FeatureStyle = {GUILayout.MaxWidth(200), GUILayout.MinWidth(100)};
        private static readonly GUILayoutOption[] ResetStyle = {GUILayout.MaxWidth(50), GUILayout.MinWidth(50)};

        private static bool Button(string label, GUILayoutOption[] style)
            => GUILayout.Button(label, EditorStyles.toolbarDropDown, style);

        private static Enum Enum(Enum value) => EditorGUILayout.EnumPopup(value, EditorStyles.miniButton, ButtonsStyle);

        private static Rect GetDropArea() {
            var rect = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
            GUILayout.Space(5f);
            return rect;
        }
    }
}