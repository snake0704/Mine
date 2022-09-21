using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ���͌�̃u���b�N�̊��N���X
/// </summary>
public abstract class BlockTypeBase : MonoBehaviour
{
    public enum BlockType
    {
        Number,     // �����i�Y�����Ȃ��j
        Bomb,       // ���e
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
    /// �u���b�N�C�x���g���擾����
    /// </summary>
    /// <returns></returns>
    protected virtual Block.BlockEvent GetOpenEvent()
    {
        //TODO 9/1 return�ŉ���Ԃ��Ă���̂ł��傤���B
        /*
        ANS: 9/2
        None�̏ꍇ�͉����N����Ȃ��C�x���g�ƂȂ�܂��B
        Block.BlockEvent�Ɋւ��Ă͒�`�ֈړ����Ă��炦��Ώ�����Ă��܂����u���b�N�����s����C�x���g�̒�`�ƂȂ�܂��B
        */

        return Block.BlockEvent.None;
    }

    /// <summary>
    /// �u���b�N���J���Ƃ��i�E�N���b�N���ꂽ�Ƃ��j�ɔ��s�����C�x���g
    /// </summary>
    /// <returns></returns>
    public Block.BlockEvent IsOpen()
    {
        onActiveObject.SetActive(true);

        return GetOpenEvent();
    }

}
