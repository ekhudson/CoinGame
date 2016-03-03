using UnityEngine;
using System.Collections;

public class GrendelPlayer : Entity
{    
    public SearchRadius MySearchRadius;

    public float playerMoveSpeed = 1f; 
    //private Vector3 _tempRotation;
       private Vector3 _move;
    
    public enum PLAYERSTATES
    {
        IDLE,
        MOVING,                
        CINEMATIC,        
    }
    
    private PLAYERSTATES _state = PLAYERSTATES.MOVING;
    
    private static GrendelPlayer instance;    
    
    public static GrendelPlayer Instance
    {
        get { return instance; }
    }
    
    // Use this for initialization
    protected override void Awake()
    {        
        base.Awake();
        instance = this;        
    }
    
    protected override void Start()
    {        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Console.Instance.ShowConsole) //HACK: Replace with Pause gamestate
        {
            //Stuff to do when not paused
            
            _move = Vector3.zero;
            
            switch(_state)
            {
                case PLAYERSTATES.MOVING:
                    
                
                break;                
                
                case PLAYERSTATES.IDLE:
                
                break;
                
                case PLAYERSTATES.CINEMATIC:
                break;            
            }         

            mTransform.localPosition += ( _move  * Time.deltaTime);
            
        }
        else
        {
            //Stuff to do when Paused
        }        
        
    }//end update
    
    public void SetState(PLAYERSTATES newState)
    {
        switch(newState)
        {
            case PLAYERSTATES.MOVING:
            
                switch(_state)
                {
                    case PLAYERSTATES.MOVING:
                
                    break;
                    
                    case PLAYERSTATES.IDLE:
                
                        _state = PLAYERSTATES.MOVING;
                
                    break;        
                    
                    
                    case PLAYERSTATES.CINEMATIC:
                        _state = PLAYERSTATES.MOVING;
                    break;
                
                    default:                        
                        _state = PLAYERSTATES.MOVING;
                    break;
                }
            
            break;
            
            case PLAYERSTATES.IDLE:
            
                switch(_state)
                {
                    case PLAYERSTATES.MOVING:
                        _state = PLAYERSTATES.IDLE;
                    break;                    
                    
                    case PLAYERSTATES.CINEMATIC:
                        _state = PLAYERSTATES.IDLE;
                    break;
                
                    case PLAYERSTATES.IDLE:
                        _state = PLAYERSTATES.IDLE;
                    break;
                
                    default:
                        _state = PLAYERSTATES.IDLE;
                    break;
                }
            
            break;    
            
            case PLAYERSTATES.CINEMATIC:
            
                switch(_state)
                {
                    case PLAYERSTATES.MOVING:
                        _state = PLAYERSTATES.CINEMATIC;
                    break;
                    
                    case PLAYERSTATES.IDLE:
                        _state = PLAYERSTATES.CINEMATIC;
                    break;                    
                    
                    case PLAYERSTATES.CINEMATIC:
                    break;
                
                    default:
                        _state = PLAYERSTATES.CINEMATIC;
                    break;
                }
            
            break;
            
            default:
                
                _state = PLAYERSTATES.MOVING;
            
            break;
        }
    }
    
    public override void CalledUpdate()
    {
        //do nothing
    }
    
//    public override int TakeDamage(int amount)
//    {        
//        return base.TakeDamage(amount);
//    }
    
    public void ResetPlayer()
    {
        mTransform.position = new Vector3(-1f, 1.3f, 1f);
    }
    
    public void ResetPlayer(ConsoleCommandParams parameters)
    {
        ResetPlayer();
    }

    public void MovePlayer(Vector3 move)
    {
        //move1

        transform.localPosition += move;
    }
}//end class
