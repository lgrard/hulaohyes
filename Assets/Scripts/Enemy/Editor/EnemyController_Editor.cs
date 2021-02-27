using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hulaohyes.enemy;

[CustomEditor(typeof(EnemyController))]
public class EnemyController_Editor : Editor
{
    const float DEG2RAD = Mathf.PI / 180;

    private void OnSceneGUI()
    {
        EnemyController enemyController = (EnemyController)target;
        Handles.color = Color.green;
        DrawArc(enemyController, enemyController.ViewAngles, enemyController.ViewRadius);
        Handles.DrawWireDisc(enemyController.transform.position, Vector3.up, enemyController.ViewRadius);
        Handles.color = Color.yellow;
        DrawArc(enemyController, enemyController.GrabAngles, -enemyController.GrabRadius);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(enemyController.transform.position, Vector3.up, enemyController.MeleeRadius);
    }

    void DrawArc(EnemyController pEnemyController, float pAngle, float pRadius)
    {
        Vector3 lPos = pEnemyController.transform.position;
        float lAngle = (pAngle) / 2;

        Handles.DrawLine(lPos, new Vector3(lPos.x+Mathf.Sin((lAngle + pEnemyController.transform.eulerAngles.y) * DEG2RAD) * pRadius, lPos.y, lPos.z+Mathf.Cos((lAngle + pEnemyController.transform.eulerAngles.y) * DEG2RAD) * pRadius));
        Handles.DrawLine(lPos, new Vector3(lPos.x+Mathf.Sin((-lAngle+ pEnemyController.transform.eulerAngles.y) * DEG2RAD) * pRadius, lPos.y, lPos.z+Mathf.Cos((-lAngle+pEnemyController.transform.eulerAngles.y) * DEG2RAD) * pRadius));

        Handles.DrawWireArc(lPos, Vector3.up, pEnemyController.transform.forward, lAngle, pRadius);
        Handles.DrawWireArc(lPos, Vector3.up, pEnemyController.transform.forward, -lAngle, pRadius);
    }
}
