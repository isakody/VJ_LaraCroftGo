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
    public bool moving = false;
    public bool hasWon1= false;
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
    bool hasToGoDownEast = false;
    bool hasToGoDownSouth = false;
    bool canMoveClimbing = false;
    bool hasDied = false;
    bool hasDiedClimbing = false;
    public GameObject audioSource;
    private bool hasToDie = false;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hasWon1) return;
        if (hasDied)
        {
            t += Time.deltaTime;
            if (t > 2)
            {
                Destroy(this.gameObject);
            }
            else return;
        }
        else if (hasDiedClimbing)
        {
            t += Time.deltaTime;
            if (t > 1.2f)
            {
                Destroy(this.gameObject);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,transform.position.y - 0.08f, transform.position.z);
                return;
            }
        }
        if (Input.GetKey(KeyCode.W) && !moving)
        {    calculateRotation(KeyCode.W);
            if (CalculateDestiny(KeyCode.W))
            {
               
                Move();
            }
                
        }

        if (Input.GetKey(KeyCode.S) && !moving)
        {
            calculateRotation(KeyCode.S);
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
            if ((isClimbingZ || isClimbingX))
            {
               if(canMoveClimbing) checkForAnimation();
            }
            if (eastDown)
            {
                
                hasToGoDownEast = true;
            }
            else if (southDown)
            {
                
                hasToGoDownSouth = true;
            }
            ResetDirections();
            moving = true;
        }
        if (rotating)
        {
            Vector3 targetPostition = new Vector3(destiny.x,
                                      this.transform.position.y,
                                      destiny.z);
            if (hasToGoDownSouth && isClimbingZ)
            {
                targetPostition.z = targetPostition.z + 2;
               
            }
            else if (hasToGoDownEast && isClimbingX)
            {
                targetPostition.x = targetPostition.x - 2;
            }
            
            Quaternion rotation = Quaternion.LookRotation(targetPostition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed * 10);
            float diff = transform.rotation.eulerAngles.y - rotation.eulerAngles.y;
            if (Mathf.Abs(diff) <= 0.01f)
            {
                rotating = false;
                hasToGoDownEast = false;
                hasToGoDownSouth = false;
                checkForAnimation();
            }
        }
        else
        {
            if(isKilling) timePassed += Time.deltaTime;
            if(isKilling && timePassed > 1.1f)
            {
                if (timePassed > 2.1f)
                {
                    unsetAnimation();
                    anim.SetTrigger("isRunning");
                    timePassed = 0;
                }
                if (enemyToKill != null)
                {
                    enemyToKill.SendMessage("die");
                    audioSource.SendMessage("PlayKickSound");
                    enemyToKill = null;
                }
                
               
                
            }
            if (Mathf.Abs(Vector3.Distance(destiny, transform.position)) <= 0.01f)
            {
                transform.position = destiny;
                if (hasToDie)
                {
                    die();
                    return;
                }
                
                
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
                    anim.ResetTrigger("isClimbing");
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
                canMoveClimbing = true;
                updateAllEnemiesWithDirections("Up");
                return true;
            }

            else if (north)
            {
                destiny = transform.position;
                destiny.z += 1;
                t = 0;
                updateAllEnemiesWithDirections("North");
                if (isClimbingX) anim.SetTrigger("isMovingRight");
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
                if (isClimbingX) anim.SetTrigger("isMovingLeft");
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
                    anim.ResetTrigger("isClimbing");
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
            else if (down && isClimbingZ)
            {
                destiny = transform.position;
                destiny.y -= 1;
                t = 0;
                canMoveClimbing = true;
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
                    anim.ResetTrigger("isClimbing");
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
                canMoveClimbing = true;
                updateAllEnemiesWithDirections("Up");
                return true;
            }
            else if (west)
            {
                destiny = transform.position;
                destiny.x -= 1;
                t = 0;
                updateAllEnemiesWithDirections("West");
                if (isClimbingZ) anim.SetTrigger("isMovingLeft");
                return true;
            }
            else return false;
        }
        if (key == KeyCode.D)
        {
            if (east == false && eastDown == false && down == false) return false;
            else if (down && isClimbingX)
            {
                destiny = transform.position;
                destiny.y -= 1;
                t = 0;
                canMoveClimbing = true;
                updateAllEnemiesWithDirections("Down");
                return true;
            }
            else if (eastDown == true)
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
                    anim.ResetTrigger("isClimbing");
                    t = 0;
                    updateAllEnemiesWithDirections("EastDown");
                    return true;
                }


            }
            else if (east)
            {
                if (!isClimbingX)
                {
                    destiny = transform.position;
                    destiny.x += 1;
                    t = 0;
                    updateAllEnemiesWithDirections("East");
                    if (isClimbingZ) anim.SetTrigger("isMovingRight");
                    return true;
                }
                
                else
                {
                    return false;
                }

            }
            else return false;
        }
        else return false;

    }

    void calculateRotation(KeyCode code)
    {
        if (isClimbingX || isClimbingZ)
        {
            rotating = false;

        }
        else rotating = true;
    }
    
    void checkForAnimation()
    {
        if(isClimbingX || isClimbingZ)
        {
            if (canMoveClimbing) anim.SetTrigger("isMoving");
            anim.SetTrigger("isClimbing");
            
        }
        
        if(!isClimbingX && !isClimbingZ)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up*0.5f, transform.forward, out hit, 1.0f))
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
        if(isClimbingX || isClimbingZ)
        {
            anim.ResetTrigger("isMoving");
            canMoveClimbing = false;
            anim.ResetTrigger("isMovingRight");
            anim.ResetTrigger("isMovingLeft");
        }
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

    void hasWon(bool won)
    {
        if (won)
        {
            hasWon1 =  true;
            unsetAnimation();
            anim.SetTrigger("hasWon");
            transform.LookAt(transform.position - transform.forward);
        }
    }

    void die()
    {
        if (transform.position != destiny && moving)
        {
            hasToDie = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            if (!isClimbingX && !isClimbingZ)
            {
                anim.SetTrigger("hasDied");
                hasDied = true;
                t = 0;
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                anim.SetTrigger("hasDiedClimbing");
                hasDiedClimbing = true;
                t = 0;
                //gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        
    }
}
