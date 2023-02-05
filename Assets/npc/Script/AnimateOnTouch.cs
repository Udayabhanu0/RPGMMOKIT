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
        if (Input.GetButtonDown("X"))
        {
            animator.SetTrigger("Talking");
        }
    }


}
