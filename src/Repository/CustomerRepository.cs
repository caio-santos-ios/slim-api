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
    public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Customer> pagination)
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
                                            "customer-contract"
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
                }),
                new("$project", new BsonDocument
                {
                    {"_id", 0}, 
                    {"_address", 0}, 
                }),
                new("$sort", pagination.PipelineSort),
                new("$skip", pagination.Skip),
                new("$limit", pagination.Limit),

            };

            List<BsonDocument> results = await context.Customers.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Clientes");
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

            BsonDocument? response = await context.Customers.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Clientes não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Clientes");
        }
    }
    
    public async Task<ResponseApi<Customer?>> GetByIdAsync(string id)
    {
        try
        {
            Customer? plan = await context.Customers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(plan);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Clientes");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<Customer> pagination)
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

        List<BsonDocument> results = await context.Customers.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Customer?>> CreateAsync(Customer plan)
    {
        try
        {
            await context.Customers.InsertOneAsync(plan);

            return new(plan, 201, "Clientes criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Clientes");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Customer?>> UpdateAsync(Customer plan)
    {
        try
        {
            await context.Customers.ReplaceOneAsync(x => x.Id == plan.Id, plan);

            return new(plan, 201, "Clientes atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Clientes");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Customer>> DeleteAsync(string id)
    {
        try
        {
            Customer? plan = await context.Customers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(plan is null) return new(null, 404, "Clientes não encontrado");
            plan.Deleted = true;
            plan.DeletedAt = DateTime.UtcNow;

            await context.Customers.ReplaceOneAsync(x => x.Id == id, plan);

            return new(plan, 204, "Clientes excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Clientes");
        }
    }
    #endregion
}
}