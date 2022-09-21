using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMain : MonoBehaviour
{
    //Unity��Inspector����F�X�ύX�ł���悤�ɂ��Ă���
    [SerializeField]
    Button buttonEasy;
    [SerializeField]
    Button buttonNormal;
    [SerializeField]
    Button buttonHard;

    private void Start()
    {
        /*TODO�@8/31 �Ⴆ�΁uEasy�v�̃{�^�����N���b�N�����ƁA�uStartEasy�v�ɃA�N�Z�X����Ƃ������Ƃ��Ǝv���̂ł����A
        �uAddListener�v�͉����Ӗ����Ă���̂ł��傤���B�܂��A���̎��̓����_���ŕ\���Ă���Ǝv���̂ł����A���ʂ̎��ŏ����Ƃǂ��Ȃ�̂ł��傤���B*/
        /*
        ANS: 9/1
        onClick.AddListener��Button�ɗp�ӂ���Ă���u�N���b�N�������ꂽ�Ƃ��ɃR�[���o�b�N����֐��v��o�^����@�\�ł��B
        �����_���ŏ����Ȃ��ꍇ��Action��n���̂Ɠ����Ȃ̂�
        buttonEasy.onClick.AddListener(StartEasy);
        ���̂悤�ɁuStartEasy()�v�ł͂Ȃ��uStartEasy�v�ƋL�q���܂��B
        �������A���̏������̏ꍇ�͊֐��Ɉ�����n�����Ƃ��o���Ȃ��̂ŕ��i�̓����_�����g�p���Ă��邽�ߎ�ȂŃ����_���ŏ����Ă��܂��B
        ����ł���΃����_���ɂ���K�v�͂���܂���ł����B
        �Q�l�FAddListener�ň�����n���ihttps://blog.narumium.net/2017/04/15/%E3%80%90unity%E3%80%91addlistener%E3%81%A7%E5%BC%95%E6%95%B0%E3%82%92%E6%B8%A1%E3%81%99/�j
         */
        buttonEasy.onClick.AddListener(() => StartEasy());
        buttonNormal.onClick.AddListener(() => StartNormal());
        buttonHard.onClick.AddListener(() => StartHard());
    }

   
    void StartEasy()
    {
        //�c���̃}�X�ڂ̐��A���e�̐��i�Ō�̂��́j��ݒ肵�Ă���
        GameController.Instance.PlayStageData.SetData(6, 6, 5,1,120);
        
        OnNextScene();
    }
    void StartNormal()
    {
        GameController.Instance.PlayStageData.SetData(8, 10, 16,3,240);
        OnNextScene();
    }
    void StartHard()
    {
        GameController.Instance.PlayStageData.SetData(12, 20, 50,5,360);
        OnNextScene();
    }

    void OnNextScene()
    {
        //    TODO 8/31 �S����������Ă���̂�������܂���
        //�uLoadScene�v���Ӗ����Ă��邱�ƁA�Ȃ��A�i�j�̒���"Main"�Ȃ̂���������܂���B��������邱�Ƃɂ���ĉ����\�ɂ��Ă���̂ł��傤���B
        /*
        ANS: 9/1
        ���̊֐����Ăяo�����Ƃ�Main�V�[���֑J�ځi�ړ��j���Ă��܂��B
        Title�V�[������Q�[�����n�܂�AEasy���̃{�^������������Main�V�[���ֈړ����ăQ�[�����X�^�[�g���܂���
        �����ł�Main�V�[���ւ̈ړ����s���Ă��܂��B
        ����͒��ׂ�Ώo�Ă��܂��̂�UnityEngine����񋟂���Ă���N���X��֐����g�p���Ă���ꍇ�͂܂��͒��ׂĂ݂܂��傤�B
        �Q�l�FSceneManager.LoadScene�ɂ��āihttps://mogi0506.com/unity-scenemanager-loadscene/�j
        */

        SceneManager.LoadScene("Main");
    }
}
