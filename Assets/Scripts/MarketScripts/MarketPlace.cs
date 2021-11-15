using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MarketPlace : MonoBehaviour
{
    PlayerControls controls;
    [HideInInspector] public ItemSlot[,,,] marketItems = new ItemSlot[5, 3, 3, 5];
    [HideInInspector] public Image[,] itemImages = new Image[3, 5];
    public float xstart;
    public float ystart;
    public float xdistance;
    public float ydistance;
    public int currentTab = 0;
    public int currentPage = 0;
    public Transform imageParent;
    public Image chosenImage;
    public Vector3Int currentLoc = Vector3Int.zero;
    public bool player;
    public Inventory inventory;
    Sprite baseImage;
    public Sprite lastPage;
    public GameObject imagePrefab;
    public Vendor vendor;
    public GameObject transferButton;
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        //controls.Interact.Enter.performed += ctx => open = true;
        controls.Interact.Enter.performed += Close;
        controls.Interact.Enter.Enable();
        baseImage = GetComponent<Image>().sprite;
        for (int tab = 0; tab < 5; tab++)
        {
            for (int page = 0; page < 3; page++)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        marketItems[tab, page, row, col] = new ItemSlot();
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
    /// <summary>
    /// Closes the market place and resumes the game
    /// </summary>
    /// <param name="ctx"></param>
    void Close(CallbackContext ctx)
    {
        manager.ResumeGame();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Updates images for page change
    /// </summary>
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
                RefreshImage(itemImages[row, col], marketItems[currentTab, currentPage, row, col].getSprite());
            }
        }
    }
    /// <summary>
    /// Changes item at given position and updates image
    /// </summary>
    /// <param name="refItem">Script for new item</param>
    /// <param name="imgPos">Position to update</param>
    public void UpdateImage(ItemSlot refItem, Vector4 imgPos)
    {
        marketItems[(int)imgPos.x, (int)imgPos.y, (int)imgPos.z, (int)imgPos.w].addExisting(refItem);
        if (imgPos.x.Equals(currentTab) && imgPos.y.Equals(currentPage))
            RefreshImage(itemImages[(int)imgPos.z, (int)imgPos.w], marketItems[(int)imgPos.x, (int)imgPos.y, (int)imgPos.z, (int)imgPos.w].getSprite());
    }
    /// <summary>
    /// Updates players items using inventory
    /// </summary>
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
                    marketItems[tab, page, marketrow, marketcol].addExisting(inventory.getItemSlot(tab, invrow, invcol));
                }
            }
        }
        ChangeItems();
    }
    /// <summary>
    /// Sets the vendor source script
    /// </summary>
    /// <param name="vendorSource">Vendor source script</param>
    public void SetVendor(Vendor vendorSource)
    {
        vendor = vendorSource;
        UpdateVendor();
    }
    /// <summary>
    /// Updates the vendor items with new vendor script
    /// </summary>
    public void UpdateVendor()
    {
        ItemSlot[,,,] vendorItems = vendor.vendorItems;
        for (int tab = 0; tab < 5; tab++)
        {
            for (int page = 0; page < 3; page++)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        marketItems[tab, page, row, col].addExisting(vendorItems[tab, page, row, col]);
                    }
                }
            }
        }
        ChangeItems();
    }
    /// <summary>
    /// Updates chosen item values given new chosen item
    /// </summary>
    /// <param name="arrayPos">Array position of clicked item</param>
    public void ChooseItem(Vector2Int arrayPos)
    {
        currentLoc = new Vector3Int(currentPage, arrayPos.x, arrayPos.y);
        ItemSlot currentItem = marketItems[currentTab, currentPage, currentLoc.x, currentLoc.y];
        if ((transferButton.GetComponent<BuyandSell>().buy && manager.gold >= currentItem.getCost()) || !transferButton.GetComponent<BuyandSell>().buy)
        {
            transferButton.GetComponent<BuyandSell>().currentItem.addExisting(currentItem);
            transferButton.SetActive(true);
        }
        RefreshImage(chosenImage, currentItem.getSprite());
    }
    /// <summary>
    /// Updates sprite of image
    /// </summary>
    /// <param name="image">Image to update</param>
    /// <param name="sprite">Sprite to put in the image</param>
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
    private void OnDestroy()
    {
        if (controls != null)
            controls.Disable();
    }
}
