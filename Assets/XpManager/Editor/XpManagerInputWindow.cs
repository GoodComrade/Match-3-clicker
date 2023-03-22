using UnityEngine;
using UnityEditor;
using System;

public class XpManagerInputWindow : EditorWindow
{
    public static Action<int> currentCallback;
    public static int currentValue;

    public static XpManagerInputWindow ShowInput(int value, Vector2 position, Action<int> callback)
    {
        currentValue = value;
        currentCallback = callback;
        var window = CreateInstance<XpManagerInputWindow>();
#if UNITY_5
        window.position = new Rect(position.x-40, position.y-30, 80, 50);
#else
        window.position = new Rect(position.x-40, position.y-10, 80, 50);
#endif
        window.ShowPopup();
        window.Focus();
        window.wantsMouseEnterLeaveWindow = true;
        return window;
    }

    void OnGUI()
    {
        GUI.SetNextControlName("Value");
        currentValue = EditorGUILayout.IntField( currentValue);
        EditorGUI.FocusTextInControl("Value");

        if (GUILayout.Button("Set"))
        {
            if(currentCallback != null) currentCallback.Invoke(currentValue);
            this.Close();
        }

        if (Event.current != null)
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                if(currentCallback != null) currentCallback.Invoke(currentValue);
                this.Close();
            }

            switch (Event.current.rawType)
            {
                case EventType.MouseLeaveWindow:
                    this.Close();
                    break;
            }
        }
    }
}