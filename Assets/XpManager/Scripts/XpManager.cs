using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
public class XpManager : MonoBehaviour
{
	[System.Serializable]
	public class XPGroup
	{
		public string				GraphName = "Graph Name";

		public int					levelMax = 1;
		public int					LevelMax
		{
			get{ return levelMax; }
			set
			{
				levelMax = Mathf.Clamp(value,1,9999);
			}
		}

		public int					xpMin = 24;
		public int					XpMin
		{
			get { return xpMin; }
			set { xpMin = value; }
		}
			
		public int					xpMax = 120;
		public int					XpMax
		{
			get { return xpMax; }
			set { xpMax = value; }
		}

		public bool					isOpen = true;
		public bool					isShowListMode = true;
		public Vector2				listModeScroll = new Vector2(0,0);
		public bool					ShowValues = true;
		public bool					UseCurve = true;

		public Color				BoxColor;
		public List<float>			XPValueList = new List<float>();

		public AnimationCurve		CurveValue;

		public XPGroup()
		{
			GraphName = "_Graph Name_";
			LevelMax = 10;
			XpMin = 24;
			XpMax = 120;
			BoxColor = new Color(0,1,0,1);
			XPValueList = new List<float>(LevelMax);
			UseCurve = true;
			CurveValue = new AnimationCurve(){keys = new Keyframe[] { new Keyframe(0,0) , new Keyframe(1,1) } };
		}
	}

	[HideInInspector]
	public List<XPGroup> XPGroupList = new List<XPGroup>();

	[HideInInspector]
	public int windowId = -1;

	/// <summary>
	/// Get the XP value
	/// </summary>
	/// <param name="_graphName">The name of the graph you want to fetch the value. e.g. Strenght </param>
	/// <param name="_level">The current level of player </param>
	/// <returns>The XP Value</returns>
	public float GetXPValue(string _graphName, int _level)
	{
		var graph = XPGroupList.FirstOrDefault(x => x.GraphName == _graphName);

		if(graph == null)
		{
			Debug.Log("GraphName is invalid!");
			return -1;
		}
		else if(_level > graph.XPValueList.Count)
		{
			return graph.XPValueList[graph.XPValueList.Count];
		}
		else
			return graph.XPValueList[_level-1];
	}

 }
