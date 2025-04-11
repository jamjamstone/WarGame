using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shootSpeed=3;
    public void Shoot()
    {
        var ball=Instantiate(bullet,Camera.main.transform.position,Camera.main.transform.rotation);
        ball.GetComponent<Rigidbody>().velocity = shootSpeed * Camera.main.transform.forward;
    }



    [SerializeField] ARRaycastManager raycastManager;
    public void ShootRay()
    {
        Ray ray = new Ray();
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(ray, hits))
        {
            Instantiate(bullet, hits[0].pose.position, hits[0].pose.rotation);
        }
    }





   
}
