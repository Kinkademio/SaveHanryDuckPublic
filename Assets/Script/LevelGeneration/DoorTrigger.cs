using RoomInteriorGeneratorTag;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject MainCamera;
    public GenerationManager generationManager;
    public GameManagerScript gameManagerScript;
    public Direction direction;
    public bool firstDoor = false;
    int RoomNumInMemory = -1;

    public Animator animator;
    bool Close = false;

    //public bool bforward = false;
    //public bool bright = false;
    //public bool bleft = false;
    //public bool bdown = false;

    void Start()
    {
        MainCamera = GameObject.Find("Main Camera");
        generationManager = MainCamera.GetComponent<GenerationManager>();
        gameManagerScript = GameObject.Find("Manager").GetComponent<GameManagerScript>();
        animator = this.GetComponent<Animator>();

        if (firstDoor)
        {
            DoorOpenCloseLogic(GameObject.Find("Duck").GetComponent<Collider2D>());
        }
        //DoorOpenCloseLogic(GameObject.Find("Duck").GetComponent<Collider2D>());

        //RoomNumInMemory = generationManager.SpawnRoom(new Pair<int, int>(0, 5), Direction.Top, 0);
        //generationManager.rooms.Add(new Room(new Pair<int, int>(0, 5), 0, Direction.Top, true));
        //generationManager.SpawnRoom(rooms[0], rooms[0].startDoorPoint, rooms[0].startDoorCoord, rooms[0].direction);
        // взять переменную скорости
        //script.Money = script.Money + 10;

        //if ((-x == startDoor.First) && (y == startDoor.Second))
        //{
        //    room.room.Second[i, j].GetComponent<CircleCollider2D>().enabled = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Duck" && gameManagerScript.getCurretGameState() == GameManagerScript.GameState.GameProcess)
        {
            if (!firstDoor)
            {
                if (RoomNumInMemory == -1)
                {
                    double x = this.transform.position.x; double y = this.transform.position.y;
                    RoomNumInMemory = generationManager.SpawnRoom(new PointDouble(x, y), direction, 1);
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    //Destroy(this.gameObject.GetComponent<BoxCollider2D>());

                    //
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    animator.Play("Door_open");
                }
                else
                {
                    generationManager.ActiveRoom(generationManager.rooms[RoomNumInMemory]);
                    //
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    animator.Play("Door_open");
                }
            } 
            else
            {
                DoorOpenCloseLogic(collision);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Duck" && gameManagerScript.getCurretGameState() == GameManagerScript.GameState.GameProcess)
        {
            if (!firstDoor)
            {
                bool Destroy = PlayerInRoom(collision);

                if (Destroy == true)
                {
                    int roomsPased = int.Parse(ScoreController.getCurrentScoreByName("rooms_passed").scoreValue);
                    roomsPased++;
                    ScoreController.setCurrentScoreNewValue("rooms_passed", roomsPased.ToString());

                    // Код от бесконечного режима
                    //if ((MainCamera.GetComponent<GenerationManager>().BaseRoomSize <= 30) && (roomsPased % 4 == 0))
                    //{
                    //    MainCamera.GetComponent<GenerationManager>().BaseRoomSize += 1;
                    //}

                    gameManagerScript.resetTimer();
                    generationManager.DestroyAllWithout(RoomNumInMemory);
                    //this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    //this.gameObject.AddComponent<BoxCollider2D>();
                    //AddComponent<BoxCollider2D>()
                }
                else
                {

                    generationManager.DisActiveRoom(generationManager.rooms[RoomNumInMemory]);
                    //
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    animator.Play("Door_close");
                }
            } 
            else
            {
                DoorOpenCloseLogic(collision);
            }
        }
    }

    bool PlayerInRoom(Collider2D collision)
    {
        bool InRoom = false;
        switch (direction)
        {
            case Direction.Top:
                if (collision.transform.position.y > this.transform.position.y)
                {
                    InRoom = true;
                }
                break;
            case Direction.Left:
                if (collision.transform.position.x < this.transform.position.x)
                {
                    InRoom = true;
                }
                break;
            case Direction.Right:
                if (collision.transform.position.x > this.transform.position.x)
                {
                    InRoom = true;
                }
                break;
            case Direction.Bottom:
                if (collision.transform.position.y < this.transform.position.y)
                {
                    InRoom = true;
                }
                break;
            default:
                break;
        }
        return InRoom;
    }

    void DoorOpenCloseLogic(Collider2D collision)
    {
        bool NewClose = false;
        switch (direction)
        {
            case Direction.Top:
                if (collision.transform.position.y < this.transform.position.y)
                {
                    NewClose = true;
                }
                break;
            case Direction.Left:
                if (collision.transform.position.x > this.transform.position.x)
                {
                    NewClose = true;
                }
                break;
            case Direction.Right:
                if (collision.transform.position.x < this.transform.position.x)
                {
                    NewClose = true;
                }
                break;
            case Direction.Bottom:
                if (collision.transform.position.y > this.transform.position.y)
                {
                    NewClose = true;
                }
                break;
            default:
                break;
        }

        if ((NewClose == true) != Close) 
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            animator.Play("Door_broken");
        }
        if (!NewClose)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (this.gameObject.activeSelf)
                animator.Play("Door_open");
        }
        Close = NewClose;

        //if (Close != NewClose)
        //{
        //    Close = NewClose;
        //    if (Close)
        //    {
        //        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //        animator.Play("Door_broken");
        //    }
        //    else
        //    {
        //        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //        animator.Play("Door_open");
        //    }
        //}
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
