using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerStatePattern : MonoBehaviour
{
    enum PLAYER_STATE { S_WALK, S_IDLE, S_RUN, S_JUMP};
    PLAYER_STATE state;
    public Animator anmi;
    // Start is called before the first frame update
    void Start()
    {
        state = PLAYER_STATE.S_IDLE;
        anmi = GetComponent<Animator>();
        LinkedList list = new LinkedList();
        list.Add("Hello");
        list.Add("worldn");
        list.Add("!");
        list.TraverseAndPring();
    }
     void OnCollisionEnter(Collision collision)
    {
        
        if(state== PLAYER_STATE.S_JUMP)
        {
            state = PLAYER_STATE.S_WALK;
            anmi.SetTrigger("walk");
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case PLAYER_STATE.S_IDLE:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    anmi.SetTrigger("walk");
                    state = PLAYER_STATE.S_WALK;             
                }
                break;
            case PLAYER_STATE.S_WALK:
                if (Input.GetKeyUp(KeyCode.W))
                {
                    anmi.SetTrigger("stop");
                    state = PLAYER_STATE.S_IDLE;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    anmi.SetTrigger("run");
                    state = PLAYER_STATE.S_RUN;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anmi.SetTrigger("jump");
                    state = PLAYER_STATE.S_JUMP;
                }

                break;
            case PLAYER_STATE.S_RUN:
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    anmi.SetTrigger("walk");
                    state = PLAYER_STATE.S_WALK;
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    anmi.SetTrigger("stop");
                    state = PLAYER_STATE.S_IDLE;
                }
                break;
            case PLAYER_STATE.S_JUMP:
                state = PLAYER_STATE.S_WALK;
                anmi.SetTrigger("walk");
                break;
        }
    }
}
