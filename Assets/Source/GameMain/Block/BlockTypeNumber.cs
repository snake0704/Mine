using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 数字ブロックを提供する
/// </summary>
public class BlockTypeNumber : BlockTypeBase
{
    [SerializeField]
    GameObject[] numbers;

    public override BlockType CuurentBlockType => BlockType.Number;

    //TODO 9/2 JoinCountというものは何でしょうか。
    /*
    ANS: 9/2
    JoinCountは隣接している爆弾の数です。
    JoinCountは public void Setup(int joinCount)で代入をしています。
    代入元の関数呼び出しはBlock.csの public void SetParamNumber(int joinCount)で行われています。
    */
    public int JoinCount { get; private set; }

    /// <summary>
    /// 数字を押したときのイベント
    /// </summary>
    /// <returns></returns>
    protected override Block.BlockEvent GetOpenEvent()
    {
        if (JoinCount == 0)
        {
            // 0 の時は周りを開くことになるのでOpenWideイベントを返す.
            return Block.BlockEvent.OpenWide;
        }
        // 0以外の時はイベントなし
        return Block.BlockEvent.None;
    }

    // 隣接している爆弾の数
    public void Setup(int joinCount)
    {
        JoinCount = joinCount;

        for (int i = 0; i < numbers.Length; ++i)
        {
            numbers[i].SetActive(joinCount == i);
        }
    }
}
