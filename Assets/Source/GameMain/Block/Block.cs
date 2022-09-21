using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Stageに配置する一つのブロック
/// ゲーム開始は全てBlankから開始、1つ目を選択したときに数字ブロックなどになる
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // ブロックが発行するイベントの定義
    public enum BlockEvent
    {
        None,
        OpenWide,           // 数値が0の時自動的に回りをオープンする
        GameOver,// ゲームオーバー
      //add
        Lifedic,  //ライフの減少
    }

    // 左クリックをしたときの状態変化
    public enum BlockType
    {
        Blank,      // 何もない状態
        Flag,       // 旗を立てている状態
        Question,   // ？を立てている状態
        Length
    }
    [SerializeField]
    Image imageBlock;
    [SerializeField]
    BlockBlankData blockBlankData;
    [SerializeField]
    BlockTypeBomb blockTypeBomb;
    [SerializeField]
    BlockTypeNumber blockTypeNumber;

    BlockTypeBase CurrentBlockType;
    bool isPointerDown;
    System.Action<Block> initStageCallback;
    System.Action<BlockEvent, Block> openBlockCallback;
    System.Action onFlagCallback;

    // Block座標
    public int X { get; private set; }
    public int Y { get; private set; }
    // 開いている状態か
    public bool IsOpen { get; private set; }
    // 自分が爆弾状態か
    public bool IsBomb => CurrentBlockType != null ? CurrentBlockType.CuurentBlockType == BlockTypeBase.BlockType.Bomb : false;
    // 初期化が完了している？
    public bool IsInit => CurrentBlockType != null;
    // 旗がたっているか
    public bool IsFlag => blockBlankData.IsFlag;

    /// <summary>
    /// ブロックの設定
    /// </summary>
    /// <param name="y"> y座標 </param>
    /// <param name="x"> x座標 </param>
    /// <param name="initStageCallback"> ステージ生成用のコールバック、最初の1個目をタップした時に呼び出される </param>
    /// <param name="openBlockCallback"> ブロックを右クリックで開いた時に呼び出される </param>
    /// <param name="onFlagCallback"> 左クリックを押したときに呼び出される </param>
    public void Setup(int y, int x, System.Action<Block> initStageCallback, System.Action<BlockEvent, Block> openBlockCallback, System.Action onFlagCallback)
    {
        X = x;
        Y = y;
        this.initStageCallback = initStageCallback;
        this.openBlockCallback = openBlockCallback;
        this.onFlagCallback = onFlagCallback;

        // 全てを初期値を入れておく
        IsOpen = false;
    }

    /// <summary>
    /// パラメータ設定、数字
    /// </summary>
    /// <param name="joinCount"></param>
    public void SetParamNumber(int joinCount)
    {
        //TODO 9/1 下の2行は何を行っているのでしょうか。
        /*
        ANS: 9/2
        blockTypeNumberのSetupの定義へ移動してもらえるとわかるかと思いますが数字ブロックの設定を行っています。
        CurrentBlockType = blockTypeNumberは基底クラスCurrentBlockTypeに派生クラスblockTypeNumberを入れる事で
        同じソースコードでblockTypeNumberとblockTypeBombを使用できるようにしています。
        継承の応用だと考えてもらえれば大丈夫です。
        */

        blockTypeNumber.Setup(joinCount);
        CurrentBlockType = blockTypeNumber;
    }

    /// <summary>
    /// パラメータ設定、爆弾
    /// </summary>
    /// <param name="isBomb"></param>
    public void SetParamBomb(bool isBomb)
    {
        CurrentBlockType = blockTypeBomb;
    }

    /// <summary>
    /// ブロックを開く、右クリック時の関数
    /// </summary>
    public void SetOpen()
    {
        if (IsOpen)
        {
            return;
        }
        // ブロックのオープン
        IsOpen = true;
        
        blockBlankData.SetBlankType(BlockBlankData.BlockBlankType.Blank);

        var openEvent = CurrentBlockType.IsOpen();

        openBlockCallback?.Invoke(openEvent, this);
    }

    /// <summary>
    /// 右クリックを押したときに呼び出される関数、IPointerDownHandlerを継承する事で使用可能
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO 9/1  blockBlankData.IsTapLimit && IsOpenは何を意味しているのでしょうか。
        /*
        ANS: 9/2
        左クリックを行った時にブロックがオープンを状態か調べています。
        IsTapLimitは定義へ移動してもらえれば書いてありますが、「CurrentBlankType == BlockBlankType.Flag」の時にtrueとなります。
        つまり、旗が立っている状態であればtrueになります。
        ゲームのルール上旗が立っている時に左クリックは行えません。
        */

        if (eventData.button == PointerEventData.InputButton.Left && blockBlankData.IsTapLimit && IsOpen)
        {
            // 左クリックの時はFlag以外かつオープンしていない状態でないとダメ.
            return;
        }
        imageBlock.color = Color.gray;
        isPointerDown = true;
    }

    /// <summary>
    /// 右クリックを離した時に呼び出される関数、IPointerUpHandlerを継承する事で使用可能
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        imageBlock.color = Color.white;
        if (isPointerDown)
        {
            // 左クリック時の挙動
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClick();
            }
            // 右クリック時の挙動
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClick();
            }
        }
    }

    /// <summary>
    /// 右クリックを押している時に領域外に離れた時に呼び出される関数、IPointerExitHandlerを継承する事で使用可能
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        imageBlock.color = Color.white;
        isPointerDown = false;
    }

    // 左クリック
    void LeftClick()
    {
        Debug.Log("左クリック");
        if (!IsInit)
        {
            // 初めての左クリックなので、ここを基準にステージを初期化する.
            initStageCallback?.Invoke(this);
        }
        SetOpen();
    }
    // 右クリック
    void RightClick()
    {
        Debug.Log("右クリック");

        blockBlankData.NextBlankType();

        onFlagCallback?.Invoke();
    }
}
