using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToMoveTilesController : MonoBehaviour {

    // Use this for initialization
    public bool isDown = false;
    public GameObject platform;
    public GameObject startTileInfo;
    public GameObject endTileInfo;
    public bool isNorthEnabledStartTile = true;
    public bool isSouthEnabledEndTile = true;
    bool isMoving = false;
    Vector3 targetPosition;
    float currentLerpTime = 0;
    public float lerpTime = 1.0f;
    bool canChangeState = false;
    public Animator leverAnim;
    public GameObject audioSource;
	void Start () {
        if (isDown)
        {
            if (startTileInfo.GetComponent<FloorTile>().canGoNorth)
            {
                startTileInfo.GetComponent<FloorTile>().canGoNorth = false;
            }
            if (endTileInfo.GetComponent<FloorTile>().canGoSouth)
            {
                endTileInfo.GetComponent<FloorTile>().canGoSouth = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (canChangeState || isMoving)
        {
            if (isMoving)
            {
                if (targetPosition == platform.transform.position)
                {
                    isMoving = false;

                }
                else
                {
                    currentLerpTime += Time.deltaTime / lerpTime;
                    platform.transform.position = Vector3.Lerp(platform.transform.position, targetPosition, currentLerpTime);

                }
            }
        }
	}

    void OnMouseDown()
    {
        if (canChangeState)
        {
            if (!isDown && !isMoving)
            {
                isDown = true;
                isMoving = true;
                targetPosition = platform.transform.position;
                targetPosition.y -= 5;
                currentLerpTime = 0;
                changeTileState();
                leverAnim.ResetTrigger("isActivated");
                audioSource.SendMessage("LeverPull");
            }
            else if (!isMoving)
            {
                isDown = false;
                isMoving = true;
                targetPosition = platform.transform.position;
                targetPosition.y += 5;
                currentLerpTime = 0;
                changeTileState();
                leverAnim.SetTrigger("isActivated");
                audioSource.SendMessage("LeverPull");
            }
        }
    }

    void changeTileState()
    {
        if (isDown)
        {
            if (startTileInfo.GetComponent<FloorTile>().canGoNorth)
            {
                startTileInfo.GetComponent<FloorTile>().canGoNorth = false;
            }
            if (endTileInfo.GetComponent<FloorTile>().canGoSouth)
            {
                endTileInfo.GetComponent<FloorTile>().canGoSouth = false;
            }
        }
        else if (!isDown)
        {
            if (!startTileInfo.GetComponent<FloorTile>().canGoNorth)
            {
                startTileInfo.GetComponent<FloorTile>().canGoNorth = true;
            }
            if (!endTileInfo.GetComponent<FloorTile>().canGoSouth)
            {
                endTileInfo.GetComponent<FloorTile>().canGoSouth = true;
            }
        }
    }

    public void canMoveFloor(bool can)
    {
        canChangeState = can;
    }
}
