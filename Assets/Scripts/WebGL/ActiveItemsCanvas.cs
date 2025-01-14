using UnityEngine;

public class ActiveItemsCanvas : MonoBehaviour
{
    public static ActiveItemsCanvas Instance { get; private set; }

    [SerializeField] GameObject gloveText, coatText, gogglesText;

    [HideInInspector] public bool isWearingGloves;

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

    private void OnPutOnLeftGlove()
    {
        isWearingGloves = true;
        UpdateItemUI();
    }

    private void OnPutOnRightGlove()
    {
        isWearingGloves = true;
        UpdateItemUI();
    }

    private void OnTakeOffLeftGlove()
    {
        isWearingGloves = false;
        UpdateItemUI();
    }

    private void OnTakeOffRightGlove()
    {
        isWearingGloves = false;
        UpdateItemUI();
    }

    public void UpdateItemUI()
    {
        gogglesText.SetActive(WearGoggles.IsWearing());
        coatText.SetActive(WearCoat.IsWearing());
        gloveText.SetActive(isWearingGloves);
    }
}
