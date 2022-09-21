using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームのステージを提供する、ステージがゲームの状態を判断してGameMainへコールバックとして渡す
/// ステージの管理だけを行い、ゲームの管理はGameMainが行う
/// </summary>
/// 
public class Stage : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    GridLayoutGroup gridLayoutGorup;
    [SerializeField]
    Block blockBase;

    Block[][] blocks;
    int xLength;
    int yLength;
    int bombCount;
    //add
   public int lifeCount;
    public float timeLimit2;
    public float elapsedTime;
    public int LifeCount
    {
        set { lifeCount = value; }
        get { return lifeCount; }
    }
    public float TimeLimit2
    {
        set { timeLimit2 = value; }
        get { return timeLimit2; }
    }
    public float ElapsedTime
    {
        set { elapsedTime = value; }
        get { return elapsedTime; }
    }
    bool isGameover;
    // 爆弾の個数表示を更新する
    System.Action<int> onBombCountRefresh;
    // ゲームオーバー
    System.Action onGameOver;
    // ゲームクリア
    System.Action onGameClear;
    //add
    //ライフの減少
    System.Action<int> onLifeDic;
    bool isInit;
    public bool IsInit
    {
        get { return isInit; }
    }
    bool stopCount;
    public bool StopCount
    {
        get { return stopCount; }
        set { stopCount = value; }
    }
    //制限時間
    //System.Action<float> onTL;
   

    public void GameEnd()
    {
        canvasGroup.blocksRaycasts = false;
        //add
        //timerStop();
    }

    /// <summary>
    /// このステージの設定を行う
    /// </summary>
    /// <param name="yLength"> Xのブロック数 </param>
    /// <param name="xLength"> Yのブロック数 </param>
    /// <param name="bombCount"> 爆弾の数 </param>
    /// <param name="onBombCountRefresh"> 爆弾の残り個数を渡すコールバック </param>
    /// <param name="onGameOver"> ゲームオーバーの時に呼び出すコールバック </param>
    /// <param name="onGameClear"> ゲームクリアの時に呼び出すコールバック </param>
   //add
    public void Setup(int yLength, int xLength, int bombCount,int lifeCount,float timeLimit, float elapsedTime, System.Action<int> onBombCountRefresh, System.Action onGameOver, System.Action onGameClear, System.Action<int> onLifeDic)
    {
        this.xLength = xLength;
        this.yLength = yLength;
        //TODO 9/1 「yLength * xLength - 1」は何のために書く必要があるのでしょうか。また、どうして 「Mathf.Min」で最小の値を求める必要があるのでしょうか。
        /*
        ANS: 9/1 18:00
        「yLength * xLength - 1」というのは「ブロック数 - 1」と同じ意味となります。
        ゲームのルール上、必ず1つめのブロックは開けるようにしないといけないので
        爆弾の最大数は「yLength * xLength - 1」となります。
        Mathf.Minは小さい数値を取得できますので
        例えばブロック数が30個ある場合に爆弾を50個の設定を行っていた場合
        Mathf.Min(50, 29)となり、戻り値は29になるのでルールに沿った爆弾の個数にすることが出来ます。
         */
        this.bombCount = Mathf.Min(bombCount, yLength * xLength - 1);
        this.lifeCount = lifeCount;
        this.timeLimit2 = timeLimit;
        this.elapsedTime = elapsedTime;
        this.onBombCountRefresh = onBombCountRefresh;
        this.onGameOver = onGameOver;
        this.onGameClear = onGameClear;
        //add
        this.onLifeDic = onLifeDic;
        this.isInit = false;
        this.stopCount = false;
     
        isGameover = false;

        // 入力の制限（あんまり気にしないでいい）
        canvasGroup.blocksRaycasts = true;

        // 爆弾表示を更新
        //Invokeは別スレッドから特定のスレッドに対して指示を出すことができる
        onBombCountRefresh?.Invoke(bombCount);

        // 表示個数を設定（あんまり気にしないでいい）
        gridLayoutGorup.constraintCount = xLength;

        // 既にブロックが作られている時に削除
        if (blocks != null)
        {
            //例えば６＊６の場合、横1列ずつ消している
            for (int y = 0; y < blocks.Length; ++y)
            {
                for (int x = 0; x < blocks[y].Length; ++x)
                {
                    Destroy(blocks[y][x].gameObject);
                }
            }
        }

        // 改めてブロックを作る
        blocks = new Block[yLength][];

        for (int y = 0; y < blocks.Length; ++y)
        {
            blocks[y] = new Block[xLength];
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //Instantiateはゲームのキャラクターなどを作成するときに使われる。例えば、シューティングゲームで「ボタンを押したときに自機から弾を生成し発射!」ということもできる
                var tempObj = Instantiate(blockBase, transform, false);

                tempObj.gameObject.SetActive(true);
                //TODO 9/1 「$"{y}_{x}"」は何を意味しているのでしょうか。
                /*
                ANS: 9/1 18:00
                こちらは実行時にHierarchyに表示されるInstantiateされたブロックの名前です。
                わかりやすくするために名前付けを行っていますがゲームに直接影響はありません。
                参考：ゲームオブジェクトの名前をスクリプトで変更する（https://miyagame.net/obj-name-change-script/）
                 */

                tempObj.name = $"{y}_{x}";

                // ブロックの設定を行う
                tempObj.Setup(y, x, InitStageCallback, OpenBlockCallback, OnFlagCallback);
                blocks[y][x] = tempObj;
            }
        }
    }

    /// <summary>
    /// 1個目のブロックを開いた時に呼び出される、ステージ全体に爆弾の配置を行い、爆弾に応じて数字を配置する
    /// </summary>
    /// <param name="block"> 1個目のブロック、このブロックは絶対に安全になるルールとなっている </param>
    void InitStageCallback(Block block)
    {
        // blockはボム以外にしてそれ以外はランダムでボムを設置する.
        int[] bombIndexs = new int[bombCount];
        //TODO 9/1 yLength * xLengthを何のために行われているのでしょうか。
        /*
        ANS: 9/1 18:00
        ここでは爆弾を設置します。
        爆弾の設置は整数値を一つランダムで作成して、その番号に応じた場所に爆弾の設置を行っています。
        maxIndexとはyLength * xLength（縦×横）のブロックの数を表しています。
         */
        int maxIndex = yLength * xLength;

        //TODO 9/1  block.Y * xLength + block.Xは何のために行われているのでしょうか。
        /*
        ANS: 9/1 18:00
        爆弾を設置する際に、必ず爆弾ではない場所を表しています。
        計算で出しているのはブロックを一列に並べたときの番号になります。
        blockは一番最初にタップされたブロックが入っています。

        例えば
        x5 * y5のブロック配列で、タップされた場所が (x1, y3)の場合、
        block.Y * xLength + block.X = 3 * 5 + 1となります。
        つまり(x1, y3) = 16の位置となるという事です。

        5 * 5は次の様に番号を振っているイメージです。

         0, 1, 2, 3, 4,
         5, 6, 7, 8, 9,
        10,11,12,13,14,
        15,16,17,18,19,
        20,21,22,23,24,

         */
        int ignoreIndex = block.Y * xLength + block.X;

        for (int i = 0; i < bombCount; ++i)
        {
            int index = Random.Range(0, maxIndex);

            while (true)
            {
                bool isOk = true;

                if (index == ignoreIndex)
                {
                    index = Random.Range(0, maxIndex);
                    continue;
                }

                for (int j = 0; j < bombIndexs.Length; ++j)
                {
                    if (bombIndexs[j] == index)
                    {
                        // 既に設定してあったらindexを進める. indexが最大値まで進むと0にする
                        index = (index + 1) % maxIndex;
                        isOk = false;
                        break;
                    }
                }
                if (isOk)
                {
                    bombIndexs[i] = index; 
                    int y = index / xLength;
                    int x = index % xLength;
                    blocks[y][x].SetParamBomb(true);
                    break;
                }
            }
        }
        // 爆弾に応じて番号を設定する
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //TODO 9/1 このif文は何をしているのでしょうか。
                /*
                ANS: 9/1 18:00
                187行目でblocks[y][x].SetParamBomb(true);を行っています。
                こちらで爆弾の設定をしたものはblocks[y][x].IsBombがtrueで返ってきます。
                つまり、IsBombは爆弾である場合はtrueになります
                爆弾の場合は番号を設定する必要はないので、IsBombがtrueのときにcontineを行っています。
                 */
                if (blocks[y][x].IsBomb)
                {
                    continue;
                }
                blocks[y][x].SetParamNumber(GetBlockNumber(y, x));

            }
        }
        isInit = true;
    }

    /// <summary>
    /// 座標のブロックの位置の周りに爆弾が何個あるのか返す関数
    /// </summary>
    /// <param name="y"> y座標 </param>
    /// <param name="x"> x座標 </param>
    /// <returns></returns>
    int GetBlockNumber(int y, int x)
    {
        int ret = 0;
        //TODO 9/1 if (y > 0)のif文全体で何をしているのか良くわかりません。
        /*
        ANS: 9/1 18:00
        ここでは周囲8個のブロックに爆弾が何個あるのかを調べています。

        通常パターンの場合は次のように特にもんだなく調べる事が出来ます。
        1 2 3
        4 @ 5 
        6 7 8

        もしもyが0の時に周囲の爆弾を調べようとした場合は次のようになります。
        - - -
        4 @ 5
        6 7 8

        「1 2 3」は y = -1になるので存在しません。
        そのため yが0の場合は 「1 2 3 」は調べてはいけないとなる事がわかると思います。
        今回のif (y > 0)は yが0を超える場合（0以下の場合はNG）にブロックの数を調べています。
         */

        if (y > 0)
        {
            ret += blocks[y - 1][x].IsBomb ? 1 : 0;
            if (x > 0)
            {
                ret += blocks[y - 1][x - 1].IsBomb ? 1 : 0;
            }
            if (x < (xLength - 1))
            {
                ret += blocks[y - 1][x + 1].IsBomb ? 1 : 0;
            }
        }
        if (x > 0)
        {
            ret += blocks[y][x - 1].IsBomb ? 1 : 0;
        }
        if (x < (xLength - 1))
        {
            ret += blocks[y][x + 1].IsBomb ? 1 : 0;
        }
        if (y < (yLength - 1))
        {
            ret += blocks[y + 1][x].IsBomb ? 1 : 0;
            if (x > 0)
            {
                ret += blocks[y + 1][x - 1].IsBomb ? 1 : 0;
            }
            if (x < (xLength - 1))
            {
                ret += blocks[y + 1][x + 1].IsBomb ? 1 : 0;
            }
        }
        return ret;
    }

    /// <summary>
    /// ブロックが開いたときのイベントを受け取る
    /// </summary>
    /// <param name="blockEvent"> 発行されたイベント </param>
    /// <param name="block"> 開いたブロック </param>
    void OpenBlockCallback(Block.BlockEvent blockEvent, Block block)
    {
       
        switch (blockEvent)
        {
            case Block.BlockEvent.OpenWide:
                EventOpenWide(block);
                break;
                //add
            case Block.BlockEvent.Lifedic:
                lifeCount--;
                break;
            
        }
        //add
       
        if (lifeCount<0)
        {
            EventGameOver();
            isGameover = true;
        }
        else
        {
            onLifeDic(lifeCount);
        }
      
        if (isGameover)
        {
            // ゲームオーバーなのでクリアにはならない.
            return;
        }
        // クリア判定.
        var blankCount = 0;
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //TODO 9/1 !blocks[y][x]は何を意味しているのでしょうか。
                /*
                ANS: 9/1 18:00
                ここでは状態が判別されていないブロック数を数えています。
                残りブロック数が爆弾の個数と同じ時にゲームクリアとなります。

                blocks[y][x].IsOpenとは開いている状態（数字のブロックを開いている状態）です。
                これを否定しているのでif (!blocks[y][x].IsOpen)とはブロックが開いていない状態（爆弾か数字がわからない状態）を調べています。
                 */
                if (!blocks[y][x].IsOpen)
                {
                    ++blankCount;
                }
            }
        }
        if (blankCount == bombCount)
        {
            onGameClear?.Invoke();
        }
    }
    public void GameOverAction()
    {

        EventGameOver();
        isGameover = true;

    }

    /// <summary>
    /// 旗を立てた時、旗がなくなった時に呼び出される
    /// </summary>
    /// <param name="block"></param>
    void OnFlagCallback()
    {
        var flagCount = bombCount;
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                if (blocks[y][x].IsFlag)
                {
                    --flagCount;
                }
            }
        }
        onBombCountRefresh?.Invoke(flagCount);
    }

    /// <summary>
    /// OpenWideイベントの時に呼び出す
    /// </summary>
    /// <param name="block"> 開く中心のブロック、このブロックは数字が0 </param>
    void EventOpenWide(Block block)
    {
        // JoinCountが0の時は周りを自動的に開く.
        int y = block.Y;
        int x = block.X;
        if (y > 0)
        {
            blocks[y - 1][x].SetOpen();
            if (x > 0)
            {
                blocks[y - 1][x - 1].SetOpen();
            }
            //TODO 9/1 どうして下の if (x < (xLength - 1))が必要なのでしょうか。
            /*
            ANS: 9/1 18:00
            blocks[y - 1][x + 1].SetOpen()を行っているのでxが最大値 - 1（配列の最大要素番号）の時には調べてはいけません

            例えば new blocks[5][5]の場合に次の様な記述を行うとエラーとなりますのでif文での制御が必要となります。
            blocks[y - 1][4 + 1].SetOpen();     // 4 + 1がアクセス違反
             */
            if (x < (xLength - 1))
            {
                blocks[y - 1][x + 1].SetOpen();
            }
        }
        if (x > 0)
        {
            blocks[y][x - 1].SetOpen();
        }
        if (x < (xLength - 1))
        {
            blocks[y][x + 1].SetOpen();
        }
        if (y < (yLength - 1))
        {
            blocks[y + 1][x].SetOpen();
            if (x > 0)
            {
                blocks[y + 1][x - 1].SetOpen();
            }
            if (x < (xLength - 1))
            {
                blocks[y + 1][x + 1].SetOpen();
            }
        }
    }

    /// <summary>
    /// GameOverイベントの時に呼び出す
    /// </summary>
    void EventGameOver()
    {
        // ゲームオーバー.
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                blocks[y][x].SetOpen();
            }
        }
        onGameOver?.Invoke();
    }
}
