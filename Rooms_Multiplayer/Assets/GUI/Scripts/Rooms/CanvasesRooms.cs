using UnityEngine;

/// <summary>
/// Class keeping track of canvases
/// </summary>
public class CanvasesRooms : MonoBehaviour
{
    [SerializeField] private CanvasCreateOrJoinRoom canvasCreateOrJoinRoom;
    public CanvasCreateOrJoinRoom CanvasCreateOrJoinRoom { get { return canvasCreateOrJoinRoom; } }

    [SerializeField] private CanvasCurrentRoom canvasCurrentRoom;
    public CanvasCurrentRoom CanvasCurrentRoom { get { return canvasCurrentRoom; } }

    /// <summary>
    /// Provides reference to this class for all necesssary child components
    /// </summary>
    private void Awake()
    {
        Debug.Log("Awake canvases rooms, initializing");
        CanvasCreateOrJoinRoom.Initialize(this);
        CanvasCurrentRoom.Initialize(this);
    }

    
}
