using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour 
{
    //private static Tile _previousSelected = null;

    private Button _button;
	private bool _isSelected = false;
    private bool _matchFound = false;

    public TileScriptableData Data { get; private set; }
    public Image Image { get; private set; }

    public Vector2 Index { get; private set; }
    //public int Xindex { get; private set; }
    //public int Yindex { get; private set; }


    private Vector2[] _adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2[] _verticalDirections = new Vector2[] { Vector2.up, Vector2.down};
    private Vector2[] _horizontalDirections = new Vector2[] { Vector2.left, Vector2.right };

    private void Awake() 
	{
        Image = GetComponent<Image>();
		_button = GetComponent<Button>();

		if (_button != null)
			_button.onClick.AddListener(OnSelected);
    }

	private void OnDisable()
	{
		_button.onClick.RemoveListener(OnSelected);	
	}

	public void SetIndex(int x, int y)
	{
		Index = new Vector2(x, y);
	}

	public void SetData(TileScriptableData data)
	{
        Data = data;
        Image.sprite = data.Tile;
	}

	/*public Sprite GetSprite()
	{
		if(Image.sprite != null)
			return Image.sprite;
		else
			return null;
	}*/

	public bool HasNoImage()
	{
		if (Image.sprite == null)
			return true;
		else
			return false;
	}

	private void Select() 
	{
		_isSelected = true;
        MatchBoard.Instance.SetPreviousSelected(this);
	}

	private void Deselect() 
	{
        _isSelected = false;
        MatchBoard.Instance.SetPreviousSelected(null);
    }

	void OnSelected() 
	{
        Tile previousSelected = MatchBoard.Instance.PreviousSelected;

		if (Image.sprite == null || MatchBoard.Instance.IsShifting)
            return;

        if (_isSelected) 
		{
			Deselect();
			return;
		}

        if (previousSelected == null)
        { 
            Select();
            Debug.Log("Selected first");
        }
        else
        {
            //TODO: check adjacentment of previous selected tile.
            // If true - swap tiles and launch match finding logic.
            // If false - deselect previous selected and set current tile as selected.

            if (TryGetAllAdjacentTiles(_adjacentDirections))
            {
                Debug.Log("Adjacent");
                SwapSprite(MatchBoard.Instance.PreviousSelected);
                MatchBoard.Instance.PreviousSelected.Deselect();
                //TODO: launch match finding logic here
            }
            else
            {
                MatchBoard.Instance.PreviousSelected.Deselect();
                Select();
            }
        }
    }

	private void SwapSprite(Tile tileSwapWith) 
	{
		if (Data.TileType == tileSwapWith.Data.TileType)
            return;

        TileScriptableData tempData = tileSwapWith.Data;
        tileSwapWith.SetData(Data);
        SetData(tempData);
        Debug.Log("swapped");
    }

    private bool TryGetAllAdjacentTiles(Vector2[] directions) 
	{
        List<Vector2> adjacentTiles = new List<Vector2>();

        for (int i = 0; i < directions.Length; i++)
        {
            adjacentTiles.Add(FindIndex(directions[i], 1));
        }

        if (adjacentTiles.Contains(MatchBoard.Instance.PreviousSelected.Index))
            return true;
        else
            return false;
    }

    //refactor this
    public void ClearAllMatches() 
	{
		if (Image.sprite == null)
			return;

		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });

		if (_matchFound) 
		{
			Debug.Log("Match");
            Image.sprite = null;
			_matchFound = false;

			//MatchBoard.Instance.StartFindNullTiles();
		}
	}

    //refactor this
    private void ClearMatch(Vector2[] paths)
    {
        List<Tile> matchingTiles = new List<Tile>();

        for (int i = 0; i < paths.Length; i++)
        {
            //matchingTiles.AddRange(FindMatch(paths[i]));
        }

        if (matchingTiles.Count >= 2)
        {
            for (int i = 0; i < matchingTiles.Count; i++)
            {
                matchingTiles[i].Image.sprite = null;
            }

            _matchFound = true;
        }
    }

    //refactor this
    /*private List<Tile> FindMatch(Vector2 castDir)
    {
        int matchDepth = 1;
        List<Tile> matchingTiles = new List<Tile>();

        Vector2 directionToMatchFind = SetDirectionToMatchFind(castDir, matchDepth);
        Tile tileToCheck = MatchBoard.Instance.GetTile((int)directionToMatchFind.x, (int)directionToMatchFind.y);

        while (tileToCheck != null && tileToCheck.GetSprite() == Image.sprite)
        {
            Debug.Log(directionToMatchFind);
            matchingTiles.Add(tileToCheck);
            matchDepth++;
            directionToMatchFind = SetDirectionToMatchFind(castDir, matchDepth);
            tileToCheck = MatchBoard.Instance.GetTile((int)directionToMatchFind.x, (int)directionToMatchFind.y);
        }

        return matchingTiles;
    }*/

    /*private Vector2 SetDirectionToMatchFind(Vector2 castDir, int castRange)
	{
        Vector2 directionToMatchFind = new Vector2();

        if (castDir == Vector2.up || castDir == Vector2.down)
            directionToMatchFind = new Vector2(Xindex, FindIndex(castDir, castRange));

        if (castDir == Vector2.left || castDir == Vector2.right)
            directionToMatchFind = new Vector2(FindIndex(castDir, castRange), Yindex);

		return directionToMatchFind;
    }*/

	private Vector2 FindIndex(Vector2 castDir, int castRange)
	{
        Vector2 tempIndex = new Vector2();

        switch (castDir)
        {
            case Vector2 v when v.Equals(Vector2.up):
                tempIndex = new Vector2((int)Index.x, (int)Index.y + castRange);
                break;

            case Vector2 v when v.Equals(Vector2.down):
                tempIndex = new Vector2((int)Index.x, (int)Index.y - castRange);
                break;

            case Vector2 v when v.Equals(Vector2.left):
                tempIndex = new Vector2((int)Index.x - castRange, (int)Index.y);
                break;

            case Vector2 v when v.Equals(Vector2.right):
                tempIndex = new Vector2((int)Index.x + castRange, (int)Index.y);
                break;
        }

		if (tempIndex.x < 0)
			tempIndex.x = 0;

        if (tempIndex.y < 0)
            tempIndex.y = 0;

        return tempIndex;
    }
}