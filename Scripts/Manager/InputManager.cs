using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Transform clickPositionTransform;

    AmputationManager amputationManager;

    void Awake()
    {
        amputationManager = GetComponent<AmputationManager>();
    }

    void Update()
    {
        if (AmputationManager.instance.isAmputating && Input.GetMouseButtonDown(0))
        {
            Vector3 screenPosition = Input.mousePosition;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(screenPosition.x, screenPosition.y, 10f)
            );

            clickPositionTransform.position = worldPosition;

            if (!AmputationManager.instance.CheckClickAvailable(worldPosition)) return;

            AmputationManager.instance.CutDown(worldPosition);
            AmputationManager.instance.surgeon.AnimateCutDown(true);
        }

        if(AmputationManager.instance.isAmputating && Input.GetMouseButtonUp(0))
        {
            AmputationManager.instance.surgeon.AnimateCutDown(false);
        }
    }
}
