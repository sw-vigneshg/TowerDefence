using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 Target;
    public float lifetime = 5f;

    private void OnEnable()
    {
        transform.position = Vector3.zero;
    }

    public void Fire(Vector3 target)
    {
        Target = target;
        CancelInvoke(nameof(OnLifeTimeEnds));
        Invoke(nameof(OnLifeTimeEnds), lifetime);
        StopCoroutine(nameof(OnFire));
        StartCoroutine(nameof(OnFire));
    }

    private void OnLifeTimeEnds()
    {
        if (this.gameObject.CompareTag("Bullet_1"))
            GameManager.Instance.PoolManager.ReturnSmallBulletObject(this);
        else if (this.gameObject.CompareTag("Bullet_3"))
            GameManager.Instance.PoolManager.ReturnLauncherBulletObject(this);
    }

    private IEnumerator OnFire()
    {
        while (this.gameObject.activeInHierarchy)
        {
            if (Target != null)
            {
                //transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);
                transform.rotation = Quaternion.identity;
                Target = new Vector3(Target.x, 0, Target.z);
                transform.Translate(Target * speed * Time.deltaTime, Space.World);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnLifeTimeEnds();
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnLifeTimeEnds();
    }
}
