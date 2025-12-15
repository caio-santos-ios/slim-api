using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{    
    public class CustomerContractor : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
       
        [BsonElement("code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("saleDate")]
        public string SaleDate { get; set; } = string.Empty;

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("costCenter")]
        public string CostCenter { get; set; } = string.Empty;

        [BsonElement("sellerId")]
        public string SellerId { get; set; } = string.Empty;

        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;

        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        [BsonElement("receiptAccount")]
        public string ReceiptAccount { get; set; } = string.Empty;

        [BsonElement("paymentCondition")]
        public string PaymentCondition { get; set; } = string.Empty;

        [BsonElement("paymentInstallmentQuantity")]
        public int PaymentInstallmentQuantity { get; set; } = 0;

        [BsonElement("dueDateInstallment")]
        public string DueDateInstallment { get; set; } = string.Empty;

        [BsonElement("chargingMethodInstallment")]
        public string ChargingMethodInstallment { get; set; } = string.Empty;

        [BsonElement("typeRecurrence")]
        public string TypeRecurrence { get; set; } = string.Empty;

        [BsonElement("recurrence")]
        public string Recurrence { get; set; } = string.Empty;

        [BsonElement("endRecurrence")]
        public string EndRecurrence { get; set; } = string.Empty;

        [BsonElement("dueDate")]
        public string DueDate { get; set; } = string.Empty;

        [BsonElement("billingPeriod")]
        public string BillingPeriod { get; set; } = string.Empty;

        [BsonElement("billing")]
        public string Billing { get; set; } = string.Empty;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
    } 
}