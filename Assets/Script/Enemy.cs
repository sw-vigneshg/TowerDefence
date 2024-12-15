using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _HitCount;
    private float _Speed = 5f;
    private int _Damage = 10;

    private void OnEnable()
    {
        _HitCount = 2;
        StopCoroutine(nameof(MoveCoroutine));
        StartCoroutine(nameof(MoveCoroutine));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(MoveCoroutine));
    }

    private IEnumerator MoveCoroutine()
    {
        Transform target = GameManager.Instance.GetTowerPosition();
        bool isGameOver = GameManager.Instance.IsGameOver;
        while (transform.position != target.position && !isGameOver)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _Speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (other.gameObject.CompareTag("Tower"))
        {
            GameManager.Instance.OnTowerHealthChanges(_Damage);
            GameManager.Instance.PoolManager.ReturnEnemyObject(this);
        }
        else if (other.gameObject.CompareTag("Bullet_1"))
        {
            Debug.Log("Bullet Found");
            _HitCount -= 1;
            if (_HitCount <= 0)
            {
                GameManager.Instance.PoolManager.ReturnEnemyObject(this);
                GameManager.Instance.OnGoldValueChanges(50, true);
            }
            GameManager.Instance.PoolManager.ReturnSmallBulletObject(other.gameObject.GetComponent<Bullet>());
        }
        else if (other.gameObject.CompareTag("Bullet_3"))
        {
            _HitCount = 0;
            GameManager.Instance.PoolManager.ReturnEnemyObject(this);
            GameManager.Instance.PoolManager.ReturnLauncherBulletObject(other.gameObject.GetComponent<Bullet>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (collision.gameObject.CompareTag("Bullet_1"))
        {
            Debug.LogWarning("Bullet Found");
            _HitCount -= 1;
            if (_HitCount <= 0)
            {
                GameManager.Instance.PoolManager.ReturnSmallBulletObject(collision.gameObject.GetComponent<Bullet>());
                GameManager.Instance.PoolManager.ReturnEnemyObject(this);
            }
        }
        else if (collision.gameObject.CompareTag("Tower"))
        {
            Debug.LogWarning("Tower Found");
            GameManager.Instance.OnTowerHealthChanges(_Damage);
            GameManager.Instance.PoolManager.ReturnEnemyObject(this);
        }
        else if (collision.gameObject.CompareTag("Bullet_3"))
        {
            _HitCount = 0;
            GameManager.Instance.PoolManager.ReturnEnemyObject(this);
            GameManager.Instance.PoolManager.ReturnLauncherBulletObject(collision.gameObject.GetComponent<Bullet>());
        }
    }
}
