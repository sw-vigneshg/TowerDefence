using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Pooled Objects")]
    [SerializeField] private List<Enemy> EnemyObjects;
    [SerializeField] private List<Bullet> BulletObjects1;
    [SerializeField] private List<Bullet> BulletObjects2;
    [SerializeField] private List<GunHandler> PistolObjects;
    [SerializeField] private List<GunHandler> MachineGunObjects;
    [SerializeField] private List<GunHandler> LauncherObjects;

    [Header("Containers")]
    [SerializeField] private Transform EnemyContainer;
    [SerializeField] private Transform BulletContainer1;
    [SerializeField] private Transform BulletContainer2;
    [SerializeField] private Transform PistolContainer;
    [SerializeField] private Transform MachineGunContainer;
    [SerializeField] private Transform LauncherContainer;

    private void Start()
    {
        ResetPooledObjects();
    }

    private void ResetPooledObjects()
    {
        foreach (Enemy enemy in EnemyObjects)
        {
            enemy.gameObject.SetActive(false);
            enemy.transform.parent = EnemyContainer.transform;
        }
        foreach (Bullet bullet in BulletObjects1)
        {
            bullet.gameObject.SetActive(false);
            bullet.transform.parent = BulletContainer1.transform;
        }
        foreach (Bullet bullet in BulletObjects2)
        {
            bullet.gameObject.SetActive(false);
            bullet.transform.parent = BulletContainer2.transform;
        }
        foreach (GunHandler pistol in PistolObjects)
        {
            pistol.gameObject.SetActive(false);
            pistol.transform.parent = PistolContainer.transform;
        }
        foreach (GunHandler machineGun in MachineGunObjects)
        {
            machineGun.gameObject.SetActive(false);
            machineGun.transform.parent = MachineGunContainer.transform;
        }
        foreach (GunHandler launcher in LauncherObjects)
        {
            launcher.gameObject.SetActive(false);
            launcher.transform.parent = LauncherContainer.transform;
        }
    }

    // Get and Return Piston Objects .............
    public GunHandler GetPistolGunObject()
    {
        PistolObjects[0].gameObject.SetActive(true);
        PistolObjects[0].transform.localPosition = Vector3.zero;
        GunHandler gun = PistolObjects[0];
        PistolObjects.RemoveAt(0);
        return gun;
    }

    public void ReturnPistolGunObject(GunHandler pistolObject)
    {
        pistolObject.transform.SetParent(PistolContainer);
        pistolObject.gameObject.SetActive(false);
        PistolObjects.Add(pistolObject);
    }

    // Get and Return MachineGun Objects .............
    public GunHandler GetMachineGunObject()
    {
        MachineGunObjects[0].gameObject.SetActive(true);
        MachineGunObjects[0].transform.localPosition = Vector3.zero;
        GunHandler gun = MachineGunObjects[0];
        MachineGunObjects.RemoveAt(0);
        return gun;
    }

    public void ReturnMachineGunObject(GunHandler machineGunObject)
    {
        machineGunObject.transform.SetParent(MachineGunContainer);
        machineGunObject.gameObject.SetActive(false);
        MachineGunObjects.Add(machineGunObject);
    }

    // Get and Return Launcher Objects .............
    public GunHandler GetLauncherGunObject()
    {
        LauncherObjects[0].gameObject.SetActive(true);
        LauncherObjects[0].transform.localPosition = Vector3.zero;
        GunHandler gun = LauncherObjects[0];
        LauncherObjects.RemoveAt(0);
        return gun;
    }

    public void ReturnLauncherGunObject(GunHandler launcherGunObject)
    {
        launcherGunObject.transform.SetParent(LauncherContainer);
        launcherGunObject.gameObject.SetActive(false);
        LauncherObjects.Add(launcherGunObject);
    }

    // Get and Return Enemy Objects .............
    public Enemy GetEnemyObject()
    {
        EnemyObjects[0].gameObject.SetActive(true);
        EnemyObjects[0].transform.localPosition = Vector3.zero;
        Enemy enemy = EnemyObjects[0];
        EnemyObjects.RemoveAt(0);
        return enemy;
    }

    public void ReturnEnemyObject(Enemy enemyObject)
    {
        enemyObject.transform.SetParent(EnemyContainer);
        enemyObject.gameObject.SetActive(false);
        EnemyObjects.Add(enemyObject);
    }

    // Get and Return Piston and Machine Gun Bullet .............
    public Bullet GetSmallBullet()
    {
        BulletObjects1[0].gameObject.SetActive(true);
        BulletObjects1[0].transform.localPosition = Vector3.zero;
        Bullet bullet = BulletObjects1[0];
        BulletObjects1.RemoveAt(0);
        return bullet;
    }

    public void ReturnSmallBulletObject(Bullet bulletObject)
    {
        bulletObject.transform.SetParent(BulletContainer1);
        bulletObject.gameObject.SetActive(false);
        BulletObjects1.Add(bulletObject);
    }

    // Get and Return Launcher Gun Bullet .............
    public Bullet GetLauncherBullet()
    {
        BulletObjects2[0].gameObject.SetActive(true);
        BulletObjects2[0].transform.localPosition = Vector3.zero;
        Bullet bullet = BulletObjects2[0];
        BulletObjects2.RemoveAt(0);
        return bullet;
    }

    public void ReturnLauncherBulletObject(Bullet bulletObject)
    {
        bulletObject.transform.SetParent(BulletContainer2);
        bulletObject.gameObject.SetActive(false);
        BulletObjects2.Add(bulletObject);
    }
}
