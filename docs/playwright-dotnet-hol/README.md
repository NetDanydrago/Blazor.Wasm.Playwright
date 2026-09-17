# HOL-01: Probar una aplicación Blazor con Playwright .NET

Crea y ejecuta una prueba end-to-end para la aplicación de gestión de usuarios. La prueba comprueba que la aplicación carga, que el formulario comienza oculto y que **New user** lo abre como modal.

> Basado en [Installation | Playwright .NET](https://playwright.dev/dotnet/docs/intro).

## ¿Qué es Playwright?

Playwright es una biblioteca de automatización y un framework de pruebas end-to-end para aplicaciones web modernas. Controla un navegador real mediante una API y reproduce acciones de una persona: navegar, pulsar botones, completar formularios, subir archivos y validar resultados.

Sus características principales son:

- **Ejecución multiplataforma** en Windows, Linux y macOS, tanto localmente como en integración continua.
- **Modo headless o visible** para ejecutar rápidamente en segundo plano o mirar el navegador durante la depuración.
- **Auto-waiting** antes de interactuar: espera que el elemento esté visible, estable y habilitado.
- **Aislamiento** mediante contextos de navegador independientes, similares a perfiles nuevos y ligeros.
- **Emulación** de tamaños de pantalla, dispositivos móviles, ubicación, permisos, idioma y zona horaria.
- **Diagnóstico** mediante capturas, video, logs y [Trace Viewer](https://playwright.dev/dotnet/docs/trace-viewer-intro), que permite recorrer cada acción de una prueba.

## Lenguajes y runners

Playwright mantiene APIs oficiales para varios ecosistemas:

| Lenguaje | Paquete habitual | Runner frecuente |
|----------|------------------|------------------|
| JavaScript / TypeScript | `@playwright/test` | Playwright Test |
| C# / .NET | `Microsoft.Playwright` | xUnit, MSTest o NUnit |
| Python | `playwright` | Pytest o Unittest |
| Java | `com.microsoft.playwright` | JUnit o TestNG |

Las APIs comparten los conceptos de browser, context, page, locator y assertion, aunque la sintaxis y las herramientas del runner cambian. Este laboratorio usa **C#**, **.NET 10** y **xUnit 2.x** con la integración de Playwright para .NET (`Microsoft.Playwright.Xunit`).

## Navegadores compatibles

Playwright automatiza los tres motores principales:

| Motor | Navegadores representados |
|-------|--------------------------|
| Chromium | Chromium, Google Chrome y Microsoft Edge |
| Firefox | Firefox |
| WebKit | Safari y navegadores basados en WebKit |

Probar los tres motores ayuda a detectar diferencias de renderizado y comportamiento. Playwright también permite emular dispositivos móviles, pero esa emulación no sustituye una prueba en hardware real cuando se requiere validar comportamiento específico del dispositivo.

## Usos comunes

- **Pruebas end-to-end** de recorridos completos como registro, inicio de sesión, compras y CRUD.
- **Pruebas de interfaz** para formularios, modales, navegación, validaciones y estados de carga.
- **Pruebas entre navegadores** con el mismo escenario en Chromium, Firefox y WebKit.
- **Pruebas de API** con `APIRequestContext`, sin abrir una página, para validar endpoints REST.
- **Preparación y comprobación de datos** mediante API antes o después de interactuar con la interfaz.
- **Pruebas visuales** comparando capturas con imágenes de referencia.
- **Simulación de red** para interceptar peticiones, devolver respuestas controladas o probar errores y latencia.
- **Autenticación reutilizable** guardando cookies y almacenamiento local para evitar iniciar sesión en cada prueba.
- **Automatización en CI/CD** para bloquear una entrega cuando existe una regresión.
- **Automatización general del navegador**, como validación de sitios o extracción controlada de información, respetando permisos y términos de uso.

## Playwright e inteligencia artificial

Playwright puede servir como la capa de ejecución que permite a un agente de IA observar y operar una aplicación web. La IA decide qué comprobar o qué acción realizar; Playwright aporta una interacción determinista con el navegador.

### Playwright MCP

[Playwright MCP](https://github.com/microsoft/playwright-mcp) expone capacidades del navegador mediante Model Context Protocol. Un asistente compatible puede:

- Navegar y explorar una aplicación mediante snapshots estructurados del árbol de accesibilidad.
- Pulsar controles, completar formularios y comprobar estados sin depender únicamente de imágenes.
- Investigar un fallo y reproducir el recorrido del usuario.
- Generar código de automatización en TypeScript, Python, Java o C# mediante la opción `--codegen`.

MCP es útil para exploración interactiva y flujos autónomos. No reemplaza la suite versionada: los escenarios importantes deben terminar como tests revisables y repetibles en el repositorio.

### Agentes de Playwright Test

La documentación oficial de Playwright Test presenta tres agentes:

1. **Planner** - Explora la aplicación y produce un plan de pruebas en Markdown.
2. **Generator** - Convierte el plan en pruebas ejecutables y comprueba localizadores y aserciones.
3. **Healer** - Ejecuta pruebas fallidas, investiga cambios de interfaz y propone reparaciones.

Estos agentes están documentados actualmente para **Playwright Test en JavaScript/TypeScript** y se inicializan con `npx playwright init-agents`. En un proyecto .NET moderno con xUnit v3, se puede usar Playwright MCP para explorar la aplicación y generar C#, pero el código debe revisarse y validarse con `dotnet test`.

### Buenas prácticas al usar IA

- Pedir escenarios basados en requisitos y resultados observables, no en detalles internos de implementación.
- Preferir roles, labels y nombres accesibles frente a selectores CSS generados.
- Revisar que la IA no incluya contraseñas, tokens ni datos personales en código, prompts, trazas o capturas.
- Ejecutar siempre las pruebas generadas y revisar sus aserciones; una prueba que pasa puede no comprobar el requisito correcto.
- Mantener límites de dominio, aislamiento de datos y acciones destructivas controladas al permitir navegación autónoma.

## Duración estimada

~10 minutos

## Conceptos que explorarás

| Concepto | Descripción |
|----------|-------------|
| `IAsyncLifetime` | Gestiona la inicialización y cierre del navegador para cada clase de prueba. |
| Localizadores por rol | Encuentran elementos mediante su rol y nombre accesible. |
| Aserciones web-first | Esperan automáticamente el estado esperado de la interfaz. |
| Configuración de assembly | Aplica opciones de xUnit a todo el proyecto. |

## Requisitos previos

- SDK de .NET 10
- PowerShell 7 (`pwsh`)
- La raíz del repositorio como directorio actual
- La app y las pruebas ya están creadas en este workspace; si se recrea desde cero, usa los comandos del paso 1

## Ejercicio paso a paso (desde cero)

> Este laboratorio está pensado para que puedas recrearlo completo en un equipo limpio, sin depender de ninguna estructura ya creada previamente. La idea es crear la aplicación Blazor, crear la suite de pruebas, instalar el navegador y ejecutar la prueba end-to-end real.

### Paso 1: Requisitos previos

Asegúrate de tener instalado:

- .NET SDK 10
- PowerShell 7 (`pwsh`)
- Git
- Acceso a internet para restaurar paquetes NuGet y descargar Chromium

Desde una terminal, comprueba que el SDK esté disponible:

```powershell
dotnet --version
```

Si ves una versión 10.x, puedes continuar.

### Paso 2: Crear la app Blazor WebAssembly

En una carpeta nueva, crea la aplicación:

```powershell
mkdir Demo.PlayWright
cd Demo.PlayWright

dotnet new blazorwasm --framework net10.0 --name UserManagementDemo --output src/UserManagementDemo
```

Esto crea la app en `src/UserManagementDemo`.

### Paso 3: Crear el proyecto de pruebas Playwright

Dentro de la carpeta raíz del repositorio:

```powershell
mkdir tests
cd tests
dotnet new xunit --framework net10.0 --name UserManagementDemo.E2ETests --output UserManagementDemo.E2ETests
```

Ahora añade los paquetes necesarios para Playwright y xUnit:

```powershell
cd ..
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package Microsoft.Playwright --version 1.62.0
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package Microsoft.Playwright.Xunit --version 1.62.0
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package xunit --version 2.9.3
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package Microsoft.NET.Test.Sdk --version 17.12.0
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package xunit.runner.visualstudio --version 2.8.2
```

También es recomendable forzar la copia de dependencias del lock file al output para evitar problemas de runtime en algunos entornos:

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <LangVersion>latest</LangVersion>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
</PropertyGroup>
```

> Importante: `Microsoft.Playwright.Xunit` usa xUnit 2.x, no xUnit 3. Si usas `xunit.v3` o el runner de xUnit 3, el proyecto puede fallar con errores de `PageTest` o `FactAttribute` duplicados.

### Paso 4: Ajustar el proyecto de pruebas

El archivo del proyecto debe quedar parecido a esto:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="Microsoft.Playwright.Xunit" Version="1.62.0" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

### Paso 5: Crear la configuración de xUnit

Crea el archivo `tests/UserManagementDemo.E2ETests/XunitSettings.cs` con:

```csharp
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 0)]
```

Esto evita conflictos al ejecutar varias pruebas con varios navegadores/contextos.

### Paso 6: Crear la prueba E2E

Reemplaza el contenido de `tests/UserManagementDemo.E2ETests/UserManagementTests.cs` por:

```csharp
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
```

### Paso 7: Instalar el navegador de Playwright

Primero compila para restaurar el proyecto y generar el script de Playwright:

```powershell
dotnet build tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj
```

Luego instala Chromium:

```powershell
pwsh tests/UserManagementDemo.E2ETests/bin/Debug/net10.0/playwright.ps1 install chromium
```

Esto descarga el navegador que Playwright usará en la ejecución real.

### Paso 8: Ejecutar la aplicación

Abre una terminal y lanza la app:

```powershell
cd src/UserManagementDemo

dotnet run --urls http://localhost:5187
```

Si ese puerto está ocupado, usa otro: por ejemplo:

```powershell
dotnet run --urls http://localhost:5188
```

### Paso 9: Ejecutar las pruebas

En otra terminal:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5187"
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj --no-restore
```

Si la app arrancó en 5188:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5188"
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj --no-restore
```

### Paso 10: Ver el navegador y ejecutar la prueba

#### Opción A: depuración paso a paso con Inspector

Para pausar y avanzar manualmente por cada acción, usa `PWDEBUG=1`. Esta variable abre el navegador, inicia el Inspector de Playwright y establece el timeout en `0` mientras depuras.

Para depurar el test que abre el modal:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5187"
$env:PWDEBUG = "1"
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj --filter "FullyQualifiedName~NewUserButtonOpensCreateDialog"
```

Para depurar el test completo de creación de usuario, cambia el filtro:

```powershell
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj --filter "FullyQualifiedName~CreateUserAddsUserToTable"
```

Al terminar, vuelve al modo normal:

```powershell
$env:PWDEBUG = $null
```

#### Opción B: ejecución normal con navegador visible y `SlowMo`

Para ejecutar la prueba normalmente, pero viendo el navegador y dejando tiempo entre cada operación, usa las opciones de lanzamiento de Playwright después de `--`. `Headless=false` muestra el navegador y `SlowMo=250` añade 250 milisegundos entre operaciones.

Con la aplicación ejecutándose en `http://localhost:5187`, abre otra terminal:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5187"
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj `
  --no-restore `
  --filter "FullyQualifiedName~NewUserButtonOpensCreateDialog" `
  -- `
  Playwright.LaunchOptions.Headless=false `
  Playwright.LaunchOptions.SlowMo=250
```

Para ver el recorrido completo de creación de usuario:

```powershell
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj `
  --no-restore `
  --filter "FullyQualifiedName~CreateUserAddsUserToTable" `
  -- `
  Playwright.LaunchOptions.Headless=false `
  Playwright.LaunchOptions.SlowMo=250
```

Para una demostración más lenta, cambia el valor a `1000`:

```powershell
Playwright.LaunchOptions.SlowMo=1000
```

Para ejecutar los dos tests, elimina la opción `--filter` y conserva las opciones de lanzamiento.


### Paso 11: Validación de salida esperada

La prueba debería terminar con una salida parecida a esta:

```text
Resumen de pruebas: total: 2, con errores: 0, correcto: 2, omitido: 0
Compilación realizado correctamente.
```

## Solución de problemas comunes

### Error: `PageTest` no existe

Esto suele pasar por usar xUnit 3 con Playwright .NET. Usa estas referencias:

```powershell
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package Microsoft.Playwright.Xunit --version 1.62.0
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package xunit --version 2.9.3
```

### Error: puerto ocupado

Si `http://localhost:5187` ya está en uso, cambia el puerto:

```powershell
dotnet run --project src/UserManagementDemo/UserManagementDemo.csproj --urls http://localhost:5188
```

Y luego:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5188"
dotnet test tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj --no-restore
```

### Error: `testhost.dll` o dependencias faltantes

Asegúrate de incluir:

```powershell
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package Microsoft.NET.Test.Sdk --version 17.12.0
dotnet add tests/UserManagementDemo.E2ETests/UserManagementDemo.E2ETests.csproj package xunit.runner.visualstudio --version 2.8.2
```

Y en el archivo .csproj:

```xml
<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
```

### Error al instalar el navegador

Ejecuta:

```powershell
pwsh tests/UserManagementDemo.E2ETests/bin/Debug/net10.0/playwright.ps1 install chromium
```

Si la ruta cambia, revisa la carpeta `bin/Debug/net10.0` del proyecto de pruebas.

## Conclusión

Con estos pasos puedes recrear el laboratorio desde cero en un equipo limpio. El flujo correcto es:

1. crear app Blazor
2. crear proyecto de pruebas
3. instalar paquetes correctos
4. crear la clase de prueba
5. instalar Chromium
6. arrancar la app
7. ejecutar las pruebas E2E

Ese es el punto en el que el README deja de ser un apunte parcial y se convierte en una guía realmente ejecutable desde cero.
