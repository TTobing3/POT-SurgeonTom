using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmputationManager : MonoBehaviour
{
    public static AmputationManager instance;

    [Header("��ǥ �׽�Ʈ")]
    public Transform[] targetAreaTransform; // [0] arm [1] leg
    public Transform hitPositionTransform, startPositionTransform;

    [Header("ĳ����")]
    public Surgeon surgeon;
    public Patient patient;

    public bool isAmputating = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartAmputation()
    {
        isAmputating = true;
    }

    public void SetPatient()
    {
        patient.Set( DataCarrier.instance.nextPatientData.name == "" ?  DataManager.instance.AllPatientDatas["�ø���"] : DataCarrier.instance.nextPatientData );
    }

    public void FinishAmputation(bool isSuccess)
    {
        isAmputating = false;

        for (int i = 0; i < 2; i++)
        {
            targetAreaTransform[i].GetComponent<SpriteRenderer>().DOFade(0, 1);
        }

        if (isSuccess)
        {
            DataCarrier.instance.honor += patient.patientData.honor;
            DataCarrier.instance.gold += patient.patientData.gold;
            UIManager.instance.ShowScreen("���� ����!", () => { RoundManager.instance.FinishAmputation(isSuccess); });
        }
        else
        {
            DataCarrier.instance.honor -= patient.patientData.honor;
            DataCarrier.instance.killScore += 1;
            UIManager.instance.ShowScreen("���", () => { RoundManager.instance.FinishAmputation(isSuccess); });
        }

        surgeon.AnimateStand();
    }

    public void CutDown(Vector3 clickPos)
    {
        var startPos = startPositionTransform.position;
        var targetY = hitPositionTransform.position.y;

        var hitAreaX = targetAreaTransform[(int)patient.hitPointType].position.x;

        // Ÿ�� x��ǥ ���
        float resultPosX = GetHitX(startPos, clickPos, targetY);

        if (resultPosX < hitAreaX - 0.6f) resultPosX = hitAreaX - 0.6f;
        else if(resultPosX > hitAreaX + 0.6f) resultPosX = hitAreaX + 0.6f;

        var resultPos = new Vector3(resultPosX, targetY, 0);

        hitPositionTransform.position = resultPos;

        // ���� ��� �� ����
        float hitAngle = GetAngle(startPos, resultPos);
        hitPositionTransform.eulerAngles = new Vector3(0, 0, Random.Range(-10, 10f));
        Surgeon.instance.transform.eulerAngles = new Vector3(0, 0, -hitAngle);

        // �ӽ� ����Ʈ
        var hitSpriteRenderer = hitPositionTransform.GetComponent<SpriteRenderer>();
        DOTween.Kill(hitSpriteRenderer);
        hitSpriteRenderer.color = Color.white;
        hitSpriteRenderer.DOFade(0, 0.5f);

        //�ǰ� ���� ���
        var targetAreaWidth = targetAreaTransform[(int)patient.hitPointType].GetComponent<BoxCollider2D>().size.x * targetAreaTransform[(int)patient.hitPointType].localScale.x;

        if (hitAreaX - targetAreaWidth/5 < resultPosX && resultPosX < hitAreaX + targetAreaWidth / 5 )
        {
            patient.Hit(HitType.Perfect);
        }
        else if (hitAreaX - targetAreaWidth / 2 < resultPosX && resultPosX < hitAreaX + targetAreaWidth / 2)
        {
            patient.Hit(HitType.Hit);
        }
        else
        {
            patient.Hit(HitType.Miss);
        }

        // ����Ʈ
        hitPositionTransform.GetComponent<ParticleSystem>().Play();
    }

    public bool CheckClickAvailable(Vector3 clickPos)
    {
        if (clickPos.x < targetAreaTransform[(int)patient.hitPointType].position.x - 2 || targetAreaTransform[(int)patient.hitPointType].position.x + 2 < clickPos.x) return false;
        else return true;
    }

    float GetHitX(Vector3 startPos, Vector3 resultPos, float targetY)
    {
        float resultPosX;

        // �� ���� �������� ���ĵ� ��� (X ��ǥ�� ������ ����)
        if (Mathf.Approximately(startPos.x, resultPos.x))
        {
            resultPosX = startPos.x; // X ��ǥ�� ����
        }
        else
        {
            // ���� �������� ���� ���
            float slope = (resultPos.y - startPos.y) / (resultPos.x - startPos.x);

            // y = mx + c ������ c (����) ���
            float intercept = startPos.y - slope * startPos.x;

            // targetY������ x�� ���
            float xAtY = (targetY - intercept) / slope;

            resultPosX = xAtY;
        }

        var randomAcc = Random.Range(-surgeon.curWeapon.accuracy, surgeon.curWeapon.accuracy);

        print(resultPosX+" : "+randomAcc);

        return resultPosX + randomAcc;
    }

    float GetAngle(Vector3 startPos, Vector3 clickPos)
    {
        // ���� ���� ���
        Vector3 direction = clickPos - startPos;

        // atan2�� ����� ���� ��� (���� ���� ���� ��ȯ)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��� ������ ���� ���� ������ 0~360���� ��ȯ
        if (angle < 0)
        {
            angle += 360f;
        }

        return angle + 90;
    }

}
