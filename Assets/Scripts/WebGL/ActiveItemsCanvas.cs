using UnityEngine;
using UnityEngine.UI;

public class ActiveItemsCanvas : MonoBehaviour
{
    #region Variables
    public static ActiveItemsCanvas Instance { get; private set; }

    [SerializeField] GameObject gloveText, coatText, gogglesText;
    [SerializeField] Image interactIcon;
    [SerializeField] Color noGloveColor, gloveColor;

    [HideInInspector] public bool isWearingGloves;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateItemUI();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove += OnPutOnLeftGlove;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove += OnPutOnRightGlove;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += OnTakeOffLeftGlove;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += OnTakeOffRightGlove;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove -= OnPutOnLeftGlove;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove -= OnPutOnRightGlove;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove -= OnTakeOffLeftGlove;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove -= OnTakeOffRightGlove;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Called when the player puts on the left glove. Updates the UI to show that the player is wearing gloves.
    /// </summary>
    private void OnPutOnLeftGlove()
    {
        isWearingGloves = true;
        UpdateItemUI();
    }

    /// <summary>
    /// Called when the player puts on the right glove. Updates the UI to show that the player is wearing gloves.
    /// </summary>
    private void OnPutOnRightGlove()
    {
        isWearingGloves = true;
        UpdateItemUI();
    }

    /// <summary>
    /// Called when the player takes off the left glove. Updates the UI to show that the player is not wearing gloves.
    /// </summary>
    private void OnTakeOffLeftGlove()
    {
        isWearingGloves = false;
        UpdateItemUI();
    }

    /// <summary>
    /// Called when the player takes off the right glove. Updates the UI to show that the player is not wearing gloves.
    /// </summary>
    private void OnTakeOffRightGlove()
    {
        isWearingGloves = false;
        UpdateItemUI();
    }

    /// <summary>
    /// Updates the UI to show the items the player is wearing.
    /// </summary>
    public void UpdateItemUI()
    {
        gogglesText.SetActive(WearGoggles.IsWearing());
        coatText.SetActive(WearCoat.IsWearing());
        gloveText.SetActive(isWearingGloves);

        if (isWearingGloves)
            interactIcon.color = gloveColor;
        else
            interactIcon.color = noGloveColor;
    }
    #endregion
}
