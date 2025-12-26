using System.Net.Http.Headers;
using api_slim.src.Interfaces;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Newtonsoft.Json;

namespace api_slim.src.Services
{
    public class TelemedicineService : ITelemedicineService
    {
        private readonly HttpClient client = new();
        private readonly string uri = "https://api.rapidoc.tech/tema/api";
        private readonly string clientId = "ad656f8f-c6b9-4b65-95fe-4917c1310404";
        private readonly string token = "eyJhbGciOiJSUzUxMiJ9.eyJjbGllbnQiOiJTTElNIFNBw5pERSJ9.j8V7VD0e6LD2NPzo-leyaguxoTX8gob-iHVMeqBLxGscH_K0w8ed2V_scibiB7ag7BLYlhXHrYDMbHkBXs6vWm8XEZM06QJUl-NONffkh1Bs6hffzgqI070BecZcxfyGGFmVeTe0QMq2ikwjauoBpe-gsLRm_jsFnhserPCfYvmyK-_RBESP6b0oPR2aRnvJXD7LDLCx64VZ4fS7P9sR1rJBZev9oHm8OyXgI0QT6PQoEYTCC8RJ-4izaJESria3BUxVFJU0_MJeZoHaEgAofQOaaL6p6Xtrfmbed-1y9IO8QTAU7J7FKv7EHKNuHehf14W6kKilmZKKOK69Ly7AjhaaeeZyovdHZTBxuQjldGvV3toECN1XX1YGx8ocyROZYT99_3x0YEB_YUlm2EOaCTxT_UL22y822b-2V-2QKwH2lF9axOrZa3SlBp5U8E4bwuI0Oc1wEwRc7lVDeVzmdUWD9PZYI6japNUo7LNERUKYSqszvvJQgExwzVReJeDVFgRASbGFdt6UbqDPSrKoEuB_EHPrpNrbvQpF_Y44AsYqBlLACmvxnRXqvKrLLtlLSMIZQm5a3Xx3GAjepPnnEC9LebCtx9ig0tcyiiIM1nAb1raBsPuhamjlf6yKc-DRHAXjYIeOUNhVNUKqyBnDksvfb_vZh8K_MFWeraipNH4";

        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync()
        {
            try
            {
                var requestHeader = new HttpRequestMessage(HttpMethod.Get, $"{uri}/beneficiaries");
                requestHeader.Headers.Add("Authorization", $"Bearer {token}");
                requestHeader.Headers.Add("clientId", clientId);
                
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.rapidoc.tema-v2+json");
                requestHeader.Content = content;
                var response = await client.SendAsync(requestHeader);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();

                dynamic? result = JsonConvert.DeserializeObject(jsonResponse);
                List<dynamic> list = [];
                if(result is not null) 
                {
                    if(result.success == "true") 
                    {
                        foreach (dynamic item in result.beneficiaries)
                        {
                            list.Add(new {
                                id = item.uuid.ToString(),
                                dateOfBirth = item.birthday.ToString(),
                                cpf = item.cpf.ToString(),
                                name = item.name.ToString(),
                                status = item.isActive.ToString()
                            });                            
                        }
                    };
                }

                return new(list);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
        #region  UPDATE
        public async Task<ResponseApi<dynamic?>> UpdateStatusAsync(UpdateStatusTelemedicineDTO request)
        {
            try
            {
                var requestHeader = new HttpRequestMessage(HttpMethod.Delete, $"{uri}/beneficiaries/{request.Id}");
                requestHeader.Headers.Add("Authorization", $"Bearer {token}");
                requestHeader.Headers.Add("clientId", $"{clientId}");
                var response = await client.SendAsync(requestHeader);
                Console.WriteLine(await response.Content.ReadAsStringAsync());


                return new(null, 204, "Atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}