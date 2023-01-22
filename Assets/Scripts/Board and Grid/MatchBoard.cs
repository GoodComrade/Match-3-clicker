using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchBoard : MonoBehaviour
{
	public static MatchBoard Instance;

	[SerializeField] private List<TileScriptableData> _tileDatas = new List<TileScriptableData>();
	[SerializeField] private Tile _tile;
	[SerializeField] private int _xSize;
	[SerializeField] private int _ySize;

	private Tile[,] _tiles;

    public Tile PreviousSelected { get; private set; }

    public bool IsShifting { get; private set; }

	public Tile[,] Tiles => _tiles;

	private void Start () 
	{
		Instance = GetComponent<MatchBoard>();

		RectTransform tileRect = _tile.GetComponent<RectTransform>();

		Vector2 offset = new Vector2(tileRect.sizeDelta.x, tileRect.sizeDelta.y);
        CreateBoard(offset.x, offset.y);
    }

	public void SetPreviousSelected(Tile tile)
	{
		PreviousSelected = tile;
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
				Tile newTile = Instantiate(_tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), _tile.transform.rotation);
				_tiles[x, y] = newTile;
				newTile.SetIndex(x, y);
				newTile.transform.SetParent(transform);

				List<TileScriptableData> possibleCharacters = new List<TileScriptableData>();
				possibleCharacters.AddRange(_tileDatas);

				possibleCharacters.Remove(previousLeft[y]);
				possibleCharacters.Remove(previousBelow);

                TileScriptableData newTileData = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
				newTile.SetData(newTileData);
				previousLeft[y] = newTileData;
				previousBelow = newTileData;
			}
        }
    }

	//refactor this
	/*public IEnumerator FindNullTiles() 
	{
		for (int x = 0; x < _xSize; x++) 
		{
			for (int y = 0; y < _ySize; y++) 
			{
				if (_tiles[x, y].HasNoImage()) 
				{
					ShiftTilesDown(x, y);
                    yield return null;
                    break;
                }
			}
		}

		CheckOtherMatches();
    }

    //refactor this
    private void CheckOtherMatches()
	{
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                _tiles[x, y].ClearAllMatches();
            }
        }
    }*/

	//refactor this
	private void ShiftTilesDown(int x, int yStart, float shiftDelay = 0.03f) 
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
			//yield return new WaitForSeconds(shiftDelay);

			for (int k = 0; k < renders.Count - 1; k++) 
			{
                renders[k].SetData(renders[k + 1].Data);
				renders[k + 1].SetData(GetNewSpriteData(x, _ySize - 1));
			}
		}

		IsShifting = false;
	}

	private TileScriptableData GetNewSpriteData(int x, int y) 
	{
		List<TileScriptableData> possibleCharacters = new List<TileScriptableData>();
		possibleCharacters.AddRange(_tileDatas);

		if (x > 0) 
		{
			possibleCharacters.Remove(_tiles[x - 1, y].Data);
		}

		if (x < _xSize - 1) 
		{
			possibleCharacters.Remove(_tiles[x + 1, y].Data);
		}

		if (y > 0) 
		{
			possibleCharacters.Remove(_tiles[x, y - 1].Data);
		}

        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }
}
