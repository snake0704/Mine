using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 入力前のブロックの挙動
/// </summary>
public class BlockBlankData : MonoBehaviour
{
    public enum BlockBlankType
    {
        Blank,      // 何もない状態
        Flag,       // 旗を立てている状態
        Question,   // ？を立てている状態
        Length
    }
    [SerializeField]
    GameObject objectFlag;
    [SerializeField]
    GameObject objectQuestion;

    // 現在の空欄状態.
    public BlockBlankType CurrentBlankType {get; private set; }
    // 左クリック出来ない状態ならtrue
    public bool IsTapLimit => CurrentBlankType == BlockBlankType.Flag;
    // 現在の状態が旗状態ならtrue
    public bool IsFlag => CurrentBlankType == BlockBlankType.Flag;


    // BlockBlankTypeを移動、右クリック想定.
    public void NextBlankType()
    {
        //TODO 9/1 ((int)CurrentBlankType + 1) % (int)BlockBlankType.Length));が何を行っているのかわかりません。
        /*
        ANS: 9/2
        この手法は数字を加算した場合に範囲を超過した時にループさせる手法です。
        例えば範囲が3まで用意してある変数をループさせる場合は次様にします。

        (value + 1) % 3
        この時数値の推移は次のようになり範囲が最大の時に足し算が行われると0に戻ります。
        
        0 → 1
        1 → 2
        2 → 0

        今回はこれをenumのBlockBlankTypeを1進めてBlockBlankType.Lengthで余りを求めています。

        Blank,      // 何もない状態
        Flag,       // 旗を立てている状態
        Question,   // ？を立てている状態
        
        この3つの状態を1づつ進めているという事にです。
        */
        SetBlankType((BlockBlankType)(((int)CurrentBlankType + 1) % (int)BlockBlankType.Length));
    }
    public void SetBlankType(BlockBlankType blankType)
    {
        CurrentBlankType = blankType;
        Refresh();
    }

    // 描画物の更新
    void Refresh()
    {
        switch (CurrentBlankType)
        {
            case BlockBlankType.Blank:
                objectFlag.SetActive(false);
                objectQuestion.SetActive(false);
                break;
            case BlockBlankType.Flag:
                objectFlag.SetActive(true);
                objectQuestion.SetActive(false);
                break;
            case BlockBlankType.Question:
                objectFlag.SetActive(false);
                objectQuestion.SetActive(true);
                break;
        }
    }
}
