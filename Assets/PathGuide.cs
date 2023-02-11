using UnityEngine;

public class PathGuide : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    public float speed = 2f;
    public float threshold = 0.5f;

    private ParticleSystem particleSysteml;

    private void Start()
    {
        particleSysteml = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
       
        
        float distance = Vector3.Distance(player.transform.position, target.position);
        float pdistance = Vector3.Distance(target.position,particleSysteml.transform.position);

        if (distance > threshold && pdistance >threshold)
        {
       
            particleSysteml.transform.position = Vector3.MoveTowards(particleSysteml.transform.position, target.position, speed * Time.deltaTime);
          
        }
        else if(pdistance <threshold){
            
            particleSysteml.transform.position = player.transform.position;
        }
        if(distance < threshold)
        {
            Destroy(GetComponent<ParticleSystem>());
        }
    }
}
