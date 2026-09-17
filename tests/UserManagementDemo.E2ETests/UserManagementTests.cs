using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace UserManagementDemo.E2ETests;

public class UserManagementTests : PageTest
{
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL") ?? "http://localhost:5187";

    private async Task OpenHomePageAsync()
    {
        await Page.GotoAsync(_baseUrl);
    }

    [Fact]
    public async Task NewUserButtonOpensCreateDialog()
    {
        await OpenHomePageAsync();

        var createDialog = Page.GetByRole(AriaRole.Dialog, new() { Name = "Create user" });

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "User management" })).ToBeVisibleAsync();
        await Expect(createDialog).ToHaveCountAsync(0);

        await Page.GetByRole(AriaRole.Button, new() { Name = "New user" }).ClickAsync();

        await Expect(createDialog).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CreateUserAddsUserToTable()
    {
        await OpenHomePageAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "New user" }).ClickAsync();

        var createDialog = Page.GetByRole(AriaRole.Dialog, new() { Name = "Create user" });

        await createDialog.GetByLabel("Full name").FillAsync("Sofia Herrera");
        await createDialog.GetByLabel("Email address").FillAsync("sofia.herrera@example.com");
        await createDialog.GetByLabel("Role").SelectOptionAsync("Editor");
        await createDialog.GetByRole(AriaRole.Button, new() { Name = "Create user" }).ClickAsync();

        await Expect(createDialog).ToHaveCountAsync(0);
        await Expect(Page.GetByRole(AriaRole.Status)).ToContainTextAsync("User created successfully.");

        var createdUserRow = Page.GetByRole(AriaRole.Row).Filter(new() { HasText = "sofia.herrera@example.com" });
        await Expect(createdUserRow).ToContainTextAsync("Sofia Herrera");
        await Expect(createdUserRow).ToContainTextAsync("Editor");
        await Expect(createdUserRow).ToContainTextAsync("Active");
    }
}