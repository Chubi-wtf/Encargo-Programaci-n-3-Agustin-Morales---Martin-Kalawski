using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardiaScript : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}