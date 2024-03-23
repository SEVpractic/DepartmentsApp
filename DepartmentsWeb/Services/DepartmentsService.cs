using DepartmentsWeb.Models.Dto;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DepartmentsWeb.Services
{
	public class DepartmentsService : IDepartmentsService
	{
		private readonly IHttpClientFactory clientFactory;
        //ToDo вынести в конфиг
        private string Url = "http://localhost:5105/departments";

        public DepartmentsService(IHttpClientFactory clientFactory)
		{
			this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
		}

		public async Task<List<DepartmentDto>> GetFromApiAsync()
		{
            ResponseDto apiResponceDto = await CallApi(HttpMethod.Get);

            if(apiResponceDto.IsSuccess) 
            {
                List<DepartmentDto> departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(apiResponceDto.Result));
                return departments;
            }

		    return new List<DepartmentDto>();		
		}

		public async Task<bool> SynchronizeDb(Stream fileStream)
		{
            List<DepartmentDto> departments = ReadFile(fileStream);
            if (departments.Count > 0) 
            {
                ResponseDto response = await CallApi(HttpMethod.Post, departments);
                return response.IsSuccess;
            }            
            return false;
		}

        private List<DepartmentDto> ReadFile(Stream fileStream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string json = reader.ReadToEnd();
                    List<DepartmentDto> departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(json);

                    return departments;
                }
            }
            catch
            {
                return new List<DepartmentDto>();
            }
            
        }

		private async Task<ResponseDto> CallApi(HttpMethod method, object? data = null)
		{
			try
			{
                //Создаем клиент запроса к сервису
                HttpClient client = clientFactory.CreateClient("DepartmentsAPI");

                // Формируем запрос
                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(Url);
                message.Method = method;
                if (data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(data),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                // Получаем ответ
                HttpResponseMessage apiResponse = await client.SendAsync(message);

                string apiContent = await apiResponse.Content.ReadAsStringAsync();
                ResponseDto apiResponceDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                return apiResponceDto;
            }
            catch (Exception ex) 
			{ 
				return new ResponseDto()
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
			}
        }
    }
}
