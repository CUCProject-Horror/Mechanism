﻿using System;
using System.Collections.Generic;

[Serializable]
public class Inventory {
	[Serializable]
	public class ItemRecord {
		public Item item;
		public uint count;

		public ItemRecord(Item item, uint count) {
			this.item = item;
			this.count = count;
		}

		public ItemRecord(Item item) : this(item, 0) { }
	}
	public List<ItemRecord> records;

	public void Add(Item item) {
		foreach(ItemRecord record in records) {
			if(record.item != item)
				continue;
			++record.count;
			return;
		}
		ItemRecord newRecord = new ItemRecord(item, 1);
		records.Add(newRecord);
	}
}