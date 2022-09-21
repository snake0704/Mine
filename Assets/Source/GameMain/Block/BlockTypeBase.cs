using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 入力後のブロックの基底クラス
/// </summary>
public abstract class BlockTypeBase : MonoBehaviour
{
    public enum BlockType
    {
        Number,     // 数字（該当がない）
        Bomb,       // 爆弾
        Length
    }

    [SerializeField]
    GameObject onActiveObject;

    public abstract BlockType CuurentBlockType { get; }

    private void Start()
    {
        onActiveObject.SetActive(false);
    }

    /// <summary>
    /// ブロックイベントを取得する
    /// </summary>
    /// <returns></returns>
    protected virtual Block.BlockEvent GetOpenEvent()
    {
        //TODO 9/1 returnで何を返しているのでしょうか。
        /*
        ANS: 9/2
        Noneの場合は何も起こらないイベントとなります。
        Block.BlockEventに関しては定義へ移動してもらえれば書かれていますがブロックが発行するイベントの定義となります。
        */

        return Block.BlockEvent.None;
    }

    /// <summary>
    /// ブロックが開くとき（右クリックされたとき）に発行されるイベント
    /// </summary>
    /// <returns></returns>
    public Block.BlockEvent IsOpen()
    {
        onActiveObject.SetActive(true);

        return GetOpenEvent();
    }

}
