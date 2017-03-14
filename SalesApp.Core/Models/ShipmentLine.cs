using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class ShipmentLine: IEquatable<ShipmentLine>
    {
        public int LineNumber { get; set; }
        public string ShipmentId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitMeasurement { get; set; }

        public ShipmentLine() { }
        public ShipmentLine(int lineNumber, string shipmentId, int itemId, string itemName, double qty, string unitMeasurement)
        {
            LineNumber = lineNumber;
            ShipmentId = shipmentId;
            ItemId = itemId;
            ItemName = itemName;
            Quantity = qty;
            UnitMeasurement = unitMeasurement;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ShipmentLine o = obj as ShipmentLine;
            if (o == null) return false;
            return Equals(o);
        }
        public override int GetHashCode()
        {
            int hashItemId = ItemId == 0 ? 0 : ItemId.GetHashCode();
            int hashLineNumber = LineNumber == 0 ? 0 : LineNumber.GetHashCode();
            int hashShipmentId = ShipmentId == "" ? 0 : ShipmentId.GetHashCode();

            return hashItemId * hashLineNumber * hashShipmentId;
        }
        public bool Equals(ShipmentLine other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return ItemId == other.ItemId && ShipmentId == other.ShipmentId && LineNumber == other.LineNumber;
        }
    }
}
