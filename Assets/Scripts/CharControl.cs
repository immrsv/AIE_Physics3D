using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CharControl : MonoBehaviour {

    public float walkSpeed = 4;
    public float runSpeed = 7;
    public float jumpStrength = 5;

    Rigidbody rb;

    bool IsGrounded {
        get {
            return Physics.Raycast(transform.position, -transform.up, 1.0001f) ;
        }
    }

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 move = new Vector3();
        move += Input.GetAxis("Horizontal") * transform.right * walkSpeed;
        move += Input.GetAxis("Vertical") * transform.forward * (Input.GetAxis("Run") > 0.5 ? runSpeed : walkSpeed);

        if (Input.GetAxis("Jump") > 0.5 && IsGrounded)
            move += new Vector3(0, jumpStrength, 0);

        if (move.magnitude > 0.5)
            Debug.Log("Move: " + move);

        var velocity = rb.velocity;
        velocity.x = 0;
        velocity.z = 0;
        velocity += move;
        rb.velocity = velocity;



        Vector3 rotate = new Vector3();
        //rotate.z = -Input.GetAxis("Roll");

        if (Input.GetMouseButtonDown(2))
        { // Middle Mouse toggles mouse capture
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            rotate.x = -Input.GetAxisRaw("Mouse Y");
            rotate.y = Input.GetAxisRaw("Mouse X");
        }
        else
        {
            HandleInteraction();
        }

        rb.angularVelocity = transform.TransformDirection(rotate);
    }

    private void HandleInteraction()
    {
        // Reference: http://answers.unity3d.com/comments/366286/view.html
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
          // create a ray passing through the mouse pointer:

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            { // if something hit...
              // if you must do something with the previously
              // selected item, do it here,
              // then select the new one:

                GameObject root = null;
                if ( hit.transform.gameObject.name.StartsWith("Spire"))
                {
                    if (hit.transform.gameObject.name.Split(" (".ToCharArray())[0] == "Spire")
                        root = hit.transform.gameObject;
                    else
                        root = hit.transform.parent.gameObject;
                }

                if ( root != null )
                {
                    var script = root.GetComponent<SpringDecay>();
                    script.ResetSprings();
                }

            }
        }
    }
}
