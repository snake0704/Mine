using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


/// <summary>
/// ゲームメイン、渡されたコールバックに応じてゲーム全体に影響を与える振る舞いを行う
/// </summary>
public class GameMain : MonoBehaviour
{
    [SerializeField]
    Stage stage;
    [SerializeField]
    Button buttonRestart;
    [SerializeField]
    Button buttonTitle;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    GameObject gameClear;
    [SerializeField]
    TextMeshProUGUI textValue;
    //add
    [SerializeField]
    TextMeshProUGUI textValue2;
  
    [SerializeField]
    TextMeshProUGUI textValue4;
    [SerializeField]
    TextMeshProUGUI textValue5;


    int maxBombCount;
    
    void Start()
    {
        buttonRestart.onClick.AddListener(() => ReStart());
        buttonTitle.onClick.AddListener(() => OnTitleScene());
        ReStart();
    }

    void ReStart()
    {
        //TODO 9/1 Instance.PlayStageDataは何を意味しているのでしょうか
        /*
        ANS: 9/1
        Instance.PlayStageData は class StageDataです。
        GameController.csに
        public StageData PlayStageData { get; private set; } 
        と書かれていて、そこにアクセスを行っています。
        レッスン中に説明を行いましたが、ここではシングルトンを使用しています。
        参考：シングルトン（https://senoriblog.com/csharp-singleton/）
         */
        int y = GameController.Instance.PlayStageData.Y;
        int x = GameController.Instance.PlayStageData.X;
        maxBombCount = GameController.Instance.PlayStageData.BombCount;
        TimeLimit2 = GameController.Instance.PlayStageData.TimeLimit;

    // ステージの初期化.
    stage.Setup(y, x, maxBombCount,GameController.Instance.PlayStageData.LifeCount,  GameController.Instance.PlayStageData.ElapsedTime,GameController.Instance.PlayStageData.TimeLimit, OnBombCountRefresh, OnGameOver, OnGameClear, OnLifeDic);
        textValue2.SetText(GameController.Instance.PlayStageData.LifeCount.ToString());
        textValue5.SetText(GameController.Instance.PlayStageData.ElapsedTime.ToString());
        textValue4.SetText(GameController.Instance.PlayStageData.TimeLimit.ToString());
        gameOver.SetActive(false);
        gameClear.SetActive(false);
    }
 // add
    void OnLifeDic(int lifeCount)
    {

        textValue2.SetText(lifeCount.ToString());
    }
    //add
    public float TimeLimit2 = GameController.Instance.PlayStageData.TimeLimit;
    void Update()
    {
        if (!stage.IsInit)
        {
            return;
        }
        if(stage.StopCount)
        {
            return;
        }
        if (TimeLimit2 > 0)
        {
            TimeLimit2 -= Time.deltaTime;
            if (TimeLimit2 <= 0)
            {
               
                stage.GameOverAction();
            }
        }

        else
        {
            TimeLimit2 = 0;

        }
        textValue4.SetText(TimeLimit2.ToString("0"));
    }

    //ゲームオーバー時に呼ばれる
    void OnGameOver()
    {
        gameOver.SetActive(true);
        stage.GameEnd();
        float Limit = GameController.Instance.PlayStageData.TimeLimit;
        float clearTime = TimeLimit2;
        //経過時間を計算
        float elapsedTime = Limit - clearTime;
        textValue5.SetText(elapsedTime.ToString("Elapsed Time:"+"0"));
        stage.StopCount = true;
      
    }

    // ゲームクリア時に呼ばれる
    void OnGameClear()
    {
        gameClear.SetActive(true);
        stage.GameEnd();
        //add
        float Limit = GameController.Instance.PlayStageData.TimeLimit;
        float clearTime = TimeLimit2;
        //経過時間を計算
        float elapsedTime = Limit - clearTime;
        textValue5.SetText(elapsedTime.ToString("0"));
        stage.StopCount = true;
     
    }

    // 爆弾のカウント表示を更新する
    void OnBombCountRefresh(int count)
    {
        textValue.SetText(count.ToString());
    }

   
    

    
void OnTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
  
}
