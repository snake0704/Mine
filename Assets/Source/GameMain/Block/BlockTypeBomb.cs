using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ���e�̃u���b�N��񋟂���
/// </summary>
public class BlockTypeBomb : BlockTypeBase
{
    public override BlockType CuurentBlockType => BlockType.Bomb;

    /// <summary>
    /// ���e���������Ƃ��̃C�x���g
    /// </summary>
    /// <returns></returns>
    protected override Block.BlockEvent GetOpenEvent()
    {
        // ���e�Ȃ̂ŃQ�[���I�[�o�[�C�x���g���Ăяo��
        return Block.BlockEvent.Lifedic;
    }
}

