using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class Item : IEquatable<Item>
    {
        [PrimaryKey]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string UnitMeasurement { get; set; }
        public Item() { }
        public Item(int id, string name, double price, string unitMeasurement)
        {
            ItemId = id;
            Name = name;
            Price = price;
            UnitMeasurement = unitMeasurement;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Item o = obj as Item;
            if (o == null) return false;
            return Equals(o);
        }
        public override int GetHashCode()
        {
            int hashItemId = ItemId == 0 ? 0 : ItemId.GetHashCode();
            int hashItemName = Name == null ? 0 : Name.GetHashCode();
            int hashUnitMeasurement = UnitMeasurement == null ? 0 : UnitMeasurement.GetHashCode();
            int hashPrice = Price == 0.0 ? 0 : Price.GetHashCode();

            return hashItemId * hashItemName * hashUnitMeasurement + hashPrice;
        }
        public bool Equals(Item other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return ItemId == other.ItemId && Name == other.Name && UnitMeasurement == UnitMeasurement;
        }
    }
}
