using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleClick : MonoBehaviour
{
    public LayerMask mask;
    public SpringJoint2D joint; //Use both spring and distance?
    public DistanceJoint2D jointDist;
    public LineRenderer line;
    public float defaultDistance, defaultDampening, defaultFrequency;

    private float distance;
    private RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        joint.enabled = false;
        jointDist.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Grappling hook on mouse click
        if (Input.GetButtonDown("Fire1"))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, mask);
            if (hit.collider != null)
            {
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                Debug.Log("Target collider: " + hit.collider);
                joint.enabled = true;
                joint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                jointDist.connectedBody = joint.connectedBody;
                distance = Vector2.Distance(transform.position, hit.point);
                //Line draw
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                if (distance > defaultDistance) // pull towards the dot
                {
                    //Debug.Log("Far away, springing in");
                }
                else // just use a normal distance joint
                {
                    joint.distance = distance;
                    joint.dampingRatio = 0f;
                    joint.frequency = 0f;
                }
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            joint.enabled = false;
            line.enabled = false;
            jointDist.enabled = false;
            joint.distance = defaultDistance;
            joint.dampingRatio = defaultDampening;
            joint.frequency = defaultFrequency;
        }
        distance = Vector2.Distance(transform.position, hit.point);
        if (distance <= defaultDistance && joint.enabled)
        {
            Debug.Log("swapping tether");
            joint.enabled = false;
            jointDist.enabled = true;
            //joint.dampingRatio = 0f;
            //joint.frequency = 0f;
            //joint.enabled = true;
        }
        line.SetPosition(0, transform.position);
    }
}
