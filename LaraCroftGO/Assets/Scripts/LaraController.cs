using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaraController : MonoBehaviour {
    public float timeBetweenTiles = 0.5f;
    public float rotationSpeed = 1f;
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
    public bool isClimbingX = false;
    public bool isClimbingZ = false;
    bool rotating = false;
    public List<GameObject> enemies;
    Animator anim;
    float t;
    Quaternion targetRotation;
    Vector3 destiny;
    private bool isKilling;
    private GameObject enemyToKill;
    float timePassed = 0;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && !moving)
        {    calculateRotation(KeyCode.W);
            if (CalculateDestiny(KeyCode.W))
            {
               
                Move();
            }
                
        }

        if (Input.GetKey(KeyCode.S) && !moving)
        {
            calculateRotation(KeyCode.W);
            if (CalculateDestiny(KeyCode.S))
            {
               
                Move();
            }
                
        }

        if (Input.GetKey(KeyCode.A) && !moving)
        {
            calculateRotation(KeyCode.A);
            if (CalculateDestiny(KeyCode.A))
            {
                
                Move();
            }
        }

        if (Input.GetKey(KeyCode.D) && !moving)
        {
            calculateRotation(KeyCode.D);
            if (CalculateDestiny(KeyCode.D))
            {
                
                Move();
            }
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
        if (rotating)
        {
            Vector3 targetPostition = new Vector3(destiny.x,
                                      this.transform.position.y,
                                      destiny.z);
            Quaternion rotation = Quaternion.LookRotation(targetPostition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed * 10);
            float diff = transform.rotation.eulerAngles.y - rotation.eulerAngles.y;
            if (Mathf.Abs(diff) <= 0.01f)
            {
                rotating = false;
                checkForAnimation();
            }
        }
        else
        {
            if(isKilling) timePassed += Time.deltaTime;
            if(isKilling && timePassed > 1f)
            {
                unsetAnimation();
                anim.SetTrigger("isRunning");
                timePassed = 0;
                Destroy(enemyToKill);
                
               
                
            }


            if (Mathf.Abs(Vector3.Distance(destiny, transform.position)) <= 0.01)
            {
                
                unsetAnimation();
                moving = false;
                updateAllEnemies();

            }
            else
            {
                if (!isKilling)
                {
                    t += Time.deltaTime / timeBetweenTiles;
                    transform.position = Vector3.Lerp(transform.position, destiny, t);
                }
               
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
                    destiny.z += 0.625f;
                    destiny.y += 1;
                    t = 0;
                    isClimbingZ = false;
                    updateAllEnemiesWithDirections("NorthUp");
                    return true;
                }
                else
                {
                    destiny = transform.position;
                    destiny.z += 0.375f;
                    t = 0;
                    isClimbingZ = true;
                    updateAllEnemiesWithDirections("NorthUp");
                    return true;
                }
                
            }
            else if (up && isClimbingZ)
            {
                destiny = transform.position;
                destiny.y += 1;
                t = 0;
                updateAllEnemiesWithDirections("Up");
                return true;
            }

            else if (north)
            {
                destiny = transform.position;
                destiny.z += 1;
                t = 0;
                updateAllEnemiesWithDirections("North");
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
                updateAllEnemiesWithDirections("South");
                return true;
            }
            else if (southDown)
            {
                if (isClimbingZ)
                {
                    destiny = transform.position;
                    destiny.z -= 0.375f;
                    t = 0;
                    isClimbingZ = false;
                    updateAllEnemiesWithDirections("SouthDown");
                    return true;
                }
                else
                {
                    destiny = transform.position;
                    destiny.z -= 0.625f;
                    destiny.y -= 1;
                    t = 0;
                    isClimbingZ = true;
                    updateAllEnemiesWithDirections("SouthDown");
                    return true;
                }
            }
            else if (down)
            {
                destiny = transform.position;
                destiny.y -= 1;
                t = 0;
                updateAllEnemiesWithDirections("Down");
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
                    destiny.x -= 0.625f;
                    destiny.y += 1.0f;
                    isClimbingX = false;
                    t = 0;
                    updateAllEnemiesWithDirections("WestUp");
                    return true;


                }
                else
                {
                    destiny = transform.position;
                    destiny.x -= 0.375f;
                    isClimbingX = true;
                    t = 0;
                    updateAllEnemiesWithDirections("WestUp");
                    return true;
                }

            }
            else if (up && isClimbingX)
            {
                destiny = transform.position;
                destiny.y += 1;
                t = 0;
                updateAllEnemiesWithDirections("Up");
                return true;
            }
            else if (west)
            {
                destiny = transform.position;
                destiny.x -= 1;
                t = 0;
                updateAllEnemiesWithDirections("West");
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
                updateAllEnemiesWithDirections("Down");
                return true;
            }
            else if(eastDown == true)
            {
                if (!isClimbingX)
                {
                    destiny = transform.position;
                    destiny.x += 0.625f;
                    destiny.y -= 1;
                    isClimbingX = true;
                    t = 0;
                    updateAllEnemiesWithDirections("EastDown");
                    return true;

                }
                else
                {
                    destiny = transform.position;
                    destiny.x += 0.375f;
                    isClimbingX = false;
                    t = 0;
                    updateAllEnemiesWithDirections("EastDown");
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
                    updateAllEnemiesWithDirections("East");
                    return true;
                }

                else
                {
                    destiny = transform.position;
                    destiny.z += 1;
                    t = 0;
                    updateAllEnemiesWithDirections("North");
                    return true;
                }
                
            }
        }
        else return false;

    }

    void calculateRotation(KeyCode code)
    {
        if (isClimbingX || isClimbingZ) rotating = false;
        else rotating = true;
    }
    
    void checkForAnimation()
    {
        
        if(!isClimbingX && !isClimbingZ)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Enemy")
                {
                    anim.SetTrigger("isKilling");
                    enemyToKill = hit.collider.gameObject;
                    isKilling = true;
                    t = 0;
                    timePassed = 0;

                }
                else anim.SetTrigger("isRunning");
            }
            else anim.SetTrigger("isRunning");
        }
    }

    void unsetAnimation()
    {
        if (!isClimbingX && !isClimbingZ)
        {
            anim.ResetTrigger("isRunning");
            anim.ResetTrigger("isKilling");
            isKilling = false;
        }
    }

    void updateAllEnemies()
    {
        Debug.Log("calling for enemies move");
        for(int i = 0; i < enemies.Count; ++i)
        {
            if(enemies[i] != null)
                enemies[i].SendMessage("EnableMovement", true);
        }
    }

    void updateAllEnemiesWithDirections(string direction)
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
                enemies[i].SendMessage("ParseLaraDirections", direction);
        }
    }
}
