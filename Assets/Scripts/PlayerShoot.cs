using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject shootObject;
    public Transform shootBase;
    public float shootStrength;

    public void Shoot() {
        Debug.Log("Shooting");
        GameObject arrow = Instantiate(shootObject, shootBase.position, Quaternion.LookRotation(shootBase.forward, shootBase.up));

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        Vector3 destination;
        RaycastHit hit;
        Gizmos.color = Color.blue;
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 3f);
            destination = hit.point;
        } else {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10f, Color.blue, 3f);
            destination = ray.origin + ray.direction * 20f;
        }
        arrow.GetComponent<Rigidbody>().AddForce((destination - shootBase.position).normalized * shootStrength);
    }
}
