using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

	[SerializeField]
	private int score = 0,highscore=0,rowsdestroy=0;

	[SerializeField]
	private Transform tetrominoContainer,NextTetro;

	[SerializeField]
	private int scoreOneLine = 40;

	[SerializeField]
	private int scoreTwoLine = 100;

	[SerializeField]
	private int scoreThreeLine = 300;

	[SerializeField]
	private int scoreFourLine = 1200;

	private int lengthDestroyRows = 0;

	public static int gridWidth = 10;
	public static int gridHeight = 20;

	public static Transform[,] grid = new Transform[gridWidth, gridHeight];
	public Text txtScore,txtRows,txtHighscore,txtGameover;

	public GameObject panelGameover;

	private void Start()
	{	
		panelGameover.SetActive(false);
		GenerateTetromino();
		highscore = PlayerPrefs.GetInt ("highscore");
		txtHighscore.text = "" + highscore;
	}

	private void Update()
	{
		if (Input.GetKeyUp (KeyCode.Space))
			ReplayGame ();
	}

	private void UpdateScore()
	{
		switch (lengthDestroyRows)
		{
		case 1:
			score += scoreOneLine;
			break;
		case 2:
			score += scoreTwoLine;
			break;
		case 3:
			score += scoreThreeLine;
			break;
		case 4:
			score += scoreFourLine;
			break;
		}

		rowsdestroy += lengthDestroyRows;
		txtScore.text = ""+score;
		txtRows.text = ""+rowsdestroy;

		lengthDestroyRows = 0;
	}

	private bool IsRowFullAt(int y)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			if (grid[x, y] == null)
				return false;
		}

		lengthDestroyRows++;

		return true;
	}

	private void DestroyRowAt(int y)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			Destroy(grid[x, y].gameObject);

			grid[x, y] = null;
		}
	}

	private void MoveRowDown(int y)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			if (grid[x, y] != null)
			{
				grid[x, y - 1] = grid[x, y];
				grid[x, y] = null;
				grid[x, y - 1].position += Vector3.down;

			}
		}
	}

	private void MoveAllRowsDown(int y)
	{
		for (int i = y; i < gridHeight; i++)
			MoveRowDown(i);
	}

	private string GetRandomTetromino()
	{
		int val = Random.Range(0, 7);
		string tetrominoName = "TetrominoT";

		switch (val)
		{
		case 0:
			tetrominoName = "TetrominoI";
			break;
		case 1:
			tetrominoName = "TetrominoJ";
			break;
		case 2:
			tetrominoName = "TetrominoL";
			break;
		case 3:
			tetrominoName = "TetrominoT";
			break;
		case 4:
			tetrominoName = "TetrominoO";
			break;
		case 5:
			tetrominoName = "TetrominoS";
			break;
		case 6:
			tetrominoName = "TetrominoZ";
			break;
		}

		return "Prefabs/" + tetrominoName;
	}

	public void DestroyRow()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			if (IsRowFullAt(y))
			{
				DestroyRowAt(y);

				MoveAllRowsDown(y + 1);

				y--;
			}
		}

		UpdateScore();
	}

	public bool IsReactLimitGrid(TetrominoHandler tetromino)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			foreach (Transform mino in tetromino.transform)
			{
				Vector3 pos = Round(mino.position);

				if (pos.y >= gridHeight - 1 &&
					!tetromino.isActiveAndEnabled)
				{
					return true;
				}
			}
		}

		return false;
	}

	public void GameOver(TetrominoHandler tetromino)
	{
		Debug.Log("Game is Over! Your score is " + score);
		panelGameover.SetActive(true);
		txtGameover.text ="Your score is : "+score;
		if (score > PlayerPrefs.GetInt ("highscore")) {
			highscore = score;
			PlayerPrefs.SetInt ("highscore", highscore);
			txtHighscore.text = "" + highscore;
		}

		Destroy(tetrominoContainer.gameObject);
	}

	public Transform GetTransformAtGridPosition(Vector3 pos)
	{
		if (pos.y > gridHeight - 1)
			return null;
		else
			return grid[(int)pos.x, (int)pos.y];
	}

	public void UpdateGrid(TetrominoHandler tetromino)
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				if (grid[x, y] != null)
				{
					if (grid[x, y].parent == tetromino.transform)
						grid[x, y] = null;
				}
			}
		}

		foreach (Transform mino in tetromino.transform)
		{
			Vector3 pos = Round(mino.position);

			if (pos.y < gridHeight)
				grid[(int)pos.x, (int)pos.y] = mino;
		}
	}

	public void GenerateTetromino()
	{	
		GameObject tetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)),
			new Vector3(5.0f, 18.0f, 0.0f),
			Quaternion.identity);
		Color[] warna={new Color32(255, 0, 0,1),
			new Color32(255, 165, 0,1),
			new Color32(255, 255, 0,1),
			new Color32(0, 128, 0,1),
			new Color32(0, 0, 255,1),
			new Color32(75, 0, 130,1),
			new Color32(238, 130, 238,1)};
		Color warnabaru=warna[Random.Range(0,7)];
		foreach (Transform mino in tetromino.transform) {
			mino.GetComponent<Renderer> ().material.color = warnabaru;
		}
		tetromino.transform.SetParent(tetrominoContainer);

	}
		

	public bool IsTetrominoInsideAGrid(Vector3 pos)
	{
		return (
			(int)pos.x >= 0 &&
			(int)pos.x < gridWidth &&
			(int)pos.y >= 0
		);
	}

	public Vector3 Round(Vector3 pos)
	{
		return new Vector3(
			Mathf.Round(pos.x),
			Mathf.Round(pos.y),
			Mathf.Round(pos.z)
		);
	}

	public void GotoMainmenu(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
	}
	public void ReplayGame(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
	}

}
