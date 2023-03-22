using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(XpManager))]
public class XpManagerEditor : Editor 
{
	// Set the icons.
	protected virtual void OnEnable()
    {
		var ty = typeof(EditorGUIUtility);
		var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
		mi.Invoke(null, new object[] { (XpManager)target, Resources.Load("GizmoIcon") });
	}

	public override void OnInspectorGUI()
	{		
		base.OnInspectorGUI();
		var content = new GUIContent(){ image = (Texture) Resources.Load("XPManagerIcon"), tooltip="Open Xp Manager Window" };

		if (GUILayout.Button(content))
		{
			var window = ScriptableObject.CreateInstance<XpManagerWindow>();
			window.ShowWindow((XpManager)target);
		}
	}
}
#endif
