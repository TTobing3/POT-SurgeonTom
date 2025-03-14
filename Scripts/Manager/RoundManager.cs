using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        StartAmputation();
    }

    [ContextMenu("수술 시작")]
    public void StartAmputation()
    {
        AmputationManager.instance.SetPatient();

        if(DataCarrier.instance.isFirst)
        {
            DataCarrier.instance.isFirst = false;
            UIManager.instance.ShowScreen("붉은색 수술 부위를 눌러서\n수술합니다.",
                () => { UIManager.instance.ShowScreen("살갖을 모두 썰면, \n뼈를 절단할 수 있습니다.", 
                    () => { UIManager.instance.ShowScreen("수술이 끝나기 전에\n체력이 모두 닳면 환자는 죽습니다.", 
                        () => { UIManager.instance.ShowScreen("장비를 눌러서\n변경할 수 있습니다.",
                            () => { UIManager.instance.ShowScreen("수술을 성공하여\n명예를 획득하십시오.", 
                                () => { UIManager.instance.ShowScreen("명예가 드높아지면,\n왕을 치료하여 부귀영화를 누릴 것입니다!",
                                    () => { UIManager.instance.ShowScreen("다만, 수술에 실패하면\n명예가 실추될 것입니다.",
                                        () => { UIManager.instance.ShowScreen("자! 올리버의 절단 수술을\n성공적으로 마치십시오!",
                                            () => { AmputationManager.instance.StartAmputation();
                                     }); }); }); }); }); }); }); });

        }
        else
        {

            UIManager.instance.ShowScreen("수술 시작", () => { AmputationManager.instance.StartAmputation(); });

        }
        // 카메라
        UIManager.instance.ZoomCamera(2.5f); //2.1
    }

    public void FinishAmputation(bool isSuccess)
    {
        UIManager.instance.SetFade(true, ()=>
        {
            DOTween.KillAll();

            if (DataCarrier.instance.killScore > 10 || DataCarrier.instance.honor < -10)
            {
                SceneManager.LoadScene("CutScene");
            }
            else if (DataCarrier.instance.honor > 10000)
            {
                SceneManager.LoadScene("CutScene");
            }
            else
            {
                SceneManager.LoadScene("CarriageScene");
            }
        });
    }

}
