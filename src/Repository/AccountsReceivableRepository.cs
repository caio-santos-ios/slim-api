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
                    
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "customers" },

                        { "let", new BsonDocument("profId", new BsonDocument("$toObjectId", "$customerId")) },

                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$and", new BsonArray
                                        {
                                            new BsonDocument("$eq", new BsonArray
                                            {
                                                "$_id",
                                                "$$profId"
                                            }),
                                        })
                                    }
                                })
                            }
                        },

                        { "as", "_customer" }
                    }),

                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$_customer" },
                        { "preserveNullAndEmptyArrays", true }
                    }),
                    
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "customer_contracts" },

                        { "let", new BsonDocument("profId", new BsonDocument("$toObjectId", "$contractId")) },

                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$and", new BsonArray
                                        {
                                            new BsonDocument("$eq", new BsonArray
                                            {
                                                "$_id",
                                                "$$profId"
                                            }),
                                        })
                                    }
                                })
                            }
                        },

                        { "as", "_contract" }
                    }),

                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$_contract" },
                        { "preserveNullAndEmptyArrays", true }
                    }),

                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "generic_tables" }, 
                        { "let", new BsonDocument("category", "$_contract.category") },
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
                    
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "generic_tables" }, 
                        { "let", new BsonDocument("costCenter", "$_contract.costCenter") },
                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$and", new BsonArray
                                        {
                                            new BsonDocument("$eq", new BsonArray { "$code", "$$costCenter" }),
                                            new BsonDocument("$eq", new BsonArray { "$table", "centro-custo" })
                                        })
                                    }
                                })
                            }
                        },
                        { "as", "_costCenter" } 
                    }),

                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$_costCenter" },
                        { "preserveNullAndEmptyArrays", true }
                    }),
                    
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "generic_tables" }, 
                        { "let", new BsonDocument("paymentMethod", "$_contract.paymentMethod") },
                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$and", new BsonArray
                                        {
                                            new BsonDocument("$eq", new BsonArray { "$code", "$$paymentMethod" }),
                                            new BsonDocument("$eq", new BsonArray { "$table", "forma-pagamento" })
                                        })
                                    }
                                })
                            }
                        },
                        { "as", "_payment_method" } 
                    }),

                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$_payment_method" },
                        { "preserveNullAndEmptyArrays", true }
                    }),

                    new BsonDocument("$addFields", new BsonDocument
                    {
                        {"total", new BsonDocument("$sum", new BsonArray 
                            { 
                                new BsonDocument("$toDouble", "$fees"), 
                                new BsonDocument("$toDouble", "$fines"),
                                new BsonDocument("$toDouble", "$value"),
                            }) 
                        },
                       
                    }),

                    new("$project", new BsonDocument
                    {
                        {"_id", 0},
                        {"id", new BsonDocument("$toString", "$_id")},
                        {"value", 1},
                        {"lowValue", 1},
                        {"code", 1},
                        {"balance", new BsonDocument("$subtract", new BsonArray 
                            { 
                                new BsonDocument("$toDouble", "$total"), 
                                new BsonDocument("$toDouble", "$lowValue") 
                            }) 
                        },
                        {"total", 1},
                        {"fees", 1},
                        {"fines", 1},                        
                        {"dueDate", 1},                        
                        {"customerName", new BsonDocument("$ifNull", new BsonArray {"$_customer.corporateName" , "" })},
                        {"contractCode", new BsonDocument("$ifNull", new BsonArray {"$_contract.code" , "" })},
                        {"categoryDescription", new BsonDocument("$ifNull", new BsonArray { "$_category.description", "" })},
                        {"costCenterDescription", new BsonDocument("$ifNull", new BsonArray { "$_costCenter.description", "" })},
                        {"paymentMethodDescription", new BsonDocument("$ifNull", new BsonArray { "$_payment_method.description", "" })},
                    }),
                    new("$sort", pagination.PipelineSort),
                };

                List<BsonDocument> results = await context.AccountsReceivables.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch
            {
                return new(null, 500, "Falha ao buscar Contas a Receber"); ;
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
        public async Task<ResponseApi<List<AccountsReceivable>>> GetByContractId(string contractId)
        {
            try
            {
                List<AccountsReceivable> accountsReceivable = await context.AccountsReceivables.Find(x => x.ContractId == contractId && !x.Deleted).ToListAsync();
                return new(accountsReceivable);
            }
            catch
            {
                return new(null, 500, "Falha ao buscar Contas a Receber"); ;
            }
        }
        public async Task<ResponseApi<long>> GetNextCodeAsync()
        {
            try
            {
                long count = await context.AccountsReceivables.Find(x => true).CountDocumentsAsync();
                return new(count);
            }
            catch
            {
                return new(0, 500, "Falha ao buscar novo código");
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
        public async Task<ResponseApi<AccountsReceivable?>> UpdateLowAsync(AccountsReceivable accountsReceivable)
        {
            try
            {
                await context.AccountsReceivables.ReplaceOneAsync(x => x.Id == accountsReceivable.Id, accountsReceivable);

                return new(accountsReceivable, 201, "Baixa feita com sucesso");
            }
            catch
            {
                return new(null, 500, "Falha ao dar baixa na Conta a Receber");
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