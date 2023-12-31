using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject ui;

    void Start()
        {
            ui.SetActive(false);
        }

    public void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                ui.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                ui.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }

}
