using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : FunkySheep.Types.SingletonClass<GameManager>
{
    public UIDocument UI;
    public void gpsStarted()
    {
        SceneManager.LoadScene("02 - Sandbox", LoadSceneMode.Additive);
    }

    public void gpsStopped()
    {
        // Return to the loading screen
        //SceneManager.LoadScene("01 - Loading", LoadSceneMode.Additive);
    }
}
