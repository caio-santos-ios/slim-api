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
    public class AccountsReceivableRepository(AppDbContext context) : IAccountsReceivableRepository
    {
        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<AccountsReceivable> pagination)
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
                        {"password", 0},
                        {"role", 0},
                        {"blocked", 0},
                        {"codeAccess", 0},
                        {"validatedAccess", 0}
                    }),
                    new("$sort", pagination.PipelineSort),
                };

                List<BsonDocument> results = await context.AccountsReceivables.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch(Exception e)
            {
                return new(null, 500, e.Message); ;
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
                        {"photo", 1}
                    }),
                ];

                BsonDocument? response = await context.AccountsReceivables.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Contas a Receber não encontrado") : new(result);
            }
            catch
            {
                return new(null, 500, "Falha ao buscar Contas a Receber"); ;
            }
        }
        public async Task<ResponseApi<AccountsReceivable?>> GetByIdAsync(string id)
        {
            try
            {
                AccountsReceivable? accountsReceivable = await context.AccountsReceivables.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                return new(accountsReceivable);
            }
            catch
            {
                return new(null, 500, "Falha ao buscar Contas a Receber"); ;
            }
        }
        public async Task<int> GetCountDocumentsAsync(PaginationUtil<AccountsReceivable> pagination)
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
                    {"password", 0},
                    {"role", 0},
                    {"blocked", 0},
                    {"codeAccess", 0},
                    {"validatedAccess", 0}
                }),
                new("$sort", pagination.PipelineSort),
            };

            List<BsonDocument> results = await context.AccountsReceivables.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
        }
        #endregion
        #region CREATE
        public async Task<ResponseApi<AccountsReceivable?>> CreateAsync(AccountsReceivable accountsReceivable)
        {
            try
            {
                await context.AccountsReceivables.InsertOneAsync(accountsReceivable);

                return new(accountsReceivable, 201, "Contas a Receber criado com sucesso");
            }
            catch
            {
                return new(null, 500, "Falha ao criar Contas a Receber");   
            }
        }
        #endregion
        #region UPDATE
        public async Task<ResponseApi<AccountsReceivable?>> UpdateAsync(AccountsReceivable accountsReceivable)
        {
            try
            {
                await context.AccountsReceivables.ReplaceOneAsync(x => x.Id == accountsReceivable.Id, accountsReceivable);

                return new(accountsReceivable, 201, "Contas a Receber atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Falha ao atualizar Contas a Receber");
            }
        }
        #endregion
        #region DELETE
        public async Task<ResponseApi<AccountsReceivable>> DeleteAsync(string userId)
        {
            try
            {
                AccountsReceivable? accountsReceivable = await context.AccountsReceivables.Find(x => x.Id == userId && !x.Deleted).FirstOrDefaultAsync();
                if(accountsReceivable is null) return new(null, 404, "Contas a Receber não encontrado");
                accountsReceivable.Deleted = true;
                accountsReceivable.DeletedAt = DateTime.UtcNow;

                await context.AccountsReceivables.ReplaceOneAsync(x => x.Id == userId, accountsReceivable);

                return new(accountsReceivable, 204, "Contas a Receber excluído com sucesso");
            }
            catch
            {
                return new(null, 500, "Falha ao excluír Contas a Receber");
            }
        }
        #endregion
    }
}