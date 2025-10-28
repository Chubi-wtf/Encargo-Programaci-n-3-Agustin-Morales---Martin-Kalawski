using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardiaScript : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}