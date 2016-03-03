using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour 
{
    public Transform BaseTransform { get; private set; }
    public GameObject BaseGameObject { get; private set; }
    public Collider BaseCollider { get; private set; }
    public BoxCollider BaseBoxCollider { get; private set; }
    public SphereCollider BaseSphereCollider { get; private set; }
    public Rigidbody BaseRigidbody { get; private set; } 

	private void Awake()
    {
        BaseTransform = transform;
        BaseGameObject = gameObject;
        BaseCollider = GetComponent<Collider>();
        BaseBoxCollider = GetComponent<BoxCollider>();
        BaseSphereCollider = GetComponent<SphereCollider>();
        BaseRigidbody = GetComponent<Rigidbody>();
    }
}
