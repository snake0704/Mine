using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲームコントローラーはシーンを移動しても必ず生存するグローバルなクラス
public class GameController
{
    #region シングルトン
    private static GameController instance = new GameController();
    public static GameController Instance => instance;

    private GameController()
    {
        // シングルトン実装.
    }
    #endregion

    /// <summary>
    /// タイトルからゲームメインに渡すデータ
    /// </summary>
    public class StageData
    {
        // X座標の長さ
        public int X { get; private set; } = 3;
        // Y座標の長さ
        public int Y { get; private set; } = 3;
        // 爆弾の個数
        public int BombCount { get; private set; } = 1;
        //add
        public int LifeCount { get; private set; } = 1;
        public float TimeLimit { get; private set; } =1;
        public float ElapsedTime{ get; private set; } =1;

        public void SetData(int y, int x, int lifeCount, float timeLimit/*,float elapsedTime*/)
        {
            //TODO 8/31 Mathf.RoundToIntが近い偶数にして（10.7の場合は10にする）ということは分かるのですが、(x * y * 0.20f)は何を表しているのでしょうか。
            /*
            ANS: 9/1
            ここでは爆弾の数をなんとなくちょうどいい数を指定しているだけなので、計算式にあまり意味はありません。ブロックの総数の20$を爆弾にするという意味となります。
            仮に爆弾の数を入力しなくてもSetDataが動作します。これを関数のオーバーロードといいます。
            参考：オーバーロード（http://www.wisdomsoft.jp/179.html）
             */
            SetData(x, y, Mathf.RoundToInt(x * y * 0.20f),lifeCount,timeLimit/*,elapsedTime*/);
        }

    
        public void SetData(int y, int x, int bombCount, int lifeCount, float timeLimit/*,float elapsedTime*/)
        {
            //それぞれ入れている
          
            X = x;
            Y = y;
            BombCount = bombCount;
            LifeCount = lifeCount;
            TimeLimit = timeLimit;
            //ElapsedTime = elapsedTime;

        }
    }

    public StageData PlayStageData { get; private set; } = new StageData();
}
