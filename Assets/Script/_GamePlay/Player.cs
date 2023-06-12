using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Transform brickContainer;
    public Transform modelTransform;
    [SerializeField] private GameObject firstBrick;
    [SerializeField] private GameObject brickStackPrefab;
    [SerializeField] private LayerMask BrickLayer;
    [SerializeField] private LayerMask BridgeLayer;
    [SerializeField] private LayerMask BrickWinLayer;
    [SerializeField] private LayerMask TurnRight;
    [SerializeField] private LayerMask TurnLeft;
    [SerializeField] private LayerMask Wall;
    [SerializeField] private LayerMask Push;

    [SerializeField] private bool needBrick = false;
    [SerializeField] private bool checkTurnLeft = false;
    [SerializeField] private bool checkTurnRight = false;
    [SerializeField] private bool checkBrick = true;
    [SerializeField] private bool checkWall = false;
    [SerializeField] private bool checkBridge = false;
    [SerializeField] private float speedPlayer = 0.1f;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isForward;
    [SerializeField] private bool isBack;
    [SerializeField] private bool isLeft;
    [SerializeField] private bool isRight;
    [SerializeField] private bool checkPush;
    [SerializeField] private bool checkAhead;
    [SerializeField] private bool checkRearwards;
    [SerializeField] public int numberofbrick = 0;

    [SerializeField] private float threshold = 500f;
    [SerializeField] private Vector3 startInput;
    [SerializeField] private Vector3 endInput;
    public Vector3 moveDirection;
    public float delayTime = 1f;
    private int i =1;

    private void Awake() 
    {
        BrickLayer = (1 << LayerMask.NameToLayer("Brick"));
        BridgeLayer = (1 << LayerMask.NameToLayer("Bridge"));
        BrickWinLayer = (1 << LayerMask.NameToLayer("BrickWin"));
        TurnRight = (1 << LayerMask.NameToLayer("TurnRight"));
        TurnLeft = (1 << LayerMask.NameToLayer("TurnLeft"));
        Wall = (1 << LayerMask.NameToLayer("Wall"));
        Push = (1 << LayerMask.NameToLayer("Push"));
    }

    private void Start() 
    {
        transform.position = new Vector3(firstBrick.transform.position.x, 0.5f, firstBrick.transform.position.z);
    }
    private void Update() 
    {
        if(UIManager.Ins.IsOpened(UIID.MainMenu)) return;
        CheckConditons();
        if(!isMoving && checkWall || !isMoving)
        {
            transform.position = new Vector3((transform.position.x > 0) ? (int)transform.position.x + 0.5f : (int)transform.position.x - 0.5f, (transform.position.y > 0) ? (int)transform.position.y + 0.5f : (int)transform.position.y - 0.5f, (transform.position.z > 0) ? (int)transform.position.z + 0.5f : (int)transform.position.z - 0.5f);
        }
        if(!isMoving && numberofbrick > 0 || (numberofbrick == 0  && !checkAhead && checkRearwards))
        {   
            GetInput();
        }
        GetTargetPosition();
        checkWall = CheckWall();
        MoveToTarget();
        if(checkWall)
        {
            moveDirection = Vector3.zero;
        }
    }
    public bool CheckBrick()
    {
        if (Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, out RaycastHit hit, 2f,BrickLayer))
        {
            Brick br = hit.collider.GetComponent<Brick>();
            if((br.isActiveMesh && isMoving) || (br.isActiveMesh && !isMoving))
            {
                numberofbrick += 1;
                br.DeActiveMesh();

                GameObject newBrick =  Instantiate(brickStackPrefab);
                newBrick.name = "Brick " + numberofbrick;
                newBrick.transform.SetParent(brickContainer);
                newBrick.transform.position = new Vector3(brickContainer.position.x, brickContainer.position.y + 0.3f * numberofbrick, brickContainer.transform.position.z);
                modelTransform.position += Vector3.up * 0.3f;
            }
            return true;
        }
        
        return false;
    }
    
    private bool CheckWall()
    {
        if(Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, out RaycastHit wall, 2f, Wall))
        {
            return true;
        }
        return false;
    }

    private bool CheckBridge()
    {
        if(Physics.Raycast(transform.position + transform.forward, Vector3.down, out RaycastHit forward, 2f, BridgeLayer))
        {
            BrickOfBridge brickOfBridge = forward.collider.GetComponent<BrickOfBridge>();
            if(brickOfBridge.isActiveMesh) checkAhead = true;
            else checkAhead = false;
        }
        else checkAhead = false;

        if(Physics.Raycast(transform.position - transform.forward, Vector3.down, out RaycastHit rearWard, 2f, BridgeLayer))
        {
            BrickOfBridge brickOfBridge = rearWard.collider.GetComponent<BrickOfBridge>();
            if(brickOfBridge.isActiveMesh) checkRearwards = true;
            else checkRearwards = false;
        }
        else checkRearwards = false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit bridge, 1.1f, BridgeLayer))
        {
            BrickOfBridge brickOfBridge = bridge.collider.GetComponent<BrickOfBridge>();
            if((!brickOfBridge.isActiveMesh && !checkAhead && checkRearwards) || (!brickOfBridge.isActiveMesh && !checkAhead && !checkRearwards))
            {
                brickOfBridge.ActiveMesh();
                GameObject objDelete = GameObject.Find("Brick " + numberofbrick );
                Destroy(objDelete);
                numberofbrick --;
                modelTransform.position += Vector3.down * 0.3f;
            }
            if((brickOfBridge.isActiveMesh && checkAhead && !checkRearwards) || (brickOfBridge.isActiveMesh && !checkAhead && !checkRearwards && checkBrick))
            {
                brickOfBridge.DeActiveMesh();

                GameObject newBrick =  Instantiate(brickStackPrefab);
                numberofbrick += 1;
                newBrick.name = "Brick " + numberofbrick;
                newBrick.transform.SetParent(brickContainer);
                newBrick.transform.position = new Vector3(brickContainer.position.x, brickContainer.position.y + 0.3f * numberofbrick, brickContainer.transform.position.z);
                modelTransform.position += Vector3.up * 0.3f;
            }
        }
        if (Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, out RaycastHit brick, 2f,BridgeLayer) && numberofbrick <= 0)
        {
            BrickOfBridge brickOfBridge = brick.collider.GetComponent<BrickOfBridge>();
            if(!brickOfBridge.isActiveMesh)
            {
                moveDirection = Vector3.zero;
            }
        }
        if (Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, out RaycastHit Bridge, 2f,BridgeLayer))
        {
            return true;
        }
        return false;
    }

    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startInput = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {

        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endInput = Input.mousePosition;
            if(Vector3.Distance(startInput, endInput) > threshold)
            {
                moveDirection = endInput - startInput;
            }
        }
    }

    public void GetTargetPosition()
    {
        MovePush();
        if(moveDirection.magnitude < 0.01f)
        {
            return;
        }
        if(isMoving || (!checkWall && numberofbrick > 0))
        {
            return;
        }
        
        if(Vector3.Angle(Vector3.up, moveDirection) <= 45f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if(transform.position == new Vector3(firstBrick.transform.position.x, 0.5f, firstBrick.transform.position.z)) return;
        if(Vector3.Angle(new Vector3(0,-1,0), moveDirection) <= 45f)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        if(Vector3.Angle(new Vector3(-1,0,0), moveDirection) <= 45f) // quay ben trai 
        {
            transform.eulerAngles = new Vector3(0,-90,0);
        }
        if((Vector3.Angle(new Vector3(1,0,0), moveDirection) <= 45f)) // quay ben phai
        {
            transform.eulerAngles = new Vector3(0,90,0);
        }
    }

    public void MoveToTarget()
    {   
        if(checkWall || moveDirection.magnitude < 0.01f)
        {
            isForward = false;
            isBack = false;
            isLeft = false;
            isRight = false;
            isMoving = false;
            moveDirection = Vector3.zero;
            return;
        }
        if((Vector3.Angle(Vector3.up, moveDirection) <= 45f && moveDirection != new Vector3(0,0,0) && !isBack && !isLeft && !isRight))
        {
            MoveForward();
            return;
        }
        if(transform.position == new Vector3(firstBrick.transform.position.x, 0.5f, firstBrick.transform.position.z)) return;
        if((Vector3.Angle(new Vector3(0,-1,0), moveDirection) <= 45f && moveDirection != new Vector3(0,0,0)  && !isForward && !isLeft && !isRight))
        {
            MoveBack();
            return;
        }
        if((Vector3.Angle(new Vector3(-1,0,0), moveDirection) <= 45f && moveDirection != new Vector3(0,0,0) && !isBack && !isForward && !isRight))
        {
            MoveLeft();
            return;
        }
        if((Vector3.Angle(new Vector3(1,0,0), moveDirection) <= 45f && moveDirection != new Vector3(0,0,0) && !isBack && !isLeft && !isForward))
        {
            MoveRight();
            return;
        }
    }

    private bool CheckRightDirection()
    {
        if (Physics.Raycast(transform.position + transform.right, Vector3.down, out RaycastHit Right, 2f, Wall))
        {
            return true;
        }
        return false;
    }

    private bool CheckLeftDirection()
    {
        //Debug.DrawLine(transform.position, transform.position + (transform.up * 10f + transform.forward * 4f) * -1.1f, Color.black);
        if (Physics.Raycast(transform.position + -transform.right, Vector3.down, out RaycastHit Left, 2f, Wall))
        {
            return true;
        }
        return false;
    }

    private bool CheckPush()
    {
        if(Physics.Raycast(transform.position + Vector3.forward * 0.1f, -transform.up, out RaycastHit push, 2f, Push))
        {
            return true;
        }
        return false;
    }

    private void MoveForward()
    {
        isForward = true;
        isMoving = true;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + Vector3.forward, speedPlayer); // di thang
    }

    private void MoveBack()
    {
        isBack = true;
        isMoving = true;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position - Vector3.forward, speedPlayer); // Di lui
    }

    private void MoveRight()
    {
        isRight = true;
        isMoving = true;
        isBack = false;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + new Vector3(1,0,0), speedPlayer); // Re phai   
    }

    private void MoveLeft()
    {
        isLeft = true;
        isMoving = true;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + new Vector3(-1,0,0), speedPlayer); // Re trai
    }

    private void MovePush()
    {
        if(checkPush && checkTurnLeft && isForward && checkWall)
        {
            transform.eulerAngles = new Vector3(0,90,0);
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(628f, -5f, 0f); // Right
            isForward = false;
            return;
        }
        if(checkPush && checkTurnRight && isForward && checkWall)
        {
            transform.eulerAngles = new Vector3(0,-90,0);
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(-400f, 0f, 0f); // Left
            isForward = false;
            return;
        }
        if(checkPush && checkTurnLeft && isLeft && checkWall)
        {
            transform.eulerAngles = new Vector3(0,90,0);
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(628f, -5f, 0f); // Right
            isLeft = false;
            return;
        }
        if(checkPush && checkTurnRight && isLeft && checkWall)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);   
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(9f, -1000f, 0f);  //Back
            isLeft = false;
            return;
        }
        if(checkPush && checkTurnLeft && isRight && checkWall)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(9f, -1000f, 0f);  //Back
            isRight = false;
            return;
        }
        if(checkPush && checkTurnRight && isRight && checkWall)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);   
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(40f, 1032f, 0f);  //Forward
            isRight = false;
            return;
        }
        if(checkPush && checkTurnLeft && isBack && checkWall)
        {
            transform.eulerAngles = new Vector3(0,-90,0); 
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(-400f, 0f, 0f); // Left
            isBack = false;
            return;
        }
        if(checkPush && checkTurnRight && isBack && checkWall)
        {
            transform.eulerAngles = new Vector3(0,90,0);
            moveDirection = Vector3.zero;
            moveDirection = new Vector3(628f, -5f, 0f); // Right
            isBack = false;
            return;
        }
    }

    private void CheckConditons()
    {
        checkWall = CheckWall();
        checkBridge = CheckBridge();
        checkBrick = CheckBrick(); 
        checkTurnRight = CheckRightDirection();
        checkTurnLeft = CheckLeftDirection();
        if(numberofbrick > 0)
        {
            checkPush = CheckPush();
        }
    }
}
