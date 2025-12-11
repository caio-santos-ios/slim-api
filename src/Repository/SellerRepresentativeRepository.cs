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
    public class SellerRepresentativeRepository(AppDbContext context) : ISellerRepresentativeRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<SellerRepresentative> pagination)
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

            List<BsonDocument> results = await context.SellerRepresentatives.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Items");
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

            BsonDocument? response = await context.SellerRepresentatives.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Item não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Item");
        }
    }
    
    public async Task<ResponseApi<SellerRepresentative?>> GetByIdAsync(string id)
    {
        try
        {
            SellerRepresentative? billing = await context.SellerRepresentatives.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(billing);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Item");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<SellerRepresentative> pagination)
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

        List<BsonDocument> results = await context.SellerRepresentatives.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<SellerRepresentative?>> CreateAsync(SellerRepresentative billing)
    {
        try
        {
            await context.SellerRepresentatives.InsertOneAsync(billing);

            return new(billing, 201, "Item criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Item");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<SellerRepresentative?>> UpdateAsync(SellerRepresentative billing)
    {
        try
        {
            await context.SellerRepresentatives.ReplaceOneAsync(x => x.Id == billing.Id, billing);

            return new(billing, 201, "Item atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Item");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<SellerRepresentative>> DeleteAsync(string id)
    {
        try
        {
            SellerRepresentative? billing = await context.SellerRepresentatives.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(billing is null) return new(null, 404, "Item não encontrado");
            billing.Deleted = true;
            billing.DeletedAt = DateTime.UtcNow;

            await context.SellerRepresentatives.ReplaceOneAsync(x => x.Id == id, billing);

            return new(billing, 204, "Item excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Item");
        }
    }
    #endregion
}
}