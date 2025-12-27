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
    public class BillingRepository(AppDbContext context) : IBillingRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Billing> pagination)
    {
        try
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$sort", pagination.PipelineSort),
                new("$skip", pagination.Skip),
                new("$limit", pagination.Limit),
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

            List<BsonDocument> results = await context.Billings.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Faturamentos");
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
                new("$addFields", new BsonDocument {
                    {"id", new BsonDocument("$toString", "$_id")},
                }),
                new("$project", new BsonDocument
                {
                    {"_id", 0},
                }),
            ];

            BsonDocument? response = await context.Billings.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Faturamento não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Faturamento");
        }
    }
    
    public async Task<ResponseApi<Billing?>> GetByIdAsync(string id)
    {
        try
        {
            Billing? billing = await context.Billings.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(billing);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Faturamento");
        }
    }

    public async Task<ResponseApi<List<dynamic>>> GetSelectAsync(PaginationUtil<Billing> pagination)
    {
        try
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$sort", pagination.PipelineSort),
                new("$project", new BsonDocument
                {
                    {"_id", 0}, 
                    {"id", new BsonDocument("$toString", "$_id")},
                    {"name", 1}
                }),
                new("$sort", pagination.PipelineSort),
            };

            List<BsonDocument> results = await context.Billings.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Faturamentos");
        }
    }    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<Billing> pagination)
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

        List<BsonDocument> results = await context.Billings.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Billing?>> CreateAsync(Billing billing)
    {
        try
        {
            await context.Billings.InsertOneAsync(billing);

            return new(billing, 201, "Faturamento criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Faturamento");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Billing?>> UpdateAsync(Billing billing)
    {
        try
        {
            await context.Billings.ReplaceOneAsync(x => x.Id == billing.Id, billing);

            return new(billing, 201, "Faturamento atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Faturamento");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Billing>> DeleteAsync(string id)
    {
        try
        {
            Billing? billing = await context.Billings.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(billing is null) return new(null, 404, "Faturamento não encontrado");
            billing.Deleted = true;
            billing.DeletedAt = DateTime.UtcNow;

            await context.Billings.ReplaceOneAsync(x => x.Id == id, billing);

            return new(billing, 204, "Faturamento excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Faturamento");
        }
    }
    #endregion
}
}