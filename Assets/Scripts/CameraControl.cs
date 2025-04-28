using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraControl : MonoBehaviourPun
{
    // Start is called before the first frame update



    public bool isMaster;
    void Start()
    {
       

    }
    private void OnEnable()
    {
        SetInitialTransform();
    }


    public void SetInitialTransform()
    {
        if (photonView.IsMine)
        {
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
        else
        {
            //gameObject.SetActive(false);
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
