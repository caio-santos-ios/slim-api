using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class CustomerContract : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
       
        [BsonElement("code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("contractorId")]
        public string ContractorId { get; set; } = string.Empty; 

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("saleDate")]
        public DateTime? SaleDate { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("costCenter")]
        public string CostCenter { get; set; } = string.Empty;

        [BsonElement("sellerId")]
        public string SellerId { get; set; } = string.Empty;
     
        [BsonElement("planId")]
        public string PlanId { get; set; } = string.Empty;
        
        [BsonElement("serviceModuleIds")]
        public List<string> ServiceModuleIds { get; set; } = [];

        [BsonElement("subTotal")]
        public decimal SubTotal { get; set; }

        [BsonElement("total")]
        public decimal Total { get; set; }

        [BsonElement("discount")]
        public decimal Discount { get; set; }

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

        [BsonElement("recurrencePeriod")]
        public string RecurrencePeriod { get; set; } = string.Empty;

        [BsonElement("endRecurrence")]
        public DateTime? EndRecurrence { get; set; }

        [BsonElement("dueDate")]
        public DateTime? DueDate { get; set; }

        [BsonElement("billingPeriod")]
        public string BillingPeriod { get; set; } = string.Empty;

        [BsonElement("billing")]
        public string Billing { get; set; } = string.Empty;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
    }
}