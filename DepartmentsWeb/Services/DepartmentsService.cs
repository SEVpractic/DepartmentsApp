using DepartmentsWeb.Configs;
using DepartmentsWeb.Models.Dto;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DepartmentsWeb.Services
{
    /// <summary>
    /// Сервис работы с информацией о подразделениях.
    /// </summary>
	public class DepartmentsService : IDepartmentsService
	{
		private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<DepartmentsService> logger;

        public DepartmentsService(IHttpClientFactory clientFactory, ILogger<DepartmentsService> logger)
		{
			this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Метод получения информации о подразделениях от DepartmentsAPI
        /// </summary>
        /// <returns></returns>
		public async Task<List<DepartmentDto>> GetFromApiAsync()
		{
            ResponseDto apiResponceDto = await CallApi(HttpMethod.Get);

            if(apiResponceDto.IsSuccess) 
            {
                List<DepartmentDto> departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(apiResponceDto.Result));
                logger.LogInformation("Получена информации о подразделениях от DepartmentsAPI");
                return departments;
            }

		    return new List<DepartmentDto>();		
		}


        /// <summary>
        /// Метод синхронизации информации о подразделениях в принятом файле и БД
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
		public async Task<bool> SynchronizeDb(Stream fileStream)
		{
            List<DepartmentDto> departments = ReadFile(fileStream);
            if (departments.Count > 0) 
            {
                ResponseDto response = await CallApi(HttpMethod.Post, departments);
                logger.LogInformation($"Запрос синхронизации информации о подразделениях успешно отправлен. ResponceDto.IsSuccess: {response.IsSuccess}");
                return response.IsSuccess;
            }            
            return false;
		}

        /// <summary>
        /// Метод парсинга файла синхронизации информации о подразделениях
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private List<DepartmentDto> ReadFile(Stream fileStream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string json = reader.ReadToEnd();
                    List<DepartmentDto> departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(json);
                    logger.LogInformation("Парсинг файла синхронизации информации о подразделениях выполнен успешно");

                    return departments;
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Ошибка парсинга файла синхронизации информации о подразделениях: {ex.Message}");
                return new List<DepartmentDto>();
            }
            
        }

        /// <summary>
        /// Метод отправки запросов к DepartmentsAPI
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		private async Task<ResponseDto> CallApi(HttpMethod method, object? data = null)
		{
			try
			{
                //Создаем клиент запроса к сервису
                HttpClient client = clientFactory.CreateClient("DepartmentsAPI");

                // Формируем запрос
                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(SD.DepartmentsApiUrl);
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

                logger.LogInformation($"Получен ответ DepartmentsAPI. ResponceDto.IsSuccess: {apiResponceDto.IsSuccess}");

                return apiResponceDto;
            }
            catch (Exception ex) 
			{
                logger.LogWarning($"Ошибка запроса DepartmentsAPI. {ex.Message}");
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
