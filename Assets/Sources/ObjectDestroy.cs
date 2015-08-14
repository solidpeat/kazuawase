using UnityEngine;
using System.Collections;

public class ObjectDestroy : MonoBehaviour
{
	/* Update()
	 * ゲームメインループメソッド
	 * 毎フレーム呼び出され，ゲームの中核となる管理者
	 */
	void Update ()
	{
		if (this.gameObject.transform.position.y <= -1) {
			Destroy (this.gameObject);
		}
	}
}
