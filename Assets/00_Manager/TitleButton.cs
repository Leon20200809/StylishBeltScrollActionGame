using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public Button titleButton;

    // Start is called before the first frame update
    void Start()
    {
        titleButton.onClick.AddListener(TitleBack);
    }

    void TitleBack()
    {
        StartCoroutine(Kankaku());
    }

    IEnumerator Kankaku() //コルーチンメソッド変数名Kankaku
    {
        //(1.5秒の間を設ける)
        yield return new WaitForSeconds(1f);
        Debug.Log("Wait");

        //ゲームシーンへ移行
        SceneManager.LoadScene("TitleScene");
    }

}
