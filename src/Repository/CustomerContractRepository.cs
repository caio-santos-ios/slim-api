using api_slim.src.Configuration;
using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace api_slim.src.Repository
{
    public class CustomerContractRepository(AppDbContext context) : ICustomerContractRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<CustomerContract> pagination)
    {
        try
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$sort", pagination.PipelineSort),  

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "addresses" },

                    { "let", new BsonDocument("profId", "$_id") },

                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$and", new BsonArray
                                    {
                                        new BsonDocument("$eq", new BsonArray
                                        {
                                            new BsonDocument("$toObjectId", "$parentId"),
                                            "$$profId"
                                        }),

                                        new BsonDocument("$eq", new BsonArray
                                        {
                                            "$parent",
                                            "customer-recipient"
                                        })
                                    })
                                }
                            })
                        }
                    },

                    { "as", "_address" }
                }),

                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$_address" },
                    { "preserveNullAndEmptyArrays", true }
                }),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "generic_tables" }, 
                    { "let", new BsonDocument("gender", "$gender") },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$and", new BsonArray
                                    {
                                        new BsonDocument("$eq", new BsonArray { "$code", "$$gender" }),
                                        new BsonDocument("$eq", new BsonArray { "$table", "genero" })
                                    })
                                }
                            })
                        }
                    },
                    { "as", "_gender" } 
                }),

                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$_gender" },
                    { "preserveNullAndEmptyArrays", true }
                }),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "generic_tables" }, 
                    { "let", new BsonDocument("category", "$category") },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$and", new BsonArray
                                    {
                                        new BsonDocument("$eq", new BsonArray { "$code", "$$category" }),
                                        new BsonDocument("$eq", new BsonArray { "$table", "categoria-contrato-cliente" })
                                    })
                                }
                            })
                        }
                    },
                    { "as", "_category" } 
                }),

                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$_category" },
                    { "preserveNullAndEmptyArrays", true }
                }),

                new("$addFields", new BsonDocument
                {
                    {"id", new BsonDocument("$toString", "$_id")},
                    {"address", new BsonDocument
                        {
                            {"id", new BsonDocument("$ifNull", new BsonArray { new BsonDocument("$toString", "$_address._id"), "" })},
                            {"street", new BsonDocument("$ifNull", new BsonArray { "$_address.street", "" })},
                            {"number", new BsonDocument("$ifNull", new BsonArray {"$_address.number" , "" })},
                            {"complement", new BsonDocument("$ifNull", new BsonArray {"$_address.complement" , "" })},
                            {"neighborhood", new BsonDocument("$ifNull", new BsonArray {"$_address.neighborhood" , "" })},
                            {"city", new BsonDocument("$ifNull", new BsonArray {"$_address.city" , "" })},
                            {"state", new BsonDocument("$ifNull", new BsonArray {"$_address.state" , "" })},
                            {"zipCode", new BsonDocument("$ifNull", new BsonArray {"$_address.zipCode" , "" })},
                            {"parent", new BsonDocument("$ifNull", new BsonArray {"$_address.parent" , "" })},
                            {"parentId", new BsonDocument("$ifNull", new BsonArray {"$_address.parentId" , "" })},
                        }
                    },
                    {"genderDescription", new BsonDocument("$ifNull", new BsonArray { "$_gender.description", "" })},
                    {"categoryDescription", new BsonDocument("$ifNull", new BsonArray { "$_category.description", "" })},
                }),
                new("$project", new BsonDocument
                {
                    {"_id", 0}, 
                    {"_address", 0}, 
                    {"_gender", 0}, 
                    {"_category", 0}
                }),
                new("$sort", pagination.PipelineSort),
                new("$skip", pagination.Skip),
                new("$limit", pagination.Limit),

            };

            List<BsonDocument> results = await context.CustomerContracts.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Contrato");
        }
    }
    
    public async Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id)
    {
        try
        {
            BsonDocument[] pipeline = [
                new("$match", new BsonDocument{
                    {"_id", new ObjectId(id)},
                    {"deleted", false}
                }),
                new("$project", new BsonDocument
                {
                    {"_id", 0},
                    {"id", new BsonDocument("$toString", "$_id")},
                    {"name", 1},
                    {"description", 1}
                }),
            ];

            BsonDocument? response = await context.CustomerContracts.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Contrato não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Contrato");
        }
    }
    
    public async Task<ResponseApi<CustomerContract?>> GetByIdAsync(string id)
    {
        try
        {
            CustomerContract? customerRecipient = await context.CustomerContracts.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(customerRecipient);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Contrato");
        }
    }
    public async Task<ResponseApi<long>> GetNextCodeAsync()
    {
        try
        {
            long count = await context.CustomerContracts.Find(x => true).CountDocumentsAsync();
            return new(count);
        }
        catch
        {
            return new(0, 500, "Falha ao buscar novo código");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<CustomerContract> pagination)
    {
        List<BsonDocument> pipeline = new()
        {
            new("$match", pagination.PipelineFilter),
            new("$sort", pagination.PipelineSort),
            new("$addFields", new BsonDocument
            {
                {"id", new BsonDocument("$toString", "$_id")},
            }),
            new("$project", new BsonDocument
            {
                {"_id", 0},
            }),
            new("$sort", pagination.PipelineSort),
        };

        List<BsonDocument> results = await context.CustomerContracts.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<CustomerContract?>> CreateAsync(CustomerContract customerRecipient)
    {
        try
        {
            await context.CustomerContracts.InsertOneAsync(customerRecipient);

            return new(customerRecipient, 201, "Contrato criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Contrato");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<CustomerContract?>> UpdateAsync(CustomerContract customerRecipient)
    {
        try
        {
            await context.CustomerContracts.ReplaceOneAsync(x => x.Id == customerRecipient.Id, customerRecipient);

            return new(customerRecipient, 201, "Contrato atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Contrato");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<CustomerContract>> DeleteAsync(string id)
    {
        try
        {
            CustomerContract? customerRecipient = await context.CustomerContracts.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(customerRecipient is null) return new(null, 404, "Contrato não encontrado");
            customerRecipient.Deleted = true;
            customerRecipient.DeletedAt = DateTime.UtcNow;

            await context.CustomerContracts.ReplaceOneAsync(x => x.Id == id, customerRecipient);

            return new(customerRecipient, 204, "Contrato excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Contrato");
        }
    }
    #endregion
}
}