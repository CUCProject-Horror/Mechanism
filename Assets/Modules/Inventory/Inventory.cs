using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

namespace Game {
	[Serializable]
	public class ItemRecord {
		public Item item;
		public bool possessed;

		public ItemRecord(Item item) {
			this.item = item;
			possessed = false;
		}
	}

	[CreateAssetMenu(menuName = "Game/Inventory")]
	public class Inventory : ScriptableObject {
		[Label("CDs")] public List<ItemRecord> cds;
		public List<ItemRecord> collectives, props, treasures;

		public IEnumerable<IEnumerable<ItemRecord>> ItemRecordLists => new IEnumerable<ItemRecord>[] {
			cds, collectives, props, treasures
		};
		public IEnumerable<ItemRecord> ItemRecords {
			get {
				foreach(var l in ItemRecordLists)
					foreach(var i in l)
						yield return i;
			}
		}

		public void Possess(Item item) => ItemRecords.First(r => r.item == item).possessed = true;
		public void Lose(Item item) => ItemRecords.First(r => r.item == item).possessed = false;
	}
}