using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

enum WeaponType { Skin, Bone };

public class CarriageManager : MonoBehaviour
{

    public static CarriageManager instance;

    public WeaponData proposeWeaponData;

    public AudioSource[] audioSfxSources; // [0] 다음 환자 [1] 구매 [2] 구매x [3] 등장

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetNextPatient();
        SetNewPropose();
    }


    public void LoadMainScene()
    {
        AudioManager.instance.PlayAudioSource(audioSfxSources[0]);
        CarriageUIManager.instance.SetFade(true, () =>
        {
            DOTween.KillAll();
            SceneManager.LoadScene("MainScene");
        });
    }

    public void SetNextPatient()
    {
        var patientList = DataManager.instance.AllPatientDataList
            .FindAll(x => DataCarrier.instance.honor >= x.honorRange[0] && DataCarrier.instance.honor <= x.honorRange[1])
            .ToList();

        PatientData patientData = patientList[Random.Range(0, patientList.Count)];

        DataCarrier.instance.nextPatientData = patientData;

        foreach (PatientData i in patientList)
            print(i.name);

        CarriageUIManager.instance.SetPatientText(patientData);

        AudioManager.instance.PlayAudioSource(audioSfxSources[3]);
    }

    public void SetNewPropose()
    {
        string type = Random.Range(0, 2) == 0 ? "뼈" : "가죽";

        var curWeapon = DataCarrier.instance.weapons.Find(x => x.type == type);

        var targetWeaponList = DataManager.instance.AllWeaponDataList
            .FindAll(x => x.type == type)
            .FindAll(x => x.name != curWeapon.name);

        proposeWeaponData = targetWeaponList[Random.Range(0, targetWeaponList.Count)];

        CarriageUIManager.instance.SetProposeText(curWeapon, proposeWeaponData);

        AudioManager.instance.PlayAudioSource(audioSfxSources[3]);
    }

    public void SelectProposeButton(bool isBuy)
    {

        print("선택");

        if(isBuy)
        {
            if (DataCarrier.instance.gold < proposeWeaponData.cost)
            {
                AudioManager.instance.PlayAudioSource(audioSfxSources[2]);
                return;
            }

            print("구매");
            var curWeapon = DataCarrier.instance.weapons.Find(x => x.type == proposeWeaponData.type);
            var curWeaponIndex = DataCarrier.instance.weapons.IndexOf(curWeapon);

            DataCarrier.instance.weapons[curWeaponIndex] = proposeWeaponData;

            DataCarrier.instance.gold -= proposeWeaponData.cost;

            AudioManager.instance.PlayAudioSource(audioSfxSources[1]);
        }
        else
        {
            AudioManager.instance.PlayAudioSource(audioSfxSources[2]);
        }

        CarriageUIManager.instance.DumpProposePaper();
        CarriageUIManager.instance.SetPropertyText();
    }
}
