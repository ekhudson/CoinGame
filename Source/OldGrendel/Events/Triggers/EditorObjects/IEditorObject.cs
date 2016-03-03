using System.Collections;

using UnityEngine;

interface IEditorObject 
{	
	void OnActivate(object caller, EventBase evt); //called when the editor object is activated
	
	void OnDeactivate(object caller, EventBase evt); //called when the editor object is deactivated
	
	void OnToggle(object caller, EventBase evt); //called when the editor object is toggled
	
	void OnEnabled(object caller, EventBase evt); //Activates other editor objects
	
	void OnDisabled(object caller, EventBase evt); //Deactivates other editor objects	
}
