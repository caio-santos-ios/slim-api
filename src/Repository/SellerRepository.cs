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
    public class SellerRepository(AppDbContext context) : ISellerRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Seller> pagination)
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

            List<BsonDocument> results = await context.Sellers.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Vendedores");
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
                    {"email", 1}
                }),
            ];

            BsonDocument? response = await context.Sellers.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Vendedor não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Vendedor");
        }
    }
    
    public async Task<ResponseApi<Seller?>> GetByIdAsync(string id)
    {
        try
        {
            Seller? seller = await context.Sellers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(seller);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Vendedor");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<Seller> pagination)
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

        List<BsonDocument> results = await context.Sellers.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Seller?>> CreateAsync(Seller seller)
    {
        try
        {
            await context.Sellers.InsertOneAsync(seller);

            return new(seller, 201, "Vendedor criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Vendedor");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Seller?>> UpdateAsync(Seller seller)
    {
        try
        {
            await context.Sellers.ReplaceOneAsync(x => x.Id == seller.Id, seller);

            return new(seller, 201, "Vendedor atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Vendedor");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Seller>> DeleteAsync(string id)
    {
        try
        {
            Seller? seller = await context.Sellers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(seller is null) return new(null, 404, "Vendedor não encontrado");
            seller.Deleted = true;
            seller.DeletedAt = DateTime.UtcNow;

            await context.Sellers.ReplaceOneAsync(x => x.Id == id, seller);

            return new(seller, 204, "Vendedor excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Vendedor");
        }
    }
    #endregion
}
}