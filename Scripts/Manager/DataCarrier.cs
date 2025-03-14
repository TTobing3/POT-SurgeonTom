using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier instance;

    [Header("Àç»ê")]
    public int gold;
    public int honor;
    public int killScore;
    public List<WeaponData> weapons;

    public PatientData nextPatientData;

    public bool isFirst = true;

    public string[] startItem;

    public bool isMute = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        weapons.Add(DataManager.instance.AllWeaponDatas[startItem[0]]);
        weapons.Add(DataManager.instance.AllWeaponDatas[startItem[1]]);
    }
}
