using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
	/* 変数宣言
	 * PreFabObjects		:GameObject型の生成用ゲームオブジェクトを管理するList配列
	 * PreFabObjectsName	:string型の生成用ゲームオブジェクトの名前を管理する配列
	 * PreFabMaterials		:Material型の生成用ゲームオブジェクトに適応するためのマテリアルを管理するList配列
	 * removableList		:削除予定のオブジェクトを格納するList配列
	 * bombPrefab			:爆弾のオブジェクトを格納する
	 * currentName			:選択したオブジェクトの名前
	 * ObjectMax			:ゲーム上に存在可能なゲームオブジェクトの最大数
	 * ObjectNum			:現在存在しているゲームオブジェクト数
	 * TotalScore			:スコアの合計値を管理
	 * timeLimit			:タイムリミットを格納
	 * countTime			:カウントダウンの数値
	 * count				:カウント値
	 * answar				:目標合計値
	 * sum					:合計値
	 * FeverCount			:フィーバーをカウントする
	 * MaxFeverCount		:フィーバーの最大値
	 * MINfever				:フィーバーオブジェクトのx座標初期位置
	 * ObjectSetFlag		:オブジェクトをセットするフェーズかどうか
	 * IsPlayingFlag		:プレイヤが操作可能なフェーズかどうか
	 * IsDraggingFlag		:ドラック中かどうか
	 * IsFeverFlag			:フィーバー中かどうか
	 * FirstBall			:最初に選択したゲームオブジェクト
	 * LastObj				:最後に選択したゲームオブジェクト
	 * feverObj				:フィーバーゲージのゲームオブジェクト
	 * scoreObj				:スコアを別シーンに送るためのゲームオブジェクト
	 * gameObj				:ゲームオブジェクト
	 * timerText			:タイマーを表示するゲームオブジェクトのTextコンポーネント
	 * scoreText			:スコアを表示するゲームオブジェクトのTextコンポーネント
	 */
	private List<GameObject> PreFabObjects = new List<GameObject> ();
	private string[] PreFabObjectsName = new string[]{ "Sphere2D" };
	private List<Sprite> PreFabMaterials = new List<Sprite> ();
	private List<GameObject> removableList = new List<GameObject> ();
	private GameObject bombPrefab;
	private string currentName;
	private int ObjectMax;
	private int ObjectNum;
	private int TotalScore;
	private int timeLimit;
	private int countTime;
	private int count;
	private int answar;
	private int sum;
	private float FeverCount;
	private float MaxFeverCount;
	private float MINfever;
	private bool ObjectSetFlag;
	private bool IsPlayingFlag;
	private bool IsDraggingFlag;
	private bool IsFeverFlag;
	private GameObject FirstObj;
	private GameObject LastObj;
	private GameObject feverObj;
	private GameObject scoreObj;
	private GameObject[] gameObj;
	private Text timerText;
	private Text scoreText;
	/* setPreFabObjects()
	 * PreFabObjectsに自動格納するメソッド
	 * PreFabObjectsNameにあるすべてのオブジェクト名をAssets/Resources/Objects/の中から，
	 * PreFabObjectsに自動格納する．
	 */
	private void setPreFabObjects ()
	{
		for (int i = 0; i < PreFabObjectsName.Length; i++) {
			PreFabObjects.Add ((GameObject)Resources.Load ("Objects/" + PreFabObjectsName [i]));
		}
	}
	/* setPreFabMaterials()
	 * PreFabMaterialsに自動格納するメソッド
	 * PreFabMaterialsNameにあるすべてのオブジェクト名をAssets/Resources/Materials/の中から，
	 * PreFabMaterialsに自動格納する．
	 */
	private void setPreFabMaterials ()
	{
		for (int j = 0; j < PreFabObjectsName.Length; j++) {
			for (int i = 0; i < 5; i++) {
				PreFabMaterials.Add (GetSprite ("Materials/" + PreFabObjectsName [j], PreFabObjectsName [j] + (i + 1)));
			}
		}
	}
	/* GetSprite(string fileName, string spriteName)
	 * ディレクトリからSpritを取得するメソッド
	 * fileNameにあるspriteNameで指定したSpriteファイルを返す．
	 * 引数
	 * fileName		:ディレクトリ名
	 * spriteName	:ファイル名
	 */
	private Sprite GetSprite (string fileName, string spriteName)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite> (fileName);
		return System.Array.Find<Sprite> (sprites, (sprite) => sprite.name.Equals (spriteName));
	}
	/* DropObjects(int length)
	 * ゲームオブジェクトのドロップ管理メソッド
	 * ゲームオブジェクトのドロップ管理を行う．
	 * 引数
	 * length	:int型のドロップするべき数
	 */
	private void DropObjects (int length)
	{
		for (int i = 0; i < length; i++) {
			StartCoroutine (WaitTime (i));
		}
	}
	/* WaitTime(int i)
	 * ゲームオブジェクトのドロップメソッド
	 * 一定時間をWaitした後，ゲームオブジェクトのドロップを行う．
	 * 引数
	 * i		:int型の１フェーズで呼び出されたこのメソッドの順番
	 */
	private IEnumerator WaitTime (int i)
	{
		yield return new WaitForSeconds (10 * i * Time.deltaTime);
		GameObject obj = (GameObject)Instantiate (RandomObject (), setPosition (), Quaternion.identity);
		float r = (float)Random.Range (-40, 40);
		obj.transform.eulerAngles = new Vector3 (0, 0, r);
		int q = 0;
		for (int j = 0; j < PreFabObjectsName.Length; j++) {
			if (PreFabObjectsName [j] + "(Clone)" == obj.name) {
				q = j;
				break;
			}
		}
		obj.name = PreFabObjectsName [q];
		RandomMaterial (obj);
	}
	/* RandomObject()
	 * ゲームオブジェクトをランダムで選択するメソッド
	 * ゲームオブジェクトをランダムで返す
	 */
	private GameObject RandomObject ()
	{
		return PreFabObjects [Random.Range (0, PreFabObjects.Count)];
	}
	/* setPosition()
	 * ゲームオブジェクトの生成位置をセットするメソッド
	 * ゲームオブジェクトの生成位置を返す
	 */
	private Vector3 setPosition ()
	{
		Vector3 Position;
		Position.x = Random.Range (-2, 2);
		Position.y = 13;
		Position.z = 0;
		return Position;
	}
	/* RandomMaterial(GameObject GO)
	 * ゲームオブジェクトのマテリアルを選択するメソッド
	 * ゲームオブジェクトのマテリアルをランダムで選択して返す
	 * 引数
	 * GO		:GameObject型のマテリアルをセットするゲームオブジェクト
	 */
	private void RandomMaterial (GameObject GO)
	{
//		int n = PreFabObjects.IndexOf(GO);
		int q = 0;
		int r = Random.Range (0, 5);
		for (int i = 0; i < PreFabObjectsName.Length; i++) {
			if (PreFabObjectsName [i] == GO.name) {
				q = i;
			}
		}
		GO.GetComponent<SpriteRenderer> ().sprite = PreFabMaterials [(q * 6) + r];
	}
	/* Start()
	 * 初期化メソッド
	 * 変数すべてを初期化する．
	 */
	void Start ()
	{
		ObjectSetFlag = true;
		IsPlayingFlag = false;
		IsDraggingFlag = false;
		IsFeverFlag = false;
		ObjectMax = 50;
		ObjectNum = 0;
		TotalScore = 0;
		timeLimit = 60;
		countTime = 5;
		count = 0;
		answar = 10;
		sum = 0;
		FeverCount = 0.0f;
		MaxFeverCount = 30.0f;
		setPreFabObjects ();
		setPreFabMaterials ();
		bombPrefab = (GameObject)Resources.Load ("Objects/Bom");
		timerText = GameObject.FindGameObjectWithTag ("TimerText").GetComponent<Text> ();
		scoreText = GameObject.FindGameObjectWithTag ("scoreText").GetComponent<Text> ();
		feverObj = GameObject.FindGameObjectWithTag ("feverObj");
		scoreObj = GameObject.FindGameObjectWithTag ("scoreObject");
		MINfever = feverObj.GetComponent<RectTransform> ().position.x;
		StartCoroutine (CountDown ());
		if (ObjectSetFlag) {
			ObjectSetFlag = false;
			DropObjects (ObjectMax - ObjectNum);
		}
	}
	/* Update()
	 * ゲームメインループメソッド
	 * 毎フレーム呼び出され，ゲームの中核となる管理者
	 */
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			OnClick ();
		}
		if (IsPlayingFlag) {
			float posX = FeverCount * (-1) * MINfever / MaxFeverCount;
			feverObj.GetComponent<RectTransform> ().position = new Vector3 (MINfever + posX, feverObj.GetComponent<RectTransform> ().position.y, feverObj.GetComponent<RectTransform> ().position.z);
			if (Input.GetMouseButton (0) && FirstObj == null) {
				OnDragStart ();
			} else if (Input.GetMouseButtonUp (0)) {
				OnDragEnd ();
			} else if (FirstObj != null) {
				OnDragging ();
			}
		}
	}
	/* UpdateFever()
	 * フィーバー要素を管理するメソッド
	 * フィーバー要素の管理を行う．
	 */
	private IEnumerator UpdateFever ()
	{
		while (IsPlayingFlag) {
			yield return new WaitForSeconds (0.05f);
			if (!IsFeverFlag) {
				FeverCount -= 1 / 80.0f;
				if (FeverCount < 0)
					FeverCount = 0;
			} else {
				FeverCount -= MaxFeverCount / 8.0f / 20.0f;
				if (FeverCount < 0) {
					FeverCount = 0;
					IsFeverFlag = false;
				}
			}
		}
	}
	/* StartCount()
	 * ゲーム開始前のカウントダウンを行うメソッド
	 * ゲーム開始前のカウントダウンを行う
	 */
	private IEnumerator CountDown ()
	{
		count = countTime;
		while (count > 0) {
			timerText.text = count.ToString ();
			yield return new WaitForSeconds (1);
			count -= 1;
		}
		timerText.text = "Start!";
		IsPlayingFlag = true;
		StartCoroutine (UpdateFever ());
		yield return new WaitForSeconds (1);
		StartCoroutine (StartTimer ());
	}
	/* StartTimer()
	 * ゲーム中のカウントダウンを行うメソッド
	 * ゲーム中のカウントダウンを行う
	 */
	private IEnumerator StartTimer ()
	{
		count = timeLimit;
		while (count > 0) {
			timerText.text = count.ToString ();
			yield return new WaitForSeconds (1);
			count -= 1;
		}
		timerText.text = "Finish";
		OnDragEnd ();
		IsPlayingFlag = false;
		StartCoroutine (setResult ());
	}
	/* OnClick()
	 * クリック時に処理を実行するメソッド
	 * クリックしたオブジェクトがBomの場合，
	 * ClearBombメソッドを実行する
	 */
	private void OnClick ()
	{
		Collider2D col = GetCurrentHitCollider ();
		if (col != null) {
			GameObject colObj = col.gameObject;
			if (colObj.name == "Bomb" && IsPlayingFlag && !IsDraggingFlag) {
				ClearBomb (colObj);
			}
		}
	}
	/* OnDragStart()
	 * ドラッグ開始時に処理を実行するメソッド
	 */
	private void OnDragStart ()
	{
		Collider2D col = GetCurrentHitCollider ();
		if (col != null) {
			GameObject colObj = col.gameObject;
			if (colObj.tag == "Object") {
				sum = 0;
				removableList = new List<GameObject> ();
				IsDraggingFlag = true;
				FirstObj = colObj;
				currentName = colObj.name;
				PushToList (colObj);
			}
		}
	}
	/* OnDragging()
	 * ドラッグ中に処理を実行するメソッド
	 */
	private void OnDragging ()
	{
		Collider2D col = GetCurrentHitCollider ();
		if (col != null) {
			GameObject colObj = col.gameObject;
			if (colObj.name == currentName) {
				if (LastObj != colObj) {
					float dist = Vector2.Distance (LastObj.transform.position, colObj.transform.position);
					if (dist <= 1.5) {
						PushToList (colObj);
					}
				}
			}
		}
	}
	/* OnDragEnd()
	 * ドラッグ解除時に処理を実行するメソッド
	 */
	private void OnDragEnd ()
	{
		if (FirstObj != null) {
			int length = removableList.Count;
			if (length >= 3) {
				if(Decision()){
					ClearRemovables (0);
				}else{
					for (int i = 0; i < length; i++) {
						GameObject ListedObj = removableList [i];
						ChangeColor (ListedObj, 1.0f);
						ListedObj.name = ListedObj.name.Substring (1, ListedObj.name.Length - 1);
					}
				}
			} else {
				for (int i = 0; i < length; i++) {
					GameObject ListedObj = removableList [i];
					ChangeColor (ListedObj, 1.0f);
					ListedObj.name = ListedObj.name.Substring (1, ListedObj.name.Length - 1);
				}
			}
			FirstObj = null;
		}
	}
	/* ClearRemovables(int mode)
	 * ゲームオブジェクトの削除を管理するメソッド
	 * ゲームオブジェクトの削除を管理する．
	 * 引数
	 * mode		:int型の削除モード(0:ボムによる削除,1:ユーザによる削除)
	 */
	private void ClearRemovables (int mode)
	{
		if (removableList != null) {
			int length = removableList.Count;
			for (int i = 0; i < length; i++) {
				if (i == length - 1 && mode == 0 && length > 6) {
					GameObject bomb = Instantiate (bombPrefab);
					GameObject obj = removableList [i];
					bomb.transform.position = obj.transform.position;
					bomb.name = "Bomb";
				}

				Destroy (removableList [i]);
			}
			int mult = 1;
			if (IsFeverFlag) {
				mult = 3;
			}
			TotalScore += ((CalculateBaseScore (length) + 50 * length)) * mult;
			count += (int)(length/2);
			scoreText.text = TotalScore.ToString ();
			Addfever (length);
			IsDraggingFlag = false;
			DropObjects (length);
		}
	}
	/* ClearBomb(GameObject colObj)
	 * ボムによるゲームオブジェクトの削除を管理するメソッド
	 * ボムによるゲームオブジェクトの削除を管理する．
	 * 引数
	 * colObj		:GameObject型のクリックされたオブジェクト
	 */
	private void ClearBomb (GameObject colObj)
	{
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Object");
		removableList = new List<GameObject> ();
		for (int i = 0; i < balls.Length; i++) {
			float dist = Vector2.Distance (colObj.transform.position, balls [i].transform.position); //ボムと各ボールの距離を計算
			if (dist < 1.8) {
				removableList.Add (balls [i]);
			}
		}
		ClearRemovables (1);
		Destroy (colObj);
	}
	/* CalculateBaseScore(int n)
	 * スコアの基礎値を返すメソッド
	 * スコアの基礎値を返す．．
	 * 引数
	 * n		:int型のオブジェクトの削除数
	 */
	private int CalculateBaseScore (int n)
	{
		int tempScore = 50 * n * (n + 1) - 300;
		return tempScore;
	}
	/* Addfever(int num)
	 * フィーバー状態になるまでを管理するメソッド
	 * フィーバー状態になるまでを管理する．
	 * 引数
	 * num		:int型のオブジェクトの削除数
	 */
	private void Addfever (int num)
	{
		if (!IsFeverFlag) {
			FeverCount += num;
			if (FeverCount > MaxFeverCount) {
				FeverCount = MaxFeverCount;
				IsFeverFlag = true;
			}
		}
	}
	/* PushToList(GameObject obj)
	 * 削除Listにゲームオブジェクトを追加するメソッド
	 * 削除Listにゲームオブジェクトを追加する．
	 * 引数
	 * obj		:削除Listに追加するゲームオブジェクト
	 */
	private void PushToList (GameObject obj)
	{
		LastObj = obj;
		ChangeColor (LastObj, 0.5f);
		removableList.Add (obj);
		obj.name = "_" + obj.name;
	}
	/* ChangeColor(GameObject obj, float transparency)
	 * ゲームオブジェクトの透明度を変更するメソッド
	 * ゲームオブジェクトの透明度を変更する．
	 * 引数
	 * obj				:透明度を変更するゲームオブジェクト
	 * transparency		:ゲームオブジェクトに適応させたい透明度
	 */
	private void ChangeColor (GameObject obj, float transparency)
	{
		SpriteRenderer ObjTexture = obj.GetComponent<SpriteRenderer> ();
		ObjTexture.color = new Color (ObjTexture.color.r, ObjTexture.color.g, ObjTexture.color.b, transparency);
	}
	/* GetCurrentHitCollider()
	 * マウスオーバーしているオブジェクトを取得するメソッド
	 * マウスの位置からマウスオーバーしているオブジェクトを取得し返す．
	 */
	private Collider2D GetCurrentHitCollider ()
	{
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector3.zero);
		return hit.collider;
	}
	/* allDestroy()
	 * GameObjectをすべて削除するメソッド
	 * 現在存在するゲームオブジェクトをすべて削除する．
	 */
	public void allDestroy ()
	{
		gameObj = GameObject.FindGameObjectsWithTag ("Object");
		int length = gameObj.Length;
		for (int i = 0; i < length; i++) {
			Destroy (gameObj [i]);
		}
		DropObjects (length);
	}
	/* Decision()
	 * 削除判定するメソッド
	 * 選択合計値がanswarとおなじか判断し返す。．
	 */
	private bool Decision(){
		sum = 0;
		for(int i = 0; i < removableList.Count; i++){
			Sprite ListedObj = removableList [i].GetComponent<SpriteRenderer>().sprite;
			sum += int.Parse(ListedObj.name.Substring(ListedObj.name.Length - 1));
			Debug.Log("sum["+i+"]:"+sum);
			if(sum > answar){
				return false;
			}
		}
		if (sum == answar) {
			return true;
		} else {
			return false;
		}
	}
	/* setResult()
	 * リザルト画面を一定時間後に呼び出すメソッド
	 * リザルト画面を一定時間後に呼び出す．
	 */
	public IEnumerator setResult ()
	{
		scoreObj.SendMessage ("setMainScore", TotalScore);
		yield return new WaitForSeconds (2 * Time.deltaTime);
		Result ();
	}
	/* Reset()
	 * ゲーム画面を呼ぶメソッド
	 * ゲーム画面を呼び出す
	 */
	private void Reset ()
	{
		Application.LoadLevel ("Game");
	}
	/* Title()
	 * タイトル画面を呼ぶメソッド
	 * タイトル画面を呼び出す
	 */
	private void Title ()
	{
		Application.LoadLevel ("Title");
	}
	/* Result()
	 * リザルト画面を呼ぶメソッド
	 * リザルト画面を呼び出す
	 */
	private void Result ()
	{
		Application.LoadLevel ("Result");
	}
}
