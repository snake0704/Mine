using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���̃X�e�[�W��񋟂���A�X�e�[�W���Q�[���̏�Ԃ𔻒f����GameMain�փR�[���o�b�N�Ƃ��ēn��
/// �X�e�[�W�̊Ǘ��������s���A�Q�[���̊Ǘ���GameMain���s��
/// </summary>
/// 
public class Stage : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    GridLayoutGroup gridLayoutGorup;
    [SerializeField]
    Block blockBase;

    Block[][] blocks;
    int xLength;
    int yLength;
    int bombCount;
    //add
   public int lifeCount;
    public float timeLimit2;
    public float elapsedTime;
    public int LifeCount
    {
        set { lifeCount = value; }
        get { return lifeCount; }
    }
    public float TimeLimit2
    {
        set { timeLimit2 = value; }
        get { return timeLimit2; }
    }
    public float ElapsedTime
    {
        set { elapsedTime = value; }
        get { return elapsedTime; }
    }
    bool isGameover;
    // ���e�̌��\�����X�V����
    System.Action<int> onBombCountRefresh;
    // �Q�[���I�[�o�[
    System.Action onGameOver;
    // �Q�[���N���A
    System.Action onGameClear;
    //add
    //���C�t�̌���
    System.Action<int> onLifeDic;
    bool isInit;
    public bool IsInit
    {
        get { return isInit; }
    }
    bool stopCount;
    public bool StopCount
    {
        get { return stopCount; }
        set { stopCount = value; }
    }
    //��������
    //System.Action<float> onTL;
   

    public void GameEnd()
    {
        canvasGroup.blocksRaycasts = false;
        //add
        //timerStop();
    }

    /// <summary>
    /// ���̃X�e�[�W�̐ݒ���s��
    /// </summary>
    /// <param name="yLength"> X�̃u���b�N�� </param>
    /// <param name="xLength"> Y�̃u���b�N�� </param>
    /// <param name="bombCount"> ���e�̐� </param>
    /// <param name="onBombCountRefresh"> ���e�̎c�����n���R�[���o�b�N </param>
    /// <param name="onGameOver"> �Q�[���I�[�o�[�̎��ɌĂяo���R�[���o�b�N </param>
    /// <param name="onGameClear"> �Q�[���N���A�̎��ɌĂяo���R�[���o�b�N </param>
   //add
    public void Setup(int yLength, int xLength, int bombCount,int lifeCount,float timeLimit, float elapsedTime, System.Action<int> onBombCountRefresh, System.Action onGameOver, System.Action onGameClear, System.Action<int> onLifeDic)
    {
        this.xLength = xLength;
        this.yLength = yLength;
        //TODO 9/1 �uyLength * xLength - 1�v�͉��̂��߂ɏ����K�v������̂ł��傤���B�܂��A�ǂ����� �uMathf.Min�v�ōŏ��̒l�����߂�K�v������̂ł��傤���B
        /*
        ANS: 9/1 18:00
        �uyLength * xLength - 1�v�Ƃ����̂́u�u���b�N�� - 1�v�Ɠ����Ӗ��ƂȂ�܂��B
        �Q�[���̃��[����A�K��1�߂̃u���b�N�͊J����悤�ɂ��Ȃ��Ƃ����Ȃ��̂�
        ���e�̍ő吔�́uyLength * xLength - 1�v�ƂȂ�܂��B
        Mathf.Min�͏��������l���擾�ł��܂��̂�
        �Ⴆ�΃u���b�N����30����ꍇ�ɔ��e��50�̐ݒ���s���Ă����ꍇ
        Mathf.Min(50, 29)�ƂȂ�A�߂�l��29�ɂȂ�̂Ń��[���ɉ��������e�̌��ɂ��邱�Ƃ��o���܂��B
         */
        this.bombCount = Mathf.Min(bombCount, yLength * xLength - 1);
        this.lifeCount = lifeCount;
        this.timeLimit2 = timeLimit;
        this.elapsedTime = elapsedTime;
        this.onBombCountRefresh = onBombCountRefresh;
        this.onGameOver = onGameOver;
        this.onGameClear = onGameClear;
        //add
        this.onLifeDic = onLifeDic;
        this.isInit = false;
        this.stopCount = false;
     
        isGameover = false;

        // ���͂̐����i����܂�C�ɂ��Ȃ��ł����j
        canvasGroup.blocksRaycasts = true;

        // ���e�\�����X�V
        //Invoke�͕ʃX���b�h�������̃X���b�h�ɑ΂��Ďw�����o�����Ƃ��ł���
        onBombCountRefresh?.Invoke(bombCount);

        // �\������ݒ�i����܂�C�ɂ��Ȃ��ł����j
        gridLayoutGorup.constraintCount = xLength;

        // ���Ƀu���b�N������Ă��鎞�ɍ폜
        if (blocks != null)
        {
            //�Ⴆ�΂U���U�̏ꍇ�A��1�񂸂����Ă���
            for (int y = 0; y < blocks.Length; ++y)
            {
                for (int x = 0; x < blocks[y].Length; ++x)
                {
                    Destroy(blocks[y][x].gameObject);
                }
            }
        }

        // ���߂ău���b�N�����
        blocks = new Block[yLength][];

        for (int y = 0; y < blocks.Length; ++y)
        {
            blocks[y] = new Block[xLength];
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //Instantiate�̓Q�[���̃L�����N�^�[�Ȃǂ��쐬����Ƃ��Ɏg����B�Ⴆ�΁A�V���[�e�B���O�Q�[���Łu�{�^�����������Ƃ��Ɏ��@����e�𐶐�������!�v�Ƃ������Ƃ��ł���
                var tempObj = Instantiate(blockBase, transform, false);

                tempObj.gameObject.SetActive(true);
                //TODO 9/1 �u$"{y}_{x}"�v�͉����Ӗ����Ă���̂ł��傤���B
                /*
                ANS: 9/1 18:00
                ������͎��s����Hierarchy�ɕ\�������Instantiate���ꂽ�u���b�N�̖��O�ł��B
                �킩��₷�����邽�߂ɖ��O�t�����s���Ă��܂����Q�[���ɒ��ډe���͂���܂���B
                �Q�l�F�Q�[���I�u�W�F�N�g�̖��O���X�N���v�g�ŕύX����ihttps://miyagame.net/obj-name-change-script/�j
                 */

                tempObj.name = $"{y}_{x}";

                // �u���b�N�̐ݒ���s��
                tempObj.Setup(y, x, InitStageCallback, OpenBlockCallback, OnFlagCallback);
                blocks[y][x] = tempObj;
            }
        }
    }

    /// <summary>
    /// 1�ڂ̃u���b�N���J�������ɌĂяo�����A�X�e�[�W�S�̂ɔ��e�̔z�u���s���A���e�ɉ����Đ�����z�u����
    /// </summary>
    /// <param name="block"> 1�ڂ̃u���b�N�A���̃u���b�N�͐�΂Ɉ��S�ɂȂ郋�[���ƂȂ��Ă��� </param>
    void InitStageCallback(Block block)
    {
        // block�̓{���ȊO�ɂ��Ă���ȊO�̓����_���Ń{����ݒu����.
        int[] bombIndexs = new int[bombCount];
        //TODO 9/1 yLength * xLength�����̂��߂ɍs���Ă���̂ł��傤���B
        /*
        ANS: 9/1 18:00
        �����ł͔��e��ݒu���܂��B
        ���e�̐ݒu�͐����l��������_���ō쐬���āA���̔ԍ��ɉ������ꏊ�ɔ��e�̐ݒu���s���Ă��܂��B
        maxIndex�Ƃ�yLength * xLength�i�c�~���j�̃u���b�N�̐���\���Ă��܂��B
         */
        int maxIndex = yLength * xLength;

        //TODO 9/1  block.Y * xLength + block.X�͉��̂��߂ɍs���Ă���̂ł��傤���B
        /*
        ANS: 9/1 18:00
        ���e��ݒu����ۂɁA�K�����e�ł͂Ȃ��ꏊ��\���Ă��܂��B
        �v�Z�ŏo���Ă���̂̓u���b�N�����ɕ��ׂ��Ƃ��̔ԍ��ɂȂ�܂��B
        block�͈�ԍŏ��Ƀ^�b�v���ꂽ�u���b�N�������Ă��܂��B

        �Ⴆ��
        x5 * y5�̃u���b�N�z��ŁA�^�b�v���ꂽ�ꏊ�� (x1, y3)�̏ꍇ�A
        block.Y * xLength + block.X = 3 * 5 + 1�ƂȂ�܂��B
        �܂�(x1, y3) = 16�̈ʒu�ƂȂ�Ƃ������ł��B

        5 * 5�͎��̗l�ɔԍ���U���Ă���C���[�W�ł��B

         0, 1, 2, 3, 4,
         5, 6, 7, 8, 9,
        10,11,12,13,14,
        15,16,17,18,19,
        20,21,22,23,24,

         */
        int ignoreIndex = block.Y * xLength + block.X;

        for (int i = 0; i < bombCount; ++i)
        {
            int index = Random.Range(0, maxIndex);

            while (true)
            {
                bool isOk = true;

                if (index == ignoreIndex)
                {
                    index = Random.Range(0, maxIndex);
                    continue;
                }

                for (int j = 0; j < bombIndexs.Length; ++j)
                {
                    if (bombIndexs[j] == index)
                    {
                        // ���ɐݒ肵�Ă�������index��i�߂�. index���ő�l�܂Ői�ނ�0�ɂ���
                        index = (index + 1) % maxIndex;
                        isOk = false;
                        break;
                    }
                }
                if (isOk)
                {
                    bombIndexs[i] = index; 
                    int y = index / xLength;
                    int x = index % xLength;
                    blocks[y][x].SetParamBomb(true);
                    break;
                }
            }
        }
        // ���e�ɉ����Ĕԍ���ݒ肷��
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //TODO 9/1 ����if���͉������Ă���̂ł��傤���B
                /*
                ANS: 9/1 18:00
                187�s�ڂ�blocks[y][x].SetParamBomb(true);���s���Ă��܂��B
                ������Ŕ��e�̐ݒ���������̂�blocks[y][x].IsBomb��true�ŕԂ��Ă��܂��B
                �܂�AIsBomb�͔��e�ł���ꍇ��true�ɂȂ�܂�
                ���e�̏ꍇ�͔ԍ���ݒ肷��K�v�͂Ȃ��̂ŁAIsBomb��true�̂Ƃ���contine���s���Ă��܂��B
                 */
                if (blocks[y][x].IsBomb)
                {
                    continue;
                }
                blocks[y][x].SetParamNumber(GetBlockNumber(y, x));

            }
        }
        isInit = true;
    }

    /// <summary>
    /// ���W�̃u���b�N�̈ʒu�̎���ɔ��e��������̂��Ԃ��֐�
    /// </summary>
    /// <param name="y"> y���W </param>
    /// <param name="x"> x���W </param>
    /// <returns></returns>
    int GetBlockNumber(int y, int x)
    {
        int ret = 0;
        //TODO 9/1 if (y > 0)��if���S�̂ŉ������Ă���̂��ǂ��킩��܂���B
        /*
        ANS: 9/1 18:00
        �����ł͎���8�̃u���b�N�ɔ��e��������̂��𒲂ׂĂ��܂��B

        �ʏ�p�^�[���̏ꍇ�͎��̂悤�ɓ��ɂ��񂾂Ȃ����ׂ鎖���o���܂��B
        1 2 3
        4 @ 5 
        6 7 8

        ������y��0�̎��Ɏ��͂̔��e�𒲂ׂ悤�Ƃ����ꍇ�͎��̂悤�ɂȂ�܂��B
        - - -
        4 @ 5
        6 7 8

        �u1 2 3�v�� y = -1�ɂȂ�̂ő��݂��܂���B
        ���̂��� y��0�̏ꍇ�� �u1 2 3 �v�͒��ׂĂ͂����Ȃ��ƂȂ鎖���킩��Ǝv���܂��B
        �����if (y > 0)�� y��0�𒴂���ꍇ�i0�ȉ��̏ꍇ��NG�j�Ƀu���b�N�̐��𒲂ׂĂ��܂��B
         */

        if (y > 0)
        {
            ret += blocks[y - 1][x].IsBomb ? 1 : 0;
            if (x > 0)
            {
                ret += blocks[y - 1][x - 1].IsBomb ? 1 : 0;
            }
            if (x < (xLength - 1))
            {
                ret += blocks[y - 1][x + 1].IsBomb ? 1 : 0;
            }
        }
        if (x > 0)
        {
            ret += blocks[y][x - 1].IsBomb ? 1 : 0;
        }
        if (x < (xLength - 1))
        {
            ret += blocks[y][x + 1].IsBomb ? 1 : 0;
        }
        if (y < (yLength - 1))
        {
            ret += blocks[y + 1][x].IsBomb ? 1 : 0;
            if (x > 0)
            {
                ret += blocks[y + 1][x - 1].IsBomb ? 1 : 0;
            }
            if (x < (xLength - 1))
            {
                ret += blocks[y + 1][x + 1].IsBomb ? 1 : 0;
            }
        }
        return ret;
    }

    /// <summary>
    /// �u���b�N���J�����Ƃ��̃C�x���g���󂯎��
    /// </summary>
    /// <param name="blockEvent"> ���s���ꂽ�C�x���g </param>
    /// <param name="block"> �J�����u���b�N </param>
    void OpenBlockCallback(Block.BlockEvent blockEvent, Block block)
    {
       
        switch (blockEvent)
        {
            case Block.BlockEvent.OpenWide:
                EventOpenWide(block);
                break;
                //add
            case Block.BlockEvent.Lifedic:
                lifeCount--;
                break;
            
        }
        //add
       
        if (lifeCount<0)
        {
            EventGameOver();
            isGameover = true;
        }
        else
        {
            onLifeDic(lifeCount);
        }
      
        if (isGameover)
        {
            // �Q�[���I�[�o�[�Ȃ̂ŃN���A�ɂ͂Ȃ�Ȃ�.
            return;
        }
        // �N���A����.
        var blankCount = 0;
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                //TODO 9/1 !blocks[y][x]�͉����Ӗ����Ă���̂ł��傤���B
                /*
                ANS: 9/1 18:00
                �����ł͏�Ԃ����ʂ���Ă��Ȃ��u���b�N���𐔂��Ă��܂��B
                �c��u���b�N�������e�̌��Ɠ������ɃQ�[���N���A�ƂȂ�܂��B

                blocks[y][x].IsOpen�Ƃ͊J���Ă����ԁi�����̃u���b�N���J���Ă����ԁj�ł��B
                �����ے肵�Ă���̂�if (!blocks[y][x].IsOpen)�Ƃ̓u���b�N���J���Ă��Ȃ���ԁi���e���������킩��Ȃ���ԁj�𒲂ׂĂ��܂��B
                 */
                if (!blocks[y][x].IsOpen)
                {
                    ++blankCount;
                }
            }
        }
        if (blankCount == bombCount)
        {
            onGameClear?.Invoke();
        }
    }
    public void GameOverAction()
    {

        EventGameOver();
        isGameover = true;

    }

    /// <summary>
    /// ���𗧂Ă����A�����Ȃ��Ȃ������ɌĂяo�����
    /// </summary>
    /// <param name="block"></param>
    void OnFlagCallback()
    {
        var flagCount = bombCount;
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                if (blocks[y][x].IsFlag)
                {
                    --flagCount;
                }
            }
        }
        onBombCountRefresh?.Invoke(flagCount);
    }

    /// <summary>
    /// OpenWide�C�x���g�̎��ɌĂяo��
    /// </summary>
    /// <param name="block"> �J�����S�̃u���b�N�A���̃u���b�N�͐�����0 </param>
    void EventOpenWide(Block block)
    {
        // JoinCount��0�̎��͎���������I�ɊJ��.
        int y = block.Y;
        int x = block.X;
        if (y > 0)
        {
            blocks[y - 1][x].SetOpen();
            if (x > 0)
            {
                blocks[y - 1][x - 1].SetOpen();
            }
            //TODO 9/1 �ǂ����ĉ��� if (x < (xLength - 1))���K�v�Ȃ̂ł��傤���B
            /*
            ANS: 9/1 18:00
            blocks[y - 1][x + 1].SetOpen()���s���Ă���̂�x���ő�l - 1�i�z��̍ő�v�f�ԍ��j�̎��ɂ͒��ׂĂ͂����܂���

            �Ⴆ�� new blocks[5][5]�̏ꍇ�Ɏ��̗l�ȋL�q���s���ƃG���[�ƂȂ�܂��̂�if���ł̐��䂪�K�v�ƂȂ�܂��B
            blocks[y - 1][4 + 1].SetOpen();     // 4 + 1���A�N�Z�X�ᔽ
             */
            if (x < (xLength - 1))
            {
                blocks[y - 1][x + 1].SetOpen();
            }
        }
        if (x > 0)
        {
            blocks[y][x - 1].SetOpen();
        }
        if (x < (xLength - 1))
        {
            blocks[y][x + 1].SetOpen();
        }
        if (y < (yLength - 1))
        {
            blocks[y + 1][x].SetOpen();
            if (x > 0)
            {
                blocks[y + 1][x - 1].SetOpen();
            }
            if (x < (xLength - 1))
            {
                blocks[y + 1][x + 1].SetOpen();
            }
        }
    }

    /// <summary>
    /// GameOver�C�x���g�̎��ɌĂяo��
    /// </summary>
    void EventGameOver()
    {
        // �Q�[���I�[�o�[.
        for (int y = 0; y < blocks.Length; ++y)
        {
            for (int x = 0; x < blocks[y].Length; ++x)
            {
                blocks[y][x].SetOpen();
            }
        }
        onGameOver?.Invoke();
    }
}
