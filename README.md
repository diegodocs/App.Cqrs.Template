# App.Crqs.Template

## Resumo

O projeto App.Crqs.Template inclui conceitos de:
 
 - Arquitetura baseada em eventos (http://en.wikipedia.org/wiki/Event-driven_architecture)
 - Domínio persistindo eventos por Martin Fowler (http://martinfowler.com/eaaDev/EventSourcing.html)
 - CQRS (http://www.codeproject.com/Articles/555855/Introduction-to-CQRS)
 - Injeção de Dependencia (http://en.wikipedia.org/wiki/Dependency_injection)
 - Baixo Acoplamento (http://en.wikipedia.org/wiki/Loose_coupling)
 - Arquitetura Cebola (http://jeffreypalermo.com/blog/the-onion-architecture-part-1/)
 - Principios Solid (http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29)
 - Preocupações multi-camadas (http://en.wikipedia.org/wiki/Cross-cutting_concern)
 

## Pacotes Externos utilizados 
 
- Autofac ( nuget.org )
 
 
## 0 - Core

App.Cqrs.Core é um grupo basico de interfaces para contrução de comandos e eventos para uma aplicação no modelo de Arquitetura baseada em eventos + CQRS. 

- Commands são criados e enviados pela aplicação
- Eles são recebidos pelos commandHandlers os quais aplicam comportamentos no domínio
- Cada comando pode gerar um ou mais eventos
- O Bus pode publicar todos os eventos
- Recebidos pelo EventHandlers, atualizarão todos os modelos de read/query 
- QueryServices podem ser consumidas diretamente pelo front end