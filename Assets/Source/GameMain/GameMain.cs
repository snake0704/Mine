using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


/// <summary>
/// �Q�[�����C���A�n���ꂽ�R�[���o�b�N�ɉ����ăQ�[���S�̂ɉe����^����U�镑�����s��
/// </summary>
public class GameMain : MonoBehaviour
{
    [SerializeField]
    Stage stage;
    [SerializeField]
    Button buttonRestart;
    [SerializeField]
    Button buttonTitle;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    GameObject gameClear;
    [SerializeField]
    TextMeshProUGUI textValue;
    //add
    [SerializeField]
    TextMeshProUGUI textValue2;
  
    [SerializeField]
    TextMeshProUGUI textValue4;
    [SerializeField]
    TextMeshProUGUI textValue5;


    int maxBombCount;
    
    void Start()
    {
        buttonRestart.onClick.AddListener(() => ReStart());
        buttonTitle.onClick.AddListener(() => OnTitleScene());
        ReStart();
    }

    void ReStart()
    {
        //TODO 9/1 Instance.PlayStageData�͉����Ӗ����Ă���̂ł��傤��
        /*
        ANS: 9/1
        Instance.PlayStageData �� class StageData�ł��B
        GameController.cs��
        public StageData PlayStageData { get; private set; } 
        �Ə�����Ă��āA�����ɃA�N�Z�X���s���Ă��܂��B
        ���b�X�����ɐ������s���܂������A�����ł̓V���O���g�����g�p���Ă��܂��B
        �Q�l�F�V���O���g���ihttps://senoriblog.com/csharp-singleton/�j
         */
        int y = GameController.Instance.PlayStageData.Y;
        int x = GameController.Instance.PlayStageData.X;
        maxBombCount = GameController.Instance.PlayStageData.BombCount;
        TimeLimit2 = GameController.Instance.PlayStageData.TimeLimit;

    // �X�e�[�W�̏�����.
    stage.Setup(y, x, maxBombCount,GameController.Instance.PlayStageData.LifeCount,  GameController.Instance.PlayStageData.ElapsedTime,GameController.Instance.PlayStageData.TimeLimit, OnBombCountRefresh, OnGameOver, OnGameClear, OnLifeDic);
        textValue2.SetText(GameController.Instance.PlayStageData.LifeCount.ToString());
        textValue5.SetText(GameController.Instance.PlayStageData.ElapsedTime.ToString());
        textValue4.SetText(GameController.Instance.PlayStageData.TimeLimit.ToString());
        gameOver.SetActive(false);
        gameClear.SetActive(false);
    }
 // add
    void OnLifeDic(int lifeCount)
    {

        textValue2.SetText(lifeCount.ToString());
    }
    //add
    public float TimeLimit2 = GameController.Instance.PlayStageData.TimeLimit;
    void Update()
    {
        if (!stage.IsInit)
        {
            return;
        }
        if(stage.StopCount)
        {
            return;
        }
        if (TimeLimit2 > 0)
        {
            TimeLimit2 -= Time.deltaTime;
            if (TimeLimit2 <= 0)
            {
               
                stage.GameOverAction();
            }
        }

        else
        {
            TimeLimit2 = 0;

        }
        textValue4.SetText(TimeLimit2.ToString("0"));
    }

    //�Q�[���I�[�o�[���ɌĂ΂��
    void OnGameOver()
    {
        gameOver.SetActive(true);
        stage.GameEnd();
        float Limit = GameController.Instance.PlayStageData.TimeLimit;
        float clearTime = TimeLimit2;
        //�o�ߎ��Ԃ��v�Z
        float elapsedTime = Limit - clearTime;
        textValue5.SetText(elapsedTime.ToString("Elapsed Time:"+"0"));
        stage.StopCount = true;
      
    }

    // �Q�[���N���A���ɌĂ΂��
    void OnGameClear()
    {
        gameClear.SetActive(true);
        stage.GameEnd();
        //add
        float Limit = GameController.Instance.PlayStageData.TimeLimit;
        float clearTime = TimeLimit2;
        //�o�ߎ��Ԃ��v�Z
        float elapsedTime = Limit - clearTime;
        textValue5.SetText(elapsedTime.ToString("0"));
        stage.StopCount = true;
     
    }

    // ���e�̃J�E���g�\�����X�V����
    void OnBombCountRefresh(int count)
    {
        textValue.SetText(count.ToString());
    }

   
    

    
void OnTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
  
}
