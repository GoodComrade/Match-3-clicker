using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;

public class MatchBoard : MonoBehaviour
{
    public event UnityAction<float> SendReward;

    public static MatchBoard Instance;

	[SerializeField] private RectTransform _origin;
	[SerializeField] private List<TileScriptableData> _tileDatas = new List<TileScriptableData>();
	[SerializeField] private Tile _tile;
	[SerializeField] private int _xSize;
	[SerializeField] private int _ySize;

    private Tile[,] _tiles;
	private Coroutine _findNullTilesCoroutine;
	

    public Tile PreviousSelected { get; private set; }

    public bool IsShifting { get; private set; }

	public Tile[,] Tiles => _tiles;

	public List<TileScriptableData> TileDatas => _tileDatas;

    private void Start () 
	{
		Instance = GetComponent<MatchBoard>();

		for (int i = 0; i < _tileDatas.Count; i++)
			_tileDatas[i].SetBaseReward();

		RectTransform tileRect = _tile.GetComponent<RectTransform>();
		tileRect.sizeDelta = _origin.sizeDelta;

		Vector2 offset = new Vector2(_origin.sizeDelta.x, _origin.sizeDelta.y);
        CreateBoard(offset.x, offset.y);
    }

	//unsubscribing match board from tiles
	/*private void OnDisable()
	{
		for(int x = 0; x < _tiles.GetLength(0) - 1; x++)
		{
			for(int y = 0; y < _tiles.GetLength(1) - 1; y++)
			{
				_tiles[x, y].FoundMatch -= OnMatchFound;
			}
		}
	}*/

	public void StartFindNullTiles()
	{
		if (_findNullTilesCoroutine != null)
			StopCoroutine(_findNullTilesCoroutine);

		_findNullTilesCoroutine = StartCoroutine(FindNullTiles());
	}

	public void SetPreviousSelected(Tile tile)
	{
		PreviousSelected = tile;
	}

    public IEnumerator FindNullTiles(float startDelay = 1f)
    {
		yield return new WaitForSeconds(startDelay);

        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tiles[x, y].Data == null)
                {
                    yield return StartCoroutine(FindTilesToShift(x, y));
                    break;
                }
            }
        }

        CheckOtherMatches();
    }

    private void CreateBoard (float xOffset, float yOffset) 
	{
		_tiles = new Tile[_xSize, _ySize];

        float startX = transform.position.x;
		float startY = transform.position.y;

		TileScriptableData[] previousLeft = new TileScriptableData[_ySize];
        TileScriptableData previousBelow = null;

		for (int x = 0; x < _xSize; x++) 
		{
			for (int y = 0; y < _ySize; y++) 
			{
				Tile newTile = Instantiate(_tile, 
					new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), 
					_tile.transform.rotation);
				_tiles[x, y] = newTile;
				newTile.SetIndex(x, y);
                newTile.transform.SetParent(_origin, false);

                List<TileScriptableData> possibleCharacters = new List<TileScriptableData>();
				possibleCharacters.AddRange(_tileDatas);

				possibleCharacters.Remove(previousLeft[y]);
				possibleCharacters.Remove(previousBelow);

                TileScriptableData newTileData = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
				newTile.SetData(newTileData);
				newTile.FoundMatch += OnMatchFound;

				previousLeft[y] = newTileData;
				previousBelow = newTileData;
			}
        }
    }

    private void CheckOtherMatches()
	{
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                _tiles[x, y].ClearAllMatches();
            }
        }
    }

	//refactor this
	private IEnumerator FindTilesToShift(int x, int yStart, float shiftDelay = 0.01f) 
	{
		IsShifting = true;
		List<Tile> renders = new List<Tile>();
		int nullCount = 0;

		for (int y = yStart; y < _ySize; y++) 
		{
            Tile render = _tiles[x, y];

			if (render.Image.sprite == null) 
			{
				nullCount++;
			}

			renders.Add(render);
		}

		for (int i = 0; i < nullCount; i++) 
		{
            for (int k = 0; k < renders.Count; k++) 
			{
                if (k + 1 < renders.Count)
				{
                    Vector3 defaultpos = renders[k + 1].Image.transform.position;
                    renders[k + 1].Image.transform.DOMoveY(renders[k].transform.position.y, shiftDelay);
                    yield return new WaitForSeconds(shiftDelay);
                    
                    renders[k].SetData(renders[k + 1].Data);
                    renders[k + 1].Image.transform.position = defaultpos;
                    renders[k + 1].Image.sprite = null;
                }
				else
				{
                    renders[k].SetData(GetNewTileData(x, _ySize - 1));
                }
            }
		}

		IsShifting = false;
	}

	private TileScriptableData GetNewTileData(int x, int y) 
	{
		List<TileScriptableData> possibleData = new List<TileScriptableData>();
		possibleData.AddRange(_tileDatas);

		if (x > 0)
            possibleData.Remove(_tiles[x - 1, y].Data);

        if (x < _xSize - 1)
            possibleData.Remove(_tiles[x + 1, y].Data);

        if (y > 0)
            possibleData.Remove(_tiles[x, y - 1].Data);

        return possibleData[Random.Range(0, possibleData.Count)];
    }

    private void OnMatchFound(float reward)
    {
        SendReward?.Invoke(reward);
    }
}
