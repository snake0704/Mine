using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 爆弾のブロックを提供する
/// </summary>
public class BlockTypeBomb : BlockTypeBase
{
    public override BlockType CuurentBlockType => BlockType.Bomb;

    /// <summary>
    /// 爆弾を押したときのイベント
    /// </summary>
    /// <returns></returns>
    protected override Block.BlockEvent GetOpenEvent()
    {
        // 爆弾なのでゲームオーバーイベントを呼び出す
        return Block.BlockEvent.Lifedic;
    }
}

