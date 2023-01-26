using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Tile : MonoBehaviour 
{
    private Button _button;
	private bool _isSelected = false;
    private bool _matchFound = false;

    private Vector2[] _adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2[] _verticalDirections = new Vector2[] { Vector2.up, Vector2.down };
    private Vector2[] _horizontalDirections = new Vector2[] { Vector2.left, Vector2.right };

    public TileScriptableData Data { get; private set; }
    public Image Image { get; private set; }
    public Vector2 Index { get; private set; }

    public event UnityAction<float> FoundMatch;

    private void Awake() 
	{
        Image = GetComponent<Image>();
		_button = GetComponent<Button>();

		if (_button != null)
			_button.onClick.AddListener(OnSelected);
    }

	/*private void OnDisable()
	{
		_button.onClick.RemoveListener(OnSelected);	
	}*/

	public void SetIndex(int x, int y)
	{
		Index = new Vector2(x, y);
	}

	public void SetData(TileScriptableData data)
	{
        if(data == null)
        {
            Data = null;
            Image.sprite = null;
        }
        else
        {
            Data = data;
            Image.sprite = data.Tile;
        }
	}

    public void InvokeRewarding()
    {
        Debug.Log(Data.Reward);
        FoundMatch?.Invoke(Data.Reward);
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

	private void OnSelected() 
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

            if (TryGetAllAdjacentTiles(_adjacentDirections))
            {
                Debug.Log("Adjacent");
                SwapSprite(MatchBoard.Instance.PreviousSelected);
                MatchBoard.Instance.PreviousSelected.ClearAllMatches();
                MatchBoard.Instance.PreviousSelected.Deselect();
                ClearAllMatches();
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

    public void ClearAllMatches() 
	{
		if (Image.sprite == null)
			return;

		ClearMatch(_horizontalDirections);
		ClearMatch(_verticalDirections);

		if (_matchFound) 
		{
            InvokeRewarding();
            Debug.Log("Match");
            SetData(null);
			_matchFound = false;
            MatchBoard.Instance.StartFindNullTiles();
		}
	}

    private void ClearMatch(Vector2[] paths)
    {
        List<Tile> matchingTiles = new List<Tile>();

        for (int i = 0; i < paths.Length; i++)
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }

        if (matchingTiles.Count < 0 || matchingTiles == null)
            return;

        if (matchingTiles.Count >= 2)
        {

            for (int i = 0; i < matchingTiles.Count; i++)
            {
                matchingTiles[i].InvokeRewarding();
                matchingTiles[i].SetData(null);
            }

            _matchFound = true;
        }
    }

    private List<Tile> FindMatch(Vector2 castDir)
    {
        int matchDepth = 1;
        Tile[,] allTiles = MatchBoard.Instance.Tiles;
        Vector2 maxLenght = new Vector2(allTiles.GetLength(0) - 1, allTiles.GetLength(1) - 1);
        List <Tile> matchingTiles = new List<Tile>();

        Vector2 directionToMatchFind = FindIndex(castDir, matchDepth);

        if (directionToMatchFind.x > maxLenght.x || directionToMatchFind.y > maxLenght.y
                || (directionToMatchFind.x < 0 || directionToMatchFind.y < 0))
            return matchingTiles;

        Tile tileToCheck = allTiles[(int)directionToMatchFind.x, (int)directionToMatchFind.y];

        while (tileToCheck.Data != null && tileToCheck.Data.TileType == Data.TileType)
        {
            matchingTiles.Add(tileToCheck);
            matchDepth++;
            directionToMatchFind = FindIndex(castDir, matchDepth);

            if (directionToMatchFind.x > maxLenght.x || directionToMatchFind.y > maxLenght.y
                || (directionToMatchFind.x < 0 || directionToMatchFind.y < 0))
                break;

            tileToCheck = allTiles[(int)directionToMatchFind.x, (int)directionToMatchFind.y];
        }

        return matchingTiles;
    }

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
			tempIndex.x = -1;

        if (tempIndex.y < 0)
            tempIndex.y = -1;

        return tempIndex;
    }
}