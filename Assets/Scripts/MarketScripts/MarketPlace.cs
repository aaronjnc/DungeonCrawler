using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MarketPlace : MonoBehaviour
{
    PlayerControls controls;
    [HideInInspector] public ItemReference[,,,] marketItems = new ItemReference[5, 3, 3, 5];
    [HideInInspector] public Image[,] itemImages = new Image[3, 5];
    public float xstart;
    public float ystart;
    public float xdistance;
    public float ydistance;
    public int currentTab = 0;
    public int currentPage = 0;
    public Transform imageParent;
    public Image chosenImage;
    public ItemReference currentItem;
    public Vector3Int currentLoc = Vector3Int.zero;
    public bool player;
    public Inventory inventory;
    Sprite baseImage;
    public Sprite lastPage;
    public GameObject imagePrefab;
    public Vendor vendor;
    public GameObject transferButton;
    GameManager manager;
    bool open = true;
    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        //controls.Interact.Enter.performed += ctx => open = true;
        controls.Interact.Enter.performed += Close;
        controls.Interact.Enter.Enable();
        currentItem = new ItemReference();
        baseImage = GetComponent<Image>().sprite;
        for (int tab = 0; tab < 5; tab++)
        {
            for (int page = 0; page < 3; page++)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        marketItems[tab, page, row, col] = new ItemReference();
                    }
                }
            }
        }
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                GameObject newObject = Instantiate(imagePrefab,imageParent);
                newObject.name = "MarketItem";
                itemImages[row, col] = newObject.GetComponent<Image>();
                itemImages[row, col].color = new Color(255, 255, 255, 0);
                RectTransform rect = newObject.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(xstart + xdistance * col, ystart - ydistance * row, -1);
                newObject.GetComponent<MarketItemClick>().SetUp(this, new Vector2Int(row, col));
            }
        }
        gameObject.SetActive(false);
    }
    void Close(CallbackContext ctx)
    {
        if (open)
        {
            manager.ResumeGame();
            gameObject.SetActive(false);
            open = false;
        }
    }
    public void ChangeItems()
    {
        if (currentPage == 2)
            GetComponent<Image>().sprite = lastPage;
        else
            GetComponent<Image>().sprite = baseImage;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                RefreshImage(itemImages[row, col], marketItems[currentTab, currentPage, row, col].itemSprite);
            }
        }
    }
    public void UpdateImage(ItemReference refItem, Vector4 imgPos)
    {
        marketItems[(int)imgPos.x, (int)imgPos.y, (int)imgPos.z, (int)imgPos.w].ChangeValues(refItem);
        if (imgPos.x.Equals(currentTab) && imgPos.y.Equals(currentPage))
            RefreshImage(itemImages[(int)imgPos.z, (int)imgPos.w], marketItems[(int)imgPos.x, (int)imgPos.y, (int)imgPos.z, (int)imgPos.w].itemSprite);
    }
    public void UpdateItems()
    {
        if (player)
        {
            for (int tab = 0; tab < 5;tab++)
            {
                for (int pos = 0; pos < 35; pos++)
                {
                    int page = (int)(pos / 15);
                    int marketrow = (pos % 15) / 5;
                    int marketcol = (pos % 15) % 5;
                    int invrow = (pos / 7);
                    int invcol = (pos % 7);
                    marketItems[tab, page, marketrow, marketcol].ChangeValues(inventory.invItems[tab, invrow, invcol]);
                }
            }
        }
        ChangeItems();
    }
    public void SetVendor(Vendor vendorSource)
    {
        vendor = vendorSource;
        UpdateVendor();
    }
    public void UpdateVendor()
    {
        ItemReference[,,,] vendorItems = vendor.vendorItems;
        for (int tab = 0; tab < 5; tab++)
        {
            for (int page = 0; page < 3; page++)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        marketItems[tab, page, row, col].ChangeValues(vendorItems[tab, page, row, col]);
                    }
                }
            }
        }
        ChangeItems();
    }
    public void ChooseItem(Vector2Int arrayPos)
    {
        currentItem.ChangeValues(marketItems[currentTab, currentPage, arrayPos.x, arrayPos.y]);
        currentLoc = new Vector3Int(currentPage, arrayPos.x, arrayPos.y);
        if ((transferButton.GetComponent<BuyandSell>().buy && manager.gold >= currentItem.cost) || !transferButton.GetComponent<BuyandSell>().buy)
        {
            transferButton.GetComponent<BuyandSell>().currentItem.ChangeValues(currentItem);
            transferButton.SetActive(true);
        }
        RefreshImage(chosenImage, currentItem.itemSprite);
    }
    public void RefreshImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        if (sprite != null)
            image.color = new Color(255, 255, 255, 255);
        else
            image.color = new Color(255, 255, 255, 0);
    }
    private void OnEnable()
    {
        if (marketItems[0, 0, 0, 0] != null)
            UpdateItems();
    }
}
