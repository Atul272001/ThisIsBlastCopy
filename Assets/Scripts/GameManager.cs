using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public List<RowData> rowsData;
    public Transform[] shootingBulletsPosition;
    public Transform[] turrretShootingPosition;
    public ShootingBulletsBox[] shootingTurrretSlots;
    public List<Transform> restingTurretPosition;
    public List<ShootingBulletsBox> restingTurretSlots;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        shootingTurrretSlots = new ShootingBulletsBox[3];
    }

    public void TryAssignTurret(ShootingBulletsBox clickedTurret, int slot)
    {
        for (int i = 0; i < turrretShootingPosition.Length; i++)
        {
            if (shootingTurrretSlots[i] == null)
            {
                shootingTurrretSlots[i] = clickedTurret;
                slot = i;
                clickedTurret.gameObject.transform.DOMove(turrretShootingPosition[i].position, 0.5f);
                clickedTurret.ShootBullets();
                clickedTurret.isTurretClickable = false;
                UpdateTurretPosition();
                StartCoroutine(MergeTurrets());
                return;
            }
        }
    }

    IEnumerator MergeTurrets()
    {
        int temp = 0;
        for (int i = 0; i < shootingTurrretSlots.Length; i++)
        {
            if (shootingTurrretSlots[0].color == shootingTurrretSlots[i].color)
            {
                temp++;
            }
        }
        if(temp == 3)
        {
            int randomNum = UnityEngine.Random.Range(0, temp);
            for (int i = 0; i < shootingTurrretSlots.Length; i++)
            {
                if(i != randomNum)
                {
                    yield return new WaitForSeconds(0.2f);
                    shootingTurrretSlots[i].gameObject.transform.DOMove(turrretShootingPosition[randomNum].position, 1f);
                    shootingTurrretSlots[randomNum].bulletCount += shootingTurrretSlots[i].bulletCount;
                    shootingTurrretSlots[randomNum].UpdateBulletCountUI();
                    Destroy(shootingTurrretSlots[i].gameObject);
                }
            }
        }
    }

    public void UpdateTurretPosition()
    {
        if (restingTurretSlots.Count == 0)
            return;
        restingTurretSlots.RemoveAt(0);

        for (int i = 0; i < restingTurretSlots.Count; i++)
        {
            if (restingTurretSlots[i] != null)
            {
                restingTurretSlots[i].transform.position = restingTurretPosition[i].position;
            }
        }
    }

}

[Serializable]
public class RowData
{
    public Transform rowPosition;
    public List<BoxComponent> boxes;
}
