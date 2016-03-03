using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchRadius : MonoBehaviour {	
	
	public List<Collider> ObjectList = new List<Collider>();
	private List<Collider> _removeList = new List<Collider>();
	private float _scrubTimeInterval = 0.5f; //how often the list is scrubbed for nulls

    public SphereCollider SearchCollider
    {
        get
        {
            return GetComponent<SphereCollider>();
        }
    }

	// Use this for initialization
	void Start () {

		StartCoroutine( ScrubList() );
	}	
	
	/// <summary>
	/// Scrubs the list for nulls
	/// </summary>	
	IEnumerator ScrubList()
	{
		while(true)
		{
			foreach(Collider other in ObjectList)
			{
				if (other != null)
				{
					continue;
				}
				else
				{
					_removeList.Add(other);
				}
			}
			
			foreach(Collider other in _removeList)
			{				
				ObjectList.Remove(other);				
			}
			
			_removeList.Clear();
			
			yield return new WaitForSeconds(_scrubTimeInterval);
		}		
		
	}

	
	void OnTriggerEnter(Collider other)
	{
		ObjectList.Add(other);
	}
	
	void OnTriggerExit(Collider other)
	{
		
		ObjectList.Remove(other);
	}
	
	

}
