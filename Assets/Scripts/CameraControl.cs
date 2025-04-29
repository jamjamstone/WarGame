using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraControl : MonoBehaviourPun
{

    public bool isMaster;


    public void SetInitialTransform()
    {
        Debug.Log("ss");
        
            if (PhotonNetwork.IsMasterClient)
            {
                Quaternion nonHostRoation = Quaternion.Euler(90, 0, 0);
                transform.rotation = nonHostRoation;
                
                var tempHost = GameManager.Instance.hostUnitSpawnPoint.transform.position;
                transform.position = tempHost;// new Vector3(temp.x, 50f, temp.z);
                
                Debug.Log("master");
                Debug.Log(tempHost);
                isMaster = true;
            }
            else
            {

                Quaternion hostRotation = Quaternion.Euler(90, 0, 180);
                transform.rotation = hostRotation;//x 90
                var tempGuest = GameManager.Instance.nonHostUnitSpawnPoint.transform.position;
                transform.position = tempGuest;//new Vector3(temp.x, 50f, temp.z);
                
                Debug.Log("guest");
                Debug.Log(tempGuest);
                isMaster = false;
            }

            gameObject.tag = "MainCamera";
            Camera.SetupCurrent(gameObject.GetComponent<Camera>());

    }

    
    private void FixedUpdate()//여기서 오류 나는듯? 
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
        if(!PhotonNetwork.IsMasterClient&&transform.position.z<=60)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -60f);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            transform.Translate(Vector3.back*5);
            
        }else if(Input.mouseScrollDelta.y > 0)
        {
            transform.Translate(Vector3.forward*5);
        }

        if (transform.position.y < 20)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        }
    }

}

