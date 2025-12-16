using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ShootingBulletsBox : MonoBehaviour
{
    public ColorType color;
    public GameObject bulletPrefab;
    public int bulletCount = 10;
    public Transform spawnPoint;
    public TMPro.TextMeshPro bulletCountText;
    public bool isTurretClickable;
    int turretSlot;


    private Coroutine shootRoutine;

    private void Start()
    {
        isTurretClickable = true;
        UpdateBulletCountUI();
    }

    public void UpdateBulletCountUI()
    {
        bulletCountText.text = bulletCount.ToString();
    }
    void OnMouseDown()
    {
        if (isTurretClickable)
            GameManager.instance.TryAssignTurret(this, turretSlot);
        Debug.Log("OnMouseClicked");
    }

    public void ShootBullets(int startRowNum = 0, int endRowNum = 10)
    {
        shootRoutine = StartCoroutine(Shoot(startRowNum, endRowNum));
        Debug.Log("StartCourotine");
    }

    public void StopShootBullets()
    {
        if (shootRoutine != null)
            StopCoroutine(shootRoutine);
        Debug.Log("StopCourotine");
    }

    IEnumerator Shoot(int startRowNum, int endRowNum)
    {
        while (bulletCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = startRowNum; i < endRowNum; i++)
            {
                Debug.Log(i + " rowsssssssssssss");
                if (GameManager.instance.rowsData[i].boxes[0].colorType == color)
                {
                    ShootBullet(i);
                    GameManager.instance.rowsData[i].boxes.RemoveAt(0);
                    bulletCountText.text = (--bulletCount).ToString();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return null;
        }
        GameManager.instance.shootingTurrretSlots[turretSlot] = null;
        Destroy(gameObject);
    }

    private void ShootBullet(int row)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 targetPos = GameManager.instance.shootingBulletsPosition[row].position;
        Debug.Log(targetPos);

        Vector3 direction = (targetPos - spawnPoint.position).normalized;

        bullet.GetComponent<BulletComponnet>().Init(targetPos);
        /*rb.linearVelocity = direction * 10f;*/
        Debug.DrawRay(spawnPoint.position, direction * 5f, Color.red, 2f);
    }
}