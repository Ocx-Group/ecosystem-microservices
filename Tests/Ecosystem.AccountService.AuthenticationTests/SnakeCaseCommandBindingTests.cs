using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Commands.User;
using Newtonsoft.Json;
using Xunit;

namespace Ecosystem.AccountService.AuthenticationTests;

/// <summary>
/// The Angular admin dashboards (web, recycoin, recybot) post the snake_case objects they
/// received from the corresponding DTO. AccountService uses Newtonsoft with no naming
/// strategy, so every multi-word command property needs an explicit [JsonProperty] or it
/// binds to null and the handler overwrites stored data with it.
/// </summary>
public class SnakeCaseCommandBindingTests
{
    [Fact]
    public void UpdateUserCommand_BindsSnakeCasePayloadFromUsersListEditModal()
    {
        const string payload = """
        {
          "id": 42,
          "rol_id": 3,
          "is_affiliate": 0,
          "user_name": "jdoe",
          "rol_name": "Administrador",
          "name": "John",
          "last_name": "Doe",
          "email": "jdoe@example.com",
          "phone": "+34600000000",
          "address": "Calle Falsa 123",
          "observation": "sin observaciones",
          "status": true,
          "image_profile_url": ""
        }
        """;

        var command = JsonConvert.DeserializeObject<UpdateUserCommand>(payload)!;

        Assert.Equal("jdoe", command.UserName);
        Assert.Equal("Doe", command.LastName);
        Assert.Equal(3, command.RolId);
        Assert.Equal("John", command.Name);
        Assert.Equal("jdoe@example.com", command.Email);
        Assert.True(command.Status);
    }

    [Fact]
    public void UpdateUserCommand_LeavesOmittedFieldsNullSoHandlerCanKeepStoredValues()
    {
        var command = JsonConvert.DeserializeObject<UpdateUserCommand>("""{"id":42}""")!;

        Assert.Null(command.UserName);
        Assert.Null(command.LastName);
        Assert.Null(command.RolId);
        Assert.Null(command.Status);
    }

    [Fact]
    public void CreateUserCommand_BindsSnakeCasePayloadFromUsersListCreateModal()
    {
        const string payload = """
        {
          "rol_id": 2,
          "user_name": "newuser",
          "password": "s3cr3t",
          "name": "New",
          "last_name": "User",
          "email": "new@example.com",
          "phone": "+34600000001",
          "address": "Calle Nueva 1",
          "observation": "alta",
          "status": true
        }
        """;

        var command = JsonConvert.DeserializeObject<CreateUserCommand>(payload)!;

        Assert.Equal("newuser", command.UserName);
        Assert.Equal("User", command.LastName);
        Assert.Equal(2, command.RolId);
        Assert.Equal("s3cr3t", command.Password);
    }

    [Fact]
    public void UpdateAffiliateCommand_BindsSnakeCasePayloadFromAffiliatesListEditModal()
    {
        const string payload = """
        {
          "id": 7,
          "user_name": "affiliate1",
          "name": "Ana",
          "last_name": "Lopez",
          "email": "ana@example.com",
          "phone": "+34600000002",
          "identification": "12345678Z",
          "zip_code": "28001",
          "country": 66,
          "state_place": "Madrid",
          "city": "Madrid",
          "address": "Gran Via 1",
          "birthday": "1990-05-01T00:00:00",
          "tax_id": "ES12345678Z",
          "beneficiary_name": "Luis Lopez",
          "beneficiary_email": "luis@example.com",
          "beneficiary_phone": "+34600000003",
          "legal_authorized_first": "Primero",
          "legal_authorized_second": "Segundo",
          "status": 1,
          "affiliate_type": "standard",
          "father": 3,
          "sponsor": 4,
          "termsConditions": true
        }
        """;

        var command = JsonConvert.DeserializeObject<UpdateAffiliateCommand>(payload)!;

        Assert.Equal("affiliate1", command.UserName);
        Assert.Equal("Lopez", command.LastName);
        Assert.Equal("28001", command.ZipCode);
        Assert.Equal("ES12345678Z", command.TaxId);
        Assert.Equal("Luis Lopez", command.BeneficiaryName);
        Assert.Equal("luis@example.com", command.BeneficiaryEmail);
        Assert.Equal("+34600000003", command.BeneficiaryPhone);
        Assert.Equal("Primero", command.LegalAuthorizedFirst);
        Assert.Equal("Segundo", command.LegalAuthorizedSecond);
        Assert.Equal("Madrid", command.StatePlace);
        Assert.Equal("standard", command.AffiliateType);
        Assert.True(command.TermsConditions);
    }

    [Fact]
    public void UpdateAffiliateCommand_OmittedTermsConditionsStaysNullInsteadOfFalse()
    {
        var command = JsonConvert.DeserializeObject<UpdateAffiliateCommand>("""{"id":7}""")!;

        Assert.Null(command.TermsConditions);
    }

    [Fact]
    public void UpdateUserProfileCommand_BindsSnakeCasePayloadFromMyProfile()
    {
        const string payload = """
        {
          "id": 9,
          "identification": "87654321X",
          "binary_matrix_side": 2,
          "address": "Calle Perfil 5",
          "phone": "+34600000004",
          "zip_code": "08001",
          "country": 66,
          "birthday": "1985-11-20T00:00:00",
          "tax_id": "ES87654321X",
          "legal_authorized_first": "Uno",
          "legal_authorized_second": "Dos",
          "beneficiary_name": "Maria",
          "beneficiary_email": "maria@example.com",
          "beneficiary_phone": "+34600000005"
        }
        """;

        var command = JsonConvert.DeserializeObject<UpdateUserProfileCommand>(payload)!;

        Assert.Equal("87654321X", command.Identification);
        Assert.Equal((byte)2, command.BinaryMatrixSide);
        Assert.Equal("08001", command.ZipCode);
        Assert.Equal("ES87654321X", command.TaxId);
        Assert.Equal("Uno", command.LegalAuthorizedFirst);
        Assert.Equal("maria@example.com", command.BeneficiaryEmail);
    }

    [Fact]
    public void RouteIdOverridesBodyId()
    {
        var command = JsonConvert.DeserializeObject<UpdateUserCommand>("""{"id":1,"user_name":"jdoe"}""")!;

        var updated = command with { Id = 42 };

        Assert.Equal(42, updated.Id);
        Assert.Equal("jdoe", updated.UserName);
    }
}
