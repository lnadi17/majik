using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrowPickup : MonoBehaviour
{
    [SerializeField]
    private GameObject explosiveArrow;
    [SerializeField]
    private float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerShoot ps = other.transform.parent.GetComponent<PlayerShoot>();
            if (ps) {
                ps.shootObject = explosiveArrow;
            }
            Destroy(gameObject);
        }
    }
}
