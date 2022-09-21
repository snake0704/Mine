using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Q�[���R���g���[���[�̓V�[�����ړ����Ă��K����������O���[�o���ȃN���X
public class GameController
{
    #region �V���O���g��
    private static GameController instance = new GameController();
    public static GameController Instance => instance;

    private GameController()
    {
        // �V���O���g������.
    }
    #endregion

    /// <summary>
    /// �^�C�g������Q�[�����C���ɓn���f�[�^
    /// </summary>
    public class StageData
    {
        // X���W�̒���
        public int X { get; private set; } = 3;
        // Y���W�̒���
        public int Y { get; private set; } = 3;
        // ���e�̌�
        public int BombCount { get; private set; } = 1;
        //add
        public int LifeCount { get; private set; } = 1;
        public float TimeLimit { get; private set; } =1;
        public float ElapsedTime{ get; private set; } =1;

        public void SetData(int y, int x, int lifeCount, float timeLimit/*,float elapsedTime*/)
        {
            //TODO 8/31 Mathf.RoundToInt���߂������ɂ��āi10.7�̏ꍇ��10�ɂ���j�Ƃ������Ƃ͕�����̂ł����A(x * y * 0.20f)�͉���\���Ă���̂ł��傤���B
            /*
            ANS: 9/1
            �����ł͔��e�̐����Ȃ�ƂȂ����傤�ǂ��������w�肵�Ă��邾���Ȃ̂ŁA�v�Z���ɂ��܂�Ӗ��͂���܂���B�u���b�N�̑�����20$�𔚒e�ɂ���Ƃ����Ӗ��ƂȂ�܂��B
            ���ɔ��e�̐�����͂��Ȃ��Ă�SetData�����삵�܂��B������֐��̃I�[�o�[���[�h�Ƃ����܂��B
            �Q�l�F�I�[�o�[���[�h�ihttp://www.wisdomsoft.jp/179.html�j
             */
            SetData(x, y, Mathf.RoundToInt(x * y * 0.20f),lifeCount,timeLimit/*,elapsedTime*/);
        }

    
        public void SetData(int y, int x, int bombCount, int lifeCount, float timeLimit/*,float elapsedTime*/)
        {
            //���ꂼ�����Ă���
          
            X = x;
            Y = y;
            BombCount = bombCount;
            LifeCount = lifeCount;
            TimeLimit = timeLimit;
            //ElapsedTime = elapsedTime;

        }
    }

    public StageData PlayStageData { get; private set; } = new StageData();
}
