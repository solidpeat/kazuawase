using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	/* 変数宣言
	 * MainScore		:シーン間における引渡作業用のスコア
	 * ScoreObject		:引渡先のゲームオブジェクト
	 */
	public int MainScore;
	private GameObject ScoreObject;
	/* Start()
	 * 最初に呼ばれる初期化メソッド
	 * 変数を初期化する
	 */
	void Start ()
	{
		MainScore = 0;
	}
	/* Update()
	 * ゲームメインループメソッド
	 * 毎フレーム呼び出され，ゲームの中核となる管理者
	 * 現在のシーンを確認し，シーンごとに処理を実行させる．
	 */
	void Update ()
	{
		if (Application.loadedLevelName == "Result") {
			ScoreObject = GameObject.FindGameObjectWithTag ("SNS");
			ScoreObject.SendMessage ("setScore", MainScore);
		} else if (Application.loadedLevelName == "Title") {
			Destroy (this.gameObject);
		}
	}
	/* Awake()
	 * シーン遷移しても削除しないようにするメソッド
	 * シーン遷移してもゲームオブジェクトを保持するようにする．
	 */
	void Awake ()
	{
		DontDestroyOnLoad (this);
	}
	/* setMainScore()
	 * スコアを引き渡し用のスコア変数に代入するメソッド
	 * スコアを引き渡し用のスコア変数に格納する．
	 * 引数
	 * s		:int型のスコア
	 */
	private void setMainScore (int s)
	{
		MainScore = s;
	}
}
