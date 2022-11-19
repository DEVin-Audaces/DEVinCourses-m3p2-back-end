using System.Text;
using DEVCoursesAPI.Data.DTOs;
using Newtonsoft.Json;

namespace DEVCoursesAPI.Tests.ControllersTests;

[TestCaseOrderer("DEVCoursesAPI.Tests.Order.AlphabeticalOrderer", "DEVCoursesAPI.Tests")]

public class UserIntegrationTests: ConfigurationHostApi
{
    [Fact]
    public async Task A_Consumir_Api_User_Login_Get_Sucesso()
    {
        //Arrange
        string serv = "/users/CreateUser";
        Random randNum = new Random();
        long cpf = randNum.Next();
        var body = new DataUser{Name = "Rodrigo Metzker Raiche", Age = 40, Email = "raiche@gmail.com",CPF = cpf, Password = "RRR1525B", PasswordRepeat = "RRR1525B"};
        var jsonContent = JsonConvert.SerializeObject(body);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var returns = await client.PostAsync(serv,contentString);

        serv = "/users/LoginUser";
        string email = "Email=raiche%40gmail.com";
        string password = "Password=RRR1525B";
        var enders = $"{serv}?{email}&{password}";

        //Act
        returns = await client.GetAsync(enders);

        //Assert
        Assert.True(returns.IsSuccessStatusCode);

        jsonContent = await returns.Content.ReadAsStringAsync();
        var objetoResponse = JsonConvert.DeserializeObject<JWTResult>(jsonContent);

        Assert.NotEqual("", objetoResponse.AccessToken);
    }

    [Fact]
    public async Task B_Consumir_Api_User_Login_Usuario_Get_Sem_Sucesso()
    {
        //Arrange
        string serv = "/users/LoginUser";
        string email = "Email=raiche%40gmail.com";
        string password = "Password=RRR1525C";
        var enders = $"{serv}?{email}&{password}";

        //Act
        var returns = await client.GetAsync(enders);

        //Assert
        Assert.False(returns.IsSuccessStatusCode);

    }

    
}