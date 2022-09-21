using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ���͑O�̃u���b�N�̋���
/// </summary>
public class BlockBlankData : MonoBehaviour
{
    public enum BlockBlankType
    {
        Blank,      // �����Ȃ����
        Flag,       // ���𗧂ĂĂ�����
        Question,   // �H�𗧂ĂĂ�����
        Length
    }
    [SerializeField]
    GameObject objectFlag;
    [SerializeField]
    GameObject objectQuestion;

    // ���݂̋󗓏��.
    public BlockBlankType CurrentBlankType {get; private set; }
    // ���N���b�N�o���Ȃ���ԂȂ�true
    public bool IsTapLimit => CurrentBlankType == BlockBlankType.Flag;
    // ���݂̏�Ԃ�����ԂȂ�true
    public bool IsFlag => CurrentBlankType == BlockBlankType.Flag;


    // BlockBlankType���ړ��A�E�N���b�N�z��.
    public void NextBlankType()
    {
        //TODO 9/1 ((int)CurrentBlankType + 1) % (int)BlockBlankType.Length));�������s���Ă���̂��킩��܂���B
        /*
        ANS: 9/2
        ���̎�@�͐��������Z�����ꍇ�ɔ͈͂𒴉߂������Ƀ��[�v�������@�ł��B
        �Ⴆ�Δ͈͂�3�܂ŗp�ӂ��Ă���ϐ������[�v������ꍇ�͎��l�ɂ��܂��B

        (value + 1) % 3
        ���̎����l�̐��ڂ͎��̂悤�ɂȂ�͈͂��ő�̎��ɑ����Z���s�����0�ɖ߂�܂��B
        
        0 �� 1
        1 �� 2
        2 �� 0

        ����͂����enum��BlockBlankType��1�i�߂�BlockBlankType.Length�ŗ]������߂Ă��܂��B

        Blank,      // �����Ȃ����
        Flag,       // ���𗧂ĂĂ�����
        Question,   // �H�𗧂ĂĂ�����
        
        ����3�̏�Ԃ�1�Âi�߂Ă���Ƃ������ɂł��B
        */
        SetBlankType((BlockBlankType)(((int)CurrentBlankType + 1) % (int)BlockBlankType.Length));
    }
    public void SetBlankType(BlockBlankType blankType)
    {
        CurrentBlankType = blankType;
        Refresh();
    }

    // �`�敨�̍X�V
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
