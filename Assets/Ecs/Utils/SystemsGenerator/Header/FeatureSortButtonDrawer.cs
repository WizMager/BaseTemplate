using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Utils.SystemsGenerator.Sort.Impls;
using UnityEditor;
using UnityEngine;

namespace Ecs.Utils.SystemsGenerator.Header
{
	public class FeatureSortButtonDrawer : ASortButtonDrawer
	{
		private readonly FeatureSortStrategy _sortStrategy;
		private readonly FeatureDropDownPopUp _dropDownPopUp;

		private Rect _buttonRect;

		public FeatureSortButtonDrawer(
			FeatureSortStrategy sortStrategy,
			GUILayoutOption[] style,
			List<AttributeRecord> attributes
		) : base(sortStrategy, style)
		{
			var features = attributes.Select(record => record.Features)
				.Aggregate((array1, array2) => array1.Concat(array2).ToArray())
				.Distinct().ToArray();
			_dropDownPopUp = new FeatureDropDownPopUp(features);
			sortStrategy.Filter = _dropDownPopUp;
			_sortStrategy = sortStrategy;
		}

		public override bool OnClick()
		{
			var onClick = GUILayout.Button(GetButtonLabel(), EditorStyles.toolbarDropDown, Style);
			if (onClick)
			{
				PopupWindow.Show(_buttonRect, _dropDownPopUp);
			}

			if (Event.current.type == EventType.Repaint)
				_buttonRect = GUILayoutUtility.GetLastRect();
			return onClick;
		}

		protected override string GetButtonLabel() => "Feature " + _sortStrategy.Name;
	}

	public class FeatureDropDownPopUp : PopupWindowContent, IFeatureFilter
	{
		private static readonly Vector2 WindowSize = new(190, 200);

		private readonly List<FeatureToggle> _features = new();

		private bool _mouseEnter;

		private Vector2 _scrollPosition;
		private string[] _featuresFilter;
		private bool _hasChanges;

		public FeatureDropDownPopUp(string[] features)
		{
			features = features.OrderBy(s => s).ToArray();
			_featuresFilter = features;

			foreach (var feature in features)
			{
				_features.Add(new FeatureToggle(feature, OnChange));
			}
		}

		private void OnChange() => _hasChanges = true;

		public override Vector2 GetWindowSize() => WindowSize;

		public override void OnGUI(Rect rect)
		{
			ProcessFocusCheck();

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Select All"))
				SetAllToggles(true);
			if (GUILayout.Button("Deselect All"))
				SetAllToggles(false);
			GUILayout.EndHorizontal();

			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);
			foreach (var feature in _features)
			{
				feature.Draw();
			}

			EditorGUILayout.EndScrollView();

			if (!_hasChanges)
				return;

			_hasChanges = false;
			_featuresFilter = _features.Where(f => f.IsSelected).Select(s => s.Feature).ToArray();
		}

		private void SetAllToggles(bool isSelected)
		{
			foreach (var feature in _features)
			{
				feature.IsSelected = isSelected;
			}

			_featuresFilter = isSelected
				? _features.Select(s => s.Feature).ToArray()
				: Array.Empty<string>();
		}

		private void ProcessFocusCheck()
		{
			var eventType = Event.current.type;
			if (!_mouseEnter && eventType == EventType.MouseEnterWindow)
				_mouseEnter = true;
#if !UNITY_EDITOR_OSX
			if (_mouseEnter && eventType == EventType.MouseLeaveWindow)
				editorWindow.Close();
#endif
		}

		public override void OnOpen()
		{
			editorWindow.wantsMouseEnterLeaveWindow = true;
		}

		private class FeatureToggle
		{
			private readonly Action _onChange;

			public readonly string Feature;
			public bool IsSelected;

			public FeatureToggle(string feature, Action onChange)
			{
				Feature = feature;
				IsSelected = true;
				_onChange = onChange;
			}

			public void Draw()
			{
				var value = EditorGUILayout.Toggle(Feature, IsSelected);
				if (IsSelected == value)
					return;

				IsSelected = value;
				_onChange();
			}
		}

		public string[] GetFeatures() => _featuresFilter;

		public void Reset() => SetAllToggles(true);
	}
}