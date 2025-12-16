using UnityEngine;
using DG.Tweening;

public class BulletComponnet : MonoBehaviour
{
    private Vector3 direction;
    public float speed = 10f;

    public void Init(Vector3 dir)
    {
        transform.DOMove(dir, 0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.GetComponent<BoxComponent>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
