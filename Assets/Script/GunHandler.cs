using System.Collections;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private Bullet _BulletPrefab;
    [SerializeField] private Transform _BulletPosition;
    [SerializeField] private Transform _Head;
    [SerializeField] private float _FireRate;
    [SerializeField] private int _TotalGunLife;
    [SerializeField] private int _GunIndex;
    public int GunTimer;
    public bool CanFire = false;

    public void Fire()
    {
        GunTimer = _TotalGunLife;
        StopCoroutine(nameof(FireCoroutine));
        StartCoroutine(nameof(FireCoroutine));

        StopCoroutine(nameof(StartGunTimer));
        StartCoroutine(nameof(StartGunTimer));
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitUntil(() => CanFire);
        GameManager gameManager = GameManager.Instance;
        while (CanFire)
        {
            Bullet bullet = gameManager.PoolManager.GetSmallBullet();
            if (bullet != null)
            {
                GameObject nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    bullet.transform.SetParent(_BulletPosition);
                    bullet.transform.localPosition = Vector3.zero;
                    Vector3 initialDirec = nearestEnemy.transform.position - _BulletPosition.position;
                    bullet.Fire(initialDirec);
                    Vector3 targetPosition = _BulletPosition.position + initialDirec.normalized;
                    _Head.LookAt(targetPosition);
                    Vector3 headRotation = _Head.eulerAngles;
                    _Head.eulerAngles = new Vector3(0, headRotation.y, 0);
                }
                else
                {
                    bullet.gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(_FireRate);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private IEnumerator StartGunTimer()
    {
        GameManager gameManager = GameManager.Instance;
        while (GunTimer > 0)
        {
            GunTimer--;
            gameManager.UpdateGunTimer(_GunIndex, GunTimer);
            yield return new WaitForSeconds(1);
        }
        CanFire = false;

        if (_GunIndex == 0)
            GameManager.Instance.PoolManager.ReturnPistolGunObject(this);
        else if (_GunIndex == 1)
            GameManager.Instance.PoolManager.ReturnMachineGunObject(this);
        else if (_GunIndex == 2)
            GameManager.Instance.PoolManager.ReturnLauncherGunObject(this);
        GameManager.Instance.ResetTimer(_GunIndex);
    }
}
