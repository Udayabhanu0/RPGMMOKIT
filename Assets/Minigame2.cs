using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame2 : MonoBehaviour
{
    [System.Serializable]
    public class Fruit
    {
        public string name;
        public Sprite image;
    }

    [System.Serializable]
    public class Box
    {
        public string name;
        public string[] acceptedFirstLetters;
        public GameObject boxGameObject;
    }

    [System.Serializable]
    public class FruitSet
    {
        public string setName;
        public List<Fruit> fruitList = new List<Fruit>();
    }

    public List<FruitSet> fruitSetsList = new List<FruitSet>();
    public List<Box> boxesList = new List<Box>();

    public Transform fruitHolder;
    public Transform boxHolder;

    private GameObject fruitPrefab;

    private bool isDragging = false;
    private GameObject draggedFruit;
    private Vector2 originalPosition;
    private Transform originalParent;

    void Start()
    {
        fruitPrefab = Resources.Load<GameObject>("FruitPrefab");

        foreach (FruitSet fruitSet in fruitSetsList)
        {
            foreach (Fruit fruit in fruitSet.fruitList)
            {
                GameObject fruitGameObject = Instantiate(fruitPrefab, fruitHolder);
                fruitGameObject.name = fruit.name;
                fruitGameObject.GetComponent<Image>().sprite = fruit.image;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Fruit")
            {
                isDragging = true;
                draggedFruit = hit.collider.gameObject;
                originalPosition = draggedFruit.transform.position;
                originalParent = draggedFruit.transform.parent;
                draggedFruit.transform.SetParent(boxHolder);
            }
        }

        if (isDragging)
        {
            draggedFruit.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                draggedFruit.transform.SetParent(originalParent);

                RaycastHit2D hit = Physics2D.Raycast(draggedFruit.transform.position, Vector2.zero);
                if (hit.collider != null && hit.collider.tag == "Box")
                {
                    Box box = boxesList.Find(b => b.boxGameObject == hit.collider.gameObject);
                    bool isAccepted = false;
                    foreach (string acceptedLetter in box.acceptedFirstLetters)
                    {
                        if (draggedFruit.name.StartsWith(acceptedLetter))
                        {
                            isAccepted = true;
                            break;
                        }
                    }
                    if (isAccepted)
                    {
                        Destroy(draggedFruit);
                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("SoundEffects/Correct"), Camera.main.transform.position);
                        GameObject particleSystem = Instantiate(Resources.Load<GameObject>("Particles/CorrectParticleSystem"), box.boxGameObject.transform.position, Quaternion.identity);
                        Destroy(particleSystem, 1f);
                    }
                    else
                    {
                        draggedFruit.transform.position = originalPosition;
                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("SoundEffects/Wrong"), Camera.main.transform.position);
                        GameObject particleSystem = Instantiate(Resources.Load<GameObject>("Particles/WrongParticleSystem"), draggedFruit.transform.position, Quaternion.identity);
                        Destroy(particleSystem, 1f);
                    }
                }
                else
                {
                    draggedFruit.transform.position = originalPosition;
                }
            }
        }
    }
}
