using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Game.Ui
{
    public class CategoryUi : UiController
    {
        ItemType category;
        List<ItemRecord> records;

        public Image itemSprite;
        private Sprite defaultItemSprite;
        public GameObject itemIntroduceText;
        public string emptyText = "������";

        public ItemType Category
        {
            get => category;
            set
            {
                category = value;
                records = GameManager.instance.protagonist
                    .inventory.GetRecordsByType(value);
                if (records == null)
                    throw new UnityException($"Cannot find category \"${category}\" in protagonist inventory");
                SetUpEntries();
            }
        }

        void CleanUpEntries()
        {
            Bp.backButton.navigation.down = null;
            Utils.DestroyAllChildren(Bp.entryList.transform);
        }

        void SetUpEntries()
        {
            CleanUpEntries();
            if (records == null)
                return;
            var entries = new List<UiElement>();
            foreach (var record in records)
            {
                var entryObj = Instantiate(Bp.entryButtonPrefab, Bp.entryList.transform);
                var entry = entryObj.GetComponent<UiElement>();
                entries.Add(entry);
                if (!record.possessed)
                {
                    entry.Selectable = true;
                    entry.Text = emptyText;
                }
                else
                {
                    entry.Selectable = true;
                    entry.Text = record.item.name;
                }
            }
            Bp.SelectedElement = Bp.backButton;
            for (int i = 0; i < entries.Count; ++i)
            {
                var entry = entries[i];
                if (i == 0)
                {
                    Bp.SelectedElement = entry;
                    Bp.backButton.navigation.down = entry;
                    entry.navigation.up = Bp.backButton;
                }
                entry.onSelect.AddListener(OnEntrySelect);
                entry.onDeselect.AddListener(OnEntryDeselect);
                entry.onUse.AddListener(OnEntryView);
            }
            Bp.SetUpEntriesNagivation();
        }

        public ItemRecord GetRecordByEntryButton(UiElement button)
        {
            if (button.transform.parent != Bp.entryList.transform)
                return null;
            var i = button.transform.GetSiblingIndex();
            return records[i];
        }

        /// <summary>
        /// 根据 item 获取其在该类型里的下标。
        /// </summary>
        /// <returns>若不在，返回 -1。</returns>
        public int GetRecordIndexByItem(Item item)
        {
            for (int i = 0; i < records.Count; ++i)
            {
                if (records[i].item == item)
                    return i;
            }
            return -1;
        }

        public int GetRecordIndexByEntryButton(UiElement button)
            => GetRecordIndexByItem(GetRecordByEntryButton(button)?.item);

        public UiElement GetEntryButtonByRecordIndex(int i)
            => Bp.entryList.transform.GetChild(i)?.GetComponent<UiElement>();

        public void ViewItem(Item item)
        {
            if (item == null)
            {
                // 刷掉
                return;
            }
            // 正常显示
            if (item.showDescription)
            {
                itemIntroduceText.SetActive(true);
                itemIntroduceText.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
                // 如果用Sprite则调用这个
                // 
                // 如果用Text则调用这个
                // Text = item.description;
            }
            switch (item) {
                case CD cd:
                    cd.playVid.Invoke();
                    break;
            }
        }

        void OnEntrySelect()
        {
            var entry = Bp.SelectedElement;
            var i = GetRecordIndexByEntryButton(entry);
            if (i == -1)
                return;
            var record = records[i];
            if (record.possessed)
            {
                itemSprite.sprite = record.item.selectSprite;
            }
        }

        void OnEntryDeselect()
        {
            var entry = Bp.SelectedElement;
            var i = GetRecordIndexByEntryButton(entry);
            if (i == -1)
                return;
            // TODO
            var record = records[i];
            if (record.possessed)
            {
                itemIntroduceText.SetActive(false);
                itemSprite.sprite = defaultItemSprite;
                //itemIntroduceText.GetComponent<Image>().sprite = null;
            }
        }

        void OnEntryView()
        {
            var entry = Bp.SelectedElement;
            var i = GetRecordIndexByEntryButton(entry);
            if (i == -1)
                return;
            var record = records[i];
            if (record.possessed)
                ViewItem(record.item);
        }

        protected void OnEnable()
        {
            SetUpEntries();
        }

        protected void OnDisable()
        {
            CleanUpEntries();
        }

        #region Life Cycle
        private void Start()
        {
            defaultItemSprite = itemSprite.sprite;
        }
        #endregion
    }
}