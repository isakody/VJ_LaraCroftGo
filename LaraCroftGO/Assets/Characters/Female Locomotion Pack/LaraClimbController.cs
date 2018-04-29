using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaraClimbController : MonoBehaviour {
    public bool isClimbing;
    bool inPosition;
    float tick;
    Vector3 startPos;
    Vector3 targetPos;
    Quaternion startRot;
    Quaternion targetRot;
    public float possitionOffset;
    public float offsetFormWall = 0.3f;
    public float speedMultiplier = 0.2f;
    bool isLerping;
    Animator anim;
    public float climbSpeed = 3;
    public float rotateSpeed = 5;
    public float horizontal, vertical;
    

    Transform helper;

	// Use this for initialization
	void Start () {
        Init();
	}

    public void Init()
    {
        anim = GetComponent<Animator>();
        helper = new GameObject().transform;
        helper.name = "climb helper";
        CheckForClimb();
    }
	
	// Update is called once per frame
	void Update () { 
        float delta = Time.deltaTime;
        Tick(delta);
	}

    public void CheckForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 1.4f;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(origin,dir,out hit, 5))
        {
            helper.position = PostionWithOffset(origin, hit.point);
            InitForClimb(hit);
        }
    }

    void InitForClimb(RaycastHit hit)
    { 
        isClimbing = true;
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * offsetFormWall);
        tick = 0;
        inPosition = false;
        anim.CrossFade("climb", 2);
    }

    bool CanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        float dis = possitionOffset;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis);
        RaycastHit hit;
        if(Physics.Raycast(origin,dir,out hit, dis))
        {
            return false;
        }

        origin += moveDir * dis;
        dir = helper.forward;
        float dis2 = 0.5f;

        Debug.DrawRay(origin, dir*dis2);
        if(Physics.Raycast(origin,dir, out hit,dis))
        {
            helper.position = PostionWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }
        origin += dir * dis2;
        dir = -Vector3.up;

        Debug.DrawRay(origin, dir);
        if(Physics.Raycast(origin, dir, out hit, dis2))
        {
            float angle = Vector3.Angle(helper.up, hit.normal);
            if(angle < 40)
            {
                helper.position = PostionWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }
        return false;
    }

    public void Tick(float delta)
    {
        if (!inPosition)
        {
            GetInPosition(delta);
            return;
        }
        if (!isLerping)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            Vector3 h = helper.right * horizontal;
            Vector3 v = helper.up * vertical;
            Vector3 moveDir = (h + v).normalized;

            bool canMove = CanMove(moveDir);
            if(!canMove || moveDir == Vector3.zero)
            {
                return;
            }

            tick = 0;
            isLerping = true;
            startPos = transform.position;
            //Vector3 tp = helper.position - transform.position;
            targetPos = helper.position;

        }
        else
        {
            tick += delta * climbSpeed;
            if(tick > 1)
            {
                tick = 1;
                isLerping = false;
            }
            Vector3 cp = Vector3.Lerp(startPos, targetPos, tick);
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }
    }

    void GetInPosition(float delta)
    {
        tick += delta;
        if(tick > 1)
        {
            tick = 1;
            inPosition = true;
        }

        Vector3 tp = Vector3.Lerp(startPos, targetPos, tick);
        transform.position = tp;
        transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);

    }
    Vector3 PostionWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFormWall;
        return target + offset;
    }
}
