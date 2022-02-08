# TRADEART Demo Project Documentation

* TradeArt demo project is implemented by Doruk Demirci and it has a layered architecture.
* This document was prepared to give detailed information about the project, its layers, and usage.
* TradeArt solution consists of seven projects including one test project. The target framework of the projects is .NET 5.
* Swagger implementation is ready and the endpoints can be seen in `/swagger` path.

The details of each project are described below.

### TradeArtWebAPI
It is an ASP.NET Web API and has the entry point of the solution. There is a `TaskController` where the required tasks can be accessed. 
The Dependency Injection principle is used in `ServiceCollectionExtensions` and `TaskService` and `BlocktapIOService` is injected here.

### TradeArt.Interfaces
It is a class library and contains the interfaces of service classes. There are declarations of `TradeArt.TaskService` and `TradeArt.BlockTapIOService`.

### TradeArt.TaskService
It is a class library and includes the business logic for the first, second and third tasks.

### TradeArt.BlocktapIOService
It is a class library devoted to `Blocktap API` and relevant GraphQL processes. It is a unique project which has dependencies on relevant `GraphQL.CLient` packages.

### TradeArt.BlocktapIOService.Data
It is a class library used to store models and requests for `Blocktap API`. It is separated from the main service project to increase abstraction and reduce dependencies to it.

### TradeArt.Core
It is a class library and contains common objects and properties of the solution. The `Result` object is the most important class in it. Details of it will be described at later stages.
Extension methods and common objects are also available in the project. Built-in `Chunk()` comes with C# 10, so an IEnumerable extension `Chunk()` method is implemented in the project.

### TradeArt.Tests
It is an NUnit test project and encloses some test scenarios to investigate the expected operating status. `Moq` library is used in Controller Tests to mock required services.

## Helper objects

### Result
The `Result` object is a wrapper that is used for the third and fourth tasks. It is a generic class and has three properties. 
It is accessible by static `AsSuccess` and `AsFailure` methods. A successful result has a `data` and `true` flag and a failed result contains an `errorMessage` and `false` flag. 
Because throwing an exception is expensive, a boolean `IsSuccess` property is used to determine the return code of the controller methods.

## Task Descriptions

### Task1: api/Task/Task1

In the description of the task, there is a specific test but a generic endpoint is created to invert any given text. Built-in functions are used during the implementation to return the result as fast and cheaply as possible. 

### Task2: api/Task/Task2

There are two constraints,
- `Function A` will work as fast as possible without blocking.
- `Function B` has to process all data before `Function A` is completed. 

`Function A` has a task list and each task item will run `Function B` with an argument. `Function B` has a processing period of 100ms and that is mimicked by the `Task.Delay(100)` function. Then `Function A` returns true if and only if the statuses of all tasks are in `RanToCompletion` status.
The process is completely asynchronous and faster than the synchronous version approximately 300 times. The asynchronous version would be completed in approximately 100 seconds. (100 ms x 1000)

### Task3: api/Task/Task3

A unique constraint is not loading the file into memory.
`System.IO.FileStream` reads the file without loading it into the memory. That is why `FileStream` is used for the task.
If the specified file does not exist, the response is wrapped with `NotFound`.

### Task4: api/Task/Task4

Top 100 assets are fetched and chunked into five separate groups. According to the given `quoteCurrency` input, the maximum market price is tried to be retrieved. If there is a found price, it will be added to `successList`. Some assets do not have any price information in all given exchanges so they are inserted into `failureList`. Field `ticker` was not defined by type MarketFilter, so in the solution, fetching not null ticker and limiting by 1 could not be used. The maximum price info is fetched by `System.Linq` library. That is why the implementation of `IBlocktapIOService.GetMarketForBaseAndQuoteCurrency` is not as expected. The process is completed in approximately 20 seconds.

### Docker Support
`Dockerfile` can be found in the root directory of the solution
