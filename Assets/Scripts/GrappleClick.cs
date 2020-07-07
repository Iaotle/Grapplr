using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleClick : MonoBehaviour
{
    public LayerMask mask;
    public SpringJoint2D joint; //Use both spring and distance?
    public DistanceJoint2D jointDist;
    public LineRenderer line;
    public float defaultDistance, maxDistance, defaultDampening, defaultFrequency; //Calculate maxDistance?
    public bool isGrappling;
    public bool isLeashed;

    private float distance;
    private RaycastHit2D hit;
    private Vector2 leashVect;
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        isGrappling = false;
        isLeashed = false;
        joint.enabled = false;
        jointDist.enabled = false;
        line.enabled = false;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVect = body.velocity;
        //Grappling hook on mouse click
        if (Input.GetButtonDown("Fire1"))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, mask);
            if (hit.collider != null) //TODO: fix mask
            {
                isGrappling = true;
                joint.enabled = true;
                joint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                jointDist.connectedBody = joint.connectedBody;
                distance = Vector2.Distance(transform.position, hit.point);
                //Line draw
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                //if (distance > defaultDistance) // pull towards the dot
                //{
                //    //Debug.Log("Far away, springing in");
                //}
                //else // just use a normal distance joint
                //{
                //    joint.distance = distance;
                //    joint.dampingRatio = 0f;
                //    joint.frequency = 0f;
                //}
            }
        }
        leashVect = transform.position - new Vector3(hit.point.x, hit.point.y, 0f);
        float angle = Vector2.Angle(moveVect, leashVect);
        Debug.Log(angle);
        line.SetPosition(0, transform.position);

        distance = Vector2.Distance(transform.position, hit.point);
        if (isGrappling && !isLeashed && ((angle > 80f && angle < 100f && distance < maxDistance) || (distance < defaultDistance)))
        {
            jointDist.distance = distance;
            Debug.Log("Disabling spring");
            //isGrappling = true;
            isLeashed = true;
            joint.enabled = false;
            jointDist.enabled = true;

            //joint.dampingRatio = 0f;
            //joint.frequency = 0.01f;
            //joint.enabled = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            joint.enabled = false;
            line.enabled = false;
            jointDist.enabled = false;
            isGrappling = false;
            isLeashed = false;
            joint.distance = defaultDistance;
            joint.dampingRatio = defaultDampening;
            joint.frequency = defaultFrequency;
        }
    }
}
