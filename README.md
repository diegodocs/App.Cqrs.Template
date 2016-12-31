# App.Crqs.Template

## Summary

This project App.Crqs.Template cover concepts about:
 
 - Event Driven Architecture - (http://en.wikipedia.org/wiki/Event-driven_architecture)
 - Event Sourcing with Martin Fowler (http://martinfowler.com/eaaDev/EventSourcing.html)
 - CQRS (http://www.codeproject.com/Articles/555855/Introduction-to-CQRS)
 - Dependency Injection (http://en.wikipedia.org/wiki/Dependency_injection)
 - Loose Coupling (http://en.wikipedia.org/wiki/Loose_coupling)
 - Onion Architecture (http://jeffreypalermo.com/blog/the-onion-architecture-part-1/)
 - SOLID Principles (http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29)
 - Cross Cutting Concerns: (http://en.wikipedia.org/wiki/Cross-cutting_concern)
 

## 3rd Party Nuget Packages 
 
- Autofac ( nuget.org )
 
 
## 0 - Core

App.Cqrs.Core is responsible for contract/interface definitions for commands and events on CQRS. 

- Commands are created and send to application
- They are received by commandHandlers which apply domain changes 
- Each command can generate one or more events
- Bus can publish all events
- EventHandlers will receive all events, and it will update ReadModels / QueryModels.
- QueryServices can ben consumed directly from front end
