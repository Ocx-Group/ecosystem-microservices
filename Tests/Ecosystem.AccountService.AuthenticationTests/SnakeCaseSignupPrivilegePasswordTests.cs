using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Commands.Privilege;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Application.Mappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Ecosystem.AccountService.AuthenticationTests;

/// <summary>
/// Companion to <see cref="SnakeCaseCommandBindingTests"/> covering the three flows outside
/// the users/affiliates admin CRUD that speak snake_case: affiliate signup, role privileges
/// and password changes. Privileges are asserted in both directions because the Angular
/// Privilege/MenuConfiguration models read snake_case off the response too.
/// </summary>
public class SnakeCaseSignupPrivilegePasswordTests
{
    [Fact]
    public void CreateAffiliateCommand_BindsSnakeCasePayloadFromSignup()
    {
        // Exactly what signup.component.ts posts (CreateAffiliate model) in all three frontends.
        const string payload = """
        {
          "user_name": "nuevo1",
          "name": "Nuevo",
          "password": "s3cr3t",
          "last_name": "Afiliado",
          "email": "nuevo@example.com",
          "country": 66,
          "affiliate_type": "01_Membresia_10",
          "father": 5,
          "sponsor": 5,
          "binary_sponsor": 5,
          "phone": "+34600000000",
          "state_place": "",
          "city": "",
          "binary_matrix_side": 1,
          "status": 1
        }
        """;

        var command = JsonConvert.DeserializeObject<CreateAffiliateCommand>(payload)!;

        Assert.Equal("nuevo1", command.UserName);
        Assert.Equal("Afiliado", command.LastName);
        Assert.Equal("01_Membresia_10", command.AffiliateType);
        Assert.Equal(5, command.BinarySponsor);
        Assert.Equal((byte)1, command.BinaryMatrixSide);
        Assert.Equal(string.Empty, command.StatePlace);
        Assert.Equal("nuevo@example.com", command.Email);
        Assert.Equal("s3cr3t", command.Password);
        Assert.Equal(5, command.Father);
    }

    [Fact]
    public void CreatePrivilegeCommand_BindsSnakeCasePayloadFromPermissionsModal()
    {
        const string payload = """
        {
          "id": 0,
          "rol_id": 4,
          "menu_configuration_id": 12,
          "can_create": true,
          "can_read": true,
          "can_delete": false,
          "can_edit": true
        }
        """;

        var command = JsonConvert.DeserializeObject<CreatePrivilegeCommand>(payload)!;

        Assert.Equal(4, command.RolId);
        Assert.Equal(12, command.MenuConfigurationId);
        Assert.True(command.CanCreate);
        Assert.True(command.CanRead);
        Assert.False(command.CanDelete);
        Assert.True(command.CanEdit);
    }

    [Fact]
    public void UpdatePrivilegeCommand_BindsSnakeCaseAndKeepsOmittedFlagsNull()
    {
        var bound = JsonConvert.DeserializeObject<UpdatePrivilegeCommand>(
            """{"id":3,"can_create":false,"can_read":true,"can_delete":false,"can_edit":true}""")!;

        Assert.False(bound.CanCreate);
        Assert.True(bound.CanRead);
        Assert.True(bound.CanEdit);

        var partial = JsonConvert.DeserializeObject<UpdatePrivilegeCommand>("""{"id":3}""")!;

        Assert.Null(partial.CanCreate);
        Assert.Null(partial.CanRead);
        Assert.Null(partial.CanDelete);
        Assert.Null(partial.CanEdit);
    }

    [Fact]
    public void PrivilegesDto_SerializesSnakeCaseForTheAngularPrivilegeModel()
    {
        var json = JObject.Parse(JsonConvert.SerializeObject(new PrivilegesDto
        {
            Id = 3,
            RolId = 4,
            MenuConfigurationId = 12,
            CanCreate = true,
            CanRead = true,
            CanDelete = false,
            CanEdit = true
        }));

        Assert.Equal(4, (int)json["rol_id"]!);
        Assert.Equal(12, (int)json["menu_configuration_id"]!);
        Assert.True((bool)json["can_create"]!);
        Assert.True((bool)json["can_read"]!);
        Assert.False((bool)json["can_delete"]!);
        Assert.True((bool)json["can_edit"]!);
        Assert.NotNull(json["created_at"]);
    }

    [Fact]
    public void PrivilegeMenuConfigurationDto_SerializesSnakeCaseForTheAngularMenuModel()
    {
        var json = JObject.Parse(JsonConvert.SerializeObject(new PrivilegeMenuConfigurationDto
        {
            PrivilegeId = 3,
            MenuConfigurationId = 12,
            MenuName = "Usuarios",
            PageName = "users-list",
            CanCreate = true,
            CanRead = true,
            CanDelete = false,
            CanEdit = false
        }));

        Assert.Equal(3, (long)json["privilege_id"]!);
        Assert.Equal(12, (int)json["menu_configuration_id"]!);
        Assert.Equal("Usuarios", (string)json["menu_name"]!);
        Assert.Equal("users-list", (string)json["page_name"]!);
        Assert.True((bool)json["can_read"]!);
        Assert.False((bool)json["can_edit"]!);
    }

    [Fact]
    public void CreatePrivilege_SnakeCaseJsonMapsAllTheWayToTheEntity()
    {
        // CreatePrivilegeHandler does _mapper.Map<Privilege>(request), so binding alone is not
        // enough — the AutoMapper profile has to carry the flags through as well.
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PrivilegeMappingProfile>());
        var mapper = config.CreateMapper();

        var command = JsonConvert.DeserializeObject<CreatePrivilegeCommand>(
            """{"rol_id":4,"menu_configuration_id":12,"can_create":true,"can_read":true,"can_delete":false,"can_edit":true}""")!;

        var entity = mapper.Map<Ecosystem.AccountService.Domain.Models.Privilege>(command);

        Assert.Equal(4, entity.RolId);
        Assert.Equal(12, entity.MenuConfigurationId);
        Assert.True(entity.CanCreate);
        Assert.True(entity.CanRead);
        Assert.False(entity.CanDelete);
        Assert.True(entity.CanEdit);
    }

    [Fact]
    public void UpdatePasswordCommand_BindsNewPasswordFromSnakeCase()
    {
        // UpdatePassword model: { id, password, new_password, confirm_password }
        var command = JsonConvert.DeserializeObject<UpdatePasswordCommand>(
            """{"id":42,"password":"vieja","new_password":"nueva123","confirm_password":"nueva123"}""")!;

        Assert.Equal("vieja", command.Password);
        Assert.Equal("nueva123", command.NewPassword);
    }

    [Fact]
    public void UpdateAffiliatePasswordCommand_BindsNewPasswordFromSnakeCase()
    {
        var command = JsonConvert.DeserializeObject<UpdateAffiliatePasswordCommand>(
            """{"id":7,"password":"vieja","new_password":"nueva123","confirm_password":"nueva123"}""")!;

        Assert.Equal("vieja", command.Password);
        Assert.Equal("nueva123", command.NewPassword);
    }
}
