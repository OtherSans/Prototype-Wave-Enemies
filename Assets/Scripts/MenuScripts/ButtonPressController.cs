using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPressController : MonoBehaviour
{
    [SerializeField] private MenuButtonController menuButtonController;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorFunctions animFunc;
    [SerializeField] private int thisIndex;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private float animTime;
    void Update()
    {
        if(menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("pressed", true);
                StartCoroutine(WaitForAnimationFinished());
            }
            else if(animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animFunc.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }
    public IEnumerator WaitForAnimationFinished()
    {
        yield return new WaitForSeconds(animTime);
        onPress.Invoke();
    }
}
