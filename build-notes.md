# How this was built


- Separate projects for Web/Api/Presentation, Application/Core, Domain, Common, and Data
- Set up relationships between them:
    - Web => Application
    - Web => Domain
    - Application => Domain
    - Application => Data
    - Data => Domain
- Plug in the packages I always use:
    - AutoMapper, FluentValidation, Mediatr, EFCore$^1$
    - FluentAssertions, NSubstitute
- Add providers to Common (temporal/identifier)
- Add exceptions to Common (NotFoundException to start)
- Create controller endpoints, set up xml documentation
    - Add read/write dummy records
    - Add dummy automapper profile
    - Add automapper to configuration
- Create use case for create, set up first validator
    - change class1.cs to Startup.cs and add to Web configuration
- Create and configure dbContext
    - Fight EF core to get it set up with local appsettings
    - change class1.cs to Startup.cs and add to Core configuration
- Flesh out the controllers and use cases for POST and GET endpoints
- Add some swagger pipeline stuff (xml docs, examples)
- Create/get some examples to check it's all connected, make sure data is persisted

> **Checkpoint** - can create and read dummy entity via API, changes persist across service restarts.

- Go back and add tests around the work that's been done so far
- Go TDD from here on out - update, delete.
    - We'll be throwing ugly exceptions - that's fine for now

> **Checkpoint** - great test coverage, happy paths work, exceptions throw ugly

- Add exception handler
- (optional) add authorisation (scopes or rbac)
    - Generally, I'd expect to see this in the presentation layer - especially for "platform" stuff.  Individual applications can choose to make that part of their business logic later on (i.e. limit by properties of the entity, which should be in the application layer)


1 - Do we want to enforce ORM?  EF core is great for getting started, but can be a problem once your project grows.
2 - UoW and separate read/write connections can be a bit fiddly.  I'd assume most projects will start out with a single appsettings-based connection string.
