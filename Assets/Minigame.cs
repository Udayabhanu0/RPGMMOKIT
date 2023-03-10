using MultiplayerARPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{

    public Image animalImage;
    public InputField animalNameInput;
    public Canvas canvas;
    public Button submitButton;
    public Button quitButton;
    public ParticleSystem psytem;
    public GameObject doorl;
    public GameObject doorR;

   
    public List<Sprite> animalImages;
    public List<string> animalNames;


    private List<Animal> animals;
    private Animal randomAnimal;
    private bool canvasActive;
    public float rotationSpeed = 30f;


    void Start()
    {
    
        animals = new List<Animal>();
        for (int i = 0; i < animalImages.Count; i++)
        {
            animals.Add(new Animal(animalNames[i], animalImages[i]));
        }

       
        canvas.gameObject.SetActive(false);

        submitButton.onClick.AddListener(SubmitAnswer);

    
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
      
        if (canvasActive)
        {
        
            if (Input.GetKeyDown(KeyCode.Return) && animalNameInput.isFocused)
            {
                SubmitAnswer();
            }
        }
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
        {
           
         
            randomAnimal = animals[Random.Range(0, animals.Count)];
            animalImage.sprite = randomAnimal.image;
            canvas.gameObject.SetActive(true);
            canvasActive = true;
        }
    }

 
    private void SubmitAnswer()
    {

        string userInput = animalNameInput.text;

       
        if (userInput.ToLower() == randomAnimal.name.ToLower())
        {
            Debug.Log("Correct!");
           
            randomAnimal = animals[Random.Range(0, animals.Count)];
            animalImage.sprite = randomAnimal.image;
            animalNameInput.text = "";
            canvas.gameObject.SetActive(false); 
            canvasActive = false;
            StartCoroutine(RotateDoor());
        }
        else
        {
            Debug.Log("Incorrect!");
        }
    }

   
    private void QuitGame()
    {
        canvas.gameObject.SetActive(false);
        canvasActive = false; 
    }

    private IEnumerator RotateDoor()
    {
        float angleRemaining = 90f;
        while (angleRemaining > 0f)
        {
            float angleToRotate = Mathf.Min(rotationSpeed * Time.deltaTime, angleRemaining);
            doorl.transform.Rotate(0f, angleToRotate, 0f);
            doorR.transform.Rotate(0f, -angleToRotate, 0f);
            angleRemaining -= angleToRotate;
            yield return null;
        }

        
        psytem.Play();
        Invoke("StopParticleSystem", 5f);
    }

    private void StopParticleSystem()
    {
        psytem.Stop();
        Destroy(this);
    }

    private class Animal
    {
        public string name;
        public Sprite image;

        public Animal(string name, Sprite image)
        {
            this.name = name;
            this.image = image;
        }
    }
}
