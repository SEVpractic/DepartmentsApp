using DepartmentsWeb.Models.Dto;
using Newtonsoft.Json;

namespace DepartmentsWeb.Services
{
	public class DepartmentsService : IDepartmentsService
	{
		private readonly IHttpClientFactory clientFactory;

		public DepartmentsService(IHttpClientFactory clientFactory)
		{
			this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
		}

		public async Task<List<DepartmentDto>> GetFromApiAsync()
		{
			//ToDo вынести в конфиг
			string Url = "http://localhost:5105/departments";

			try
			{
				//Создаем клиент запроса к сервису
				HttpClient client = clientFactory.CreateClient("DepartmentsAPI");

				HttpRequestMessage message = new HttpRequestMessage();
				message.Headers.Add("Accept", "application/json");
				//ToDo вынести в конфиг
				message.RequestUri = new Uri(Url);
				message.Method = HttpMethod.Get;

				HttpResponseMessage apiResponse = await client.SendAsync(message);

				var apiContent = await apiResponse.Content.ReadAsStringAsync();
				var apiResponceDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
				var departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(apiResponceDto.Result));
				return departments;
			}
			catch (Exception ex) 
			{
				return new List<DepartmentDto>();
			}			
		}
	}
}
