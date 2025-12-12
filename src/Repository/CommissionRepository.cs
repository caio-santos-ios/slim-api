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
    public class CommissionRepository(AppDbContext context) : ICommissionRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Commission> pagination)
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

            List<BsonDocument> results = await context.Commissions.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Regras de Comissão");
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
                    {"ruleName", 1},
                    {"saleType", 1}
                }),
            ];

            BsonDocument? response = await context.Commissions.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Regra de Comissão não encontrada") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Regra de Comissão");
        }
    }
    
    public async Task<ResponseApi<Commission?>> GetByIdAsync(string id)
    {
        try
        {
            Commission? commission = await context.Commissions.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(commission);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Regra de Comissão");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<Commission> pagination)
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

        List<BsonDocument> results = await context.Commissions.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Commission?>> CreateAsync(Commission commission)
    {
        try
        {
            await context.Commissions.InsertOneAsync(commission);

            return new(commission, 201, "Regra de Comissão criada com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Regra de Comissão");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Commission?>> UpdateAsync(Commission commission)
    {
        try
        {
            await context.Commissions.ReplaceOneAsync(x => x.Id == commission.Id, commission);

            return new(commission, 201, "Regra de Comissão atualizada com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Regra de Comissão");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Commission>> DeleteAsync(string id)
    {
        try
        {
            Commission? commission = await context.Commissions.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(commission is null) return new(null, 404, "Regra de Comissão não encontrada");
            commission.Deleted = true;
            commission.DeletedAt = DateTime.UtcNow;

            await context.Commissions.ReplaceOneAsync(x => x.Id == id, commission);

            return new(commission, 204, "Regra de Comissão excluída com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Regra de Comissão");
        }
    }
    #endregion
}
}