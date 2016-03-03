using UnityEngine;
using System.Collections;

public class HealthUpdateEvent : EventBase
{	
	protected int mUpdateAmount;
	protected int mNewHealthAmount;
    protected Entity mSubject;	
	
	public int UpdateAmount
	{
		get { return mUpdateAmount; }
	}

    public Entity Subject
    {
        get { return mSubject; }
    }

    public HealthUpdateEvent(object sender, Entity subject,int updateAmount, int newHealthAmount, Vector3 position) : base(position, sender)
    {   		
        mSubject = subject;       
		mUpdateAmount = updateAmount;
		mNewHealthAmount = newHealthAmount;
    }	
	
	
}
