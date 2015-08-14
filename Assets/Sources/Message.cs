using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Message : MonoBehaviour
{
	/* 変数宣言
	 * InputText		:投稿コメントの内容
	 * Score			:プレイヤのスコアを管理
	 */
	private string InputText;
	private string GameTitle;
	private int Score;
	/* Start()
	 * 最初に呼ばれる初期化メソッド
	 * 変数を初期化する
	 */
	void Start ()
	{
		GameTitle = "Sort Puzzle(仮)";
	}
	/* setScore()
	 * スコアをカプセル化するメソッド
	 * スコアを格納させる．
	 * 引数
	 * InputScore		:int型のscoreをカプセル化するための変数
	 */
	public void setScore (int InputScore)
	{
		Score = InputScore;
		setScoreText ();
	}
	/* setScoreText()
	 * スコアを表示するためのテキストを管理するメソッド
	 * スコア表示を管理する
	 */
	private void setScoreText ()
	{
		GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<Text> ().text = "Score:" + Score;
	}
	/* setText()
	 * 投稿するテキストを設定するメソッド
	 * 投稿用テキストの管理を行う
	 */
	void setText ()
	{
		InputText = "「" + GameTitle + "」\nSCORE:" + Score + "\n";
		InputText += GameObject.FindGameObjectWithTag ("InputText").GetComponent<Text> ().text;
		InputText += "\n#SortPuzzleRank,#SPR";
	}
	/* SendMessageTwitter()
	 * Twitterへの投稿を管理するメソッド
	 * Twitterへの投稿を行う
	 */
	public void SendMessageTwitter ()
	{
		setText ();
		string format = "https://twiter.com/intent/tweet?&text={0}";
		string url = string.Format (format, WWW.EscapeURL (InputText));
		Application.OpenURL (url);
	}
	/* SendMessageLine()
	 * LINEへの投稿を管理するメソッド
	 * LINEへの投稿を行う
	 */
	public void SendMessageLine ()
	{
		setText ();
		Application.OpenURL ("http://line.naver.jp/R/msg/text/?" + WWW.EscapeURL (InputText, System.Text.Encoding.UTF8));
	}
	/* SendMessageTitle()
	 * ”タイトルへ”ボタンが押された時に呼び出すメソッド
	 * Titleへ画面遷移する．
	 */
	public void SendMessageTitle ()
	{
		Application.LoadLevel ("Title");
	}
}
