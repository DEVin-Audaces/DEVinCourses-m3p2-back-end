using System.Text;
using DEVCoursesAPI.Data.DTOs;
using Newtonsoft.Json;

namespace DEVCoursesAPI.Tests.ControllersTests;

[TestCaseOrderer("DEVCoursesAPI.Tests.AlphabeticalOrderer", "DEVCoursesAPI.Tests")]

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
        var bodyLogin = new LoginUser{Email = "raiche@gmail.com", Password = "RRR1525B"};
        jsonContent = JsonConvert.SerializeObject(bodyLogin);
        contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //Act
        returns = await client.PostAsync(serv, contentString);

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
        var bodyLogin = new LoginUser{Email = "raiche@gmail.com", Password = "RRR1525C"};
        var jsonContent = JsonConvert.SerializeObject(bodyLogin);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //Act
        var returns = await client.PostAsync(serv, contentString);

        //Assert
        Assert.False(returns.IsSuccessStatusCode);

    }

    [Fact]
    public async Task C_Consumir_Api_Create_User_Post_Sucesso()
    {
        //Arrange
        string serv = "/users/CreateUser";
        Random randNum = new Random();
        long cpf = randNum.Next();
        var body = new DataUser { Name = "José Alves", Age = 30, Email = "josealves" + cpf + "@gmail.com", CPF = cpf, Password = "AAAA2222", PasswordRepeat = "AAAA2222" };
        var jsonContent = JsonConvert.SerializeObject(body);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");



        //Act
        var returns = await client.PostAsync(serv, contentString);

        //Assert
        Assert.True(returns.IsSuccessStatusCode);

        jsonContent = await returns.Content.ReadAsStringAsync();

        Assert.NotEqual("", jsonContent);
    }

    [Fact]
    public async Task D_Consumir_Api_Create_User_Post_Sem_Sucesso()
    {
        //Arrange
        string serv = "/users/CreateUser";
        var body = new DataUser { Name = "José Alves", Age = 30, Email = "josealves@gmail.com", CPF = 15226925877, Password = "AAAAA2222", PasswordRepeat = "AAAA2222" };
        var jsonContent = JsonConvert.SerializeObject(body);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //Act
        var returns = await client.PostAsync(serv, contentString);

        //Assert
        Assert.False(returns.IsSuccessStatusCode);

    }

    
    [Fact]
    public async Task G_Consumir_Api_Update_User_Put_Sucesso()
    {
        //Arrange
        string serv = "/users/LoginUser";
        serv = "/users/LoginUser";
        var bodyLogin = new LoginUser{Email = "lucaspereira@gmail.com", Password = "AAAA2223"};
        var jsonContent = JsonConvert.SerializeObject(bodyLogin);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var returns = await client.PostAsync(serv, contentString);


        jsonContent = await returns.Content.ReadAsStringAsync();
        var objetoResponse =  JsonConvert.DeserializeObject<JWTResult>(jsonContent);

        var accessToken = objetoResponse.AccessToken;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        serv = "/users/UpdateUser";
        var body = new DataUser{Name = "Lucas Pereira Silva", Age = 42, Email = "lucaspereira@gmail.com",CPF = 1551152260, Password = "AAAA2223", PasswordRepeat = "AAAA2223"};
        jsonContent = JsonConvert.SerializeObject(body);
        contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //Act
        returns = await client.PutAsync(serv,contentString);

        //Assert
        Assert.True(returns.IsSuccessStatusCode);

        jsonContent = await returns.Content.ReadAsStringAsync();

        Assert.NotEqual("", jsonContent);
    }

    [Fact]
    public async Task H_Consumir_Api_Update_User_Put_Sem_Sucesso()
    {

        //Arrange
        string serv = "/users/LoginUser";
        serv = "/users/LoginUser";
        var bodyLogin = new LoginUser{Email = "lucaspereira@gmail.com", Password = "AAAA2223"};
        var jsonContent = JsonConvert.SerializeObject(bodyLogin);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var returns = await client.PostAsync(serv, contentString);


        jsonContent = await returns.Content.ReadAsStringAsync();
        var objetoResponse =  JsonConvert.DeserializeObject<JWTResult>(jsonContent);

        var accessToken = objetoResponse.AccessToken;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        serv = "/users/UpdateUser";
        var body = new DataUser{Name = "Lucas Pereira Silva", Age = 42, Email = "lucaspereiraL@gmail.com",CPF = 1137213194, Password = "AAAA2223", PasswordRepeat = "AAAA2223"};
        jsonContent = JsonConvert.SerializeObject(body);
        contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //Act
        returns = await client.PutAsync(serv,contentString);

        //Assert
        Assert.False(returns.IsSuccessStatusCode);
    }

}