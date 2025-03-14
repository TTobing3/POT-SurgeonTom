using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class CarriageUIManager : MonoBehaviour
{
    public static CarriageUIManager instance;

    [Header("화면")]
    public Image fade;

    [Header("재화 UI")]
    public TextMeshProUGUI killScore;
    public TextMeshProUGUI goldText, honorText;

    [Header("수술 의뢰서 UI")]
    public RectTransform amutationPaper;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI tierText, jobText, statusText, historyText;

    [Header("제안서 UI")]
    public RectTransform proposePaper;
    public TextMeshProUGUI descriptionText, proposeText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        SetFade(false, null);

        ZoomCamera(4);

        if (CarriageManager.instance != null && DataCarrier.instance != null)
        {
            SetPropertyText();
        }
    }

    public void SetPropertyText()
    {
        killScore.DOText(DataCarrier.instance.killScore + "", 1);
        goldText.DOText(DataCarrier.instance.gold + "", 1);
        honorText.DOText(DataCarrier.instance.honor + "", 1);

    }

    public void SetFade(bool isFadeOut, System.Action action)
    {
        if (isFadeOut)
        {
            fade.color = new Color(0, 0, 0, 0);
            fade.gameObject.SetActive(true);

            fade.DOFade(1, 1).OnComplete(() =>
            {
                if (action != null) action();
            });
        }
        else
        {

            fade.color = new Color(0, 0, 0, 1);
            fade.gameObject.SetActive(true);

            fade.DOFade(0, 1).OnComplete(() =>
            {
                fade.gameObject.SetActive(false);
            });
        }
    }

    public void ZoomCamera(float size)
    {
        Camera.main.DOOrthoSize(size, 3);
    }

    public void SetPatientText(PatientData patientData)
    {
        amutationPaper.anchoredPosition = new Vector2(0, -1200);

        amutationPaper.DOAnchorPosY(0, 2).SetEase(Ease.OutBack);

        tierText.text = "계급 : "+patientData.tier;
        jobText.text = "직업 : " + patientData.job;

        nameText.text = "이름 : " + patientData.name;

        statusText.text = "특이사항 : " + patientData.status;
        historyText.text = "[ 배경 ]\n" + patientData.history;

        historyText.text += $"\n\n명성 ±{patientData.honor} 금화 {patientData.gold}";
        if (DataCarrier.instance.killScore + 1 > 10 || DataCarrier.instance.honor - patientData.honor < -10)
            historyText.text += "\n\n[ 실패 시 사형 ]";
    }

    public void SetProposeText(WeaponData curWeaponData, WeaponData weaponData)
    {
        proposePaper.anchoredPosition = new Vector2(0, -1200);

        proposePaper.DOAnchorPosY(0, 2).SetEase(Ease.OutBack);

        proposeText.text = $"당신이 {curWeaponData.name}(와)과\n" +
            $"금화 {weaponData.cost}닢을 준다면\n" +
            $"내가 {weaponData.name}(을)를 주겠소.";

        descriptionText.text = $"{weaponData.name}\n\n" +
            $"{weaponData.description}";
    }

    public void DumpProposePaper()
    {
        print("치워");
        proposePaper.DOAnchorPosY(-1200, 1).SetEase(Ease.OutBack);
    }
}
