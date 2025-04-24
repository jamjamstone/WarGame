using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            var temp = GameManager.Instance.hostUnitSpawnPoint.transform.position;
            transform.position = new Vector3(temp.x,100f , temp.z);
            Quaternion hostRotation = Quaternion.Euler(90, 0, 180);
            transform.rotation =hostRotation;//x 90

        }
        else
        {
            //Debug.Log("nonmaster");
            var temp = GameManager.Instance.hostUnitSpawnPoint.transform.position;
            transform.position = new Vector3(temp.x, 100f, temp.z);
            Quaternion nonHostRoation = Quaternion.Euler(90, 0, 0);
            transform.rotation = nonHostRoation;//x 90, z 180
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left);
        }
        if(PhotonNetwork.IsMasterClient&&transform.position.z>=-60)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -60f);
        }
        else if(!PhotonNetwork.IsMasterClient&&transform.position.z>=-60)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -60f);
        }



    }


}
