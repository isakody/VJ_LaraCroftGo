using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaraController : MonoBehaviour {
    public float timeBetweenTiles = 0.5f;
    public bool north;
    public bool south;
    public bool east;
    public bool west;
    public bool northUp;
    public bool westUp;
    public bool eastDown;
    public bool southDown;
    public bool up;
    public bool down;
    bool moving = false;
    bool isClimbingX = false;
    bool isClimbingZ = false;
    float t;
    Vector3 destiny;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            if(CalculateDestiny(KeyCode.W))
                Move(); 
        }

        if (Input.GetKey(KeyCode.S) && !moving)
        {
            if (CalculateDestiny(KeyCode.S))
                Move();
        }

        if (Input.GetKey(KeyCode.A) && !moving)
        {
            if (CalculateDestiny(KeyCode.A))
                Move();
        }

        if (Input.GetKey(KeyCode.D) && !moving)
        {
            if (CalculateDestiny(KeyCode.D))
                Move();
        }

            if (moving)
        {
            Move();
        }
	}

   public void parseInfoFloor(bool north, bool south, bool east, bool west)
    {
        this.north = north;
        this.south = south;
        this.east = east;
        this.west = west;
    }

    public void parseWallDirections(bool northUp, bool southDown, bool eastDown, bool westUp, bool up, bool down)
    {
        this.westUp = westUp;
        this.northUp = northUp;
        this.southDown = southDown;
        this.eastDown = eastDown;
        this.up = up;
        this.down = down;
    }

    void Move()
    {
        if (!moving)
        {
            ResetDirections();
            moving = true;
        }
        else
        {   
            if(destiny == transform.position)
            {
                Debug.Log("final Postition");
                moving = false;

            }
            else
            {
                t += Time.deltaTime / timeBetweenTiles;
                transform.position = Vector3.Lerp(transform.position, destiny, t);
            }
           
        }

    }

    void ResetDirections()
    {
        north = false;
        south = false;
        west = false;
        east = false;
        northUp = false;
        southDown = false;
        westUp = false;
        eastDown = false;
        up = false;
        down = false;
    }

    bool CalculateDestiny(KeyCode key)
    {
        if (key == KeyCode.W)
        {
            if (north == false && northUp == false && up == false) return false;
            else if (northUp == true)
            {
                if (isClimbingZ)
                {
                    destiny = transform.position;
                    destiny.z += 0.75f;
                    destiny.y += 1;
                    t = 0;
                    isClimbingZ = false;
                    return true;
                }
                else
                {
                    destiny = transform.position;
                    destiny.z += 0.25f;
                    t = 0;
                    isClimbingZ = true;
                    return true;
                }
                
            }
            else if (up && isClimbingZ)
            {
                destiny = transform.position;
                destiny.y += 1;
                t = 0;
                return true;
            }

            else if (north)
            {
                destiny = transform.position;
                destiny.z += 1;
                t = 0;
                return true;
            }
            else return false;
        }
        if (key == KeyCode.S)
        {
            if (south == false && southDown == false && down == false) return false;
            else if (south)
            {
                destiny = transform.position;
                destiny.z -= 1;
                t = 0;
                return true;
            }
            else if (southDown)
            {
                if (isClimbingZ)
                {
                    destiny = transform.position;
                    destiny.z -= 0.25f;
                    t = 0;
                    isClimbingZ = false;
                    return true;
                }
                else
                {
                    destiny = transform.position;
                    destiny.z -= 0.75f;
                    destiny.y -= 1;
                    t = 0;
                    isClimbingZ = true;
                    return true;
                }
            }
            else if (down)
            {
                destiny = transform.position;
                destiny.y -= 1;
                t = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        if (key == KeyCode.A)
        {
            if (west == false && westUp == false && up == false) return false;
            else if (westUp == true)
            {
                if (isClimbingX)
                {
                    destiny = transform.position;
                    destiny.x -= 0.75f;
                    destiny.y += 1.0f;
                    isClimbingX = false;
                    t = 0;
                    return true;


                }
                else
                {
                    destiny = transform.position;
                    destiny.x -= 0.25f;
                    isClimbingX = true;
                    t = 0;
                    return true;
                }

            }
            else if (up && isClimbingX)
            {
                destiny = transform.position;
                destiny.y += 1;
                t = 0;
                return true;
            }
            else if (west)
            {
                destiny = transform.position;
                destiny.x -= 1;
                t = 0;
                return true;
            }
            else return false;
        }
        if (key == KeyCode.D)
        {
            if (east == false && eastDown == false && down == false) return false;
            else if (down)
            {
                destiny = transform.position;
                destiny.y -= 1;
                t = 0;
                return true;
            }
            else if(eastDown == true)
            {
                if (!isClimbingX)
                {
                    destiny = transform.position;
                    destiny.x += 0.75f;
                    destiny.y -= 1;
                    isClimbingX = true;
                    t = 0;
                    return true;

                }
                else
                {
                    destiny = transform.position;
                    destiny.x += 0.25f;
                    isClimbingX = false;
                    t = 0;
                    return true;
                }
                
                
            }
            else
            {
                if (!isClimbingX)
                {
                    destiny = transform.position;
                    destiny.x += 1;
                    t = 0;
                    return true;
                }

                else
                {
                    destiny = transform.position;
                    destiny.z += 1;
                    t = 0;
                    return true;
                }
                
            }
        }
        else return false;

    }
}
