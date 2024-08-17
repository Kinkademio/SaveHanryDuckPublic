using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float dumping = 1.8f;
    float zoomSpeed = 1000f; // скорость зума
    public GameObject Player;
    Camera Camera;

    void Start()
    {
        Player = GameObject.Find("Duck");
        Camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(Player.transform.position.x, Player.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, target, dumping * Time.deltaTime);

        float mw = Input.GetAxis("Mouse ScrollWheel");
        if ((mw != 0) && Camera.orthographic)
        {
            if ((mw > 0) && (Camera.orthographicSize > 5)) Camera.orthographicSize -= mw * Time.deltaTime * zoomSpeed;
            if ((mw < 0) && (Camera.orthographicSize < 10)) Camera.orthographicSize -= mw * Time.deltaTime * zoomSpeed;
        }
        
        
        //+= transform.forward* Time.deltaTime * mw * wheel_speed;
    }
}
    
