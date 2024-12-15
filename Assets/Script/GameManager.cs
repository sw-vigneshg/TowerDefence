using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Transforms")]
    [SerializeField] private Transform TowerPosition;
    [SerializeField] private Transform[] GunHolders;
    [SerializeField] private Transform EnemiesSpawnPosition;
    private GameObject _selectedObject;

    [Header("Canvas Variables")]
    [SerializeField] private Image TowerHealthIndicator;
    [SerializeField] private TMP_Text GoldValueText;
    [SerializeField] private TMP_Text TowerHealthText;
    [SerializeField] private TMP_Text[] GunTimers;
    [SerializeField] private GameObject GameOverPanel;

    [Header("Data Types")]
    [SerializeField] public int TowerHealth;
    [SerializeField] private int GoldValue;
    [SerializeField] public int SelectedGunHolder = 0;
    public bool CanBuy = false;
    public bool GameStarted;
    public bool IsGameOver;
    private bool _IsFirstRound;

    public static Action<int> OnTowerHealthAction;
    public static Action<int, bool> OnGoldValueAction;

    [Header("Script Reference")]
    public PoolManager PoolManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetGameData();
    }

    public void OnPlayAgain()
    {
        CancelInvoke(nameof(ResetGameData));
        Invoke(nameof(ResetGameData), 3);
    }

    private void ResetGameData()
    {
        GameOverPanel.SetActive(false);
        TowerHealth = 100;
        GoldValue = 1000;
        SelectedGunHolder = 0;
        CanBuy = true;
        GameStarted = true;
        IsGameOver = false;
        _IsFirstRound = false;
        TowerHealthIndicator.fillAmount = 1;
        TowerHealthText.text = $"{TowerHealth}%";
        OnGoldValueChanges(0, true);
        foreach (TMP_Text item in GunTimers)
        {
            item.text = string.Empty;
        }
        StopCoroutine(nameof(SpawnEnemies));
        StartCoroutine(nameof(SpawnEnemies));
    }

    public Transform GetTowerPosition()
    {
        return TowerPosition;
    }

    private IEnumerator SpawnEnemies()
    {
        while (GameStarted)
        {
            Enemy enemy = PoolManager.GetEnemyObject();
            enemy.transform.parent = EnemiesSpawnPosition.transform;
            enemy.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), 0.5f, -15);
            yield return new WaitForSeconds(0.75f);
        }
    }

    public void OnGoldValueChanges(int amount, bool isAdding = true)
    {
        GoldValue = isAdding ? (GoldValue + amount) : (GoldValue - amount);
        GoldValueText.text = $"GOLD : {GoldValue}";
    }

    public void OnTowerHealthChanges(int damage)
    {
        TowerHealth -= damage;
        if (TowerHealth <= 0)
        {
            Debug.Log("Game Over");
            GameStarted = false;
            IsGameOver = true;
            OnGameOver();
            return;
        }
        TowerHealthIndicator.fillAmount = Mathf.InverseLerp(1, 100, TowerHealth);
        TowerHealthText.text = $"{TowerHealth}%";
    }

    private void OnGameOver()
    {
        GameOverPanel.SetActive(true);
        if (EnemiesSpawnPosition.transform.childCount > 0)
        {
            foreach (Transform child in EnemiesSpawnPosition)
            {
                child.gameObject.SetActive(false);
                child.transform.localPosition = new Vector3(0, 0.5f, -15);
                PoolManager.ReturnEnemyObject(child.GetComponent<Enemy>());
            }
        }
    }

    public void UpdateGunTimer(int gunIndex, int value)
    {
        GunTimers[gunIndex].text = $"{value}s";
    }

    public void ResetTimer(int gunIndex)
    {
        GunTimers[gunIndex].text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    private void SelectObject()
    {
        CanBuy = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != null)
            {
                DeselectObject();
                if (hitObject.CompareTag("GunHolder_1"))
                {
                    SelectedGunHolder = 1;
                    _selectedObject = hitObject;
                    HighlightObject(_selectedObject);
                }
                else if (hitObject.CompareTag("GunHolder_2"))
                {
                    SelectedGunHolder = 2;
                    _selectedObject = hitObject;
                    HighlightObject(_selectedObject);
                }
                else if (hitObject.CompareTag("GunHolder_3"))
                {
                    SelectedGunHolder = 3;
                    _selectedObject = hitObject;
                    HighlightObject(_selectedObject);
                }
                CanBuy = (SelectedGunHolder > 0);
                Debug.Log("Selected Object: " + hitObject.name + "....." + SelectedGunHolder);
            }
        }
    }

    private void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
    }

    private void DeselectObject()
    {
        if (_selectedObject != null)
        {
            Renderer renderer = _selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white;
            }
            _selectedObject = null;
        }
    }

    public void SpawnGun(int index)
    {
        if (SelectedGunHolder <= 0) return;
        GunHandler gun;
        if (index == 0)
        {
            gun = PoolManager.GetPistolGunObject();
        }
        else if (index == 1)
        {
            gun = PoolManager.GetMachineGunObject();
        }
        else if (index == 2)
        {
            gun = PoolManager.GetLauncherGunObject();
        }
        else
        {
            gun = null;
        }
        gun.transform.parent = GunHolders[SelectedGunHolder - 1].transform;
        gun.transform.localPosition = Vector3.zero;
        gun.CanFire = true;
        SelectedGunHolder = 0;
        StartCoroutine(InitFire(gun));
    }

    private IEnumerator InitFire(GunHandler gun)
    {
        if (!_IsFirstRound)
        {
            _IsFirstRound = true;
            yield return new WaitForSeconds(2);
            gun.CanFire = true;
            gun.Fire();
        }
        else
        {
            gun.CanFire = true;
            gun.Fire();
        }
    }
}
