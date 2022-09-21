using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �����u���b�N��񋟂���
/// </summary>
public class BlockTypeNumber : BlockTypeBase
{
    [SerializeField]
    GameObject[] numbers;

    public override BlockType CuurentBlockType => BlockType.Number;

    //TODO 9/2 JoinCount�Ƃ������͉̂��ł��傤���B
    /*
    ANS: 9/2
    JoinCount�͗אڂ��Ă��锚�e�̐��ł��B
    JoinCount�� public void Setup(int joinCount)�ő�������Ă��܂��B
    ������̊֐��Ăяo����Block.cs�� public void SetParamNumber(int joinCount)�ōs���Ă��܂��B
    */
    public int JoinCount { get; private set; }

    /// <summary>
    /// �������������Ƃ��̃C�x���g
    /// </summary>
    /// <returns></returns>
    protected override Block.BlockEvent GetOpenEvent()
    {
        if (JoinCount == 0)
        {
            // 0 �̎��͎�����J�����ƂɂȂ�̂�OpenWide�C�x���g��Ԃ�.
            return Block.BlockEvent.OpenWide;
        }
        // 0�ȊO�̎��̓C�x���g�Ȃ�
        return Block.BlockEvent.None;
    }

    // �אڂ��Ă��锚�e�̐�
    public void Setup(int joinCount)
    {
        JoinCount = joinCount;

        for (int i = 0; i < numbers.Length; ++i)
        {
            numbers[i].SetActive(joinCount == i);
        }
    }
}
