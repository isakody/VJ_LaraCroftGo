using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController1 : MonoBehaviour {
    public GameObject lara;
    private float timer;
    public int numberOfGemsFound = 0;
    public List<Image> images;
    public GameObject collectable;
    public bool isShowingCollectable;
    public Vector3 finalDistanceFormCamera;
    public Vector3 initialDistanceFormCamera;
    float t = 0;
    public float moveSpeed = 1;
    public float rotationSpeed;
    public GameObject deathEffect;
    bool endShow = false;
    public GameObject LoadingScene;
    public Slider slider;
    bool loadingNextScene = false;
    int partFound1 = 0;
    int partFound2 = 0;
    int partFound3 = 0;
    public bool hasToDisplayPart = false;
    public GameObject collectable2;
    bool showingCollectable2 = false;

    

	// Use this for initialization
	void Start () {
        timer = 0;
        partFound1 = PlayerPrefs.GetInt("partFound1");
        partFound2 = PlayerPrefs.GetInt("partFound2");
        partFound3 = PlayerPrefs.GetInt("partFound3");
        
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hasToDisplayPart)
        {
            displayPart();
            
        }
        if (numberOfGemsFound >= 3 && !hasToDisplayPart) showCollectable();
        if(lara != null)
        {
            if (Input.GetKeyDown("p") && !loadingNextScene)
                LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            else if (Input.GetKeyDown("o") && SceneManager.GetActiveScene().buildIndex > 0 && !loadingNextScene)
                LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
            else if (Input.GetKeyDown("l"))
                numberOfGemsFound = 3;
            
            else if (Input.GetKeyDown("v"))
                PlayerPrefs.DeleteAll();

            if (lara.gameObject.GetComponent<LaraController>().hasWon1)
            {
                timer += Time.deltaTime;
                if(timer > 5.0f && !loadingNextScene)
                {
                    if (SceneManager.GetActiveScene().buildIndex == 3) SceneManager.LoadScene(0);
                    LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }

        }
        else
        {
            timer += Time.deltaTime;
            if(timer > 2.0f && !loadingNextScene)
            {
                if (SceneManager.GetActiveScene().buildIndex == 3) SceneManager.LoadScene(0);
                LoadLevel(SceneManager.GetActiveScene().buildIndex);
            }

           
        }
		
	}

    public void gemFound()
    {
        numberOfGemsFound++;
        changeGemColor();
    }

    void showCollectable()
    {
        if (endShow) return;
        if (!isShowingCollectable)
        {
            collectable.transform.position = lara.transform.position - initialDistanceFormCamera;
            collectable.SetActive(true);
            isShowingCollectable = true;
        }
        
        else
        {
            if (Mathf.Abs(Vector3.Distance(collectable.transform.position, lara.transform.position - finalDistanceFormCamera)) > 0.01f)
            {
                t += Time.deltaTime / moveSpeed;
                collectable.transform.position = Vector3.Lerp(collectable.transform.position, lara.transform.position - finalDistanceFormCamera, t);
            }
            else
            {
                t += Time.deltaTime;
                if (t > moveSpeed * 10)
                {
                    die();
                    
                }
            }
            Debug.Log("rotating");
            collectable.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        }
    }

    void changeGemColor()
    {
        Color gemColor = images[numberOfGemsFound - 1].color;
        gemColor.a = 1;
        images[numberOfGemsFound - 1].color = gemColor;
    }
    void die()
    {
        endShow = true;
        Destroy(Instantiate(deathEffect, collectable.transform.position + Vector3.up * 0.25f, Quaternion.identity) as GameObject, 5.0f);
        Destroy(collectable);

    }
    public void LoadLevel(int sceneIndex)
    {
        loadingNextScene = true;
        LoadingScene.SetActive(true);
        StartCoroutine(LoadAsyncronously(sceneIndex));

    }

    public void artifactPartFound()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "Map1")
        {
            partFound1 = 1;
        }
        if (SceneManager.GetActiveScene().name == "Map2")
        {
            partFound2 = 1;
        }
        if (SceneManager.GetActiveScene().name == "Map3")
        {
            partFound3 = 1;
        }
        hasToDisplayPart = true;
        PlayerPrefs.SetInt("partFound1", partFound1);
        PlayerPrefs.SetInt("partFound2", partFound2);
        PlayerPrefs.SetInt("partFound3", partFound3);


    }
    public void displayPart()
    {
        if (lara == null) return;
        if (!showingCollectable2)
        {
            collectable2.transform.position = lara.transform.position - initialDistanceFormCamera;
            if (partFound1 == 1)
            {
                collectable2.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (partFound2 == 1)
            {
                collectable2.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (partFound3 == 1)
            {
                collectable2.transform.GetChild(2).gameObject.SetActive(true);
            }
            showingCollectable2 = true;
        }
        else
        {
            if (Mathf.Abs(Vector3.Distance(collectable2.transform.position, lara.transform.position - finalDistanceFormCamera)) > 0.01f)
            {
                t += Time.deltaTime / moveSpeed;
                collectable2.transform.position = Vector3.Lerp(collectable2.transform.position, lara.transform.position - finalDistanceFormCamera, t);
            }
            else
            {
                t += Time.deltaTime;
                if (t > moveSpeed * 10)
                {
                    hasToDisplayPart = false;
                    Destroy(collectable2);
                    showingCollectable2 = false;
                    t = 0;

                }
            }
            collectable2.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
}
