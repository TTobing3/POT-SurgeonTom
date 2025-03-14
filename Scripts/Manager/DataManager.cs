using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[System.Serializable]
public class WeaponData
{
    public int number;
    public string name, type;
    public int cost, chop, crush,  stun;
    public float accuracy;
    public string description;

    public WeaponData(int number, string name, string type, int cost, int chop, int crush, float accuracy, int stun, string description)
    {
        this.number = number;
        this.name = name;
        this.type = type;
        this.cost = cost;
        this.chop = chop;
        this.crush = crush;
        this.accuracy = accuracy;
        this.stun = stun;
        this.description = description;
    }

}

[System.Serializable]
public class PatientData
{
    public int number;
    public string name, tier, job;
    public int[] honorRange;
    public int life;
    public float size;
    public int blood, skin, bone, stun, honor, gold;
    public string status, history;

    public PatientData(int number, string name, string tier, string job, int[] honorRange, int life, float size, int blood, int skin, int bone, int stun, int honor, int gold, string status, string history)
    {
        this.number = number;
        this.name = name;
        this.tier = tier;
        this.job = job;
        this.honorRange = honorRange;
        this.life = life;
        this.size = size;
        this.blood = blood;
        this.skin = skin;
        this.bone = bone;
        this.stun = stun;
        this.honor = honor;
        this.gold = gold;
        this.status = status;
        this.history = history;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string[] TextData = new string[10];

    public Dictionary<string, WeaponData> AllWeaponDatas = new Dictionary<string, WeaponData>();
    public List<WeaponData> AllWeaponDataList = new List<WeaponData>();

    public Dictionary<string, PatientData> AllPatientDatas = new Dictionary<string, PatientData>();
    public List<PatientData> AllPatientDataList = new List<PatientData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // WeaponData
        string[] line = TextData[0].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            // WeaponData 객체 생성
            var weaponData = new WeaponData(
                int.Parse(e[0]),  // numbe
                e[1],             // name
                e[2],             // type
                int.Parse(e[3]),  // cose
                int.Parse(e[4]),  // cut
                int.Parse(e[5]),  // crush
                float.Parse(e[6]),  // accuracy
                int.Parse(e[7]),  // stun
                e[8]              // description
            );

            // Dictionary 및 List에 추가
            AllWeaponDatas.Add(e[1], weaponData); // name을 키로 사용
            AllWeaponDataList.Add(weaponData);
        }

        // Patient
        line = TextData[1].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            // patientData 객체 생성
            var patientData = new PatientData(
                int.Parse(e[0]),
                e[1],
                e[2],
                e[3],
                e[4].Split('~').Select( x => int.Parse(x) ).ToArray(),
                int.Parse(e[5]),
                float.Parse(e[6]),
                int.Parse(e[7]),
                int.Parse(e[8]),
                int.Parse(e[9]),
                int.Parse(e[10]),
                int.Parse(e[11]),
                int.Parse(e[12]),
                e[13],
                e[14]
            );

            // Dictionary 및 List에 추가
            AllPatientDatas.Add(e[1], patientData);
            AllPatientDataList.Add(patientData);
        }
    }
    #region Data Load

    const string
        weaponURL = "https://docs.google.com/spreadsheets/d/1i20q0PFQDbTsHzRZOxcnTZLUeC-oxiaUP7tRlALyjCY/export?format=tsv&gid=0",
        patientURL = "https://docs.google.com/spreadsheets/d/1i20q0PFQDbTsHzRZOxcnTZLUeC-oxiaUP7tRlALyjCY/export?format=tsv&gid=2124025612";


    [ContextMenu("Data Load")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(weaponURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 0);

        www = UnityWebRequest.Get(patientURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 1);

        Debug.Log("Data Load Success");
    }

    void SetDataList(string tsv, int i)
    {
        TextData[i] = tsv;
    }

    #endregion
}
