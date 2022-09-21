using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMain : MonoBehaviour
{
    //UnityのInspectorから色々変更できるようにしている
    [SerializeField]
    Button buttonEasy;
    [SerializeField]
    Button buttonNormal;
    [SerializeField]
    Button buttonHard;

    private void Start()
    {
        /*TODO　8/31 例えば「Easy」のボタンをクリックされると、「StartEasy」にアクセスするということだと思うのですが、
        「AddListener」は何を意味しているのでしょうか。また、下の式はラムダ式で表していると思うのですが、普通の式で書くとどうなるのでしょうか。*/
        /*
        ANS: 9/1
        onClick.AddListenerはButtonに用意されている「クリックを押されたときにコールバックする関数」を登録する機能です。
        ラムダ式で書かない場合はActionを渡すのと同等なので
        buttonEasy.onClick.AddListener(StartEasy);
        このように「StartEasy()」ではなく「StartEasy」と記述します。
        ただし、この書き方の場合は関数に引数を渡すことが出来ないので普段はラムダ式を使用しているため手癖でラムダ式で書いています。
        今回であればラムダ式にする必要はありませんでした。
        参考：AddListenerで引数を渡す（https://blog.narumium.net/2017/04/15/%E3%80%90unity%E3%80%91addlistener%E3%81%A7%E5%BC%95%E6%95%B0%E3%82%92%E6%B8%A1%E3%81%99/）
         */
        buttonEasy.onClick.AddListener(() => StartEasy());
        buttonNormal.onClick.AddListener(() => StartNormal());
        buttonHard.onClick.AddListener(() => StartHard());
    }

   
    void StartEasy()
    {
        //縦横のマス目の数、爆弾の数（最後のもの）を設定している
        GameController.Instance.PlayStageData.SetData(6, 6, 5,1,120);
        
        OnNextScene();
    }
    void StartNormal()
    {
        GameController.Instance.PlayStageData.SetData(8, 10, 16,3,240);
        OnNextScene();
    }
    void StartHard()
    {
        GameController.Instance.PlayStageData.SetData(12, 20, 50,5,360);
        OnNextScene();
    }

    void OnNextScene()
    {
        //    TODO 8/31 全く何をやっているのか分かりません
        //「LoadScene」が意味していること、なぜ、（）の中が"Main"なのかも分かりません。これをすることによって何を可能にしているのでしょうか。
        /*
        ANS: 9/1
        この関数を呼び出すことでMainシーンへ遷移（移動）しています。
        Titleシーンからゲームが始まり、Easy等のボタンを押したらMainシーンへ移動してゲームがスタートしますが
        ここではMainシーンへの移動を行っています。
        これは調べれば出てきますのでUnityEngineから提供されているクラスや関数を使用している場合はまずは調べてみましょう。
        参考：SceneManager.LoadSceneについて（https://mogi0506.com/unity-scenemanager-loadscene/）
        */

        SceneManager.LoadScene("Main");
    }
}
