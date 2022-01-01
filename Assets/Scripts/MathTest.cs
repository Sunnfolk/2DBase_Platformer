using UnityEngine;

public class MathTest : MonoBehaviour
{
    [SerializeField] private float dir;

    [SerializeField] private float speed = 7f;

    private Input input;

    public float velocity;
    
    // Start is called before the first frame update
    private void Start() => input = GetComponent<Input>();

    // Update is called once per frame
    private void Update()
    {
        dir = input.MoveVector.x + input.MoveVector.y;
        var VectorDirection = transform.InverseTransformDirection(input.MoveVector);
            
        velocity = speed / input.MoveVector.magnitude * dir;
            
            
        // var velocity = (current - previous) / Time.deltaTime;
    }
}