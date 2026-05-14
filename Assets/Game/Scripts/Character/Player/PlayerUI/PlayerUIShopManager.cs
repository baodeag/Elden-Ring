using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace baodeag
{
    public class PlayerUIShopManager : PlayerUIMenu
    {
        private enum ShopViewMode
        {
            Buy,
            Sell
        }

        private static readonly Color EntryNormalColor = new Color(0.08f, 0.08f, 0.08f, 1f);
        private static readonly Color EntrySelectedColor = new Color(0.28f, 0.18f, 0.08f, 1f);

        private ShopInventory currentShopInventory;
        private ShopInventory runtimeGlobalShopInventory;
        private ShopViewMode currentViewMode = ShopViewMode.Buy;
        private Item currentSelectedItem;
        private readonly List<Button> shopEntryButtons = new List<Button>();
        private readonly Dictionary<Button, Item> shopEntryItems = new Dictionary<Button, Item>();
        private readonly Dictionary<Button, Image[]> shopEntryColumnImages = new Dictionary<Button, Image[]>();
        private readonly List<GameObject> spawnedColumnEntries = new List<GameObject>();

        [Header("Shop UI")]
        [SerializeField] private RectTransform listContentRoot;
        [SerializeField] private RectTransform stockContentRoot;
        [SerializeField] private RectTransform priceContentRoot;
        [SerializeField] private ScrollRect listScrollRect;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI runeText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemMetaText;
        [SerializeField] private Button modeButton;
        [SerializeField] private Button actionButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button entryButtonTemplate;
        [SerializeField] private Image stockEntryTemplate;
        [SerializeField] private Image priceEntryTemplate;
        [SerializeField] private TextMeshProUGUI stockHeaderText;

        private void Awake()
        {
            ConfigureStaticUI();
        }

        public void OpenGlobalShop(string shopTitle)
        {
            if (runtimeGlobalShopInventory == null)
            {
                GameObject runtimeShopObject = new GameObject("Runtime Global Shop");
                runtimeShopObject.transform.SetParent(transform, false);
                runtimeGlobalShopInventory = runtimeShopObject.AddComponent<ShopInventory>();
            }

            runtimeGlobalShopInventory.shopName = shopTitle;
            OpenShop(runtimeGlobalShopInventory);
        }

        public void OpenShop(ShopInventory shopInventory)
        {
            currentShopInventory = shopInventory;
            currentViewMode = ShopViewMode.Buy;
            currentSelectedItem = null;

            ConfigureStaticUI();
            base.OpenMenu();
            RefreshCurrentView();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
            currentSelectedItem = null;
            ClearEntryButtons();
        }

        public void RefreshCurrentView()
        {
            if (currentShopInventory == null || PlayerUIManager.instance == null || PlayerUIManager.instance.localPlayer == null)
                return;

            if (titleText != null)
                titleText.text = currentShopInventory.shopName.ToUpper();

            if (runeText != null)
            {
                runeText.text = "RUNES: " + PlayerUIManager.instance.localPlayer.playerShopManager.GetCurrentRunes()
                    + " | TIER: " + currentShopInventory.GetEffectiveShopProgressionTier();
            }

            if (stockHeaderText != null)
                stockHeaderText.text = currentViewMode == ShopViewMode.Buy ? "STOCK" : "OWNED";

            SetButtonLabel(modeButton, currentViewMode == ShopViewMode.Buy ? "VIEW: BUY" : "VIEW: SELL");
            SetButtonLabel(actionButton, currentViewMode == ShopViewMode.Buy ? "BUY" : "SELL");

            PopulateEntries();
            RefreshSelectionDetails();
        }

        private void PopulateEntries()
        {
            ClearEntryButtons();

            List<Item> itemsToDisplay = currentViewMode == ShopViewMode.Buy
                ? GetBuyItems()
                : PlayerUIManager.instance.localPlayer.playerShopManager.GetSellableInventoryItems();

            for (int i = 0; i < itemsToDisplay.Count; i++)
            {
                Item item = itemsToDisplay[i];

                if (item == null)
                    continue;

                Button entryButton = CreateEntryButton(item);
                shopEntryButtons.Add(entryButton);
                shopEntryItems[entryButton] = item;

                if (currentSelectedItem == null)
                    currentSelectedItem = item;
            }

            if (shopEntryButtons.Count == 0)
                currentSelectedItem = null;

            UpdateEntrySelectionVisuals();
            ResetScrollPosition();
        }

        private List<Item> GetBuyItems()
        {
            List<Item> items = new List<Item>();
            List<ShopStockEntry> stockEntries = currentShopInventory.GetStockEntries();

            for (int i = 0; i < stockEntries.Count; i++)
            {
                if (stockEntries[i] == null || stockEntries[i].item == null)
                    continue;

                items.Add(stockEntries[i].item);
            }

            return items;
        }

        private Button CreateEntryButton(Item item)
        {
            GameObject buttonObject = Instantiate(entryButtonTemplate.gameObject, listContentRoot);
            buttonObject.name = item.itemName + " Shop Button";
            buttonObject.SetActive(true);
            spawnedColumnEntries.Add(buttonObject);

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 72f);

            Button button = buttonObject.GetComponent<Button>();
            Image itemPanelImage = FindEntryImage(buttonObject.transform, "Item Panel");
            GameObject stockObject = CreatePassiveEntry(stockEntryTemplate, stockContentRoot, item.itemName + " Stock");
            GameObject priceObject = CreatePassiveEntry(priceEntryTemplate, priceContentRoot, item.itemName + " Price");

            BindEntryButton(buttonObject.transform, stockObject != null ? stockObject.transform : null, priceObject != null ? priceObject.transform : null, item);
            shopEntryColumnImages[button] = new[]
            {
                itemPanelImage,
                stockObject != null ? stockObject.GetComponent<Image>() : null,
                priceObject != null ? priceObject.GetComponent<Image>() : null
            };

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                currentSelectedItem = item;
                RefreshSelectionDetails();
                UpdateEntrySelectionVisuals();
            });

            return button;
        }

        private void BindEntryButton(Transform buttonTransform, Transform stockTransform, Transform priceTransform, Item item)
        {
            TextMeshProUGUI nameText = FindEntryText(buttonTransform, "Item Name Text");
            TextMeshProUGUI priceText = FindEntryText(priceTransform, "Item Price Text");
            TextMeshProUGUI stateText = FindEntryText(stockTransform, "Item State Text");

            if (nameText != null)
                nameText.text = item.itemName;

            if (currentViewMode == ShopViewMode.Buy)
            {
                int price = GetBuyPrice(item);
                int remainingStock = currentShopInventory != null ? currentShopInventory.GetRemainingQuantity(item) : -1;

                if (priceText != null)
                    priceText.text = price + " runes";

                if (stateText != null)
                    stateText.text = remainingStock >= 0 ? remainingStock.ToString() : "--";

                return;
            }

            int sellPrice = currentShopInventory != null ? currentShopInventory.GetSellPrice(item) : Mathf.Max(0, item.sellPrice);

            if (priceText != null)
                priceText.text = sellPrice + " runes";

            if (stateText != null)
            {
                int owned = PlayerUIManager.instance.localPlayer.playerShopManager.GetOwnedAmount(item);
                stateText.text = owned.ToString();
            }
        }

        private int GetBuyPrice(Item item)
        {
            if (currentShopInventory != null)
                return currentShopInventory.GetBuyPrice(item);

            return item != null ? item.purchasePrice : 0;
        }

        private void RefreshSelectionDetails()
        {
            if (currentSelectedItem == null)
            {
                if (itemDescriptionText != null)
                    itemDescriptionText.text = "No item selected.";

                if (itemMetaText != null)
                    itemMetaText.text = string.Empty;

                return;
            }

            int owned = PlayerUIManager.instance.localPlayer.playerShopManager.GetOwnedAmount(currentSelectedItem);
            int price = currentViewMode == ShopViewMode.Buy
                ? GetBuyPrice(currentSelectedItem)
                : currentShopInventory != null ? currentShopInventory.GetSellPrice(currentSelectedItem) : Mathf.Max(0, currentSelectedItem.sellPrice);
            int remainingStock = currentViewMode == ShopViewMode.Buy && currentShopInventory != null
                ? currentShopInventory.GetRemainingQuantity(currentSelectedItem)
                : -1;

            if (itemDescriptionText != null)
            {
                itemDescriptionText.text = string.IsNullOrWhiteSpace(currentSelectedItem.itemDescription)
                    ? currentSelectedItem.itemName
                    : currentSelectedItem.itemDescription;
            }

            if (itemMetaText != null)
            {
                string stockLine = remainingStock >= 0 ? $"\nStock: {remainingStock}" : string.Empty;
                itemMetaText.text = $"Item: {currentSelectedItem.itemName}\nPrice: {price}\nOwned: {owned}{stockLine}";
            }
        }

        private void ToggleViewMode()
        {
            currentViewMode = currentViewMode == ShopViewMode.Buy ? ShopViewMode.Sell : ShopViewMode.Buy;
            currentSelectedItem = null;
            RefreshCurrentView();
        }

        private void PerformCurrentTransaction()
        {
            if (currentSelectedItem == null || PlayerUIManager.instance.localPlayer == null)
                return;

            bool success = false;

            if (currentViewMode == ShopViewMode.Buy)
            {
                ShopStockEntry selectedEntry = new ShopStockEntry();
                selectedEntry.item = currentSelectedItem;
                selectedEntry.buyPriceOverride = GetBuyPrice(currentSelectedItem);
                success = PlayerUIManager.instance.localPlayer.playerShopManager.TryBuyItem(selectedEntry, currentShopInventory);
            }
            else
            {
                success = PlayerUIManager.instance.localPlayer.playerShopManager.TrySellItem(currentSelectedItem, currentShopInventory);
            }

            if (success)
            {
                PlayerUIManager.instance.PlayConfirmSFX();
                RefreshCurrentView();
            }
            else
            {
                PlayerUIManager.instance.PlayUnableToContinueSFX();
            }
        }

        private void ConfigureStaticUI()
        {
            ResolveRuntimeReferences();

            ConfigureColumnContentRoot(listContentRoot, 12, 12);
            ConfigureColumnContentRoot(stockContentRoot, 8, 8);
            ConfigureColumnContentRoot(priceContentRoot, 8, 8);

            if (listScrollRect != null)
            {
                listScrollRect.horizontal = false;
                listScrollRect.vertical = true;
                listScrollRect.scrollSensitivity = 24f;
                listScrollRect.onValueChanged.RemoveListener(SyncAuxiliaryColumnScrolls);
                listScrollRect.onValueChanged.AddListener(SyncAuxiliaryColumnScrolls);
            }

            if (modeButton != null)
            {
                modeButton.onClick.RemoveAllListeners();
                modeButton.onClick.AddListener(ToggleViewMode);
            }

            if (actionButton != null)
            {
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(PerformCurrentTransaction);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() => CloseMenuAfterFixedFrame());
            }

            ConfigureEntryTemplate();

            if (itemDescriptionText != null)
            {
                itemDescriptionText.textWrappingMode = TextWrappingModes.Normal;
                itemDescriptionText.overflowMode = TextOverflowModes.Overflow;
            }

            if (itemMetaText != null)
                itemMetaText.textWrappingMode = TextWrappingModes.Normal;
        }

        private void ResolveRuntimeReferences()
        {
            if (menu == null)
                return;

            Transform root = menu.transform;

            titleText = titleText != null ? titleText : FindText(root, "Shop Title Text");
            runeText = runeText != null ? runeText : FindText(root, "Shop Rune Text");
            itemMetaText = itemMetaText != null ? itemMetaText : FindText(root, "Shop Item Meta Text");
            itemDescriptionText = itemDescriptionText != null ? itemDescriptionText : FindText(root, "Shop Description Text");
            stockHeaderText = stockHeaderText != null ? stockHeaderText : FindText(root, "Shop Stock Header Text");
            listContentRoot = listContentRoot != null ? listContentRoot : FindRect(root, "Shop List Content");
            stockContentRoot = stockContentRoot != null ? stockContentRoot : FindRect(root, "Shop Stock Content");
            priceContentRoot = priceContentRoot != null ? priceContentRoot : FindRect(root, "Shop Price Content");
            stockEntryTemplate = stockEntryTemplate != null ? stockEntryTemplate : FindImage(root, "Stock Panel");
            priceEntryTemplate = priceEntryTemplate != null ? priceEntryTemplate : FindImage(root, "Price Panel");
        }

        private void ConfigureEntryTemplate()
        {
            if (entryButtonTemplate == null)
                return;

            entryButtonTemplate.gameObject.SetActive(false);

            Image buttonImage = entryButtonTemplate.GetComponent<Image>();

            if (buttonImage != null)
                buttonImage.color = new Color(0f, 0f, 0f, 0f);

            Transform stockPanel = FindChild(entryButtonTemplate.transform, "Stock Panel");
            Transform pricePanel = FindChild(entryButtonTemplate.transform, "Price Panel");
            RectTransform itemPanelRect = FindRect(entryButtonTemplate.transform, "Item Panel");

            if (stockPanel != null)
                stockPanel.gameObject.SetActive(false);

            if (pricePanel != null)
                pricePanel.gameObject.SetActive(false);

            if (itemPanelRect != null)
            {
                itemPanelRect.anchorMin = new Vector2(0f, 0f);
                itemPanelRect.anchorMax = new Vector2(1f, 1f);
                itemPanelRect.anchoredPosition = Vector2.zero;
                itemPanelRect.sizeDelta = new Vector2(-8f, -8f);
                itemPanelRect.pivot = new Vector2(0.5f, 0.5f);
            }

            ConfigureEntryText(FindEntryText(entryButtonTemplate.transform, "Item Name Text"), 24f, TextAlignmentOptions.Left);

            Image itemPanelImage = FindEntryImage(entryButtonTemplate.transform, "Item Panel");

            if (itemPanelImage != null)
                itemPanelImage.color = EntryNormalColor;

            if (stockEntryTemplate != null)
            {
                stockEntryTemplate.gameObject.SetActive(false);
                ConfigureEntryText(FindEntryText(stockEntryTemplate.transform, "Item State Text"), 18f, TextAlignmentOptions.Center);
            }

            if (priceEntryTemplate != null)
            {
                priceEntryTemplate.gameObject.SetActive(false);
                ConfigureEntryText(FindEntryText(priceEntryTemplate.transform, "Item Price Text"), 22f, TextAlignmentOptions.Right);
            }
        }

        private void ConfigureEntryText(TextMeshProUGUI text, float fontSize, TextAlignmentOptions alignment)
        {
            if (text == null)
                return;

            text.alignment = alignment;
            text.fontSize = fontSize;
            text.enableAutoSizing = false;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Ellipsis;
        }

        private static TextMeshProUGUI FindText(Transform root, string childName)
        {
            if (root == null)
                return null;

            Transform[] children = root.GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null && children[i].name == childName)
                    return children[i].GetComponent<TextMeshProUGUI>();
            }

            return null;
        }

        private static RectTransform FindRect(Transform root, string childName)
        {
            Transform child = FindChild(root, childName);
            return child != null ? child.GetComponent<RectTransform>() : null;
        }

        private static Image FindImage(Transform root, string childName)
        {
            Transform child = FindChild(root, childName);
            return child != null ? child.GetComponent<Image>() : null;
        }

        private static TextMeshProUGUI FindEntryText(Transform root, string childName)
        {
            return FindText(root, childName);
        }

        private static Image FindEntryImage(Transform root, string childName)
        {
            return FindImage(root, childName);
        }

        private static Transform FindChild(Transform root, string childName)
        {
            if (root == null)
                return null;

            Transform[] children = root.GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null && children[i].name == childName)
                    return children[i];
            }

            return null;
        }

        private void UpdateEntrySelectionVisuals()
        {
            for (int i = 0; i < shopEntryButtons.Count; i++)
            {
                Button button = shopEntryButtons[i];

                if (button == null)
                    continue;

                bool isSelected = shopEntryItems.TryGetValue(button, out Item item) && item == currentSelectedItem;

                if (!shopEntryColumnImages.TryGetValue(button, out Image[] columnImages))
                    continue;

                for (int imageIndex = 0; imageIndex < columnImages.Length; imageIndex++)
                {
                    if (columnImages[imageIndex] != null)
                        columnImages[imageIndex].color = isSelected ? EntrySelectedColor : EntryNormalColor;
                }
            }
        }

        private void ResetScrollPosition()
        {
            if (listScrollRect == null)
                return;

            Canvas.ForceUpdateCanvases();
            listScrollRect.verticalNormalizedPosition = 1f;
            SyncAuxiliaryColumnScrolls(Vector2.zero);
        }

        private void SetButtonLabel(Button button, string label)
        {
            if (button == null)
                return;

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);

            if (buttonText != null)
                buttonText.text = label;
        }

        private void ClearEntryButtons()
        {
            for (int i = 0; i < spawnedColumnEntries.Count; i++)
            {
                if (spawnedColumnEntries[i] != null
                    && spawnedColumnEntries[i] != entryButtonTemplate.gameObject
                    && (stockEntryTemplate == null || spawnedColumnEntries[i] != stockEntryTemplate.gameObject)
                    && (priceEntryTemplate == null || spawnedColumnEntries[i] != priceEntryTemplate.gameObject))
                {
                    Destroy(spawnedColumnEntries[i]);
                }
            }

            spawnedColumnEntries.Clear();
            shopEntryButtons.Clear();
            shopEntryItems.Clear();
            shopEntryColumnImages.Clear();
            ResetScrollPosition();
        }

        private void ConfigureColumnContentRoot(RectTransform contentRoot, int leftPadding, int rightPadding)
        {
            if (contentRoot == null)
                return;

            VerticalLayoutGroup layoutGroup = contentRoot.GetComponent<VerticalLayoutGroup>();

            if (layoutGroup == null)
                layoutGroup = contentRoot.gameObject.AddComponent<VerticalLayoutGroup>();

            layoutGroup.spacing = 10f;
            layoutGroup.padding = new RectOffset(leftPadding, rightPadding, 12, 12);
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = false;

            ContentSizeFitter fitter = contentRoot.GetComponent<ContentSizeFitter>();

            if (fitter == null)
                fitter = contentRoot.gameObject.AddComponent<ContentSizeFitter>();

            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private GameObject CreatePassiveEntry(Image template, RectTransform parent, string objectName)
        {
            if (template == null || parent == null)
                return null;

            GameObject entryObject = Instantiate(template.gameObject, parent);
            entryObject.name = objectName;
            entryObject.SetActive(true);
            spawnedColumnEntries.Add(entryObject);

            RectTransform rect = entryObject.GetComponent<RectTransform>();

            if (rect != null)
                rect.sizeDelta = new Vector2(0f, 72f);

            return entryObject;
        }

        private void SyncAuxiliaryColumnScrolls(Vector2 _)
        {
            if (listContentRoot == null)
                return;

            SyncContentPosition(stockContentRoot);
            SyncContentPosition(priceContentRoot);
        }

        private void SyncContentPosition(RectTransform contentRoot)
        {
            if (contentRoot == null)
                return;

            contentRoot.anchoredPosition = listContentRoot.anchoredPosition;
        }
    }
}
