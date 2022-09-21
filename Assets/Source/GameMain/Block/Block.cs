using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Stage�ɔz�u�����̃u���b�N
/// �Q�[���J�n�͑S��Blank����J�n�A1�ڂ�I�������Ƃ��ɐ����u���b�N�ȂǂɂȂ�
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // �u���b�N�����s����C�x���g�̒�`
    public enum BlockEvent
    {
        None,
        OpenWide,           // ���l��0�̎������I�ɉ����I�[�v������
        GameOver,// �Q�[���I�[�o�[
      //add
        Lifedic,  //���C�t�̌���
    }

    // ���N���b�N�������Ƃ��̏�ԕω�
    public enum BlockType
    {
        Blank,      // �����Ȃ����
        Flag,       // ���𗧂ĂĂ�����
        Question,   // �H�𗧂ĂĂ�����
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

    // Block���W
    public int X { get; private set; }
    public int Y { get; private set; }
    // �J���Ă����Ԃ�
    public bool IsOpen { get; private set; }
    // ���������e��Ԃ�
    public bool IsBomb => CurrentBlockType != null ? CurrentBlockType.CuurentBlockType == BlockTypeBase.BlockType.Bomb : false;
    // ���������������Ă���H
    public bool IsInit => CurrentBlockType != null;
    // ���������Ă��邩
    public bool IsFlag => blockBlankData.IsFlag;

    /// <summary>
    /// �u���b�N�̐ݒ�
    /// </summary>
    /// <param name="y"> y���W </param>
    /// <param name="x"> x���W </param>
    /// <param name="initStageCallback"> �X�e�[�W�����p�̃R�[���o�b�N�A�ŏ���1�ڂ��^�b�v�������ɌĂяo����� </param>
    /// <param name="openBlockCallback"> �u���b�N���E�N���b�N�ŊJ�������ɌĂяo����� </param>
    /// <param name="onFlagCallback"> ���N���b�N���������Ƃ��ɌĂяo����� </param>
    public void Setup(int y, int x, System.Action<Block> initStageCallback, System.Action<BlockEvent, Block> openBlockCallback, System.Action onFlagCallback)
    {
        X = x;
        Y = y;
        this.initStageCallback = initStageCallback;
        this.openBlockCallback = openBlockCallback;
        this.onFlagCallback = onFlagCallback;

        // �S�Ă������l�����Ă���
        IsOpen = false;
    }

    /// <summary>
    /// �p�����[�^�ݒ�A����
    /// </summary>
    /// <param name="joinCount"></param>
    public void SetParamNumber(int joinCount)
    {
        //TODO 9/1 ����2�s�͉����s���Ă���̂ł��傤���B
        /*
        ANS: 9/2
        blockTypeNumber��Setup�̒�`�ֈړ����Ă��炦��Ƃ킩�邩�Ǝv���܂��������u���b�N�̐ݒ���s���Ă��܂��B
        CurrentBlockType = blockTypeNumber�͊��N���XCurrentBlockType�ɔh���N���XblockTypeNumber�����鎖��
        �����\�[�X�R�[�h��blockTypeNumber��blockTypeBomb���g�p�ł���悤�ɂ��Ă��܂��B
        �p���̉��p���ƍl���Ă��炦��Α��v�ł��B
        */

        blockTypeNumber.Setup(joinCount);
        CurrentBlockType = blockTypeNumber;
    }

    /// <summary>
    /// �p�����[�^�ݒ�A���e
    /// </summary>
    /// <param name="isBomb"></param>
    public void SetParamBomb(bool isBomb)
    {
        CurrentBlockType = blockTypeBomb;
    }

    /// <summary>
    /// �u���b�N���J���A�E�N���b�N���̊֐�
    /// </summary>
    public void SetOpen()
    {
        if (IsOpen)
        {
            return;
        }
        // �u���b�N�̃I�[�v��
        IsOpen = true;
        
        blockBlankData.SetBlankType(BlockBlankData.BlockBlankType.Blank);

        var openEvent = CurrentBlockType.IsOpen();

        openBlockCallback?.Invoke(openEvent, this);
    }

    /// <summary>
    /// �E�N���b�N���������Ƃ��ɌĂяo�����֐��AIPointerDownHandler���p�����鎖�Ŏg�p�\
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO 9/1  blockBlankData.IsTapLimit && IsOpen�͉����Ӗ����Ă���̂ł��傤���B
        /*
        ANS: 9/2
        ���N���b�N���s�������Ƀu���b�N���I�[�v������Ԃ����ׂĂ��܂��B
        IsTapLimit�͒�`�ֈړ����Ă��炦��Ώ����Ă���܂����A�uCurrentBlankType == BlockBlankType.Flag�v�̎���true�ƂȂ�܂��B
        �܂�A���������Ă����Ԃł����true�ɂȂ�܂��B
        �Q�[���̃��[������������Ă��鎞�ɍ��N���b�N�͍s���܂���B
        */

        if (eventData.button == PointerEventData.InputButton.Left && blockBlankData.IsTapLimit && IsOpen)
        {
            // ���N���b�N�̎���Flag�ȊO���I�[�v�����Ă��Ȃ���ԂłȂ��ƃ_��.
            return;
        }
        imageBlock.color = Color.gray;
        isPointerDown = true;
    }

    /// <summary>
    /// �E�N���b�N�𗣂������ɌĂяo�����֐��AIPointerUpHandler���p�����鎖�Ŏg�p�\
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        imageBlock.color = Color.white;
        if (isPointerDown)
        {
            // ���N���b�N���̋���
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClick();
            }
            // �E�N���b�N���̋���
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClick();
            }
        }
    }

    /// <summary>
    /// �E�N���b�N�������Ă��鎞�ɗ̈�O�ɗ��ꂽ���ɌĂяo�����֐��AIPointerExitHandler���p�����鎖�Ŏg�p�\
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        imageBlock.color = Color.white;
        isPointerDown = false;
    }

    // ���N���b�N
    void LeftClick()
    {
        Debug.Log("���N���b�N");
        if (!IsInit)
        {
            // ���߂Ă̍��N���b�N�Ȃ̂ŁA��������ɃX�e�[�W������������.
            initStageCallback?.Invoke(this);
        }
        SetOpen();
    }
    // �E�N���b�N
    void RightClick()
    {
        Debug.Log("�E�N���b�N");

        blockBlankData.NextBlankType();

        onFlagCallback?.Invoke();
    }
}
