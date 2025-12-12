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
    public class ProfessionalRepository(AppDbContext context) : IProfessionalRepository
{
    #region READ
    public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Professional> pagination)
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
                    { "from", "generic_tables" }, 
                    { "let", new BsonDocument("typeId", new BsonDocument("$toObjectId", "$type")) },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$typeId" }) }
                            })
                        }
                    },
                    { "as", "_type" } 
                }),

                new BsonDocument("$unwind", "$_type"),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "generic_tables" }, 
                    { "let", new BsonDocument("specialtyId", new BsonDocument("$toObjectId", "$specialty")) },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$specialtyId" }) }
                            })
                        }
                    },
                    { "as", "_specialty" } 
                }),

                new BsonDocument("$unwind", "$_specialty"),
               
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "generic_tables" }, 
                    { "let", new BsonDocument("registrationId", new BsonDocument("$toObjectId", "$registration")) },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$registrationId" }) }
                            })
                        }
                    },
                    { "as", "_registration" } 
                }),

                new BsonDocument("$unwind", "$_registration"),
                
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "addresses" },

                    { "let", new BsonDocument("profId", "$_id") },

                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr",
                                    new BsonDocument("$eq",
                                        new BsonArray
                                        {
                                            new BsonDocument("$toObjectId", "$parentId"), 
                                            "$$profId"                                     
                                        }
                                    )
                                }
                            })
                        }
                    },

                    { "as", "_address" }
                }),

                new BsonDocument("$unwind", "$_address"),
                
                new("$addFields", new BsonDocument
                {
                    {"id", new BsonDocument("$toString", "$_id")},
                    {"specialtyName", "$_specialty.description"},
                    {"typeName", "$_type.description"},
                    {"registrationName", "$_registration.description"},
                    {"address", new BsonDocument
                        {
                            {"id", new BsonDocument("$toString", "$_address._id")},
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
                    {"_specialty", 0}, 
                    {"_address", 0}, 
                    {"_type", 0}, 
                    {"_registration", 0},
                }),
                new("$sort", pagination.PipelineSort),
            };

            List<BsonDocument> results = await context.Professionals.Aggregate<BsonDocument>(pipeline).ToListAsync();
            List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
            return new(list);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Profissionais");
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
                    {"email", 1},
                }),
            ];

            BsonDocument? response = await context.Professionals.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
            return result is null ? new(null, 404, "Profissional não encontrado") : new(result);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Profissional");
        }
    }
    
    public async Task<ResponseApi<Professional?>> GetByIdAsync(string id)
    {
        try
        {
            Professional? professional = await context.Professionals.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            return new(professional);
        }
        catch
        {
            return new(null, 500, "Falha ao buscar Profissional");
        }
    }
    
    public async Task<int> GetCountDocumentsAsync(PaginationUtil<Professional> pagination)
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

        List<BsonDocument> results = await context.Professionals.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).Count();
    }
    #endregion
    
    #region CREATE
    public async Task<ResponseApi<Professional?>> CreateAsync(Professional professional)
    {
        try
        {
            await context.Professionals.InsertOneAsync(professional);

            return new(professional, 201, "Profissional criado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao criar Profissional");  
        }
    }
    #endregion
    
    #region UPDATE
    public async Task<ResponseApi<Professional?>> UpdateAsync(Professional professional)
    {
        try
        {
            await context.Professionals.ReplaceOneAsync(x => x.Id == professional.Id, professional);

            return new(professional, 201, "Profissional atualizado com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao atualizar Profissional");
        }
    }
    #endregion
    
    #region DELETE
    public async Task<ResponseApi<Professional>> DeleteAsync(string id)
    {
        try
        {
            Professional? professional = await context.Professionals.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
            if(professional is null) return new(null, 404, "Profissional não encontrado");
            professional.Deleted = true;
            professional.DeletedAt = DateTime.UtcNow;

            await context.Professionals.ReplaceOneAsync(x => x.Id == id, professional);

            return new(professional, 204, "Profissional excluído com sucesso");
        }
        catch
        {
            return new(null, 500, "Falha ao excluír Profissional");
        }
    }
    #endregion
}
}