using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.SceneManagement;
//using UnityEngine.SceneManagement;

public class XpManagerWindow : EditorWindow 
{ 
	public int id = -1;
	public XpManager master;
	Vector2	scroll;
	GUIStyle EditorStyle = new GUIStyle();
	XpManagerInputWindow inputWindow;

	private void OnEnable()
	{
		var content = new GUIContent(){ image = (Texture)Resources.Load("GizmoIcon"), text = " XpManager"};
		this.titleContent = content;
	}

	public void ShowWindow(XpManager _master)
	{
		id = _master.GetInstanceID();
		master = _master;
		Show();
	}

	public void OnGUI()
	{
		try
        {
			if (master == null) master = (XpManager)EditorUtility.InstanceIDToObject(id);
        }
        catch { }

		if (master == null || master.XPGroupList == null) return;

		EditorStyle.normal.background = new Texture2D(1,1);
		if (master.XPGroupList.Count <= 0) master.XPGroupList.Add(new XpManager.XPGroup());

		scroll = GUILayout.BeginScrollView(scroll);

		for (int g = 0; g < master.XPGroupList.Count; g++)
		{
			DrawGroupBox(master.XPGroupList[g]);
		}

		// Add Graph
		GUILayout.Space(12);
		if (GUILayout.Button("Add Graph", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
		{
			master.XPGroupList.Add(new XpManager.XPGroup());				
			EditorUtility.SetDirty(master);
		}

		if(!Application.isPlaying)  EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

		GUILayout.Space(32);

		GUILayout.EndScrollView();
	}

	void DrawGroupBox(XpManager.XPGroup _data)
	{
		GUILayout.Space(12);
		GUILayout.BeginHorizontal(GUILayout.Height(60));
#if UNITY_5
		_data.BoxColor = EditorGUILayout.ColorField(new GUIContent(), _data.BoxColor, false, false, false, null, GUILayout.MaxWidth(14), GUILayout.ExpandHeight(true));
#else
		_data.BoxColor = EditorGUILayout.ColorField(new GUIContent(), _data.BoxColor, false, false, false, GUILayout.MaxWidth(14), GUILayout.ExpandHeight(true));
#endif
		GUILayout.Space(4);
		DrawDataBox(_data);
        if (_data.isShowListMode)
        {
            DrawListValues(_data);
            GUILayout.Space(4);
        }
        GUILayout.Space(4);
		DrawContentBox(_data);
		GUILayout.EndHorizontal();
	}

	void DrawDataBox(XpManager.XPGroup _data)
	{
		GUILayout.BeginVertical(GUILayout.MaxWidth(40));

		// Data.
		_data.GraphName =			GUILayout.TextField(_data.GraphName);

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Range", GUILayout.Width(88));
		_data.LevelMax =			EditorGUILayout.IntField(_data.LevelMax, GUILayout.Width(50));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Value Min/Max", GUILayout.Width(88));
		_data.XpMin = EditorGUILayout.IntField(_data.XpMin, GUILayout.Width(50));
		_data.XpMax = EditorGUILayout.IntField(_data.XpMax, GUILayout.Width(50));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Use Curve", GUILayout.Width(88));
		_data.UseCurve =			EditorGUILayout.Toggle("", _data.UseCurve, GUILayout.Width(50));
		if(!_data.UseCurve)			GUI.enabled = false;
		_data.CurveValue =			EditorGUILayout.CurveField(_data.CurveValue);
		GUI.enabled = true;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show values", GUILayout.Width(88));
		_data.ShowValues =			EditorGUILayout.Toggle("", _data.ShowValues, GUILayout.Width(50));
		GUILayout.EndHorizontal();

		// Delete button.
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Delete Graph", GUILayout.Width(95), GUILayout.Height(28)))
		{
			if (EditorUtility.DisplayDialog("Delete Graph?","Are you sure you want to delete this graph?", "DELETE", "No"))
            {
				master.XPGroupList.Remove(_data);
            }
        }

		var buttonStyle = new GUIStyle(GUI.skin.button);
		//buttonStyle.fontSize = 9;
		_data.isShowListMode = GUILayout.Toggle(_data.isShowListMode, (_data.isShowListMode) ? "Hide List" : "Show List", buttonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(28));
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}

	void DrawContentBox(XpManager.XPGroup _data)
	{
		// Box Rect.
		var bar = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		var areaRect = bar;

		var array = _data.XPValueList.ToArray();
		Array.Resize(ref array, _data.LevelMax);

		_data.XPValueList = array.ToList();
		if(_data.XPValueList.Count < _data.LevelMax) _data.XPValueList.Capacity = _data.LevelMax;		

		_data.BoxColor.a = 0.1f;
		GUI.color = _data.BoxColor;
		GUI.Box(bar, "", EditorStyle);

		
		//  ------------------------------------------------------------------------  Draw by mouse position.
		// Get the point location.
		int selection = -1;
		float xpValue = 0;

		var mousePos = new Vector2(Event.current.mousePosition.x - areaRect.x, Event.current.mousePosition.y - areaRect.y );

		if (areaRect.Contains(Event.current.mousePosition))
		{
			var xPorcentage = Mathf.InverseLerp(0, areaRect.width, mousePos.x );
			var yPorcentage = Mathf.InverseLerp(0, areaRect.height, mousePos.y );

			// Convert to level range.
			selection = Mathf.FloorToInt( Mathf.Lerp(0, _data.LevelMax, xPorcentage) );

			// Convert to xp range.
			xpValue = Mathf.FloorToInt( Mathf.Lerp(_data.XpMax, _data.XpMin, yPorcentage) );
		}

		// ----------------------------------------------------------------------- Set by curve.
		if(_data.UseCurve)
		{
			for (int i = 0; i < _data.LevelMax; i++)
			{
				var time = Mathf.InverseLerp(0, _data.LevelMax-1, i);
				_data.XPValueList[i] =  Mathf.FloorToInt( Mathf.Lerp(_data.XpMin, _data.XpMax, _data.CurveValue.Evaluate(time)  ) );
			}
		}

		//Repaint();
		// ------------------------------------------- Apply value to bar.
		if (Event.current != null)
		{
			switch (Event.current.rawType)
			{
				case EventType.MouseDrag:
					if (selection >= 0) _data.XPValueList[selection] = xpValue;
					Repaint();
					break;
				case EventType.ContextClick:
					if (selection >= 0)
                    {
						if (inputWindow != null) inputWindow.Close();
						var pos = new Vector2(this.position.x + Event.current.mousePosition.x, this.position.y + Event.current.mousePosition.y);
						inputWindow = XpManagerInputWindow.ShowInput(Mathf.FloorToInt(_data.XPValueList[selection]), pos, (int value) => 
						{
							_data.XPValueList[selection] =  Mathf.Clamp(Mathf.FloorToInt(value), _data.XpMin, (_data.XpMax));
						});
                    }
					break;
			}
		}

		// ------------------------------------------- Draw the bars.
		bar.y += bar.height;
		bar.width /= _data.LevelMax;
		bar.width -= 1;

		_data.BoxColor.a += 0.4f;
		GUI.color = _data.BoxColor;

		for (int i = 0; i < _data.LevelMax; i++)
		{
			var p = Mathf.InverseLerp(_data.XpMin, _data.XpMax, _data.XPValueList[i]);
			bar.height =  Mathf.Lerp(0, -areaRect.height, p);

			var color = GUI.color;		

			// Bars.
			if(_data.XPValueList.Count >= i)
			{
				// -----------------------------------------
				// Highlight.
				if(selection >= 0 && selection == i)	GUI.color += new Color(0,0,0,1);
				// Draw the bar;
				GUI.Box(bar,"", EditorStyle);
				// -----------------------------------------

				// -----------------------------------------
				GUI.color = color;
				GUI.color += new Color(0.4f,0.4f,0.4f,0f);
				if(selection >= 0 && selection == i)	GUI.color += new Color(0,0,0,1);

				var style = new GUIStyle(){ fontStyle = FontStyle.Bold, fontSize = 8 };

				if(_data.ShowValues)	GUI.Label(new Rect(bar.x, bar.y -14, 5, 30 ), _data.XPValueList[i]+ "", style);
				// -----------------------------------------
				
			}

			// Offset bars.
			bar.x += bar.width + 1;

			Repaint();
			GUI.color = color;	
		}

		// Stats.
		var rect = areaRect;

		rect.y += 2;
		rect.x += 10;
		var titleStyle = new GUIStyle(){ fontStyle = FontStyle.Bold , richText = true, alignment = TextAnchor.UpperLeft };
		GUI.Label(rect, "<color=white>"+_data.GraphName+"</color>"+((_data.UseCurve)?" > Controlled by curve" : "") , titleStyle);
		rect.y += 14;
		if (selection >= 0) GUI.Label(rect, "Level: <color=white>"+(selection+1)+"</color>", titleStyle);
		rect.y += 12;
		if (selection >= 0) GUI.Label(rect, "Value: <color=white>"+_data.XPValueList[selection]+"</color>", titleStyle);

		GUI.color = Color.white;

	}

	void DrawListValues(XpManager.XPGroup _data)
	{
        try
        {
			GUILayout.BeginVertical(GUILayout.Width(140));

			_data.listModeScroll = GUILayout.BeginScrollView(_data.listModeScroll, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandHeight(true));

            GUI.enabled = !_data.UseCurve;

            if (_data.UseCurve) EditorGUILayout.LabelField("* Locked by curve");

            for (int i = 0; i < _data.XPValueList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level " + (i + 1), GUILayout.Width(60));
                _data.XPValueList[i] = EditorGUILayout.FloatField(_data.XPValueList[i], GUILayout.Width(50));
				_data.XPValueList[i] = Mathf.Clamp(Mathf.FloorToInt(_data.XPValueList[i]), _data.XpMin, (_data.XpMax));
				GUILayout.EndHorizontal();
            }
            GUI.enabled = true;

            GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}
        catch
        {
			GUIUtility.ExitGUI();
		}
	}

}
