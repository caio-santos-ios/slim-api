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
    public class InPersonRepository(AppDbContext context) : IInPersonRepository
    {
        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<InPerson> pagination)
        {
            try
            {
                List<BsonDocument> pipeline = new()
                {
                    new("$match", pagination.PipelineFilter),
                    new("$sort", pagination.PipelineSort),
                    new("$skip", pagination.Skip),
                    new("$limit", pagination.Limit),
                    
                    MongoUtil.Lookup("customer_recipients", ["$recipientId"], ["$_id"], "_recipient", [["deleted", false]], 1),
                    MongoUtil.Lookup("accredited_networks", ["$accreditedNetworkId"], ["$_id"], "_accredited_network", [["deleted", false]], 1),
                    MongoUtil.Lookup("service_modules", ["$serviceModuleId"], ["$_id"], "_service_module", [["deleted", false]], 1),
                    MongoUtil.Lookup("procedures", ["$procedureId"], ["$_id"], "_procedure", [["deleted", false]], 1),

                    new("$project", new BsonDocument
                    {
                        {"_id", 0}, 
                        {"id", MongoUtil.ToString("$_id")},
                        {"createdAt", 1},
                        {"date", 1},
                        {"responsiblePayment", 1},
                        {"recipientDescription", MongoUtil.First("_recipient.name")},
                        {"accreditedNetworkDescription", MongoUtil.First("_accredited_network.corporateName")},
                        {"serviceModuleDescription", MongoUtil.First("_service_module.name")},
                        {"procedureDescription", MongoUtil.First("_procedure.name")}
                    }),
                    new("$sort", pagination.PipelineSort),
                };

                List<BsonDocument> results = await context.InPersons.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
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

                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "addresses" },

                        { "let", new BsonDocument("id", new BsonDocument("$toString", "$_id")) },

                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$and", new BsonArray
                                        {
                                            new BsonDocument("$eq", new BsonArray
                                            {
                                                "$parentId",
                                                "$$id"
                                            }),

                                            new BsonDocument("$eq", new BsonArray
                                            {
                                                "$parent",
                                                "accredited-network"
                                            })
                                        })
                                    }
                                })
                            }
                        },

                        { "as", "_address" }
                    }),
                    
                    new("$addFields", new BsonDocument {
                        {"id", new BsonDocument("$toString", "$_id")},
                        {"address", new BsonDocument
                            {
                                {"id", new BsonDocument("$toString", new BsonDocument("$first", "$_address._id"))},
                                {"street", new BsonDocument("$first", "$_address.street")},
                                {"number", new BsonDocument("$first", "$_address.number")},
                                {"complement", new BsonDocument("$first", "$_address.complement")},
                                {"neighborhood", new BsonDocument("$first", "$_address.neighborhood")},
                                {"city", new BsonDocument("$first", "$_address.city")},
                                {"state", new BsonDocument("$first", "$_address.state")},
                                {"zipCode", new BsonDocument("$first", "$_address.zipCode")},
                                {"parent", new BsonDocument("$first", "$_address.parent")},
                                {"parentId", new BsonDocument("$first", "$_address.parentId")},
                            }
                        }
                    }),

                    new("$project", new BsonDocument
                    {
                        {"_id", 0},
                    }),
                ];

                BsonDocument? response = await context.InPersons.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Atendimento Presencial não encontrado") : new(result);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        
        public async Task<ResponseApi<InPerson?>> GetByIdAsync(string id)
        {
            try
            {
                InPerson? inPerson = await context.InPersons.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                return new(inPerson);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        
        public async Task<int> GetCountDocumentsAsync(PaginationUtil<InPerson> pagination)
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

            List<BsonDocument> results = await context.InPersons.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
        }
        #endregion
        
        #region CREATE
        public async Task<ResponseApi<InPerson?>> CreateAsync(InPerson inPerson)
        {
            try
            {
                await context.InPersons.InsertOneAsync(inPerson);

                return new(inPerson, 201, "Atendimento Presencial criado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");  
            }
        }
        #endregion
        
        #region UPDATE
        public async Task<ResponseApi<InPerson?>> UpdateAsync(InPerson inPerson)
        {
            try
            {
                await context.InPersons.ReplaceOneAsync(x => x.Id == inPerson.Id, inPerson);

                return new(inPerson, 201, "Atendimento Presencial atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        
        #region DELETE
        public async Task<ResponseApi<InPerson>> DeleteAsync(string id)
        {
            try
            {
                InPerson? inPerson = await context.InPersons.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                if(inPerson is null) return new(null, 404, "Atendimento Presencial não encontrado");
                inPerson.Deleted = true;
                inPerson.DeletedAt = DateTime.UtcNow;

                await context.InPersons.ReplaceOneAsync(x => x.Id == id, inPerson);

                return new(inPerson, 204, "Atendimento Presencial excluído com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}