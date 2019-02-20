using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour {
	[SerializeField]
	private Transform tetrominoContainer;
	float fallSpeed=0.1f,fall=0f;
	int counter=0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		counter += 1;
		if (counter % 10 == 0) {
			GenerateTetromino ();
			counter = 0;
		}
		MoveDown ();
	}

	void MoveDown(){
		if (Time.time - fall >= fallSpeed) {
			for (int a = 0; a < tetrominoContainer.childCount; a++) {
				tetrominoContainer.GetChild (a).transform.position += Vector3.down;
				if (tetrominoContainer.GetChild (a).transform.position.y < -7)
					Destroy (tetrominoContainer.GetChild (a).gameObject);
			}
			fall = Time.time;
		}
			
	}

	public void GenerateTetromino()
	{
		GameObject tetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)),
			new Vector3((float)Random.Range (-7, 7), 6, 0.0f),
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

		int[] sudut = { 90, 180, 270 };
		tetromino.transform.Rotate (0, 0, sudut [Random.Range (0, 3)]);
		tetromino.GetComponent<TetrominoHandler> ().enabled = false;
		tetromino.transform.SetParent(tetrominoContainer);
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

	public void GotoGameplay(){
		SceneManager.LoadScene("Gameplay");
	}

	public void ExitGame(){
		Application.Quit();
	}
}
