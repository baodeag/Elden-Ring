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

        private ShopInventory currentShopInventory;
        private ShopInventory runtimeGlobalShopInventory;
        private ShopViewMode currentViewMode = ShopViewMode.Buy;
        private Item currentSelectedItem;
        private readonly List<Button> shopEntryButtons = new List<Button>();

        [Header("Shop UI")]
        [SerializeField] private RectTransform listContentRoot;
        [SerializeField] private ScrollRect listScrollRect;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI runeText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemMetaText;
        [SerializeField] private Button modeButton;
        [SerializeField] private Button actionButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button entryButtonTemplate;

        private TextMeshProUGUI itemColumnHeaderText;
        private TextMeshProUGUI descriptionColumnHeaderText;
        private Image headerBottomLineImage;
        private RectTransform headerContainer;
        private RectTransform itemColumnContainer;
        private RectTransform descriptionColumnContainer;
        private RectTransform actionPanelContainer;

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

            titleText.text = currentShopInventory.shopName.ToUpper();
            runeText.text = "RUNES: " + PlayerUIManager.instance.localPlayer.playerShopManager.GetCurrentRunes();
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

                if (currentSelectedItem == null)
                    currentSelectedItem = item;
            }

            if (shopEntryButtons.Count == 0)
                currentSelectedItem = null;

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

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 64f);

            SetButtonLabel(buttonObject.GetComponent<Button>(), BuildEntryLabel(item));

            Button button = buttonObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                currentSelectedItem = item;
                RefreshSelectionDetails();
            });

            return button;
        }

        private string BuildEntryLabel(Item item)
        {
            if (currentViewMode == ShopViewMode.Buy)
            {
                int price = GetBuyPrice(item);
                int owned = PlayerUIManager.instance.localPlayer.playerShopManager.GetOwnedAmount(item);
                return $"{item.itemName} | {price} runes | owned {owned}";
            }

            return $"{item.itemName} | {Mathf.Max(0, item.sellPrice)} runes";
        }

        private int GetBuyPrice(Item item)
        {
            List<ShopStockEntry> stockEntries = currentShopInventory.GetStockEntries();

            for (int i = 0; i < stockEntries.Count; i++)
            {
                if (stockEntries[i] == null || stockEntries[i].item == null)
                    continue;

                if (stockEntries[i].item.itemID == item.itemID)
                    return stockEntries[i].GetBuyPrice();
            }

            return item.purchasePrice;
        }

        private void RefreshSelectionDetails()
        {
            if (currentSelectedItem == null)
            {
                itemDescriptionText.text = "No item selected.";
                itemMetaText.text = string.Empty;
                return;
            }

            int owned = PlayerUIManager.instance.localPlayer.playerShopManager.GetOwnedAmount(currentSelectedItem);
            int price = currentViewMode == ShopViewMode.Buy ? GetBuyPrice(currentSelectedItem) : Mathf.Max(0, currentSelectedItem.sellPrice);

            itemDescriptionText.text = string.IsNullOrWhiteSpace(currentSelectedItem.itemDescription)
                ? currentSelectedItem.itemName
                : currentSelectedItem.itemDescription;
            itemMetaText.text = $"Item: {currentSelectedItem.itemName}\nPrice: {price}\nOwned: {owned}";
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
                success = PlayerUIManager.instance.localPlayer.playerShopManager.TryBuyItem(selectedEntry);
            }
            else
            {
                success = PlayerUIManager.instance.localPlayer.playerShopManager.TrySellItem(currentSelectedItem);
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
            EnsureInfoTexts();
            ApplyShopLayout();

            if (listContentRoot != null)
            {
                VerticalLayoutGroup layoutGroup = listContentRoot.GetComponent<VerticalLayoutGroup>();

                if (layoutGroup == null)
                    layoutGroup = listContentRoot.gameObject.AddComponent<VerticalLayoutGroup>();

                layoutGroup.spacing = 10f;
                layoutGroup.padding = new RectOffset(12, 12, 12, 12);
                layoutGroup.childAlignment = TextAnchor.UpperCenter;
                layoutGroup.childControlWidth = true;
                layoutGroup.childControlHeight = false;
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = false;

                ContentSizeFitter fitter = listContentRoot.GetComponent<ContentSizeFitter>();

                if (fitter == null)
                    fitter = listContentRoot.gameObject.AddComponent<ContentSizeFitter>();

                fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            if (listScrollRect != null)
            {
                listScrollRect.horizontal = false;
                listScrollRect.vertical = true;
                listScrollRect.scrollSensitivity = 24f;
                listScrollRect.verticalScrollbar = null;
                listScrollRect.verticalScrollbarSpacing = 0f;
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

            if (entryButtonTemplate != null)
            {
                entryButtonTemplate.gameObject.SetActive(false);
                TextMeshProUGUI buttonText = entryButtonTemplate.GetComponentInChildren<TextMeshProUGUI>(true);

                if (buttonText != null)
                {
                    buttonText.enableAutoSizing = true;
                    buttonText.fontSizeMin = 16f;
                    buttonText.fontSizeMax = 24f;
                    buttonText.textWrappingMode = TextWrappingModes.NoWrap;
                    buttonText.overflowMode = TextOverflowModes.Ellipsis;
                }
            }

            if (itemDescriptionText != null)
            {
                itemDescriptionText.textWrappingMode = TextWrappingModes.Normal;
                itemDescriptionText.overflowMode = TextOverflowModes.Overflow;
            }

            if (itemMetaText != null)
            {
                itemMetaText.textWrappingMode = TextWrappingModes.Normal;
            }
        }

        private void EnsureInfoTexts()
        {
            RectTransform menuRoot = menu != null ? menu.transform as RectTransform : null;

            if (menuRoot == null)
                return;

            TextMeshProUGUI template = entryButtonTemplate != null
                ? entryButtonTemplate.GetComponentInChildren<TextMeshProUGUI>(true)
                : GetComponentInChildren<TextMeshProUGUI>(true);

            if (template == null)
                return;

            if (titleText == null)
                titleText = CreateInfoText("Shop Title Text", menuRoot, template, new Vector2(0f, -44f), new Vector2(980f, 50f), new Vector2(0.5f, 1f), 34f, TextAlignmentOptions.Center);

            if (runeText == null)
                runeText = CreateInfoText("Shop Rune Text", menuRoot, template, new Vector2(40f, -104f), new Vector2(380f, 42f), new Vector2(0f, 1f), 24f, TextAlignmentOptions.Left);

            if (itemMetaText == null)
                itemMetaText = CreateInfoText("Shop Item Meta Text", menuRoot, template, new Vector2(-40f, -124f), new Vector2(380f, 110f), new Vector2(1f, 1f), 22f, TextAlignmentOptions.TopLeft);

            if (itemDescriptionText == null)
                itemDescriptionText = CreateInfoText("Shop Description Text", menuRoot, template, new Vector2(-40f, -252f), new Vector2(380f, 212f), new Vector2(1f, 1f), 20f, TextAlignmentOptions.TopLeft);
        }

        private void ApplyShopLayout()
        {
            RectTransform menuRoot = menu != null ? menu.transform as RectTransform : null;

            if (menuRoot == null)
                return;

            menuRoot.sizeDelta = new Vector2(900f, 900f);

            EnsureLayoutContainers(menuRoot);
            EnsureColumnDecor(menuRoot);

            if (titleText != null)
                ConfigureTextRect(titleText.rectTransform, headerContainer, new Vector2(0f, -10f), new Vector2(700f, 52f), new Vector2(0.5f, 1f), TextAlignmentOptions.Center, 34f);

            if (runeText != null)
                ConfigureTextRect(runeText.rectTransform, headerContainer, new Vector2(-10f, -54f), new Vector2(280f, 42f), new Vector2(1f, 1f), TextAlignmentOptions.Right, 28f);

            if (itemMetaText != null)
                ConfigureTextRect(itemMetaText.rectTransform, descriptionColumnContainer, new Vector2(0f, -40f), new Vector2(230f, 110f), new Vector2(0f, 1f), TextAlignmentOptions.TopLeft, 21f);

            if (itemDescriptionText != null)
                ConfigureTextRect(itemDescriptionText.rectTransform, descriptionColumnContainer, new Vector2(0f, -150f), new Vector2(230f, 250f), new Vector2(0f, 1f), TextAlignmentOptions.TopLeft, 19f);

            RectTransform viewportRect = listScrollRect != null ? listScrollRect.viewport : null;

            if (viewportRect != null)
            {
                viewportRect.SetParent(itemColumnContainer, false);
                viewportRect.anchorMin = new Vector2(0f, 0f);
                viewportRect.anchorMax = new Vector2(1f, 1f);
                viewportRect.pivot = new Vector2(0.5f, 0.5f);
                viewportRect.anchoredPosition = Vector2.zero;
                viewportRect.sizeDelta = new Vector2(-22f, 0f);
            }

            if (listContentRoot != null)
            {
                listContentRoot.anchorMin = new Vector2(0f, 1f);
                listContentRoot.anchorMax = new Vector2(1f, 1f);
                listContentRoot.pivot = new Vector2(0.5f, 1f);
                listContentRoot.anchoredPosition = Vector2.zero;
                listContentRoot.sizeDelta = new Vector2(0f, 0f);
            }

            ConfigureButtonRect(modeButton, actionPanelContainer, new Vector2(0f, -8f), "VIEW: BUY");
            ConfigureButtonRect(actionButton, actionPanelContainer, new Vector2(0f, -78f), "BUY");
            ConfigureButtonRect(closeButton, actionPanelContainer, new Vector2(0f, -148f), "CLOSE");
            DisableShopScrollbarVisual();
        }

        private void EnsureLayoutContainers(RectTransform menuRoot)
        {
            headerContainer = EnsureContainer(menuRoot, headerContainer, "Shop Header Container", new Vector2(40f, -20f), new Vector2(820f, 100f), new Vector2(0f, 1f));
            itemColumnContainer = EnsureContainer(menuRoot, itemColumnContainer, "Shop Item Column Container", new Vector2(40f, -160f), new Vector2(520f, 700f), new Vector2(0f, 1f));
            descriptionColumnContainer = EnsureContainer(menuRoot, descriptionColumnContainer, "Shop Description Column Container", new Vector2(590f, -160f), new Vector2(240f, 460f), new Vector2(0f, 1f));
            actionPanelContainer = EnsureContainer(menuRoot, actionPanelContainer, "Shop Action Panel Container", new Vector2(590f, -630f), new Vector2(240f, 190f), new Vector2(0f, 1f));
        }

        private RectTransform EnsureContainer(RectTransform parent, RectTransform currentContainer, string objectName, Vector2 anchoredPosition, Vector2 sizeDelta, Vector2 anchor)
        {
            if (currentContainer == null)
            {
                GameObject containerObject = new GameObject(objectName, typeof(RectTransform));
                currentContainer = containerObject.GetComponent<RectTransform>();
                currentContainer.SetParent(parent, false);
            }

            currentContainer.anchorMin = anchor;
            currentContainer.anchorMax = anchor;
            currentContainer.pivot = anchor;
            currentContainer.anchoredPosition = anchoredPosition;
            currentContainer.sizeDelta = sizeDelta;
            return currentContainer;
        }

        private void DisableShopScrollbarVisual()
        {
            Transform scrollbarTransform = menu != null ? menu.transform.Find("Shop Scrollbar Vertical") : null;

            if (scrollbarTransform != null)
                scrollbarTransform.gameObject.SetActive(false);
        }

        private void EnsureColumnDecor(RectTransform menuRoot)
        {
            TextMeshProUGUI template = entryButtonTemplate != null
                ? entryButtonTemplate.GetComponentInChildren<TextMeshProUGUI>(true)
                : GetComponentInChildren<TextMeshProUGUI>(true);

            if (template == null)
                return;

            if (itemColumnHeaderText == null)
                itemColumnHeaderText = CreateInfoText("Shop Item Header Text", menuRoot, template, new Vector2(34f, -100f), new Vector2(520f, 34f), new Vector2(0f, 1f), 24f, TextAlignmentOptions.Left);

            if (descriptionColumnHeaderText == null)
                descriptionColumnHeaderText = CreateInfoText("Shop Description Header Text", menuRoot, template, new Vector2(-34f, -100f), new Vector2(250f, 34f), new Vector2(1f, 1f), 24f, TextAlignmentOptions.Left);

            itemColumnHeaderText.text = "ITEM";
            descriptionColumnHeaderText.text = "DESCRIPTION";

            itemColumnHeaderText.rectTransform.SetParent(itemColumnContainer, false);
            descriptionColumnHeaderText.rectTransform.SetParent(descriptionColumnContainer, false);
            ConfigureTextRect(itemColumnHeaderText.rectTransform, headerContainer, new Vector2(140f, -82f), new Vector2(220f, 32f), new Vector2(0f, 1f), TextAlignmentOptions.Center, 24f);
            ConfigureTextRect(descriptionColumnHeaderText.rectTransform, headerContainer, new Vector2(-120f, -82f), new Vector2(220f, 32f), new Vector2(1f, 1f), TextAlignmentOptions.Center, 24f);

            headerBottomLineImage = EnsureLineImage(menuRoot, headerBottomLineImage, "Shop Header Bottom Line");
            RectTransform headerBottomRect = headerBottomLineImage.rectTransform;
            headerBottomRect.anchorMin = new Vector2(0f, 1f);
            headerBottomRect.anchorMax = new Vector2(0f, 1f);
            headerBottomRect.pivot = new Vector2(0f, 1f);
            headerBottomRect.anchoredPosition = new Vector2(40f, -130f);
            headerBottomRect.sizeDelta = new Vector2(820f, 4f);

        }

        private Image EnsureLineImage(RectTransform parent, Image currentImage, string objectName)
        {
            if (currentImage != null)
                return currentImage;

            GameObject lineObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            RectTransform lineRect = lineObject.GetComponent<RectTransform>();
            lineRect.SetParent(parent, false);

            Image lineImage = lineObject.GetComponent<Image>();
            lineImage.color = new Color(1f, 1f, 1f, 0.22f);
            return lineImage;
        }

        private void ConfigureTextRect(RectTransform rectTransform, RectTransform parent, Vector2 anchoredPosition, Vector2 sizeDelta, Vector2 anchor, TextAlignmentOptions alignment, float fontSize)
        {
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = anchor;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;

            TextMeshProUGUI text = rectTransform.GetComponent<TextMeshProUGUI>();

            if (text != null)
            {
                text.alignment = alignment;
                text.fontSize = fontSize;
            }
        }

        private void ConfigureButtonRect(Button button, RectTransform parent, Vector2 anchoredPosition, string label)
        {
            if (button == null)
                return;

            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(240f, 54f);

            SetButtonLabel(button, label);
        }

        private TextMeshProUGUI CreateInfoText(string objectName, RectTransform parent, TextMeshProUGUI template, Vector2 anchoredPosition, Vector2 sizeDelta, Vector2 anchor, float fontSize, TextAlignmentOptions alignment)
        {
            GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = anchor;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;

            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
            text.font = template.font;
            text.fontSharedMaterial = template.fontSharedMaterial;
            text.fontSize = fontSize;
            text.color = template.color;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.text = string.Empty;
            return text;
        }

        private void ResetScrollPosition()
        {
            if (listScrollRect == null)
                return;

            Canvas.ForceUpdateCanvases();
            listScrollRect.verticalNormalizedPosition = 1f;
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
            for (int i = 0; i < shopEntryButtons.Count; i++)
            {
                if (shopEntryButtons[i] != null && shopEntryButtons[i] != entryButtonTemplate)
                    Destroy(shopEntryButtons[i].gameObject);
            }

            shopEntryButtons.Clear();
            ResetScrollPosition();
        }
    }
}
