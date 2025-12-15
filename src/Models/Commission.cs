using api_slim.src.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_slim.src.Models
{
    public class Commission : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("ruleName")]
        public string RuleName { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("saleType")]
        public string SaleType { get; set; } = string.Empty;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
        
        [BsonElement("typeModality")]
        public string TypeModality { get; set; } = string.Empty;
        
        [BsonElement("valueModality")]
        public decimal ValueModality { get; set; }

        [BsonElement("percentageModality")]
        public decimal PercentageModality { get; set; }

        [BsonElement("startNumberModality")]
        public int StartNumberModality { get; set; }
        
        [BsonElement("endNumberModality")]
        public int EndNumberModality { get; set; }

        [BsonElement("startPeriod")]
        public DateTime StartPeriod { get; set; }
        
        [BsonElement("endPeriod")]
        public DateTime EndPeriod { get; set; }

        [BsonElement("liquidation")]
        public string Liquidation { get; set; } = string.Empty;
        
        [BsonElement("referenceCriteria")]
        public string ReferenceCriteria { get; set; } = string.Empty;
        
        [BsonElement("paymentDate")]
        public string PaymentDate { get; set; } = string.Empty;

        [BsonElement("campaign")]
        public Campaign Campaign { get; set; } = new();
        
        [BsonElement("isAgency")]
        public bool IsAgency { get; set; } = false;
        
        [BsonElement("agency")]
        public Agency Agency { get; set; } = new();

        [BsonElement("isRecurrence")]
        public bool IsRecurrence { get; set; } = false;
        
        [BsonElement("recurrence")]
        public Recurrence Recurrence { get; set; } = new();
    }

    public class Campaign
    {
        [BsonElement("startPeriod")]
        public DateTime StartPeriod { get; set; }
        
        [BsonElement("endPeriod")]
        public DateTime EndPeriod { get; set; }

        [BsonElement("paymentDate")]
        public DateTime PaymentDate { get; set; }
        
        [BsonElement("typeModality")]
        public DateTime TypeModality { get; set; }
        
        [BsonElement("valueModality")]
        public decimal ValueModality { get; set; }

        [BsonElement("percentageModality")]
        public decimal PercentageModality { get; set; }

        [BsonElement("startNumberModality")]
        public int StartNumberModality { get; set; }
        
        [BsonElement("endNumberModality")]
        public int EndNumberModality { get; set; }

        [BsonElement("liquidation")]
        public string Liquidation { get; set; } = string.Empty;
        
        [BsonElement("referenceCriteria")]
        public string ReferenceCriteria { get; set; } = string.Empty;
        
    } 
    public class Agency
    {
        
        [BsonElement("accession")]
        public bool Accession { get; set; }

        [BsonElement("value")]
        public decimal Value { get; set; }

        [BsonElement("percentage")]
        public decimal Percentage { get; set; }

        [BsonElement("liquidation")]
        public string Liquidation { get; set; } = string.Empty;
        
        [BsonElement("installment")]
        public Installments Installment { get; set; } = new();
    }
    
    public class Recurrence
    {
        [BsonElement("value")]
        public decimal Value { get; set; }

        [BsonElement("percentage")]
        public decimal Percentage { get; set; }

        [BsonElement("accession")]
        public bool Accession { get; set; }

        [BsonElement("valueAccession")]
        public decimal ValueAccession { get; set; }

        [BsonElement("percentageAccession")]
        public decimal PercentageAccession { get; set; }

        [BsonElement("liquidation")]
        public string Liquidation { get; set; } = string.Empty;
        
        [BsonElement("installment")]
        public Installments Installment { get; set; } = new();
    }

    public class Installments
    {
        [BsonElement("startMonthlyPayment")]
        public string StartMonthlyPayment { get; set; } = string.Empty;
      
        [BsonElement("quantityInstallments")]
        public int QuantityInstallments { get; set; }

        [BsonElement("periodicity")]
        public bool Periodicity { get; set; }

        [BsonElement("installments")]
        public List<Installment> ListInstallment { get; set; } = new();        
    } 

    public class Installment 
    {
        [BsonElement("number")]
        public decimal Number { get; set; }
        
        [BsonElement("value")]
        public decimal Value { get; set; }
        
        [BsonElement("percentage")]
        public decimal Percentage { get; set; }
    }
}