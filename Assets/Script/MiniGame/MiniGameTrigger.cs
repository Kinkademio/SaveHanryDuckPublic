using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    bool Working = true;
    private TaskChecker taskChecker;

    public void Start()
    {
        taskChecker = GameObject.Find("Manager").GetComponent<TaskChecker>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(InputController.getInput("action")) && Working && (collision.name == "Duck"))
        {
            Working = false;
            taskChecker.activeTask();


            collision.GetComponent<PlayerControl>().SetKeyboardActive(false);
            GameObject Drone = GameObject.Find("Drone");
            Drone.GetComponent<Weapon>().SetkeyboardActive(false);

            //GameObject.Find("UI MiniGame").GetComponent<Task>().TaskOption();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "Duck" && taskChecker.currentactiveTask != null && !taskChecker.currentactiveTask.taskComplete)
        {
            taskChecker.currentactiveTask.closeTask();
            Working = true;

            collision.GetComponent<PlayerControl>().SetKeyboardActive(true);
            GameObject Drone = GameObject.Find("Drone");
            Drone.GetComponent<Weapon>().SetkeyboardActive(true);
        }
        
    }
}
