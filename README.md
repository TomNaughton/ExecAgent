# ExecAgent

**ExecAgent** is a C# library and framework for running commands inside Docker containers with real-time output streaming.
It provides a reusable core library (`ExecAgent.Core`) that can be used by multiple client applications, such as console apps, web GUIs, or desktop interfaces.

---

## Features

* Execute commands in any running Docker container.
* Stream stdout and stderr in real-time.
* Retrieve command exit codes.
* Designed for reuse across multiple clients:

  * Console applications
  * Web applications (ASP.NET, SignalR)
  * Desktop GUI apps
* Cross-platform: works wherever Docker and .NET are supported.

---

## Repository Structure

```
ExecAgent/
│
├─ ExecAgent.sln              # Visual Studio solution
├─ ExecAgent.Core/            # Core library
├─ ExecAgent.Console/         # Test console application
├─ ExecAgent.Web/             # Future ASP.NET web UI
└─ .gitignore
```

* **ExecAgent.Core**: The reusable library handling Docker exec commands and streaming output.
* **ExecAgent.Console**: Example console client that demonstrates how to use the library.
* **ExecAgent.Web**: Placeholder for future web GUI using SignalR for live streaming.

---

## Getting Started

### Prerequisites

* [Docker](https://www.docker.com/get-started) installed and running.
* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed.

---

### Running the Test Console App

1. Pull a lightweight test container:

```bash
docker pull alpine:latest
docker run -d --name execagent-test alpine:latest tail -f /dev/null
```

2. Update the container name in `ExecAgent.Console/Program.cs`:

```csharp
string containerName = "execagent-test";
```

3. Run the console app:

```bash
dotnet run --project ExecAgent.Console
```

Expected output:

```
Starting ExecAgent test...
Streaming output:
StdOut: Hello ExecAgent
Execution finished with exit code: 0
```

---

## Usage in Your Own Projects

1. Add a reference to the Core library:

```bash
dotnet add reference ../ExecAgent.Core/ExecAgent.Core.csproj
```

2. Create a `DockerExecutor` and execute commands:

```csharp
var executor = new DockerExecutor();
var request = new ExecRequest
{
    Container = "your-container-name",
    Command = new[] { "your", "command" },
    Tty = true
};

var session = await executor.CreateSessionAsync(request, CancellationToken.None);

await foreach (var output in session.StreamAsync(CancellationToken.None))
{
    Console.WriteLine(output.Data);
}

var exitCode = await session.GetExitCodeAsync();
Console.WriteLine($"Command finished with exit code {exitCode}");
```

---

## Contributing

* Create a new branch for features or bug fixes (`feature/xxx`, `bugfix/xxx`).
* Make your changes and test locally.
* Open a pull request against `main`.

---

## License

This project is open-source and licensed under the MIT License.

---

## Topics / Tags

`.NET`, `C#`, `Docker`, `Exec`, `Streaming`, `Cross-platform`
