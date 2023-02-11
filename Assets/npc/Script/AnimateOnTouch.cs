using UnityEngine;

public class AnimateOnTouch : MonoBehaviour
{
    public Animator animator;

    private bool talking = false;

    private void OnMouseDown()
    {
        if (!talking)
        {
            talking = true;
            animator.SetTrigger("Talking");
        }
        talking= false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Talking");
        }
    }


}
